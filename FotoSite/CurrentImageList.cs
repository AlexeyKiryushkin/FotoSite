using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using AmsDispatch.Util;

using FotoSite.Properties;

namespace FotoSite
{
	public class CurrentImageList
	{
		public List<FileInfo> GetImages(string currPath)
		{
			Helper.Log.InfoFormat("Получаем список фоток в {0}", currPath);

			try
			{
				List<FileInfo> imagelist = Directory.GetFiles(currPath, "*.jpg").Select(s => new FileInfo(s)).ToList();

				return imagelist;
			}
			catch (Exception ex)
			{
				Helper.Log.ErrorFormat("{0} при получении списка фоток в {1}", ex.GetMessages(), currPath);

				return null;
			}
		}
	}
}