﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using NLog;

namespace Waveface.Upload
{
	class UploadQueuePersistency
	{
		#region Var
		private HashSet<UploadItem> _items;
		private readonly string filePath;
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private BufferProcesser _bufferSaveProcesser;
		#endregion

		#region Private Property
		/// <summary>
		/// Gets the items.
		/// </summary>
		/// <value>The items.</value>
		private HashSet<UploadItem> Items
		{
			get
			{
				return _items ?? (_items = new HashSet<UploadItem>());
			}
		}

		/// <summary>
		/// Gets the m_ buffer save processer.
		/// </summary>
		/// <value>The m_ buffer save processer.</value>
		private BufferProcesser m_BufferSaveProcesser
		{
			get
			{
				return _bufferSaveProcesser ?? (_bufferSaveProcesser = new BufferProcesser(() =>
					{
						Save();
					}, 1000));
			}
		}
		#endregion

		public UploadQueuePersistency(string runtimeDataPath, string user_id)
		{
			DebugInfo.ShowMethod();
			this.filePath = Path.Combine(runtimeDataPath, user_id + "_NP.txt");
		}

		public void Add(UploadItem item)
		{
			DebugInfo.ShowMethod();

			Items.Add(item);
			m_BufferSaveProcesser.WantProcess();
		}

		public void Add(IEnumerable<UploadItem> items)
		{
			DebugInfo.ShowMethod();

			foreach (var item in items)
				Items.Add(item);
			m_BufferSaveProcesser.WantProcess();
		}

		public void Remove(UploadItem item)
		{
			DebugInfo.ShowMethod();

			Items.Remove(item);
			m_BufferSaveProcesser.WantProcess();
		}

		public IEnumerable<UploadItem> Load()
		{
			DebugInfo.ShowMethod();

			try
			{
				using (StreamReader _sr = File.OpenText(filePath))
				{
					var _json = _sr.ReadToEnd();
				
					if (!GCONST.DEBUG)
						_json = StringUtility.Decompress(_json);

					var items = JsonConvert.DeserializeObject<HashSet<UploadItem>>(_json);

					this.Items.Clear();
					if (items != null)
					{
						this.Items.UnionWith(items);
					}
				}
			}
			catch (Exception _e)
			{
				logger.WarnException("Unable to load UploadQueue from " + filePath, _e);
			}

			return this.Items;
		}

		private void Save()
		{
			DebugInfo.ShowMethod();

			try
			{
				string _json = JsonConvert.SerializeObject(Items);

				if (!GCONST.DEBUG)
					_json = StringUtility.Compress(_json);

				using (StreamWriter _outfile = new StreamWriter(filePath))
				{
					_outfile.Write(_json);
				}
			}
			catch (Exception _e)
			{
				logger.WarnException("Unable to save UploadQueue", _e);
			}
		}

		
	}
}