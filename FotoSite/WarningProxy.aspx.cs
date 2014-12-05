using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FotoSite
{
	public partial class WarningProxy : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			// Надо показать, какой ip добавлять в список исключений для прокси.
			// Но, ip адресов у сервера несколько, поэтому, если обращение было
			// по IP - показываем его, а если по имени, то показываем имя.
			ServerIpLabel.Text = Request.Url.DnsSafeHost;
		}
	}
}