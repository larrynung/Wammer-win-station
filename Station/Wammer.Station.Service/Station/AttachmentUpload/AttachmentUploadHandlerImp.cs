﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wammer.Model;
using Wammer.Cloud;
using System.IO;
using System.Security.Cryptography;
using System.ComponentModel;
using Wammer.Utility;

namespace Wammer.Station.AttachmentUpload
{

	public enum UpsertResult
	{
		Insert,
		Update
	}

	public interface IAttachmentUploadHandlerDB
	{
		UpsertResult InsertOrMergeToExistingDoc(Attachment doc);
		Driver GetUserByGroupId(string groupId);
		LoginedSession FindSession(string sessionToken, string apiKey);
	}

	public class UploadData
	{
		private string _mime_type;

		public string object_id { get; set; }
		public string group_id { get; set; }
		public string mime_type
		{
			get { return string.IsNullOrEmpty(_mime_type) ? "application/octet-stream" : _mime_type; }
			set { _mime_type = value; }
		}
		public string file_name { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public AttachmentType type { get; set; }
		public ArraySegment<byte> raw_data { get; set; }
		public ImageMeta imageMeta { get; set; }

		public string api_key { get; set; }
		public string session_token { get; set; }
	}

	public class AttachmentEventArgs : EventArgs
	{
		public string AttachmentId { get; private set; }
		public bool IsFromThisWindows { get; private set; }
		public UpsertResult UpsertResult { get; private set; }
		public ImageMeta ImgMeta { get; private set; }

		public AttachmentEventArgs(string attachmentId, bool isFromThisWindows, UpsertResult upsertResult, ImageMeta meta)
			:base()
		{
			this.AttachmentId = attachmentId;
			this.IsFromThisWindows = isFromThisWindows;
			this.UpsertResult = upsertResult;
			this.ImgMeta = meta;
		}
	}

	public class AttachmentUploadHandlerImp
	{
		private readonly IAttachmentUploadHandlerDB db;
		public event EventHandler<AttachmentEventArgs> AttachmentProcessed;

		public AttachmentUploadHandlerImp(IAttachmentUploadHandlerDB db)
		{
			this.db = db;
		}

		public ObjectUploadResponse Process(UploadData uploadData)
		{
			if (uploadData == null)
				throw new ArgumentNullException("uploadData");

			if (uploadData.object_id == null)
				uploadData.object_id = Guid.NewGuid().ToString();

			Attachment dbDoc = new Attachment
			{
				object_id = uploadData.object_id,
				group_id = uploadData.group_id,
				file_name = uploadData.file_name,
				title = uploadData.title,
				description = uploadData.description,
				modify_time = DateTime.UtcNow,
				image_meta = new ImageProperty()
			};

			System.Drawing.Size imageSize = ImageHelper.GetImageSize(uploadData.raw_data);
			FileStorage storage = new FileStorage(db.GetUserByGroupId(uploadData.group_id));

			if (uploadData.imageMeta == ImageMeta.Origin || uploadData.imageMeta == ImageMeta.None)
			{
				dbDoc.mime_type = uploadData.mime_type;
				dbDoc.saved_file_name = GetSavedFileName(uploadData);
				dbDoc.file_size = uploadData.raw_data.Count;
				dbDoc.url = GetViewApiUrl(uploadData);
				dbDoc.md5 = ComputeMD5(uploadData.raw_data);
				dbDoc.image_meta.width = imageSize.Width;
				dbDoc.image_meta.height = imageSize.Height;

				storage.SaveFile(dbDoc.saved_file_name, uploadData.raw_data);
			}
			else
			{
				ThumbnailInfo thumb = new ThumbnailInfo
				{
					file_size = uploadData.raw_data.Count,
					md5 = ComputeMD5(uploadData.raw_data),
					mime_type = uploadData.mime_type,
					saved_file_name = GetSavedFileName(uploadData),
					url = GetViewApiUrl(uploadData),
					width = imageSize.Width,
					height =  imageSize.Height
				};

				dbDoc.image_meta.SetThumbnailInfo(uploadData.imageMeta, thumb);
				storage.SaveFile(thumb.saved_file_name, uploadData.raw_data);
			}

			UpsertResult dbResult = db.InsertOrMergeToExistingDoc(dbDoc);

			OnAttachmentProcessed(
				new AttachmentEventArgs(
					uploadData.object_id,
					db.FindSession(uploadData.session_token, uploadData.api_key) != null,
					dbResult,
					uploadData.imageMeta)
				);

			return ObjectUploadResponse.CreateSuccess(uploadData.object_id);
		}

		private void OnAttachmentProcessed(AttachmentEventArgs evt)
		{
			EventHandler<AttachmentEventArgs> handler = AttachmentProcessed;

			if (handler != null)
			{
				handler(this, evt);
			}
		}

		private static string GetViewApiUrl(UploadData data)
		{
			StringBuilder buf = new StringBuilder();
			buf.Append("/v2/attachments/view/?object_id=").
				Append(data.object_id);

			if (ImageMeta.Square <= data.imageMeta && data.imageMeta <= ImageMeta.Large)
			{
				buf.Append("&image_meta=").
					Append(data.imageMeta.GetCustomAttribute<DescriptionAttribute>().Description);
			}

			return buf.ToString();
		}

		private static string GetSavedFileName(UploadData data)
		{
			StringBuilder buf = new StringBuilder();
			buf.Append(data.object_id);

			if (ImageMeta.Square <= data.imageMeta && data.imageMeta <= ImageMeta.Large)
			{
				buf.Append("_").
					Append(data.imageMeta.GetCustomAttribute<DescriptionAttribute>().Description).
					Append(".dat");
			}
			else
			{
				buf.Append(Path.GetExtension(data.file_name));
			}

			return buf.ToString();
		}

		private static string ComputeMD5(ArraySegment<byte> rawData)
		{
			using (MD5 md5 = MD5.Create())
			{
				byte[] hash = md5.ComputeHash(rawData.Array, rawData.Offset, rawData.Count);
				StringBuilder buff = new StringBuilder();
				for (int i = 0; i < hash.Length; i++)
					buff.Append(hash[i].ToString("x2"));

				return buff.ToString();
			}
		}

	}
}