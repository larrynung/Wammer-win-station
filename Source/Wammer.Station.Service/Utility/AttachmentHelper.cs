﻿using System;
using System.Linq;
using System.Collections.Generic;
using Wammer.Cloud;
using Wammer.Model;
using MongoDB.Driver.Builders;

namespace Wammer.Utility
{
	public class AttachmentHelper
	{
		#region Public Static Method

		public static List<AttachmentInfo> GetAttachmentInfoList(IEnumerable<string> attachment_id_list, string code_name)
		{
			return (from attachmentID in attachment_id_list
								  let attachment =
									  AttachmentCollection.Instance.FindOne(Query.EQ("_id", attachmentID))
								  where attachment != null
								  select AttachmentHelper.GetAttachmentnfo(attachment, code_name)).ToList();
		}

		private static AttachmentInfo GetAttachmentnfo(Attachment attachment, string codeName)
		{
			var attachmentInfo = new AttachmentInfo
			                     	{
			                     		group_id = attachment.group_id,
			                     		file_name = attachment.file_name,
			                     		object_id = attachment.object_id,
			                     		creator_id = attachment.creator_id,
			                     		modify_time = DateTime.Now.Ticks,
			                     		code_name = codeName,
			                     		type = attachment.type.ToString(),
			                     		url = attachment.url,
			                     		title = attachment.title,
			                     		description = attachment.description,
			                     		hidden = attachment.group_id
			                     	};

			if (attachment.image_meta != null)
			{
				attachmentInfo.image_meta = new AttachmentInfo.ImageMeta();

				if (attachment.image_meta.small != null)
				{
					attachmentInfo.image_meta.small = new AttachmentInfo.ImageMetaDetail();
					SetAttachmentInfoImageMeta(attachment.image_meta.small, attachmentInfo.image_meta.small);
				}

				if (attachment.image_meta.medium != null)
				{
					attachmentInfo.image_meta.medium = new AttachmentInfo.ImageMetaDetail();
					SetAttachmentInfoImageMeta(attachment.image_meta.medium, attachmentInfo.image_meta.medium);
				}

				if (attachment.image_meta.large != null)
				{
					attachmentInfo.image_meta.large = new AttachmentInfo.ImageMetaDetail();
					SetAttachmentInfoImageMeta(attachment.image_meta.large, attachmentInfo.image_meta.large);
				}
			}

			return attachmentInfo;
		}

		private static void SetAttachmentInfoImageMeta(ThumbnailInfo attachmentThumbnailInfo,
		                                              AttachmentInfo.ImageMetaDetail attachmentInfoThumbnailInfo)
		{
			if (attachmentThumbnailInfo == null || attachmentInfoThumbnailInfo == null)
				return;

			attachmentInfoThumbnailInfo.url = attachmentThumbnailInfo.url;
			attachmentInfoThumbnailInfo.height = attachmentThumbnailInfo.height;
			attachmentInfoThumbnailInfo.width = attachmentThumbnailInfo.width;
			attachmentInfoThumbnailInfo.modify_time = attachmentThumbnailInfo.modify_time.Ticks;
			attachmentInfoThumbnailInfo.file_size = attachmentThumbnailInfo.file_size;
			attachmentInfoThumbnailInfo.mime_type = attachmentThumbnailInfo.mime_type;
			attachmentInfoThumbnailInfo.md5 = attachmentThumbnailInfo.md5;
		}

		#endregion
	}
}