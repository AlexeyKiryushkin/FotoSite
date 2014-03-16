using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using AmsDispatch.Util;

using FotoSite.Properties;

namespace FotoSite
{
	public class CurrentFolderList
	{
		public List<DirectoryInfo> GetFolders(string currPath)
		{
			Helper.Log.InfoFormat("Получаем список каталогов в {0}", currPath);

			try
			{
				List<DirectoryInfo> dirlist = Directory.GetDirectories(currPath).Select(s => new DirectoryInfo(s)).ToList();

				return dirlist;
			}
			catch (Exception ex)
			{
				Helper.Log.ErrorFormat("{0} при получении списка каталогов в {1}", ex.GetMessages(), currPath);

				return null;
			}
		}
	}
}