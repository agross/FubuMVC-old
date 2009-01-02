<%@ Control Language="C#" AutoEventWireup="true" Inherits="TagLink" %>
<a href="<%= this.UrlTo().Tag(Model.Name) %>"><%= Model.Name %></a>