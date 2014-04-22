using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Text;

using FotoSite.Properties;

namespace FotoSite
{
	public partial class SiteMaster : MasterPage
	{
		private const string AntiXsrfTokenKey = "__AntiXsrfToken";
		private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
		private string _antiXsrfTokenValue;

		protected void Page_Init(object sender, EventArgs e)
		{
			// The code below helps to protect against XSRF attacks
			var requestCookie = Request.Cookies[AntiXsrfTokenKey];
			Guid requestCookieGuidValue;
			if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
			{
				// Use the Anti-XSRF token from the cookie
				_antiXsrfTokenValue = requestCookie.Value;
				Page.ViewStateUserKey = _antiXsrfTokenValue;
			}
			else
			{
				// Generate a new Anti-XSRF token and save to the cookie
				_antiXsrfTokenValue = Guid.NewGuid().ToString("N");
				Page.ViewStateUserKey = _antiXsrfTokenValue;

				var responseCookie = new HttpCookie(AntiXsrfTokenKey)
				{
					HttpOnly = true,
					Value = _antiXsrfTokenValue
				};
				if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
				{
					responseCookie.Secure = true;
				}
				Response.Cookies.Set(responseCookie);
			}

			Page.PreLoad += master_Page_PreLoad;
		}

		protected void master_Page_PreLoad(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				// Set Anti-XSRF token
				ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
				ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
			}
			else
			{
				// Validate the Anti-XSRF token
				if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
						|| (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
				{
					throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
				}
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
				GetExifToolVersion();
		}

		private void GetExifToolVersion()
		{
			var exiftool = new Process();
			exiftool.StartInfo.FileName = Settings.Default.ExifToolCmd;
			exiftool.StartInfo.Arguments = "-ver";
			exiftool.StartInfo.UseShellExecute = false;	 // Значение свойства UseShellExecute должно быть равным false, если свойству RedirectStandardOutput нужно присвоить значение true
			exiftool.StartInfo.RedirectStandardOutput = true;
			exiftool.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(1251);
			exiftool.OutputDataReceived += exiftool_OutputDataReceived;
			exiftool.Start();

			exiftool.BeginOutputReadLine();

			if (!exiftool.WaitForExit(Settings.Default.ExifToolTimeoutMilliSec))
			{
				Util.KillProcessAndChildren(exiftool.Id);
			}
		}

		string _exifToolVersion = "";

		void exiftool_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Data))
			{
				_exifToolVersion = e.Data;
			}
		}

		/// <summary>
		/// Информация о версии ПО
		/// </summary>
		public string SiteVersion
		{
			get
			{
				return string.Format("v.{0} от {1}",
					Assembly.GetExecutingAssembly().GetName().Version, new FileInfo(Assembly.GetExecutingAssembly().CodeBase.Substring(8)).LastWriteTime);
			}
		}

		/// <summary>
		/// Информация о версии ExifTool
		/// </summary>
		public string ExifToolVersion
		{
			get
			{
				return string.Format("v.{0}",_exifToolVersion);
			}
		}
	}
}