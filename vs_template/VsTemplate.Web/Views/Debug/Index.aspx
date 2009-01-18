<%@ Page Inherits="DebugIndexView" MasterPageFile="~/Views/Shared/Site.Master"%>
<%@ Import Namespace="FubuMVC.Core"%>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

<ul>
<li>
    <h2><a>Conventions</a></h2>
    <table cellpadding="0" cellspacing="0" border="0">
        <tr><td style="width: 200px;">ViewFileBasePath:</td><td><b><%= Model.ViewFileBasePath %></b></td></tr>
        <tr><td>LayoutViewFileBasePath:</td><td><b><%= Model.LayoutViewFileBasePath %></b></td></tr>
        <tr><td>SharedViewFileBasePath:</td><td><b><%= Model.SharedViewFileBasePath %></b></td></tr>
        <tr><td colspan="2" style="height: 20px;"></td></tr>
    </table>
</li>
</ul>
<%= this.RenderPartial().Using<ControllerAction>().ForEachOf(Model.Controllers) %>

</asp:Content>
