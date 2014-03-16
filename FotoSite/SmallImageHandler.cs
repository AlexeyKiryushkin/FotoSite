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

			Helper.Log.InfoFormat("Запрос на фото '{0}' высотой {1}px, файл '{2}'", imagename, toHeight, fullPath);

			try
			{
				using (Bitmap bmp = new Bitmap(fullPath))
				{
					context.Response.ContentType = "image/jpeg";
					// В формате PNG (и еще куче других, кроме GIF и JPG) нельзя выводить
					// непосредственно в context.Response.OutputStream, т.к. по нему
					// можно идти только в одну сторону. Надо использовать
					// промежуточный MemoryStream!
					using (MemoryStream ms = new MemoryStream())
					{
						bmp.Save(ms, ImageFormat.Jpeg);
						ms.WriteTo(context.Response.OutputStream);
					}
				}
			}
			catch (Exception ex)
			{
				Helper.Log.ErrorFormat("{0} - Ошибка при выводе картинки '{1}' высотой {2}px", ex.GetMessages(), imagename, toHeight);
				Helper.Log.Debug(string.Format("Ошибка при выводе картинки '{0}' высотой {2}px: ", imagename, toHeight), ex);
			}
		}

		#endregion
	}
}
