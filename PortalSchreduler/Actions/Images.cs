using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace PortalSchreduler.Actions
{
	public class Images
	{
		public static void Process()
		{
			Console.WriteLine("Images => start");

			var path = @"\\fs\Programs\Photos";
			var sub = "web_photos";
			var dir = new DirectoryInfo(path);
			if (!Directory.Exists(path + "\\" + sub)) Directory.CreateDirectory(path + "\\" + sub);

			int images = 0;
			int errors = 0;

			const int Width = 100;

			foreach (var file in dir.GetFiles())
			{
				var type = file.Name.Substring(file.Name.LastIndexOf(".") + 1).ToLower();

				if ("jpg png gif jpeg bmp".Contains(type))
				{
					images++;

					if ((DateTime.Today - file.LastWriteTime).TotalDays <= 1)
					{
						try
						{
							File.Delete(path + "\\" + sub + "\\" + file.Name);
						}
						catch { }
					}

					if (!File.Exists(path + "\\" + sub + "\\" + file.Name))
					{
						try
						{
							var original = Image.FromFile(file.FullName);

							var ratio = (float)original.Width / original.Height;
							var height = (int)(Width / ratio);
							var resized = ResizeImage(original, Width, height);
							Console.WriteLine(original.Width + "x" + original.Height + " => " + Width + "x" + height + " --- " + file.Name);

							resized.Save(path + "\\" + sub + "\\" + file.Name);

							original.Dispose();
							resized.Dispose();
						}
						catch (Exception e)
						{
							Console.WriteLine("error: " + file.Name + " [" + e.Message + "]");

							errors++;
						}
					}
				}
			}

			Console.WriteLine("Images: " + images + ", errors: " + errors);
			Console.WriteLine("Images => stop");
		}

		public static Bitmap ResizeImage(Image image, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}
	}
}