using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
		}

		public string RelativeName {get;set;}

		public string ImageName {get;set;}
	}

	public class CurrentImageList
	{
		public List<ImageInfo> GetImages(string currPath)
		{
			Helper.Log.InfoFormat("Получаем список фоток в {0}", currPath);

			try
			{
				Uri rootUri = new Uri(Settings.Default.FotoFolder, UriKind.Absolute);

				List<ImageInfo> imagelist = Directory.GetFiles(currPath, "*.jpg")
					.Select(s => new ImageInfo(rootUri, s)).ToList();

				return imagelist;
			}
			catch (Exception ex)
			{
				Helper.Log.ErrorFormat("{0} при получении списка фоток в {1}", ex.GetMessages(), currPath);

				return new List<ImageInfo>();
			}
		}
	}
}