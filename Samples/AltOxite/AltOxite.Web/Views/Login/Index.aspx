<%@ Page Language="C#" Inherits="LoginIndexView"  MasterPageFile="~/Views/Shared/Site.Master"%>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <%= this.FormFor<LoginController>(l=>l.Index(null)).Class("login") %>
        <h2 class="title"><%= this.Localize("Login") %></h2>
        <div><%= this.Localize("Please enter your username and password below.")%></div>        
        <%-- //TODO: = Html.ValidationSummary() --%>
        <div class="username">
            <label for="login_username"><%= this.Localize("Username") %></label>
            <%= this.TextBoxFor(m => m.Username).ElementId("login_username").Class("text").Attr("tabindex", "1").Attr("title", this.Localize("Your username..."))%>
            <%-- //TODO: = Html.ValidationMessage("username") --%>
        </div>
        <div class="password">
            <label for="login_password"><%= this.Localize("Password")%></label>
            <%= this.TextBoxFor(m => m.Password).PasswordMode().ElementId("login_password").Class("text").Attr("tabindex", "2").Attr("title", this.Localize("Your password..."))%>
            <%-- //TODO: = Html.ValidationMessage("password") --%>
        </div>
        <div class="remember">
            <%= this.CheckBoxFor(m => m.RememberMeChecked).ElementId("login_remember").Attr("tabindex", "3") %>
            <label for="login_remember"><%= this.Localize("Remember me?")%></label>
        </div>
        <div class="submit">
            <input type="submit" value="<%= this.Localize("Login") %>" id="login_submit" class="submit button" tabindex="4" />
        </div>
        <%= this.HiddenFor(m => m.ReturnUrl) %>
    </form>
</asp:Content>
