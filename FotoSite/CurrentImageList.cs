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
		public List<string> GetImages(string currPath)
		{
			Helper.Log.InfoFormat("Получаем список фоток в {0}", currPath);

			try
			{
				Uri siteUri = new Uri(HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"], UriKind.Absolute);

				List<string> imagelist = Directory.GetFiles(currPath, "*.jpg")
//					.Select(s => HttpUtility.UrlEncode(siteUri.MakeRelativeUri(new Uri(s, UriKind.Absolute)).ToString())).ToList();
//					.Select(s => "~/" + siteUri.MakeRelativeUri(new Uri(s, UriKind.Absolute)).ToString()).ToList();
					.Select(s => siteUri.MakeRelativeUri(new Uri(s, UriKind.Absolute)).ToString()).ToList();

				return imagelist;
			}
			catch (Exception ex)
			{
				Helper.Log.ErrorFormat("{0} при получении списка фоток в {1}", ex.GetMessages(), currPath);

				return new List<string>();
			}
		}
	}
}