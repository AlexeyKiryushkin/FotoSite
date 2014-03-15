<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FotoSite._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
		<section class="featured">
        <div class="content-wrapper">
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
	<asp:Label ID="CurrentFolderLabel" runat="server" Text=""></asp:Label>

  <div style="margin-top: 5px;">
    <asp:GridView ID="FoldersGrid" runat="server" AutoGenerateColumns="False" CellPadding="4"
      GridLines="None" DataSourceID="FolderListDataSource" ShowHeader="False"
      SelectedIndex="-1">
      <RowStyle CssClass="GridViewRows" />
      <FooterStyle CssClass="GridViewHeader" />
      <PagerStyle CssClass="GridViewHeader" />
      <SelectedRowStyle CssClass="GridViewSelectedRow" />
      <HeaderStyle CssClass="GridViewHeader" />
      <EditRowStyle CssClass="GridViewEditedRow" />
      <AlternatingRowStyle CssClass="GridViewAlterRows" />
      <Columns>
        <asp:BoundField DataField="Name" ReadOnly="True" SortExpression="Name" />
      </Columns>

      <EmptyDataTemplate>
        <div>
          <asp:Label ID="EmptyDataLabel" runat="server" Text="Отсутствуют данные для отображения" CssClass="Labels" />
        </div>
      </EmptyDataTemplate>

    </asp:GridView>

  </div>

  <asp:ObjectDataSource ID="FolderListDataSource" runat="server" 
    TypeName="FotoSite.CurrentFolderList" 
    SelectMethod="GetFolders" 
    SelectCountMethod="GetFoldersCount">
		<SelectParameters>
			<asp:ControlParameter ControlID="CurrentFolderLabel" DefaultValue="" Name="currFolder" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
		</SelectParameters>
  </asp:ObjectDataSource>

</asp:Content>
