using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using AmsDispatch.Util;

using FotoSite.Properties;

namespace FotoSite
{
	public partial class _Default : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Context.Request.UserHostAddress == Settings.Default.ProxyIP)
			{
				Helper.Log.WarnFormat("[{0}] -> перенаправление на предупреждение о прокси!", Context.Request.UserHostAddress);
				Server.Transfer("WarningProxy.aspx");
			}

			if (!IsPostBack)
			{
				Helper.Log.InfoFormat("[{0}] подключение", Context.Request.UserHostAddress);
				CurrentPathLabel.Text = Path.GetFullPath(Settings.Default.FotoFolder);

				HttpCookie siteCookie = GetCookies();
				ShowExifCheckBox.Checked = Convert.ToBoolean(siteCookie["ShowExif"]); 
			}
		}

		private HttpCookie GetCookies()
		{
			HttpCookie siteCookie = Request.Cookies["FotoSite_cookies"];

			if (siteCookie == null)
			{
				siteCookie = new HttpCookie("FotoSite_cookies");
				siteCookie["ShowExif"] = Settings.Default.ShowExif.ToString();
				siteCookie.Expires = DateTime.Now.AddMonths(1);
				Response.Cookies.Add(siteCookie);
			}

			return siteCookie;
		}

		protected void OpenFolderBtn_Click(object sender, EventArgs e)
		{
			string nextFolder = ((Button)sender).Text;

			string nextPath = Util.AddBackSlash(Path.GetFullPath(Path.Combine(CurrentPathLabel.Text, nextFolder)));
			string rootPath = Util.AddBackSlash(Path.GetFullPath(Settings.Default.FotoFolder));

			if (nextPath.Length >= rootPath.Length)
			{
				Helper.Log.InfoFormat("[{0}] Переходим к {1} ", Context.Request.UserHostAddress, nextFolder );

				CurrentPathLabel.Text = nextPath;
			}
		}

		protected void ShowExifCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			HttpCookie siteCookie = GetCookies();
			siteCookie["ShowExif"] = ShowExifCheckBox.Checked.ToString();
			siteCookie.Expires = DateTime.Now.AddMonths(1);
			Response.Cookies.Add(siteCookie);
		}
	}
}