﻿#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using NLog;
using Newtonsoft.Json;
using Waveface.API.V2;

#endregion

namespace Waveface
{
    public class BatchPostManager
    {
        public event ProgressUpdateUI_Delegate UpdateUI;
        public event ShowMessage_Delegate ShowMessage;
        public event ShowMessage_Delegate UploadDone;
        public event ShowDialog_Delegate ShowFileMissDialog;
        public event ShowDialog_Delegate OverQuotaMissDialog;

        private static Logger s_logger = LogManager.GetCurrentClassLogger();

        private bool m_startUpload;
        private bool m_uploading;

        private WorkItem m_workItem;

        #region Properties

        public List<BatchPostItem> Items { get; set; }

        public bool StartUpload
        {
            get { return m_startUpload; }
            set { m_startUpload = value; }
        }

        public bool Uploading
        {
            get { return m_uploading; }
            set { m_uploading = value; }
        }

        #endregion

        public BatchPostManager()
        {
            Items = new List<BatchPostItem>();
        }

        public void Start()
        {
            m_workItem = AbortableThreadPool.QueueUserWorkItem(ThreadMethod, 0);
        }

        public WorkItemStatus AbortThread()
        {
            Save();

            return AbortableThreadPool.Cancel(m_workItem, true);
        }

        public void Add(BatchPostItem item)
        {
            Items.Add(item);

            Save();
        }

        public void Remove(BatchPostItem item)
        {
            Items.Remove(item);

            Save();
        }

        public int GetQueuedUnsendFilesCount()
        {
            int _ret = 0;

            foreach (BatchPostItem _item in Items)
            {
                _ret += (_item.Files.Count - _item.UploadedFiles.Count);
            }

            return _ret;
        }

        private void ThreadMethod(object state)
        {
            if (ShowMessage != null)
            {
                ShowMessage(I18n.L.T("BatchPostManager.DragDropHere"));
            }

            Thread.Sleep(10000);

            StartUpload = true;

            while (true)
            {
                BatchPostItem _batchPost;

                if (!Main.Current.RT.REST.IsNetworkAvailable)
                {
                    if (ShowMessage != null)
                        ShowMessage("");

                    UpdateUI(0, "");

                    continue;
                }

                lock (this)
                {
                    if (Items.Count > 0)
                    {
                        _batchPost = Items[0];
                    }
                    else
                    {
                        _batchPost = null;

                        if (ShowMessage != null)
                            ShowMessage(I18n.L.T("BatchPostManager.DragDropHere"));
                    }
                }

                if (_batchPost != null)
                {
                    if (ShowMessage != null)
                        ShowMessage("");

                    if (StartUpload)
                    {
                        BatchPostItem _retItem = BatchUploadPhoto(_batchPost);

                        if (_retItem.PostOK)
                        {
                            lock (this)
                            {
                                Remove(_batchPost);

                                if (UploadDone != null)
                                    UploadDone(I18n.L.T("PostForm.PostSuccess"));
                            }
                        }
                        else
                        {
                            if (_retItem.ErrorAndDeletePost)
                            {
                                Remove(_batchPost);

                                UpdateUI(int.MinValue, "");

                                if (UploadDone != null)
                                    UploadDone(I18n.L.T("PostForm.PostError"));

                                s_logger.Error("Remove New Post");

                                continue;
                            }

                            lock (this)
                            {
                                Save();
                            }
                        }
                    }
                }

                Thread.Sleep(5000);
            }
        }

        private BatchPostItem BatchUploadPhoto(BatchPostItem nItem)
        {
            int _count = 0;
            string _tmpStamp = DateTime.Now.Ticks.ToString();

            s_logger.Trace("[" + _tmpStamp + "]" + "BatchPhotoPost:" + nItem.Text + ", Files=" + nItem.Files.Count);

            while (true)
            {
                if (StartUpload)
                {
                    string _file = nItem.Files[_count];

                    if (nItem.UploadedFiles.Keys.Contains(_file))
                    {
                        s_logger.Trace("[" + _tmpStamp + "]" + "Batch Sended Photo [" + _count + "]" + _file);
                    }
                    else
                    {
                        try
                        {
                            Uploading = true;

                            if (!File.Exists(_file))
                            {
                                // 原始檔案不存在. 作錯誤處裡
                                s_logger.Error("Image File does not exist: [" + _file + "]");

                                if (ShowFileMissDialog != null)
                                    ShowFileMissDialog(_file);

                                while (Main.Current.NewPostThreadErrorDialogResult == DialogResult.None)
                                    Thread.Sleep(500);

                                switch (Main.Current.NewPostThreadErrorDialogResult)
                                {
                                    case DialogResult.Cancel:  // Delete Post
                                        nItem.ErrorAndDeletePost = true;
                                        nItem.PostOK = false;
                                        return nItem;

                                    case DialogResult.Yes: // Remove Picture
                                        s_logger.Error("Remove: [" + _file + "]");

                                        nItem.Files.Remove(_file);
                                        nItem.PostOK = false;

                                        UpdateUI(int.MinValue, "");

                                        return nItem;

                                    case DialogResult.Retry:  // DoNothing
                                        s_logger.Error("Ignore & Retry Miss File: [" + _file + "]");

                                        nItem.PostOK = false;
                                        return nItem;
                                }
                            }

                            if (CheckStoragesUsage() <= 0)
                            {
                                if (CheckStoragesUsage() <= 0) //Hack 
                                {
                                    // 雲端個人儲存空間不足. 作錯誤處裡 
                                    s_logger.Error("(CheckStoragesUsage() <= 0)");

                                    if (OverQuotaMissDialog != null)
                                        OverQuotaMissDialog("");

                                    while (Main.Current.NewPostThreadErrorDialogResult == DialogResult.None)
                                        Thread.Sleep(500);

                                    switch (Main.Current.NewPostThreadErrorDialogResult)
                                    {
                                        case DialogResult.Cancel: // Delete Post
                                            nItem.ErrorAndDeletePost = true;
                                            nItem.PostOK = false;
                                            return nItem;

                                        case DialogResult.Retry: // DoNothing

                                            nItem.PostOK = false;
                                            return nItem;
                                    }
                                }
                            }

                            string _text = new FileName(_file).Name;
                            string _resizedImage = ImageUtility.ResizeImage(_file, _text, nItem.LongSideResizeOrRatio, 100);

                            MR_attachments_upload _uf = Main.Current.RT.REST.File_UploadFile(_text, _resizedImage, "",
                                                                                             true);
                            if (_uf == null)
                            {
                                nItem.PostOK = false;
                                return nItem;
                            }

                            nItem.UploadedFiles.Add(_file, _uf.object_id);

                            s_logger.Trace("[" + _tmpStamp + "]" + "Batch Upload Photo [" + _count + "]" + _file);

                            // Cache Origin
                            string _localFileOrigin = Main.GCONST.CachePath + _uf.object_id + "_" + _text;
                            File.Copy(_file, _localFileOrigin);

                            // Cache Medium
                            string _localFileMedium = Main.GCONST.CachePath + _uf.object_id + "_medium_" + _text;
                            string _resizedMediumImageFilePath = ImageUtility.ResizeImage(_file, _text, "512", 100);
                            File.Copy(_resizedMediumImageFilePath, _localFileMedium);

                            // Small
                            if (_count == 0)
                            {
                                string _localFileSmall = Main.GCONST.CachePath + _uf.object_id + "_small_" + _text;
                                string _resizedSmallImageFilePath = ImageUtility.ResizeImage(_file, _text, "120", 100);
                                File.Copy(_resizedSmallImageFilePath, _localFileSmall);
                            }

                            Uploading = false;
                        }
                        catch (Exception _e)
                        {
                            Uploading = false;

                            NLogUtility.Exception(s_logger, _e, "BatchPhotoPost:File_UploadFile");
                            nItem.PostOK = false;
                            return nItem;
                        }
                    }

                    _count++;

                    int _counts = nItem.Files.Count;

                    if (UpdateUI != null)
                    {
                        string _msg;

                        if (Items.Count == 1)
                        {
                            _msg = string.Format(I18n.L.T("OnePostUpload"), _count, _counts - _count);
                        }
                        else
                        {
                            _msg = string.Format(I18n.L.T("MultiplePostUpload"), _count, _counts - _count, Items.Count - 1);
                        }

                        UpdateUI(_count * 100 / _counts, _msg);
                    }

                    if (_count == _counts)
                        break;
                }
                else
                {
                    nItem.PostOK = false;
                    return nItem;
                }
            }

            string _ids = "[";

            for (int i = 0; i < nItem.UploadedFiles.Count; i++)
            {
                _ids += "\"" + nItem.UploadedFiles[nItem.Files[i]] + "\"" + ",";
            }

            _ids = _ids.Substring(0, _ids.Length - 1); // 去掉最後一個","
            _ids += "]";


            try
            {
                MR_posts_new _np = Main.Current.RT.REST.Posts_New(nItem.Text, _ids, "", "image");

                if (_np == null)
                {
                    nItem.PostOK = false;
                    return nItem;
                }

                s_logger.Trace("[" + _tmpStamp + "]" + "Batch Post:" + nItem.Text + ", Files=" + nItem.Files.Count);
            }
            catch (Exception _e)
            {
                NLogUtility.Exception(s_logger, _e, "BatchPhotoPost:File_UploadFile");

                nItem.PostOK = false;
                return nItem;
            }

            nItem.PostOK = true;
            return nItem;
        }

        private long CheckStoragesUsage()
        {
            try
            {
                MR_storages_usage _storagesUsage = Main.Current.RT.REST.Storages_Usage();

                if (_storagesUsage != null)
                {
                    long m_avail_month_total_objects = _storagesUsage.storages.waveface.available.avail_month_total_objects;
                    long m_month_total_objects = _storagesUsage.storages.waveface.quota.month_total_objects;

                    //Hack
                    if (m_month_total_objects == -1)
                    {
                        return long.MaxValue;
                    }

                    return m_avail_month_total_objects;
                }
            }
            catch (Exception _e)
            {
                NLogUtility.Exception(s_logger, _e, "CheckStoragesUsage");
            }

            return long.MinValue;
        }

        #region IO

        public bool Save()
        {
            try
            {
                string _json = JsonConvert.SerializeObject(this);

                if (!GCONST.DEBUG)
                    _json = StringUtility.Compress(_json);

                string _filePath = Main.GCONST.CachePath + Main.Current.RT.Login.user.user_id + "_NP.dat";

                using (StreamWriter _outfile = new StreamWriter(_filePath))
                {
                    _outfile.Write(_json);
                }
            }
            catch (Exception _e)
            {
                NLogUtility.Exception(s_logger, _e, "Save");

                return false;
            }

            s_logger.Trace("Save: OK");

            return true;
        }

        public static BatchPostManager Load()
        {
            try
            {
                string _json = string.Empty;
                string _filePath = Main.GCONST.CachePath + Main.Current.RT.Login.user.user_id + "_NP.dat";

                StreamReader _sr = File.OpenText(_filePath);
                _json = _sr.ReadToEnd();
                _sr.Close();

                if (!GCONST.DEBUG)
                    _json = StringUtility.Decompress(_json);

                BatchPostManager _npm = JsonConvert.DeserializeObject<BatchPostManager>(_json);

                s_logger.Trace("Load:OK");

                return _npm;
            }
            catch (Exception _e)
            {
                NLogUtility.Exception(s_logger, _e, "Load");
            }

            return null;
        }

        #endregion
    }
}