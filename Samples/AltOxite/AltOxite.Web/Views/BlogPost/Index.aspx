<%@ Page Inherits="BlogPostIndexView" MasterPageFile="~/Views/Shared/Site.Master"%>
<%@ Import Namespace="AltOxite.Core.Web.DisplayModels"%>
<%@ Import Namespace="FubuMVC.Core"%>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="post">
    <div class="avatar"><%= this.GetGravatarImage(Model.Post.User)%></div>
    <h2 class="title"><%= Model.Post.Title %></h2>
    <div class="metadata">
        <div class="posted"><%= Model.Post.LocalPublishedDate %></div>
        <%= this.Localize("Filed under ") %><%= this.RenderPartial().Using<TagLink>().ForEachOf(Model.Post.Tags) %>
    </div>
    <div class="content"><%= Model.Post.Body %></div>
    <div class="comments">
	    <div class="status">
	        <div><a name="comments"></a></div>
		    <h3><%= this.GetCommentsText(Model.Post) %></h3>
		    <div><a href="<%= this.UrlTo().PublishedPost(Model.Post) %>#comment">leave your own</a></div>
	    </div>    
        <%= this.RenderPartial().Using<BlogPostComment>().WithDefault("<h3>{0}</h3>".ToFormat(this.Localize("No comments here."))).ForEachOf(Model.Post.Comments) %>
<%--        <div class="pager"><%= Html.SimplePager<IComment>(comments, this, "PageOfAnAdminComments", new { }) %></div>--%>
        <%= this.DisplayDependingOnLoginStatus().For(Model.CurrentUser).UseModel(new CommentFormDisplay(Model.CurrentUser)).WhenLoggedInShow<LoggedInCommentForm>().WhenLoggedOutShow<LoggedOutCommentForm>()%>
    </div>
</div>
</asp:Content>