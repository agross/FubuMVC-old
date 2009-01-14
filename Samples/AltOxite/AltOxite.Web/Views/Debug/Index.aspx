<%@ Page Inherits="DebugIndexView" MasterPageFile="~/Views/Shared/Site.Master"%>
<%@ Import Namespace="FubuMVC.Core"%>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="sections">
    <div class="primary">
        <ul class="posts">
        <li>
        <h2 class="title"><a>Conventions</a></h2>
        <div class="posted">&nbsp;</div>
        <div class="content">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr><td style="width: 200px;">ViewFileBasePath:</td><td><b><%= Model.ViewFileBasePath %></b></td></tr>
                <tr><td>LayoutViewFileBasePath:</td><td><b><%= Model.LayoutViewFileBasePath %></b></td></tr>
                <tr><td>SharedViewFileBasePath:</td><td><b><%= Model.SharedViewFileBasePath %></b></td></tr>
                <tr><td colspan="2" style="height: 20px;"></td></tr>
            </table>
        </div> 
        </li>
        </ul>
    
         <%= this.RenderPartial().Using<ControllerAction>().ForEachOf(Model.Controllers) %>
    </div>
</div>

</asp:Content>
