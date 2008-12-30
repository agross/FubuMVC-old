<%@ Import Namespace="FubuMVC.Core"%>
<%@ Import Namespace="AltOxite.Core.Web.Html"%>
<%@ Import Namespace="System.Linq"%>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="BlogPost" %>
<% var commentCount = Model.Post.Comments == null ? 0 : Model.Post.Comments.Count(); %>   
<li<%= this.GetCssForLiTag(Model.CurrentPostOnPage, Model.TotalPostsOnPage) %>>
    <h2 class="title"><a href="<%= this.UrlTo().Post(Model.Post) %>"><%= Model.Post.Title %></a></h2>
    <div class="posted"><%= Model.LocalPublishedDate %></div>
    <div class="content"><%= Model.Post.BodyShort%></div> 
    <div class="more"><%= this.DisplayTagList().ForEach(Model.Post.Tags).Display<TagLink>() %><%= this.GetCommentsLink(Model.Post) %>  <a href="<%= this.UrlTo().Post(Model.Post) %>" class="arrow">&raquo;</a></div>
</li><%--
TODO: implement this
        IArea area = post.Area;
        string className = string.Empty;
        if (post.Equals(posts.First())) className += "first ";
        if (post.Equals(posts.Last())) className += "last "; %>
                        <li<%= !string.IsNullOrEmpty(className) ? string.Format(" class=\"{0}\"", className.Trim()) : string.Empty %>>
                            <h2 class="title"><a href="<%=Url.Post(post) %>"><%= post.Title %></a></h2>
                            <div class="posted"><%=ConvertToLocalTime(post.Published.Value).ToLongDateString() %></div>
                            <div class="content"><%=post.GetBodyShort() %></div>                            
                            <div class="more"><%
        if ((int)ViewData["AreaCount"] > 1)
            Response.Write(string.Format(Localize("From the {0} {1}."), string.Format("<a href=\"{1}\">{0}</a>", post.Area.Name, Url.Area(post.Area)), post.Area.Type));
        
        int commentCount = postCounts[post.ID];
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
        %> | <a href="<%=Url.Post(post) %>#comments"><%=commentCount %> comment<%=commentCount == 1 ? "" : "s"%></a> <a href="<%=Url.Post(post) %>" class="arrow">&raquo;</a></div>
                        </li>--%>