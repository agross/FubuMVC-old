<%@ Page Inherits="BlogPostIndexView" MasterPageFile="~/Views/Shared/Site.Master"%>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="avatar"><%= this.GetGravatarImage(Model.Post.User)%></div>
<h2 class="title"><%= Model.Post.Title %></h2>
<div class="metadata">
    <div class="posted"><%= Model.Post.LocalPublishedDate %></div>
    <%= this.Localize("Filed under ") %><%= this.RenderPartial().Using<TagLink>().ForEachOf(Model.Post.Tags) %>
    <div class="content"><%= Model.Post.Body %></div>
</div><%---
    **************************************
    **** ORIGINAL OXITE VERSION BELOW ****
    **************************************
    
<div class="post"><% 
   Html.RenderPartial("ManagePost", post, ViewData); 
%>    <div class="avatar"><%= Html.Gravatar(creator, "48", Config.Site.GravatarDefault) %></div>
    <h2 class="title"><%= post.Title %></h2>
    <div class="metadata">
        <div class="posted"><%if (post.Published.HasValue)
                              { %><%=ConvertToLocalTime(post.Published.Value).ToLongDateString()%><%}
                              else
                              { %><%= Localize("Draft")%><% } %></div><%
		                                                                                          
        IEnumerable<ITag> tags = post.Tags;
        if (tags.Count() > 0)
        {
            Response.Write(
                string.Format(
                    " {0} {1}",
                    Localize("Filed under"),
                    Html.UnorderedList(
                        tags,
                        (t, i) =>
                            string.Format(
                                "<a href=\"{1}\" rel=\"tag\">{0}</a>",
                                Html.Encode(t.Name),
                                Url.Tag(t)
                            ),
                        "tags"
                    )
                )
            );
        }
                                                                                                  
%>    </div>
    <div class="content"><%=post.Body %></div>
</div>
            ---%>
            </asp:Content>