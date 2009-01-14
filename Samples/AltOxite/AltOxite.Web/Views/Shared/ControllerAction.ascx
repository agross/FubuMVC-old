<%@ Import Namespace="FubuMVC.Core"%>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="ControllerAction" %>
<h2 class="title"><a><%= Model.PrimaryUrl %></a></h2>
<div class="posted">&nbsp;<%= Model.ActionFunc %></div>
<div class="content">
    <table cellpadding="0" cellspacing="0" border="0">
        <tr><td style="width: 200px;">Controller name:</td><td><b><%= Model.ControllerType %></b></td></tr>
        <tr><td>Method signature:</td><td><b><%= Model.MethodSignature %></b></td></tr>
        <tr><td colspan="2" style="height: 5px;"></td></tr>
        <tr><td colspan="2" valign="top">Behaviors:<br /><%= this.RenderPartial().Using<DebugSingleLine>().WithDefault("No behaviors").ForEachOf(Model.Behaviors) %></td></tr>
        <tr><td colspan="2" style="height: 5px;"></td></tr>
        <tr><td colspan="2" valign="top">Other Url's:<br /><%= this.RenderPartial().Using<DebugSingleLine>().WithDefault("No other url's").ForEachOf(Model.OtherUrls) %></td></tr>
        <tr><td colspan="2" style="height: 20px;"></td></tr>
    </table>
</div> 

