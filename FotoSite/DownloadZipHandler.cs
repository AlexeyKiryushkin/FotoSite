using System;
using System.Web;
using System.IO;

using AmsDispatch.Util;

namespace FotoSite
{
	/// <summary>
	/// Обработчик для загрузки zip файлов на клиента с удалением их с сервера
	/// </summary>
	public class DownloadZipHandler : IHttpHandler
	{
		#region IHttpHandler Members

		/// <summary>
		/// Можно ли повторно использовать созданный экземпляр обработчика IHttpHandler
		/// </summary>
		public bool IsReusable
		{
			// Return false in case your Managed Handler cannot be reused for another request.
			// Usually this would be false in case you have some state information preserved per request.
			get { return true; }
		}

		/// <summary>
		/// Обработчик НТТР-запроса, собственно и осуществляющий вывод zip
		/// </summary>
		/// <param name="context">Объект HttpContext, предоставляющий ссылки на внутренние серверные объекты 
		/// (например, Request, Response, Session и Server), используемые для обслуживания HTTP-запросов. </param>
		/// <remarks>
		/// Ожидается наличие параметров в запросе:
		/// fname - имя файла в TEMP-каталоге
		/// logpage - страничка с протоколом для переадресовки при ошибках
		/// prefix - первая часть имени файла для клиента
		/// </remarks>
		public void ProcessRequest(HttpContext context)
		{
			string prefix = context.Request.Params["prefix"] ?? "";
			string fname = CleanFileName(context, context.Request.Params["fname"]);

			Helper.Log.DebugFormat("ProcessRequest( prefix='{0}', fname='{1}' )", prefix, fname);

			if (!string.IsNullOrEmpty(fname))
			{
				SendResultFile(context, new FileInfo(fname), prefix);
			}
		}

		#endregion

		private string CleanFileName(HttpContext context, string fname)
		{
			if (!string.IsNullOrEmpty(fname))
			{
				// немного предосторожностей - убираем возможность выйти за пределы TEMP
				var before = fname.Length;
				fname = fname.Replace("/", "").Replace("\\", "").Replace(":", "");
				var after = fname.Length;
				if (after != before)
				{
					Helper.Log.WarnFormat("{0} - кулхацкер детектед! Загрузка файла отменяется.", context.Request.UserHostAddress);
				}
				else
					return Path.GetTempPath() + fname;
			}
			else
			{
				Helper.Log.ErrorFormat("Неправильный url '{0}' для загрузки zip-файла от {1}", context.Request.RawUrl, context.Request.UserHostAddress);
			}

			return "";
		}

		private void SendResultFile(HttpContext context, FileInfo zipfile, string prefix)
		{
			Helper.Log.DebugFormat("SendResultFile( {0}  ", zipfile.FullName);

			try
			{
				// имя zip-файла для клиента
				string clientfname = string.Format("{0}_{1}.zip", prefix, DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss"));
				Helper.Log.InfoFormat(new TriplexFormatProvider(), "Загрузка файла {0}, размер {1:Tx} байт как {2} ...",
					zipfile.FullName, zipfile.Length, clientfname);

				context.Response.Clear();

				// предложить сохранить байты как файл на диске клиента
				//clientfname = Server.UrlEncode(clientfname).Replace(@"+", @"%20");
				context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", clientfname));
				context.Response.ContentType = "application/x-zip-compressed";
				context.Response.AddHeader("Accept-Ranges", "bytes");
				context.Response.AddHeader("Content-Length", zipfile.Length.ToString());

				// обязательно из памяти, т.к. файл сразу же удалим
				context.Response.WriteFile(zipfile.FullName, readIntoMemory: true);

				context.Response.Flush();

				zipfile.Delete();

				Helper.Log.WarnFormat("Отправка файла {0} для {1} успешно завершена!", zipfile.FullName, context.Request.UserHostAddress);
			}
			catch (Exception ex)
			{
				zipfile.Delete();

				Helper.Log.Debug(ex);
				Helper.Log.ErrorFormat("{0} - Ошибка при отправке файла '{1}'", ex.GetMessages(), zipfile.FullName);
				Helper.Log.ErrorFormat("Процесс загрузки файла завершился неудачно для {0}", context.Request.UserHostAddress);
			}
		}
	}
}
