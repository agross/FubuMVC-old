<%@ Import Namespace="AltOxite.Core.Web.DisplayModels"%>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="LoggedInCommentForm" %>
<form>
<%= this.FormFor(this.UrlTo().PublishedPost(Model.Post)).Class("user")%><%-- TODO: Link to comments section --%>
    <div><%= this.GetGravatarImage(Model.User) %></div>
    <fieldset class="comment">
        <legend>your comment</legend>
        <div>
            <label for="comment_body"><%= "Leave a comment..." %></label><%--<%= Html.ValidationMessage("Comment.Body") %>--%>
            <%= this.TextBoxFor(m => m.Body).ElementId("comment_body").MultilineMode().Attr("cols", "60").Attr("rows", "12").Attr("tabindex", "1").Attr("title", "Leave a comment...")%>
        </div>
        <div class="subscribe">
            <%= this.CheckBoxFor(m => m.UserSubscribed).ElementId("comment_usersubscribed").Attr("tabindex", "2")%>
            <label for="comment_subscribe"><%= "Subscribe?" %></label>
        </div>
        <div class="submit">
            <%= this.SubmitButton("Submit Comment", "comment_submit").ElementId("comment_submit").Class("submit").Class("button").Attr("tabindex", "3")%>
            <%-- TODO: Implement this
            <%= Html.AntiForgeryToken(ViewData["AntiForgeryToken"] as string) %>
            <%= Html.AntiForgeryTicks(ViewData["AntiForgeryTicks"] as string)%>--%>
        </div>
    </fieldset>
    <%--<% RenderStringResources(); %>--%>
    <script type="text/javascript">window.stringResources = { "comment_body.Leave a comment...": "Leave a comment..." };</script>
</form>
