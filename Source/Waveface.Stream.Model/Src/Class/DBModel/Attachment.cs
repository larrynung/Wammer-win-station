﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using System.Linq;

namespace Waveface.Stream.Model
{
	[Serializable]
	public enum ImageMeta
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("none")]
		None = 0,

		/// <summary>
		/// 128x128 pixels
		/// </summary>
		[Description("square")]
		Square = 128,

		/// <summary>
		/// 120 pixels
		/// </summary>
		[Description("small")]
		Small = 512,

		/// <summary>
		/// 720 pixels
		/// </summary>
		[Description("medium")]
		Medium = 1024,

		/// <summary>
		/// 1024 pixels
		/// </summary>
		[Description("large")]
		Large = 2048,

		/// <summary>
		/// Original image size
		/// </summary>
		[Description("origin")]
		Origin = 50 * 1024 * 1024
	}

	public enum AttachmentType
	{
		image,
		doc,
		webthumb
	}

	public interface IAttachmentInfo
	{
		string mime_type { get; }
		string saved_file_name { get; }
	}

	[BsonIgnoreExtraElements]
	public class ThumbnailInfo : IAttachmentInfo
	{
		private byte[] rawData;

		[BsonIgnoreIfNull]
		public string url { get; set; }

		[BsonIgnoreIfNull]
		public int width { get; set; }

		[BsonIgnoreIfNull]
		public int height { get; set; }

		[BsonIgnoreIfNull]
		public DateTime modify_time { get; set; }

		[BsonIgnoreIfNull]
		public long file_size { get; set; }

		[BsonIgnoreIfNull]
		public string file_name { get; set; }

		[BsonIgnoreIfNull]
		public string md5 { get; set; }

		[BsonIgnore]
		[XmlIgnore]
		public byte[] RawData
		{
			get { return rawData; }
			set
			{
				rawData = value;
				if (rawData != null)
				{
					using (MD5 md5 = MD5.Create())
					{
						byte[] hash = md5.ComputeHash(rawData);
						var buff = new StringBuilder();
						for (int i = 0; i < hash.Length; i++)
							buff.Append(hash[i].ToString("x2"));

						this.md5 = buff.ToString();
					}
				}
			}
		}

		#region IAttachmentInfo Members

		[BsonIgnoreIfNull]
		public string mime_type { get; set; }

		[BsonIgnoreIfNull]
		public string saved_file_name { get; set; }

		#endregion

		public bool ShouldSerializewidth()
		{
			return width > 0;
		}

		public bool ShouldSerializeheight()
		{
			return height > 0;
		}

		public bool ShouldSerializefile_size()
		{
			return file_size > 0;
		}
	}

	[BsonIgnoreExtraElements]
	public class ImageProperty
	{
		[BsonIgnoreIfNull]
		public exif exif { get; set; }

		[BsonIgnoreIfNull]
		public int width { get; set; }

		[BsonIgnoreIfNull]
		public int height { get; set; }

		[BsonIgnoreIfNull]
		public ThumbnailInfo small { get; set; }

		[BsonIgnoreIfNull]
		public ThumbnailInfo medium { get; set; }

		[BsonIgnoreIfNull]
		public ThumbnailInfo large { get; set; }

		[BsonIgnoreIfNull]
		public ThumbnailInfo square { get; set; }

		public ThumbnailInfo GetThumbnailInfo(ImageMeta meta)
		{
			switch (meta)
			{
				case ImageMeta.Small:
					return small;
				case ImageMeta.Medium:
					return medium;
				case ImageMeta.Large:
					return large;
				case ImageMeta.Square:
					return square;
				default:
					throw new ArgumentException("meta is not a thumbnail: " + meta);
			}
		}


		public void SetThumbnailInfo(ImageMeta meta, ThumbnailInfo thumbnail)
		{
			switch (meta)
			{
				case ImageMeta.Small:
					small = thumbnail;
					break;
				case ImageMeta.Medium:
					medium = thumbnail;
					break;
				case ImageMeta.Large:
					large = thumbnail;
					break;
				case ImageMeta.Square:
					square = thumbnail;
					break;
				default:
					throw new ArgumentException("meta is not a thumbnail: " + meta);
			}
		}

		public bool ShouldSerializewidth()
		{
			return width > 0;
		}

		public bool ShouldSerializeheight()
		{
			return height > 0;
		}
	}


	[Serializable]
	[BsonIgnoreExtraElements]
	public class WebProperty
	{
		public List<WebAccess> accesses { get; set; }
		public string favicon { get; set; }
		public string title { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	[BsonIgnoreExtraElements]
	public class WebAccess
	{
		public string from { get; set; }
		public DateTime time { get; set; }
	}


	[DataContract]
	[Serializable]
	[BsonIgnoreExtraElements]
	public class GPSInfo
	{
		[DataMember(Name = "1")]
		public string GPSLatitudeRef { get; set; }

		[DataMember(Name = "2")]
		public List<List<int>> GPSLatitude { get; set; }

		[DataMember(Name = "3")]
		public string GPSLongitudeRef { get; set; }

		[DataMember(Name = "4")]
		public List<List<int>> GPSLongitude { get; set; }
	}

	[Serializable]
	[BsonIgnoreExtraElements]
	public class Gps
	{
		public double? longitude { get; set; }
		public double? latitude { get; set; }
		[BsonIgnoreIfNull]
		public string GPSDateStamp { get; set; }
		[BsonIgnoreIfNull]
		public List<object[]> GPSTimeStamp { get; set; }
	}

	[Serializable]
	[BsonIgnoreExtraElements]
	public class exif
	{
		[BsonIgnoreIfNull]
		public List<int> YResolution { get; set; }

		//[BsonIgnoreIfNull]
		//public int? ResolutionUnit { get; set; }

		[BsonIgnoreIfNull]
		public string Make { get; set; }

		[BsonIgnoreIfNull]
		public int? Flash { get; set; }

		[BsonIgnoreIfNull]
		public string DateTime { get; set; }

		[BsonIgnoreIfNull]
		public int? MeteringMode { get; set; }

		[BsonIgnoreIfNull]
		public List<int> XResolution { get; set; }

		[BsonIgnoreIfNull]
		public int? ExposureProgram { get; set; }

		[BsonIgnoreIfNull]
		public int? ColorSpace { get; set; }

		[BsonIgnoreIfNull]
		public int? ExifImageWidth { get; set; }

		[BsonIgnoreIfNull]
		public string DateTimeDigitized { get; set; }

		[BsonIgnoreIfNull]
		public string DateTimeOriginal { get; set; }

		[BsonIgnoreIfNull]
		public List<int> ExposureTime { get; set; }

		[BsonIgnoreIfNull]
		public int? SensingMethod { get; set; }

		[BsonIgnoreIfNull]
		public List<int> FNumber { get; set; }

		[BsonIgnoreIfNull]
		public List<int> ApertureValue { get; set; }

		[BsonIgnoreIfNull]
		public List<int> FocalLength { get; set; }

		[BsonIgnoreIfNull]
		public int? WhiteBalance { get; set; }

		[BsonIgnoreIfNull]
		public string ComponentsConfiguration { get; set; }

		[BsonIgnoreIfNull]
		public int? ExifOffset { get; set; }

		[BsonIgnoreIfNull]
		public int? ExifImageHeight { get; set; }

		[BsonIgnoreIfNull]
		public int? ISOSpeedRatings { get; set; }

		[BsonIgnoreIfNull]
		public string Model { get; set; }

		[BsonIgnoreIfNull]
		public string Software { get; set; }

		[BsonIgnoreIfNull]
		public string FlashPixVersion { get; set; }

		[BsonIgnoreIfNull]
		public int? YCbCrPositioning { get; set; }

		[BsonIgnoreIfNull]
		public string ExifVersion { get; set; }

		[BsonIgnoreIfNull]
		public GPSInfo GPSInfo { get; set; }

		[BsonIgnoreIfNull]
		public Gps gps { get; set; }
	}

	[BsonIgnoreExtraElements]
	public class Attachment : IAttachmentInfo
	{
		[BsonIgnore]
		private readonly object rawDataMutex = new object();
		private ArraySegment<byte> rawData;

		private DateTime? _eventTimes;

		public Attachment()
		{
			rawData = new ArraySegment<byte>();
			Orientation = ExifOrientations.Unknown;
		}


		[BsonId]
		public string object_id { get; set; }

		[BsonIgnoreIfNull]
		public string group_id { get; set; }

		[BsonIgnoreIfNull]
		public string file_name { get; set; }

		[BsonIgnoreIfNull]
		public string title { get; set; }

		[BsonIgnoreIfNull]
		public string description { get; set; }

		[BsonIgnoreIfNull]
		[BsonElement("md5")]
		public string MD5 { get; set; }

		public AttachmentType type { get; set; }

		[BsonIgnoreIfNull]
		public string url { get; set; }

		[BsonIgnoreIfNull]
		public string image { get; set; }

		[BsonIgnoreIfNull]
		public long file_size { get; set; }

		[BsonIgnoreIfNull]
		public DateTime modify_time { get; set; }

		[BsonIgnoreIfNull]
		public ImageProperty image_meta { get; set; }

		[BsonIgnoreIfNull]
		public bool body_on_cloud { get; set; }

		[BsonIgnore]
		public ExifOrientations Orientation { get; set; }

		[BsonIgnoreIfNull]
		public string creator_id { get; set; }

		[BsonIgnoreIfNull]
		public string post_id { get; set; }

		[BsonIgnoreIfNull]
		public string file_path { get; set; }

		[BsonIgnoreIfNull]
		public DateTime? import_time { get; set; }

		[BsonIgnoreIfNull]
		public DateTime? file_create_time { get; set; }

		[BsonIgnoreIfNull]
		public DateTime? event_time 
		{
			get
			{
				if (_eventTimes == null && type == AttachmentType.doc && doc_meta != null && doc_meta.access_time != null)
				{
					return doc_meta.access_time.LastOrDefault();
				}

				return _eventTimes;
			}
			set
			{
				_eventTimes = value;
			}
		}

		[BsonIgnoreIfNull]
		public int? timezone { get; set; }

		public DateTime file_modify_time { get; set; }

		[BsonIgnoreIfNull]
		public string device_id { get; set; }

		[BsonIgnoreIfNull]
		public DocProperty doc_meta { get; set; }

		public DateTime last_access { get; set; }
		/// <summary>
		/// is the attachment is imported from this station?
		/// </summary>
		public bool fromLocal { get; set; }

		#region IAttachmentInfo Members

		[BsonIgnoreIfNull]
		public string mime_type { get; set; }

		[BsonIgnoreIfNull]
		public string saved_file_name { get; set; }

		#endregion

		public bool ShouldSerializefile_size()
		{
			return file_size > 0;
		}

		public bool HasThumbnail(ImageMeta meta)
		{
			if (image_meta == null)
				return false;

			switch (meta)
			{
				case ImageMeta.Small:
					return image_meta.small != null;
				case ImageMeta.Medium:
					return image_meta.medium != null;
				case ImageMeta.Large:
					return image_meta.large != null;
				case ImageMeta.Square:
					return image_meta.square != null;
				default:
					throw new ArgumentException("not a valid thumbmail meta: " + meta);
			}
		}

		public bool HasDocPreviews()
		{
			return doc_meta != null && doc_meta.preview_files != null && doc_meta.preview_files.Count > 0;
		}

		public IAttachmentInfo GetInfoByMeta(ImageMeta meta)
		{
			if (meta == ImageMeta.None || meta == ImageMeta.Origin)
				return this;
			return image_meta.GetThumbnailInfo(meta);
		}
	}
}