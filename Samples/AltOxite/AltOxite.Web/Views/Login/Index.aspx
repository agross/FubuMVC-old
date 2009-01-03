<%@ Page Language="C#" Inherits="LoginIndexView"  MasterPageFile="~/Views/Shared/Site.Master"%>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <%= this.FormFor<LoginController>(l=>l.Index(null)).Class("login") %>
        <h2 class="title"><%= Resources.Strings.LOGIN %></h2>
        <div><%= Resources.Strings.ENTER_USERNAME_AND_PWD %></div>
        <%-- //TODO: = Html.ValidationSummary() --%>
        <div class="username">
            <label for="login_username"><%= Resources.Strings.USERNAME %></label>
            <%= this.TextBoxFor(m => m.Username).ElementId("login_username").Class("text").Attr("tabindex", "1").Attr("title", Resources.Strings.USERNAME_TITLE)%>
            <%-- //TODO: = Html.ValidationMessage("username") --%>
        </div>
        <div class="password">
            <label for="login_password"><%= Resources.Strings.PASSWORD %></label>
            <%= this.TextBoxFor(m => m.Password).PasswordMode().ElementId("login_password").Class("text").Attr("tabindex", "2").Attr("title", Resources.Strings.PASSWORD_TITLE)%>
            <%-- //TODO: = Html.ValidationMessage("password") --%>
        </div>
        <div class="remember">
            <%= this.CheckBoxFor(m => m.RememberMeChecked).ElementId("login_remember").Attr("tabindex", "3") %>
            <label for="login_remember"><%= Resources.Strings.REMEMBER_ME %></label>
        </div>
        <div class="submit">
            <input type="submit" value="<%= Resources.Strings.LOGIN %>" id="login_submit" class="submit button" tabindex="4" />
        </div>
        <%= this.HiddenFor(m => m.ReturnUrl) %>
    </form>
</asp:Content>
