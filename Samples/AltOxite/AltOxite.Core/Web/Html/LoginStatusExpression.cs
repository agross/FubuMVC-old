using System.Web.UI;
using AltOxite.Core.Domain;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Html.Expressions;
using FubuMVC.Core.View.WebForms;

namespace AltOxite.Core.Web.Html
{
    public class LoginStatusExpression
    {
        private readonly IAltOxitePage _viewPage;
        private readonly IWebFormsViewRenderer _renderer;
        private readonly FubuConventions _conventions;

        private bool _loggedIn;
        private object _model;

        public LoginStatusExpression(IAltOxitePage viewPage, IWebFormsViewRenderer renderer, FubuConventions conventions)
        {
            _viewPage = viewPage;
            _renderer = renderer;
            _conventions = conventions;
        }

        public IRenderPartialForScope RenderExpression { get; private set; }

        public LoginStatusExpression For(User loggedInUser)
        {
            _loggedIn = (loggedInUser != null && loggedInUser.IsAuthenticated);
            return this;
        }

        public LoginStatusExpression UseModel(object model)
        {
            _model = model;
            return this;
        }

        public LoginStatusExpression WhenLoggedInShow<USERCONTROL>()
            where USERCONTROL : UserControl, IAltOxitePage
        {
            RenderExpression = _loggedIn
                                   ? new RenderPartialExpression(_viewPage, _renderer, _conventions).Using<USERCONTROL>()
                                   : RenderExpression;

            return this;
        }

        public LoginStatusExpression WhenLoggedOutShow<USERCONTROL>()
            where USERCONTROL : UserControl, IAltOxitePage
        {
            RenderExpression = ! _loggedIn
                ? new RenderPartialExpression(_viewPage, _renderer, _conventions).Using<USERCONTROL>()
                : RenderExpression;
            return this;
        }

        public override string ToString()
        {
            if (_model != null)
                return RenderExpression.For(_model);

            return RenderExpression.For(_viewPage.Model);
        }
    }
}