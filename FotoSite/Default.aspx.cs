using System;
using System.Collections.Generic;
using System.Linq;
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
			if (!IsPostBack)
			{
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
				Helper.Log.InfoFormat("Перезагружаем список каталогов для {0}", CurrentPathLabel.Text);

				FoldersListRepeater.DataBind();
			}
			catch (Exception ex)
			{
				Helper.Log.ErrorFormat("{0} при получении списка каталогов в {1}", ex.GetMessages(), CurrentPathLabel.Text);
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
				Helper.Log.InfoFormat("Переходим к {0}", nextFolder);

				CurrentPathLabel.Text = Path.GetFullPath(CurrentPathLabel.Text + nextFolder);

				FillForCurrentFolder();
			}
		}
	}
}