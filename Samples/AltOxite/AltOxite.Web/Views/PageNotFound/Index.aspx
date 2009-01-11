<%@ Page Inherits="PageNotFoundIndexView" MasterPageFile="~/Views/Shared/Site.Master"%>
<%@ Import Namespace="FubuMVC.Core"%>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <p><%= "The url you requested could not be found." %></p>
    <%= "<p>{0}</p>".ToFormat(Model.Description).If(() => Model.ShowDescription) %>
</asp:Content>
