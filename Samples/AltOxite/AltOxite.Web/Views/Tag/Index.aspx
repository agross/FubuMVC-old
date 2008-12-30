<%@ Page Inherits="TagIndexView" MasterPageFile="~/Views/Shared/Site.Master"%>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="sections">
    <div class="primary">
        <h2 class="title"><%= Model.Tag.Name %></h2>
    </div>
</div>

<%---
    **************************************
    **** ORIGINAL OXITE VERSION BELOW ****
    **************************************
<%
    if (ViewData["Tags"] != null)
    {
        IEnumerable<KeyValuePair<ITag, int>> tagsWithPostCounts = (IEnumerable<KeyValuePair<ITag, int>>)ViewData["Tags"];
        double? averagePostCount = null;
        double? standardDeviationPostCount = null;
        
        Response.Write(
            Html.UnorderedList(
                tagsWithPostCounts.OrderBy(kvp => kvp.Key.Name),
                t => string.Format(
                    "<a href=\"{2}\" rel=\"tag\" class=\"t{3}\">{0} ({1})</a> ",
                    t.Key.Name,
                    t.Value,
                    Url.Tag(t.Key),
                    t.Key.GetTagWeight(tagsWithPostCounts, ref averagePostCount, ref standardDeviationPostCount)
                ),
                "tagCloud"
            )
        );
    } %>
        </div>
        <div class="secondary"><%
    Html.RenderPartial("Search");
    Html.RenderPartial("AdminTasksContainer");
    Html.RenderPartial("Archives", (IEnumerable<KeyValuePair<DateTime, int>>)ViewData["Months"], ViewData); %>
        </div>

---%>
</asp:Content>
