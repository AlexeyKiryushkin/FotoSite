using System;
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
			if (!IsPostBack)
			{
				Helper.Log.InfoFormat("Подключение {0}", Context.Request.UserHostAddress);
				CurrentPathLabel.Text = Path.GetFullPath(Settings.Default.FotoFolder);
			}
		}

		void FillForCurrentFolder()
		{
			FillFoldersList();

			FillImagesList();
		}

		void FillFoldersList()
		{
			try
			{
				Helper.Log.InfoFormat("Перезагружаем список каталогов '{0}' [{1}]", CurrentPathLabel.Text, Context.Request.UserHostAddress);

				FoldersListRepeater.DataBind();
			}
			catch (Exception ex)
			{
				Helper.Log.ErrorFormat("{0} при получении списка каталогов '{1}' [{2}]", ex.GetMessages(), CurrentPathLabel.Text, Context.Request.UserHostAddress);
			}
		}

		void FillImagesList()
		{
		}

		protected void OpenFolderBtn_Click(object sender, EventArgs e)
		{
			string nextFolder = ((Button)sender).Text + "\\";

			if (Path.GetFullPath(CurrentPathLabel.Text + nextFolder).Length >= Path.GetFullPath(Settings.Default.FotoFolder).Length)
			{
				Helper.Log.InfoFormat("Переходим к {0} [{1}]", nextFolder, Context.Request.UserHostAddress);

				CurrentPathLabel.Text = Path.GetFullPath(CurrentPathLabel.Text + nextFolder);

				FillForCurrentFolder();
			}
		}
	}
}