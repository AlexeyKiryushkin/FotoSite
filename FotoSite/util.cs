using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace FotoSite
{
	public static class Util
	{
		public static string AddBackSlash(string foldername)
		{
			return foldername.EndsWith(Path.DirectorySeparatorChar.ToString()) || foldername.EndsWith(Path.AltDirectorySeparatorChar.ToString()) ?
					foldername : foldername + Path.DirectorySeparatorChar;
		}
	}
}