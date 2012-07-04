﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;

namespace Wammer.Utility
{
	public static class ImageHelper
	{
		private const int OrientationId = 0x0112;

		public static ImageCodecInfo JpegCodec;
		public static ImageCodecInfo PngCodec;
		public static ImageCodecInfo GifCodec;

		static ImageHelper()
		{
			var codecs = ImageCodecInfo.GetImageEncoders();
			foreach (var codec in codecs)
			{
				switch (codec.MimeType.ToLower())
				{
					case "image/gif":
						GifCodec = codec;
						break;
					case "image/jpeg":
						JpegCodec = codec;
						break;
					case "image/png":
						PngCodec = codec;
						break;
				}
			}
		}

		public static Image ScaleBasedOnLongSide(Bitmap original, int sideLength)
		{
			float ratio1 = sideLength/(float) original.Width;
			float ratio2 = sideLength/(float) original.Height;
			float ratio = (original.Width > original.Height) ? ratio1 : ratio2;


			return Scale(original, ratio);
		}

		public static Image ScaleBasedOnShortSide(Bitmap original, int sideLength)
		{
			float ratio1 = sideLength/(float) original.Width;
			float ratio2 = sideLength/(float) original.Height;
			float ratio = (original.Width < original.Height) ? ratio1 : ratio2;

			return Scale(original, ratio);
		}

		private static Image Scale(Bitmap original, float ratio)
		{
			if (ratio >= 1)
				return original;

			var scaledWidth = (int) (original.Width*ratio);
			var scaledHeight = (int) (original.Height*ratio);
			Image scaledImage = new Bitmap(scaledWidth, scaledHeight);

			using (Graphics g = Graphics.FromImage(scaledImage))
			{
				g.InterpolationMode = InterpolationMode.High;
				g.DrawImage(original, new Rectangle(0, 0, scaledWidth, scaledHeight),
							new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
			}

			//var scaledImage = original.GetThumbnailImage(scaledWidth, scaledHeight, null, IntPtr.Zero);

			return scaledImage;
		}

		public static Bitmap Crop(Image original, int width, int height)
		{
			var cropedImage = new Bitmap(width, height);

			using (Graphics g = Graphics.FromImage(cropedImage))
			{
				g.DrawImage(original, new Rectangle(0, 0, width, height),
							new Rectangle(0, 0, width, height), GraphicsUnit.Pixel);
			}

			return cropedImage;
		}


		public static Bitmap Crop(Image original, int x, int y, int squareSize)
		{
			var cropedImage = new Bitmap(squareSize, squareSize);

			using (Graphics g = Graphics.FromImage(cropedImage))
			{
				if (original.Width - x < squareSize)
					x = original.Width - squareSize;

				if (original.Height - y < squareSize)
					y = original.Height - y;

				g.DrawImage(original, new Rectangle(0, 0, squareSize, squareSize),
							new Rectangle(x, y, squareSize, squareSize), GraphicsUnit.Pixel);
			}

			return cropedImage;
		}

		public static int LongSizeLength(Image img)
		{
			return img.Width > img.Height ? img.Width : img.Height;
		}

		public static int ShortSizeLength(Image img)
		{
			return img.Width < img.Height ? img.Width : img.Height;
		}

		// Return the image's orientation.
		public static ExifOrientations ImageOrientation(Image img)
		{
			// Get the index of the orientation property.
			int orientation_index = Array.IndexOf(img.PropertyIdList, OrientationId);

			// If there is no such property, return Unknown.
			if (orientation_index < 0) return ExifOrientations.Unknown;

			// Return the orientation value.
			return (ExifOrientations) img.GetPropertyItem(OrientationId).Value[0];
		}

		public static void CorrectOrientation(ExifOrientations orientation, Image pic)
		{
			switch (orientation)
			{
				case ExifOrientations.TopRight:
					pic.RotateFlip(RotateFlipType.RotateNoneFlipX);
					break;
				case ExifOrientations.BottomRight:
					pic.RotateFlip(RotateFlipType.Rotate180FlipNone);
					break;
				case ExifOrientations.BottomLeft:
					pic.RotateFlip(RotateFlipType.RotateNoneFlipY);
					break;
				case ExifOrientations.LeftTop:
					pic.RotateFlip(RotateFlipType.Rotate90FlipX);
					break;
				case ExifOrientations.RightTop:
					pic.RotateFlip(RotateFlipType.Rotate90FlipNone);
					break;
				case ExifOrientations.RightBottom:
					pic.RotateFlip(RotateFlipType.Rotate270FlipX);
					break;
				case ExifOrientations.LeftBottom:
					pic.RotateFlip(RotateFlipType.Rotate270FlipNone);
					break;
				default:
					return;
			}

			try
			{
				pic.RemovePropertyItem(OrientationId);
			}
			catch
			{
			}
		}

		public static Size GetImageSize(ArraySegment<byte> imageRawData)
		{
			using (var m = new MemoryStream(imageRawData.Array, imageRawData.Offset, imageRawData.Count))
			using (var image = new Bitmap(m))
			{
				return image.Size;
			}
		}
	}

	public enum ExifOrientations : byte
	{
		Unknown = 0,
		TopLeft = 1,
		TopRight = 2,
		BottomRight = 3,
		BottomLeft = 4,
		LeftTop = 5,
		RightTop = 6,
		RightBottom = 7,
		LeftBottom = 8,
	}
}