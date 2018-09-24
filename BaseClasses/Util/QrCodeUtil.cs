using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web.Mvc;
using ZXing;
using ZXing.Common;

namespace BaseClasses.Util
{public static class QrCodeUtil
	{
		public static Bitmap Composition(this string url, int width = 260, int height = 260, string logoUrl = "",bool needCut = false)
		{
			var bw = new BarcodeWriter();
			var encodeOptions = new EncodingOptions
			{
				Width = width,
				Height = height,
				Margin = 0
			};
			bw.Options = encodeOptions;
			bw.Format = BarcodeFormat.QR_CODE;
			var bitmap = bw.Write(url);
			if (needCut)
			{
				var innerWidth = (int)(width * 0.95);
				var pointX = (width - innerWidth) / 2;
				var g = Graphics.FromImage(bitmap);
				// g.FillRectangle(Brushes.White, pointX, pointX, innerWidth, innerWidth);
				var sr = new Rectangle(pointX, pointX, innerWidth, innerWidth); //要截取的矩形区域
				var dr = new Rectangle(0, 0, width, width); //要显示到Form的矩形区域
				g.DrawImage(bitmap, dr, sr, GraphicsUnit.Pixel);
				//var  bitmapClone = bitmap.Clone(sr, bitmap.PixelFormat);
				g.Dispose();
			}
			if (!string.IsNullOrEmpty(logoUrl))
			{
				using (var client = new WebClient())
				{
					var content = client.DownloadData(logoUrl);
					using (var stream = new MemoryStream(content))
					{
						var bmp = new Bitmap(stream);
						//计算插入图片的大小和位置
						var middleW = Math.Min(bitmap.Width / 5, bmp.Width);
						var middleH = Math.Min(bitmap.Height / 5, bmp.Height);
						var middleL = (width - middleW) / 2;
						var middleT = (height - middleH) / 2;
						//将二维码插入图片
						var myGraphic = Graphics.FromImage(bitmap);
						//白底
						//                        myGraphic.FillRectangle(Brushes.White, middleL, middleT, middleW, middleH);
						myGraphic.DrawImage(bmp, middleL, middleT, middleW, middleH);
						myGraphic.Dispose();
					}
				}
			}
			return bitmap;
		}

		public static string Base64ImageString(this string url, int width = 260, int height = 260, string logoUrl = "", bool needCut = false)
		{
			var bitmap = url.Composition(width, height, logoUrl, true);
			var memoryString = new MemoryStream();
			bitmap.Save(memoryString, ImageFormat.Jpeg);
			bitmap.Dispose();
			var base64Image = Convert.ToBase64String(memoryString.ToArray());
			memoryString.Dispose();
			return base64Image;
		}

		public static FileContentResult FileContentResult(this string url, int width=260, int height = 260, string logoUrl = "",bool needCut = false)
		{
			var bitmap = url.Composition(width, height, logoUrl, true);
			var memoryString = new MemoryStream();
			bitmap.Save(memoryString, ImageFormat.Jpeg);
			bitmap.Dispose();
			return new FileContentResult(memoryString.ToArray(), "image/Jpeg");
		}
	}
}
