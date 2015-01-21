using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Ionic.Zip;

using AmsDispatch.Util;

using FotoSite.Properties;

namespace FotoSite
{
	public partial class _Default : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Helper.Log.InfoFormat("[{0}] подключение...", Context.Request.UserHostAddress);

				if (Context.Request.UserHostAddress == Settings.Default.ProxyIP)
				{
					Helper.Log.WarnFormat("[{0} ({1})] -> перенаправление на предупреждение о прокси!",
						Context.Request.UserHostAddress, Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);

					Server.Transfer("WarningProxy.aspx");
				}

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

		protected void DownloadImages_Click(object sender, EventArgs e)
		{
			ZipAndSendCheckedImages();
		}

		private void ZipAndSendCheckedImages()
		{
			try
			{
				// сформировать имя zip-файла во временном каталоге сервера, оно должно быть уникально, т.к. могут обратиться несколько клиентов одновременно
				string zipFileNameOnly = Path.ChangeExtension(Path.GetRandomFileName(), ".zip");
				string zipFileFullName = Path.GetTempPath() + zipFileNameOnly;

				Helper.Log.InfoFormat("Архивируем изображения в файл '{0}' ...", zipFileFullName);

				int n = 0;

				// запаковать отмеченные изображения в zip
				using (ZipFile zip = new ZipFile())
				{
					// jpg всё равно не сжимаются, на их надо только в один файл затолкать для скачивания
					zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestSpeed;

					foreach (RepeaterItem ri in ImagesListRepeater.Items)
					{
						CheckBox checked4download = (CheckBox)ri.FindControl("CheckToDownload");
						Label imageFullNameLabel = (Label)ri.FindControl("FullImageNameLabel");

						if (checked4download.Checked)
						{
							zip.AddFile(imageFullNameLabel.Text, "");
							n++;
							Helper.Log.DebugFormat("{0} добавлен в архив", imageFullNameLabel.Text);
						}
					}

					if (n > 0)
					{
						zip.Save(zipFileFullName);
						Helper.Log.DebugFormat("Архив {0} сохранен.", zipFileFullName);
					}
				}

				if( n > 0 )
					SendZipFile(zipFileNameOnly);
				else
					Helper.Log.Warn("Нет отмеченных для загрузки изображений!");
			}
			catch (Exception ex)
			{
				Helper.Log.Debug("Создание архива изображений: ", ex);
				Helper.Log.ErrorFormat("{0} при создании архива изображений", ex.GetMessages());
			}
		}

		private void SendZipFile(string zipFileName)
		{
			string redirurl = string.Format("DownloadZip.axd?fname={0}&prefix=foto", zipFileName);
			//Helper.Log.InfoFormat("Перенаправление на {0} для загрузки zip-файла с изображениями...", redirurl);

			Response.Redirect(redirurl, endResponse: false);
			// Response.Redirect не даёт нормально доработать этой странице, из-за этого не отключается блокировка UI
			// поэтому c блокировкой перенаправление делается клиентским скриптом
			//ClientScript.RegisterStartupScript(GetType(), "downloadzip", string.Format(@"window.open('{0}', '_blank')", redirurl), true);
		}

		protected void ImagesListRepeater_ItemCreated(object sender, RepeaterItemEventArgs e)
		{
			try
			{
				if (e.Item.ItemType == ListItemType.Footer)
				{
					Button downloadImagesBtn = (Button)e.Item.FindControl("DownloadImages");
					Button checkAllImagesBtn = (Button)e.Item.FindControl("CheckAllImages");

					if (ImagesListRepeater.Items.Count == 0)
					{
						downloadImagesBtn.Visible = false;
						checkAllImagesBtn.Visible = false;
					}
				}
			}
			catch (Exception ex)
			{
				Helper.Log.Debug("Видимость кнопок скачивания: ", ex);
				Helper.Log.ErrorFormat("{0} при анализе необходимости видимости кнопок скачивания", ex.GetMessages());
			}
		}

		protected void CheckAllImages_Click(object sender, EventArgs e)
		{
			foreach (RepeaterItem ri in ImagesListRepeater.Items)
			{
				CheckBox checked4download = (CheckBox)ri.FindControl("CheckToDownload");

				checked4download.Checked = true;
			}
		}

	}
}