using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Wammer.Cloud;
using Wammer.Model;
using Wammer.Station.Timeline;
using Waveface.Stream.Model;

namespace Wammer.Station
{
	[Serializable]
	public class ResourceDownloadEventArgs
	{
		public ResourceDownloadEventArgs()
		{
			failureCount = 0;
		}

		public string user_id { get; set; }
		public AttachmentInfo attachment { get; set; }
		public ImageMeta imagemeta { get; set; }
		public string filepath { get; set; }
		public int failureCount { get; set; }

		public bool IsOriginalAttachment()
		{
			return imagemeta == ImageMeta.Origin || imagemeta == ImageMeta.None;
		}

		public bool IsWebThumb()
		{
			return attachment.type.Equals("webthumb");
		}
	}



	internal static class ResourceDownloadTaskFactory
	{
		public static ResourceDownloadTask createDownloadTask(Driver driver, ImageMeta meta, AttachmentInfo attachment)
		{
			string tmpFolder;

			if (attachment.type.Equals("webthumb", StringComparison.InvariantCultureIgnoreCase))
				meta = ImageMeta.Medium;

			if (meta == ImageMeta.Origin || meta == ImageMeta.None)
				tmpFolder = new FileStorage(driver).basePath;
			else
			{
				tmpFolder = FileStorage.GetCachePath(driver.user_id);
				if (!Directory.Exists(tmpFolder))
					Directory.CreateDirectory(tmpFolder);
			}

			var evtargs = new ResourceDownloadEventArgs
			{
				user_id = driver.user_id,
				attachment = attachment,
				imagemeta = meta,
				filepath = Path.Combine(FileStorage.GetCachePath(driver.user_id), Guid.NewGuid().ToString())
			};

			TaskPriority pri;
			if (meta == ImageMeta.Medium || meta == ImageMeta.Small)
				pri = TaskPriority.High;
			else if (meta == ImageMeta.Large || meta == ImageMeta.Square)
				pri = TaskPriority.Medium;
			else
				pri = TaskPriority.Low;

			return new ResourceDownloadTask(evtargs, pri);
		}

	}
}