﻿
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using Waveface.API.V2;
using System.IO;
using System.Linq;
using Waveface.Libs.StationDB;
using System.Diagnostics;

namespace Waveface
{
    public class AttachmentUtility
    {
        public static void GetAllMediumsPhotoPathsByPost(Post post, Dictionary<string, string> files, Dictionary<string, string> mapping)
        {
			if (post.attachment_id_array == null || post.attachment_id_array.Count == 0)
				return;

			foreach (var object_id in post.attachment_id_array)
			{
				var mediumThumbnailPath = Path.Combine(Main.GCONST.ImageCachePath, object_id + "_medium.dat");
				
				files.Add(object_id, mediumThumbnailPath);

				var orig_filename = AttachmentCollection.QueryFileName(object_id);
				mapping.Add(mediumThumbnailPath, orig_filename);
			}

        }

        public static string GetRedirectURL(string orgURL, string session_token, string object_id, bool isImage)
        {
			DebugInfo.ShowMethod();

            session_token = HttpUtility.UrlEncode(session_token);
            object_id = HttpUtility.UrlEncode(object_id);

            string _url = WService.HostIP + orgURL;
            string _a_s = "&" + "apikey" + "=" + WService.APIKEY + "&" + "session_token" + "=" + session_token;

            if (orgURL == string.Empty)
            {
                _url += "/v2/attachments/view?object_id=" + object_id + _a_s + (isImage ? "&image_meta=medium" : "");
            }
            else
            {
                _url += _a_s;
            }

            return _url;
        }

        public static string GetRedirectURL_PdfCoverPage(string orgURL, string session_token)
        {
            session_token = HttpUtility.UrlEncode(session_token);

            string _url = WService.HostIP + orgURL;
            string _a_s = "&" + "apikey" + "=" + WService.APIKEY + "&" + "session_token" + "=" + session_token;
            _url += _a_s;

            return _url;
        }


        public static string GetRedirectURL_Image(string session_token, Attachment a, string imageType, out string url, out string fileName, bool forceCloud)
        {
			DebugInfo.ShowMethod();

            const string SMALL = "small";
            const string MEDIUM = "medium";
            const string ORIGIN = "origin";

            string object_id = a.object_id;
            string _imageType = imageType;


            session_token = HttpUtility.UrlEncode(session_token);
            object_id = HttpUtility.UrlEncode(object_id);

            string _ip = forceCloud ? WService.CloudIP : WService.HostIP;

            string _urlX = "/v2/attachments/view?object_id=" + object_id + "&" +
                            "apikey" + "=" + WService.APIKEY + "&" +
                            "session_token" + "=" + session_token;

            string _url = _ip + _urlX;

            if (imageType == SMALL)
            {
                if ((a.image_meta != null) && (a.image_meta.small != null))
                {
                    _imageType = SMALL;
                }
                else
                {
                    _imageType = MEDIUM;
                }
            }

            if (imageType == ORIGIN)
            {
                // if (a.url != "")
                //{
                _imageType = ORIGIN;

                url = _url;
                fileName = a.object_id + Path.GetExtension(a.file_name);

                return _imageType;
                //}
            }

            if ((imageType == SMALL) || (imageType == MEDIUM))
                url = "[IP]" + _urlX + "&" + "image_meta=" + _imageType;
            else
                url = _url; // + "&" + "image_meta=" + _imageType;			

            fileName = a.object_id + "_" + _imageType + ".dat";
            return _imageType;
        }

        public static bool CheckURL(string url)
        {
            //Send web request
            Uri _urlCheck = new Uri(url);
            HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(_urlCheck);
            _request.Timeout = 1500;

            HttpWebResponse _response;

            try
            {
                //get response
                _response = (HttpWebResponse)_request.GetResponse();
            }
            catch (Exception)
            {
                //403 or couldn't connect to internet
                Console.WriteLine("Could not connect to the internet");
                return false; //can't connect to url
            }

            //Return status
            return _response.StatusCode == HttpStatusCode.Found; //200 = all ok
        }
    }
}