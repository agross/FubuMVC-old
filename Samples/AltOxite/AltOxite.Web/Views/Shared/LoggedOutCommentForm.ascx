<%@ Import Namespace="AltOxite.Core.Web.DisplayModels"%>
<%@ Import Namespace="FubuMVC.Core"%>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="LoggedOutCommentForm" %>
<form>
<%= this.FormFor(this.UrlTo().PublishedPost(Model.Post)).Class("user") %><%-- TODO: Link to comments section --%>
    <fieldset class="info">
        <legend><%= "Your Information" %></legend>
        <div id="comment_grav"><%= this.GetGravatarImage(Model.User) %></div>
        <p class="gravatarhelp"><%= "&lt;-- It's a "%><a href="<%= "http://en.gravatar.com/site/signup" %>"><%= "gravatar"%></a>
        <div class="name">
            <label for="comment_name"><%= "Name" %></label>
            <%= this.TextBoxFor(m => m.User.DisplayName).ElementId("comment_name").Class("text").Attr("tabindex", "1").Attr("title", "Your name...")%><%--<%= Html.ValidationMessage("AnonymousUser.Name", "You must provide a name.") %>--%>
        </div>
        <div class="email">
            <label for="comment_email"><%= "Email" %><span> (saved for notifications but never distributed)<%="<br />- enter if subscribing to this post or changing your gravatar".If(() => Model.User.IsAuthenticated) %></span></label>
            <%= this.TextBoxFor(m => m.User.Email).ElementId("comment_email").Class("text").Attr("tabindex", "2").Attr("title", "Your email...")%><%--<%= Html.ValidationMessage("AnonymousUser.Email", "Your email address must be valid.") %>--%>
        </div>
        <div class="url">
            <label for="comment_url"><%= "URL" %></label>
            <%= this.TextBoxFor(m => m.User.Url).ElementId("comment_url").Class("text").Attr("tabindex", "3").Attr("title", "Your home on the interwebs (URL)...")%><%--<%= Html.ValidationMessage("AnonymousUser.Url", "URL looks a little off. URL encoding stuff like quotes and angle brackets might help.") %>--%>
        </div>
        <div class="remember">
            <%= this.CheckBoxFor(m => m.User.Remember).ElementId("comment_remember").Attr("tabindex", "5")%>
            <label for="comment_remember"><%= "Remember your info?" %></label>
        </div>
        <div class="subscribe">
            <%= this.CheckBoxFor(m => m.UserSubscribed).ElementId("comment_usersubscribed").Attr("tabindex", "6")%>
            <label for="comment_subscribe"><%= "Subscribe?" %></label>
        </div>
        <div class="submit">
            <%= this.SubmitButton("Submit Comment", "comment_submit").ElementId("comment_submit").Class("submit").Class("button").Attr("tabindex", "7")%>
        </div>
    </fieldset>
    <fieldset class="comment">
        <legend><%= "your comment" %></legend>
        <label for="comment_body"><%= "Leave a comment..." %></label><%--<%= Html.ValidationMessage("Comment.Body") %>--%>
        <%= this.TextBoxFor(m => m.Body).ElementId("comment_body").MultilineMode().Attr("cols", "60").Attr("rows", "12").Attr("tabindex", "4").Attr("title", "Leave a comment...")%>
        <%-- TODO: Implement this
        <%= Html.AntiForgeryToken(ViewData["AntiForgeryToken"] as string) %>
        <%= Html.AntiForgeryTicks(ViewData["AntiForgeryTicks"] as string)%>--%>
    </fieldset>
    <%--<% RenderStringResources(); %>--%>
    <script type="text/javascript">        window.stringResources = { "comment_body.Leave a comment...": "Leave a comment..." };</script>
</form>
<%--    <form method="post" id="comment" action="<%=Url.Post((IPost)ViewData["Post"]) %>#comment">
        <fieldset class="info">
            <legend><%= Localize("Your Information") %></legend>
            <div id="comment_grav"><%= Html.Gravatar((commentAnonymous != null ? commentAnonymous.HashedEmail ?? "@" : "@"), (commentAnonymous != null ? commentAnonymous.Name ?? "Mr. Gravatar" : "Mrs. Gravatar"), "48", Config.Site.GravatarDefault) %></div>
            <p class="gravatarhelp"><%= string.Format(Localize("&lt;-- It's a {0}"), Html.Link(Localize("gravatar"), Localize("http://en.gravatar.com/site/signup"))) %></p>
            <div class="name">
                <label for="comment_name"><%= Localize("Name") %></label>
                <%= Html.TextBox("name", (!string.IsNullOrEmpty(Request.Form["name"]) ? Request["name"] : (commentAnonymous != null ? commentAnonymous.Name : "")), new { id = "comment_name", @class = "text", tabindex = "1", title = Localize("Your name...", "comment_name", true) }) %><%= Html.ValidationMessage("AnonymousUser.Name", "You must provide a name.") %>
            </div>
            <div class="email">
                <label for="comment_email"><%= Localize("Email") %><span> (saved for notifications but never distributed)<% if (commentAnonymous != null && !string.IsNullOrEmpty(commentAnonymous.HashedEmail))
                                                                                                           { %><br />- enter if subscribing to this post or changing your gravatar<% } %></span></label>
                <%= Html.Hidden("hashedEmail", (!string.IsNullOrEmpty(Request.Form["hashedEmail"]) ? Request["hashedEmail"] : (commentAnonymous != null ? commentAnonymous.HashedEmail : "")), new { id = "comment_hashedEmail" }) %>
                <%= Html.TextBox("email", Request["email"], new { id = "comment_email", @class = "text", tabindex = "2", title = Localize("Your email...", "comment_email", true) }) %><%= Html.ValidationMessage("AnonymousUser.Email", "Your email address must be valid.") %>
            </div>
            <div class="url">
                <label for="comment_url"><%= Localize("URL") %></label>
                <%= Html.TextBox("url", (!string.IsNullOrEmpty(Request.Form["url"]) ? Request["url"] : (commentAnonymous != null ? commentAnonymous.Url : "")), new { id = "comment_url", @class = "text", tabindex = "3", title = Localize("Your home on the interwebs (URL)...", "comment_url", true) }) %><%= Html.ValidationMessage("AnonymousUser.Url", "URL looks a little off. URL encoding stuff like quotes and angle brackets might help.") %>
            </div>
            <div class="remember">
                <%= Html.CheckBox("remember", Request.Form.IsTrue("remember") || (commentAnonymous != null && !string.IsNullOrEmpty(commentAnonymous.HashedEmail)), new { id = "comment_remember", tabindex = "5" }) %>
                <label for="comment_remember"><%= Localize("Remember your info?") %></label>
            </div>
            <div class="subscribe">
                <%= Html.CheckBox("subscribe", Request.Form.IsTrue("subscribe"), new { id = "comment_subscribe", tabindex = "6" }) %>
                <label for="comment_subscribe"><%= Localize("Subscribe?") %></label>
            </div>
            <div class="submit">
                <input type="submit" value="<%= Localize("Submit Comment") %>" id="comment_submit" class="submit button" tabindex="7" />
            </div>
        </fieldset>
        <fieldset class="comment">
            <legend><%= Localize("your comment") %></legend>
            <label for="comment_body"><%= Localize("Leave a comment...") %></label><%= Html.ValidationMessage("Comment.Body") %>
            <%= Html.TextArea("body", ViewContext.HttpContext.Request.Form["body"] ?? "", 12, 60, new { id = "comment_body", tabindex = "4", title = Localize("Leave a comment...", "comment_body", true) })%>
            <%= Html.AntiForgeryToken(ViewData["AntiForgeryToken"] as string) %>
            <%= Html.AntiForgeryTicks(ViewData["AntiForgeryTicks"] as string)%>
        </fieldset>
        <% RenderStringResources(); %>
    </form>--%>