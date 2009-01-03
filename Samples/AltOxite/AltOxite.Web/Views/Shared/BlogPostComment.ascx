<%@ Control Language="C#" AutoEventWireup="true" Inherits="BlogPostComment" %>
<%--        if (userCanEdit)
        { %>
    <div class="flags">
        <form class="flag remove" method="post" action="<%= Url.RouteUrl("AdminRemoveComment") %>">
            <input type="image" class="button remove" src="<%=ViewData["SkinPath"] %>/images/delete.png" title="<%= Localize("Remove") %>" />
            <input type="hidden" name="commentId" value="<%= comment.ID %>" />
            <input type="hidden" name="returnUri" value="<%= Request.Url.AbsoluteUri %>" />
        </form><%
            if (comment.State == (byte)EntityState.PendingApproval)
            { %>
        <form class="flag approve" method="post" action="<%= Url.RouteUrl("AdminApproveComment") %>">
            <input type="image" class="button approve" src="<%=ViewData["SkinPath"] %>/images/accept.png" title="<%= Localize("Approve") %>" />
            <input type="hidden" name="commentId" value="<%= comment.ID %>" />
            <input type="hidden" name="returnUri" value="<%= Request.Url.AbsoluteUri %>" />
        </form><%
            } %>
    </div>
    <div class="flagged"><%
        } %>
--%>
<%= this.GetCommentPremalinkBookmark(Model) %>
<div class="name">
    <div><%= this.GetCommenterGravatarAndLink(Model) %></div>
    <p class="comment"><strong><%= this.GetCommenterNameAndLink(Model) %></strong><span> <%= Resources.Strings.SAID %><br /> <%= this.GetCommentPremalink(Model) %></span></p>
</div>
<div class="text">
    <p><%= Model.Body %></p>
</div>

<%--<%
        if (userCanEdit)
        { %>
    </div><%
        }  
    } %>--%>