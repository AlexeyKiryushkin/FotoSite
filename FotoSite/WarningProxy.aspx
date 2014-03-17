<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WarningProxy.aspx.cs" Inherits="FotoSite.WarningProxy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
		<section class="featured">
        <div class="content-wrapper">
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div style="height: 100px">
	</div>
	<div class="Warning">
		Чтобы не израсходовать весь свой интернет-трафик<br />
			добавьте IP-адрес этого сайта - <br />
			<asp:Label ID="ServerIpLabel" runat="server" /><br />
			- в список исключений для Proxy в настройках вашего браузера!
	</div>
	<div style="height: 200px">
	</div>
</asp:Content>
