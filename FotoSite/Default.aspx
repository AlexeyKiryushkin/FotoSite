<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FotoSite._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
		<section class="featured">
        <div class="content-wrapper">
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
					<asp:Button ID="OpenFolderBtn" runat="server" Text='<%# Bind("Name") %>' OnClick="OpenFolderBtn_Click"/>
				</div>
			</ItemTemplate>

		</asp:Repeater>
	</div>

	<div style="clear:both">
	</div>

	<div>
		<asp:Repeater ID="ImagesListRepeater" runat="server" DataSourceID="ImagesListDataSource" >
      
			<ItemTemplate>
				<div style="padding: 1px; float:left;">
					<a href="<%# Container.DataItem %>"><img src="<%# Container.DataItem %>" height="200" /></a>
				</div>
			</ItemTemplate>

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
		</SelectParameters>
  </asp:ObjectDataSource>

</asp:Content>
