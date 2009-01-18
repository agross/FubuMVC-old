<%@ Page Inherits="HomeIndexView" MasterPageFile="~/Views/Shared/Site.Master"%>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>View</h2>
    /VsTemplate/Views/Home/Index.aspx<br />
    <a href="<%= this.UrlTo().Debug() %>">Debug view</a>
</asp:Content>
