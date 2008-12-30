<%@ Import Namespace="FubuMVC.Core"%>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="TagLink" %>
<a href="<%= this.UrlTo().Tag(Model.Tag.Name) %>"><%= Model.Tag.Name %></a>