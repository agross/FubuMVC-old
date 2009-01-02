<%@ Import Namespace="FubuMVC.Core"%>
<%@ Import Namespace="AltOxite.Core.Web.Html"%>
<%@ Import Namespace="System.Linq"%>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="BlogPost" %>
<li<%= this.GetCssForLiTag(Model.CurrentPostOnPage, Model.TotalPostsOnPage) %>>
    <h2 class="title"><a href="<%= this.UrlTo().Post(Model.Post) %>"><%= Model.Post.Title %></a></h2>
    <div class="posted"><%= Model.LocalPublishedDate %></div>
    <div class="content"><%= Model.Post.BodyShort%></div> 
    <div class="more"><%-- 
TODO: implement this
if ((int)ViewData["AreaCount"] > 1)
    Response.Write(string.Format(Localize("From the {0} {1}."), string.Format("<a href=\"{1}\">{0}</a>", post.Area.Name, Url.Area(post.Area)), post.Area.Type));
    --%><%= this.DisplayTagList().ForEach(Model.Post.Tags).Display<TagLink>().AsUnorderedList().WithTrailingPipe() %><%= this.GetCommentsLink(Model.Post) %>  <a href="<%= this.UrlTo().Post(Model.Post) %>" class="arrow">&raquo;</a></div>
</li>