<%@ Page Inherits="HomeIndexView" MasterPageFile="~/Views/Shared/Site.Master"%>
<%@ Import Namespace="AltOxite.Core.Domain"%>
<%@ Import Namespace="System.Collections.Generic"%>
<%@ Import Namespace="AltOxite.Core.Web.Html"%>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
<h3>Home/Index.aspx</h3>
<%
    // TODO: replace this with this.Model.Posts
    var posts = new List<Post>
    {
        new Post {Title = "Post one", Class = "first", Published = DateTime.Now.AddDays(-10), BodyShort = "First short body"},
        new Post {Title = "Post two", Class = "", Published = DateTime.Now.AddDays(-5), BodyShort = "Second short body"},
        new Post {Title = "Post three", Class = "", Published = DateTime.Now.AddDays(-1), BodyShort = "Third short body"},
        new Post {Title = "Post four", Class = "last", Published = DateTime.Now, BodyShort = "Fourth short body"}
    };
%>
<div class="sections">
    <div class="primary">
        <ul class="posts">
            <%= this.DisplayBlogPost().ForEach(posts).Display<BlogPost>() %>
        </ul>
    </div>
</div>

<%---
    IPageOfAList<IPost> posts = ((IPageOfAList<IPost>)ViewData["Posts"]);
    Dictionary<Guid, int> postCounts = (Dictionary<Guid, int>)ViewData["PostCounts"]; %>
            <div class="sections">
                <div class="primary">
                    <ul class="posts"><%
    foreach (IPost post in posts)
    {
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
                        </li><%
    } %>
                    </ul><%
    if (posts.TotalPageCount > 1)
    { %>
                    <div class="pager">
                        <%= Html.SimplePager(posts, this, "PageOfAHome", new { })%>
                    </div><%
    } %>
                </div>
                <div class="secondary"><% 
    Html.RenderPartial("Search");
    Html.RenderPartial("AdminTasksContainer");
    Html.RenderPartial("Archives", (IEnumerable<KeyValuePair<DateTime, int>>)ViewData["Months"], ViewData); %>
                </div>
            </div>
            ---%>
</asp:Content>
