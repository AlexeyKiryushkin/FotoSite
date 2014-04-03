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

			Helper.Log.Debug(ImageName);

			//var exiftool = new Process();
			//exiftool.StartInfo.FileName = Settings.Default.ExifToolCmd;
			//exiftool.StartInfo.Arguments = Settings.Default.ExifToolArgs + " \"" + fullpath + "\"";
			//exiftool.StartInfo.UseShellExecute = false;
			//exiftool.StartInfo.RedirectStandardOutput = true;
			//exiftool.StartInfo.RedirectStandardError = true;
			//exiftool.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(1251);
			//exiftool.StartInfo.StandardErrorEncoding = Encoding.GetEncoding(1251);
			//exiftool.OutputDataReceived += exiftool_OutputDataReceived;
			//exiftool.ErrorDataReceived += exiftool_ErrorDataReceived;
			//exiftool.Start();

			//// 1. нельзя синхронно перехватывать оба потока - и output и error
			//// 2. Если процесс не завершается, то все встанет на синхронном перехвате
			////    не дойдя до WaitForExit(), поэтому - оба асинхронно
			//exiftool.BeginErrorReadLine();
			//exiftool.BeginOutputReadLine();

			//if (!exiftool.WaitForExit(Settings.Default.ExifToolTimeoutMilliSec))
			//{
			//	exiftool.Kill();
			//	Helper.Log.ErrorFormat("Превышен таймаут ожидания работы ExifTool - {0} сек!", Settings.Default.ExifToolTimeoutMilliSec);
			//}
		}

		public string RelativeName { get; set; }
		public string ImageName { get; set; }
		public string ExifInfo { get; set; }

		void exiftool_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Data))
			{
				Helper.Log.Error(e.Data);
			}
		}

		void exiftool_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Data))
				ExifInfo = e.Data;
		}
	}

	public class CurrentImageList
	{
		public List<ImageInfo> GetImages(string currPath)
		{
			Helper.Log.InfoFormat("Получаем список фоток в {0}", currPath);

			List<ImageInfo> imagelist = new List<ImageInfo>();

			// Uri считает каталогом только то, что заканчивается слэшем!
			Uri rootUri = new Uri(Util.AddBackSlash(Settings.Default.FotoFolder), UriKind.Absolute);

			string[] imagefiles = Directory.GetFiles(currPath, "*.jpg");

			var tasks = imagefiles.Select<string, Task>(async s => 
			{
				try
				{
					ImageInfo imginfo = await Task.Run(() => new ImageInfo(rootUri, s));

					lock (imagelist)
						imagelist.Add(imginfo);
				}
				catch (Exception ex)
				{
					Helper.Log.Error("Получение информации о {0}: ", ex);
				}
			}).ToArray();

			Task.WaitAll(tasks);

			return imagelist;
		}
	}
}