﻿using System;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

using AmsDispatch.Util;

using FotoSite.Properties;

namespace FotoSite
{
	public class SmallImageHandler : IHttpHandler
	{
		/// <summary>
		/// You will need to configure this handler in the Web.config file of your 
		/// web and register it with IIS before being able to use it. For more information
		/// see the following link: http://go.microsoft.com/?linkid=8101007
		/// </summary>
		#region IHttpHandler Members

		public bool IsReusable
		{
			// Return false in case your Managed Handler cannot be reused for another request.
			// Usually this would be false in case you have some state information preserved per request.
			get { return true; }
		}

		public void ProcessRequest(HttpContext context)
		{
			string imagename = context.Request.Params["name"].ToString();
			string toHeight = context.Request.Params["h"].ToString();

			string fullPath = Path.GetFullPath(Path.Combine(Settings.Default.FotoFolder, imagename));

			//Helper.Log.InfoFormat("Запрос на фото '{0}' высотой {1}px, файл '{2}'", imagename, toHeight, fullPath);

			try
			{
				using (Bitmap bigBmp = new Bitmap(fullPath))
				{
					// во сколько раз надо уменьшить картинку
					int smallWidth = bigBmp.Width;
					int smallHeigth = bigBmp.Height;
					double k = (double)bigBmp.Height / Convert.ToDouble(toHeight);

					context.Response.ContentType = "image/jpeg";

					if (k > 1.0) // надо уменьшать!
					{
						smallWidth = Convert.ToInt32(Math.Round((double)bigBmp.Width / k, MidpointRounding.AwayFromZero));
						smallHeigth = Convert.ToInt32(toHeight);

						using (Bitmap smallBmp = new Bitmap(smallWidth, smallHeigth, PixelFormat.Format24bppRgb))
						{
							using (Graphics g = Graphics.FromImage(smallBmp))
							{
								// параметры качества отсюда - http://stackoverflow.com/questions/249587/high-quality-image-scaling-library
								g.CompositingQuality = CompositingQuality.HighQuality;
								g.InterpolationMode = InterpolationMode.HighQualityBicubic;
								g.SmoothingMode = SmoothingMode.HighQuality;

								g.DrawImage(bigBmp, 0, 0, smallBmp.Width, smallBmp.Height);

								// JPEG codec
								ImageCodecInfo jpegCodec = GetEncoder(ImageFormat.Jpeg);

								// массив из одного параметра кодирования (качество) для передачи в Save
								EncoderParameters encoderParams = new EncoderParameters(1);
								// задаем максимальное качество JPEG (Обязательно 100L а не просто 100 !!!!)
								encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 100L); ;

								// В формате PNG (и еще куче других, кроме GIF и JPG) нельзя выводить
								// непосредственно в context.Response.OutputStream, т.к. по нему
								// можно идти только в одну сторону. Надо использовать
								// промежуточный MemoryStream, у нас JPG, но пусть безобразие будет единообразным!
								using (MemoryStream ms = new MemoryStream())
								{
									smallBmp.Save(ms, jpegCodec, encoderParams);
									ms.WriteTo(context.Response.OutputStream);
								}
							}
						}
					}
					else // ничего уменьшать не надо, транслируем оригинал
					{
						using (MemoryStream ms = new MemoryStream())
						{
							bigBmp.Save(ms, ImageFormat.Jpeg);
							ms.WriteTo(context.Response.OutputStream);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Helper.Log.ErrorFormat("{0} - Ошибка при выводе картинки '{1}' высотой {2}px [{3}]", 
					ex.GetMessages(), imagename, toHeight, context.Request.UserHostAddress);
				Helper.Log.Debug(string.Format("Ошибка при выводе картинки '{0}' высотой {1}px [{2}]: ", 
					imagename, toHeight, context.Request.UserHostAddress), ex);
			}
		}

		#endregion

		private ImageCodecInfo GetEncoder(ImageFormat format)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.FormatID == format.Guid)
					return codec;
			}

			return null;
		}
	}
}
