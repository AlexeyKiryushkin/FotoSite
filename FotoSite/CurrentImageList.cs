using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

using AmsDispatch.Util;

using FotoSite.Properties;

namespace FotoSite
{
	public class ImageInfo
	{
		public ImageInfo(Uri rooturi, string fullpath)
		{
			RelativeName = rooturi.MakeRelativeUri(new Uri(fullpath, UriKind.Absolute)).ToString();
			ImageName = Path.GetFileNameWithoutExtension(fullpath);

			//GetExifInfo(fullpath);

			//Helper.Log.DebugFormat("Готово. {0}: {1}", ImageName, ExifInfo);
			//Конструктор завершается раньше, чем заполняется ExifInfo в exiftool_OutputDataReceived
		}

		public void GetExifInfo(string fullpath)
		{
			var exiftool = new Process();
			exiftool.StartInfo.FileName = Settings.Default.ExifToolCmd;
			exiftool.StartInfo.Arguments = Settings.Default.ExifToolArgs + " \"" + fullpath + "\"";
			exiftool.StartInfo.UseShellExecute = false;
			exiftool.StartInfo.RedirectStandardOutput = true;
			exiftool.StartInfo.RedirectStandardError = true;
			exiftool.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(1251);
			exiftool.StartInfo.StandardErrorEncoding = Encoding.GetEncoding(1251);
			exiftool.OutputDataReceived += exiftool_OutputDataReceived;
			exiftool.ErrorDataReceived += exiftool_ErrorDataReceived;
			exiftool.Start();

			// 1. нельзя синхронно перехватывать оба потока - и output и error
			// 2. Если процесс не завершается, то все встанет на синхронном перехвате
			//    не дойдя до WaitForExit(), поэтому - оба асинхронно
			exiftool.BeginErrorReadLine();
			exiftool.BeginOutputReadLine();

			if (!exiftool.WaitForExit(Settings.Default.ExifToolTimeoutMilliSec))
			{
				//exiftool.Kill();
				//Просто Kill() не подходит, exiftool порождает дочерние exiftool и они висят после убийства родителя :(
				//Надо убивать родителя и всех детей
				Helper.Log.ErrorFormat("{0} : Превышен таймаут ожидания работы ExifTool - {1} сек! Принудительно завершаем процесс {2}",
					ImageName, Settings.Default.ExifToolTimeoutMilliSec, exiftool.Id);
				Util.KillProcessAndChildren(exiftool.Id);
			}
		}

		public string RelativeName { get; set; }
		public string ImageName { get; set; }
		public string ExifInfo { get; set; }

		void exiftool_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Data))
			{
				Helper.Log.ErrorFormat("{0}: Ошибка ExifTool: {1}", ImageName, e.Data);
			}
		}

		void exiftool_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Data))
			{
				ExifInfo = e.Data;

				//Helper.Log.DebugFormat("{0} EXIF: {1}", ImageName, ExifInfo);
			}
		}
	}

	public class CurrentImageList
	{
		public List<ImageInfo> GetImages(string currPath, bool showExif)
		{
			Helper.Log.InfoFormat("Получаем список фоток в {0}, showExif={1}", currPath, showExif);

			List<ImageInfo> imagelist = new List<ImageInfo>();

			// Uri считает каталогом только то, что заканчивается слэшем!
			Uri rootUri = new Uri(Util.AddBackSlash(Settings.Default.FotoFolder), UriKind.Absolute);

			var tasks = Directory.GetFiles(currPath, "*.jpg").Select<string, Task>(async s => 
			{
				try
				{
					ImageInfo imginfo = await GetImageInfoAsync(rootUri, s, showExif).ConfigureAwait(false); 

					lock (imagelist)
						imagelist.Add(imginfo);
				}
				catch (Exception ex)
				{
					Helper.Log.ErrorFormat("Получение информации о '{0}': {1}", s, ex.GetMessages());
				}
			}).ToArray();

			Task.WaitAll(tasks);

			Helper.Log.DebugFormat("Закончено, всего {0}", imagelist.Count);

			return imagelist;
		}

		private ImageInfo GetImageInfo(Uri rootUri, string fullpath, bool showExif)
		{
			ImageInfo imginfo = new ImageInfo(rootUri, fullpath);

			if( showExif )
				imginfo.GetExifInfo(fullpath);

			return imginfo;
		}

		private async Task<ImageInfo> GetImageInfoAsync(Uri rootUri, string fullpath, bool showExif)
		{
			return await Task.Factory.StartNew(() => GetImageInfo(rootUri, fullpath, showExif)).ConfigureAwait(false);
		}
	}
}