<%@ Import Namespace="FubuMVC.Core"%>
<%@ Import Namespace="AltOxite.Core.Web.Html"%>
<%@ Import Namespace="System.Linq"%>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="FullBlogPost" %>
<div class="avatar"><%= this.GetGravatarImage(Model.Post.User, Model.SiteConfig.GravatarDefault)%></div>
<h2 class="title"><%= Model.Post.Title %></h2>
<div class="metadata">
    <div class="posted"><%= Model.LocalPublishedDate %></div>
    <%= this.DisplayTagList().ForEach(Model.Post.Tags).Display<TagLink>().AsUnorderedList()%>
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
