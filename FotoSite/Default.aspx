<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FotoSite._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxCtrl" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
		<section class="featured">
        <div class="content-wrapper">
        		<asp:CheckBox ID="ShowExifCheckBox" runat="server" Text="Показывать для каждой фотографии информацию EXIF (замедление!)" AutoPostBack="true" OnCheckedChanged="ShowExifCheckBox_CheckedChanged" />
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

	<div>
		<asp:Label ID="CurrentPathLabel" runat="server" Text="" Visible="false"/>
	</div>
	
	<div>
		<div style="padding: 1px; float:left;">
			<asp:Button ID="OpenUpFolderBtn" runat="server" Text=".." OnClick="OpenFolderBtn_Click"/>
		</div>

		<asp:Repeater ID="FoldersListRepeater" runat="server" DataSourceID="FolderListDataSource" >
      
			<ItemTemplate>
				<div style="padding: 1px; float:left;">
					<asp:Button ID="OpenFolderBtn" runat="server" Text='<%# Eval("Name") %>' OnClick="OpenFolderBtn_Click"/>
				</div>
			</ItemTemplate>

		</asp:Repeater>
	</div>

	<div style="clear:both">
	</div>

	<div>
		<asp:Repeater ID="ImagesListRepeater" runat="server" DataSourceID="ImagesListDataSource" OnItemCreated="ImagesListRepeater_ItemCreated" >
      
			<ItemTemplate>
				<div style="padding: 2px; float:left;">

					<div style="padding: 2px;">
						<a href="SmallImage.axd?name=<%# Eval("RelativeName") %>&h=4000" title="Открыть оригинал..." target="_blank"><img src="SmallImage.axd?name=<%# Eval("RelativeName") %>&h=200" /></a>
					</div>

					<div style="padding: 2px; text-align: center; font-size: large;">

						<div style="float:left; width:10%">
							<asp:CheckBox ID="CheckToDownload" runat="server" />
							<asp:Label ID="FullImageNameLabel" runat="server" Visible="false" Text='<%# Eval("FullImageName") %>' />
						</div>
						<div style="float:left; text-align: center; width:90%">
							<asp:Panel ID="HeaderExifPanel" runat="server" Title="Показать EXIF..." CssClass="collapsePanelHeader">
								<asp:Label ID="ImageNameLabel" runat="server" Text='<%# Eval("ImageName") %>' />
							</asp:Panel>
						</div>
						<div style="clear:both">
						</div>
					</div>

					<div style="padding: 1px; text-align: center; font-size: small;">
	
						<asp:Panel ID="ExpandedExifPanel" runat="server" CssClass="collapsePanel">
							<asp:Label ID="ExifInfoLabel" runat="server" Text='<%# Eval("ExifInfo") %>' />
						</asp:Panel>

					</div>

				</div>

				<ajaxCtrl:CollapsiblePanelExtender ID="CollapsibleExifPanelExtender" runat="server" 
					TargetControlID="ExpandedExifPanel"	ExpandControlID="HeaderExifPanel" CollapseControlID="HeaderExifPanel" 
					Collapsed="False" />

			</ItemTemplate>

			<FooterTemplate>
				<div style="clear:both">
				</div>
				<div style="padding: 1px; float:left;">
					<asp:Button ID="CheckAllImages" runat="server" Text="Отметить все изображения" OnClick="CheckAllImages_Click" />	
				</div>
				<div style="padding: 1px; float:left;">
					<asp:Button ID="DownloadImages" runat="server" Text="Загрузить отмеченные изображения" OnClick="DownloadImages_Click" />	
				</div>
				<div style="clear:both">
				</div>
			</FooterTemplate>
	
		</asp:Repeater>
	</div>

  <asp:ObjectDataSource ID="FolderListDataSource" runat="server" 
    TypeName="FotoSite.CurrentFolderList" 
    SelectMethod="GetFolders"> 
		<SelectParameters>
			<asp:ControlParameter ControlID="CurrentPathLabel" DefaultValue="" Name="currPath" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
		</SelectParameters>
  </asp:ObjectDataSource>

  <asp:ObjectDataSource ID="ImagesListDataSource" runat="server" 
    TypeName="FotoSite.CurrentImageList" 
    SelectMethod="GetImages"> 
		<SelectParameters>
			<asp:ControlParameter ControlID="CurrentPathLabel" DefaultValue="" Name="currPath" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
			<asp:ControlParameter ControlID="FeaturedContent$ShowExifCheckBox" DefaultValue="false" Name="showExif" Type="Boolean" />
		</SelectParameters>
  </asp:ObjectDataSource>

</asp:Content>
