using System;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using AmsDispatch.Util;

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
			string fullPath = HttpContext.Current.Server.MapPath(imagename);

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
								g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
								g.DrawImage(bigBmp, 0, 0, smallBmp.Width, smallBmp.Height);

								// В формате PNG (и еще куче других, кроме GIF и JPG) нельзя выводить
								// непосредственно в context.Response.OutputStream, т.к. по нему
								// можно идти только в одну сторону. Надо использовать
								// промежуточный MemoryStream!

								// Безобразие должно быть единообразным!
								using (MemoryStream ms = new MemoryStream())
								{
									smallBmp.Save(ms, ImageFormat.Jpeg);
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
				Helper.Log.ErrorFormat("{0} - Ошибка при выводе картинки '{1}' высотой {2}px", ex.GetMessages(), imagename, toHeight);
				Helper.Log.Debug(string.Format("Ошибка при выводе картинки '{0}' высотой {1}px: ", imagename, toHeight), ex);
			}
		}

		#endregion
	}
}
