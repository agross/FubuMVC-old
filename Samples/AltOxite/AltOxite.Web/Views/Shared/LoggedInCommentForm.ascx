<%@ Import Namespace="AltOxite.Core.Web.DisplayModels"%>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="LoggedInCommentForm" %>
<form>
<%--<%= this.FormFor<BlogPostController>(l => l.Index(new BlogPostSetupViewModel { PostDay = 5, PostMonth = 12, PostYear = 2008, Slug = "World_Hello" })).Class("user")%>--%> <%-- TODO: Link to comments section --%>
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
    <script type="text/javascript">window.stringResources = { "comment_body.Leave a comment...": "Leave a comment..." };</script>
</form>
<%--    <form method="post" id="comment" action="<%=Url.Post((IPost)ViewData["Post"]) %>#comment" class="user">
        <div><%= Html.Gravatar(user, "48", Config.Site.GravatarDefault) %></div>
        <fieldset class="comment">
            <legend>your comment</legend>
            <div>
                <label for="comment_body"><%= Localize("Leave a comment...") %></label><%= Html.ValidationMessage("Comment.Body") %>
                <%= Html.TextArea("body", ViewContext.HttpContext.Request.Form["body"] ?? "", 12, 60, new { id = "comment_body", @class = "authed", tabindex = "4", title = Localize("Leave a comment...", "comment_body", true) }) %>
            </div>
            <div class="subscribe">
                <%= Html.CheckBox("subscribe", Request.Form.IsTrue("subscribe"), new { id = "comment_subscribe", tabindex = "6" }) %>
                <label for="comment_subscribe">Subscribe?</label>
            </div>
            <div class="submit">
                <input type="submit" value="<%= Localize("Submit Comment") %>" id="comment_submit" class="submit button" tabindex="7" />
                <%= Html.AntiForgeryToken(ViewData["AntiForgeryToken"] as string) %>
                <%= Html.AntiForgeryTicks(ViewData["AntiForgeryTicks"] as string)%>
            </div>
        </fieldset>
        <% RenderStringResources(); %>
    </form>--%>