﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Drawing;
using System.Threading;

using Wammer.Utility;
using Wammer.Cloud;
using log4net;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace Wammer.Station
{
	public class ImagePostProcessing
	{
		private readonly FileStorage fileStorage;
		private static ILog logger = LogManager.GetLogger(typeof(ImagePostProcessing));

		public ImagePostProcessing(FileStorage fileStorage)
		{
			this.fileStorage = fileStorage;
		}

		public void HandleImageAttachmentSaved(object sender, ImageAttachmentEventArgs evt)
		{
			System.Diagnostics.Debug.Assert(evt.Attachment.type == AttachmentType.image);
			if (evt.Meta != ImageMeta.Origin)
				return;

			try
			{
				using (Bitmap origImage = BuildBitmap(evt.Attachment.RawData))
				{
					ThumbnailInfo small = MakeThumbnail(origImage, ImageMeta.Small,
																		evt.Attachment.object_id);

					Attachment update = new Attachment
					{
						object_id = evt.Attachment.object_id,
						image_meta = new ImageProperty
						{
							width = origImage.Width,
							height = origImage.Height,
							small = small
						}
					};

					BsonDocument exist = evt.DbDocs.FindOneAs<BsonDocument>(
														Query.EQ("_id", evt.Attachment.object_id));
					exist.DeepMerge(update.ToBsonDocument());
					evt.DbDocs.Save(exist);


					ThreadPool.QueueUserWorkItem(this.UpstreamThumbnail,
								new UpstreamArgs(small, evt.Attachment.group_id, 
									evt.Attachment.object_id, ImageMeta.Small));
				}
			}
			catch (Exception e)
			{
				logger.Warn("Unabel to make thumbnail and upstream", e);
			}
		}

		public void HandleImageAttachmentCompleted(object sender, ImageAttachmentEventArgs evt)
		{
			if (evt.Attachment.type != AttachmentType.image || evt.Meta != ImageMeta.Origin)
				return;


			ThreadPool.QueueUserWorkItem(this.HandleImageAttachmentCompletedSync, evt);
		}

		public void HandleImageAttachmentCompletedSync(object args)
		{
			ImageAttachmentEventArgs evt = (ImageAttachmentEventArgs)args;

			if (evt.Attachment.type != AttachmentType.image || evt.Meta != ImageMeta.Origin)
				return;

			try
			{
				using (Bitmap origImage = BuildBitmap(evt.Attachment.RawData))
				{
					string origImgObjectId = evt.Attachment.object_id;
					ThumbnailInfo medium = MakeThumbnail(origImage, ImageMeta.Medium, origImgObjectId);
					ThumbnailInfo large = MakeThumbnail(origImage, ImageMeta.Large, origImgObjectId);
					ThumbnailInfo square = MakeThumbnail(origImage, ImageMeta.Square, origImgObjectId);

					Attachment update = new Attachment
					{
						object_id = evt.Attachment.object_id,
						image_meta = new ImageProperty
						{
							medium = medium,
							large = large,
							square = square
						}
					};

					BsonDocument doc = evt.DbDocs.FindOneAs<BsonDocument>(
																	Query.EQ("_id", origImgObjectId));
					doc.DeepMerge(update.ToBsonDocument());
					evt.DbDocs.Save<BsonDocument>(doc);

					UpstreamThumbnail(medium, evt.Attachment.group_id, evt.Attachment.object_id, ImageMeta.Medium);
					UpstreamThumbnail(large, evt.Attachment.group_id, evt.Attachment.object_id, ImageMeta.Large);
					UpstreamThumbnail(square, evt.Attachment.group_id, evt.Attachment.object_id, ImageMeta.Square);
				}
			}
			catch (Exception e)
			{
				logger.Warn("Image attachment post processing unsuccess", e);
			}
		}

		private static Bitmap BuildBitmap(byte[] imageData)
		{
			using (MemoryStream s = new MemoryStream(imageData))
			{
				return new Bitmap(s);
			}
		}

		private ThumbnailInfo MakeThumbnail(Bitmap origin, ImageMeta meta, string attachmentId)
		{
			Bitmap thumbnail = null;

			if (meta == ImageMeta.Square)
				thumbnail = MakeSquareThumbnail(origin);
			else
				thumbnail = ImageHelper.ScaleBasedOnLongSide(origin, (int)meta);

			string thumbnailId = Guid.NewGuid().ToString();
			using (MemoryStream m = new MemoryStream())
			{
				thumbnail.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);

				byte[] rawData = m.ToArray();

				string thumbFileName = Guid.NewGuid() + ".jpeg";
				
				fileStorage.Save(thumbFileName, rawData);

				return new ThumbnailInfo
				{
					file_name = thumbFileName,
					width = thumbnail.Width,
					height = thumbnail.Height,
					file_size = (int)m.Length, // TODO: no cast
					mime_type = "image/jpeg",
					modify_time = DateTime.UtcNow,
					url = "/v2/attachments/view/?object_id=" + attachmentId +
														"&image_meta=" + meta.ToString().ToLower(),
					RawData = rawData
				};
			}
		}

		private void UpstreamThumbnail(object state)
		{
			UpstreamArgs args = (UpstreamArgs)state;

			try
			{
				UpstreamThumbnail(args.Thumbnail, args.GroupId, args.FullImageId, args.ImageMeta);
			}
			catch (Exception e)
			{
				logger.Warn("Unable to upstream " + args.Thumbnail.file_name +
												" thumbnail of orig image " + args.FullImageId, e);
			}
		}

		private void UpstreamThumbnail(ThumbnailInfo thumbnail, string groupId, string fullImgId, 
																					ImageMeta meta)
		{
			using (MemoryStream output = new MemoryStream())
			{
				Cloud.Attachment.UploadImage(thumbnail.RawData, groupId, fullImgId, 
												thumbnail.file_name, "image/jpeg", meta);

				logger.DebugFormat("Thumbnail {0} is uploaded to Cloud", thumbnail.file_name);
			}
		}

		private static Bitmap MakeSquareThumbnail(Bitmap origin)
		{
			Bitmap tmpImage = ImageHelper.ScaleBasedOnShortSide(origin, 128);

			tmpImage = ImageHelper.Crop(tmpImage,
												ImageHelper.ShortSizeLength(tmpImage),
												ImageHelper.ShortSizeLength(tmpImage));
			return tmpImage;
		}
	}

	class UpstreamArgs
	{
		public ThumbnailInfo Thumbnail { get; private set; }
		public string FullImageId { get; private set; }
		public ImageMeta ImageMeta { get; set; }
		public string GroupId { get; set; }

		public UpstreamArgs(ThumbnailInfo tb, string groupId, string fullImgId, ImageMeta type)
		{
			Thumbnail = tb;
			FullImageId = fullImgId;
			ImageMeta = type;
			GroupId = groupId;
		}
	}
}