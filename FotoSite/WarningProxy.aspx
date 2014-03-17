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
	<div class="Warning">
		Чтобы не израсходовать весь свой интернет-трафик добавьте IP-адрес этого сайта -  
		<asp:Label ID="ServerIpLabel" runat="server" /> - в список исключений для Proxy в настройках вашего браузера!
	</div>
</asp:Content>
