using System.Linq;
using AltOxite.Core.Domain;
using AltOxite.Core.Web.WebForms;
using FubuMVC.Core.Html.Expressions;
using FubuMVC.Core.View;
using FubuMVC.Core.Html;
using FubuMVC.Core.View.WebForms;

namespace AltOxite.Core.Web.Html
{
    public class LoginStatusExpression
    {
        private readonly IAltOxitePage _viewPage;
        private readonly IWebFormsViewRenderer _renderer;

        private bool _loggedIn;

        public LoginStatusExpression(IAltOxitePage viewPage, IWebFormsViewRenderer renderer)
        {
            _viewPage = viewPage;
            _renderer = renderer;
        }

        public IRenderPartialForScope RenderExpression { get; private set; }

        public LoginStatusExpression For(User loggedInUser)
        {
            _loggedIn = loggedInUser != null;
            return this;
        }

        public LoginStatusExpression WhenLoggedInShow<USERCONTROL>()
            where USERCONTROL : AltOxiteUserControl<ViewModel>
        {
            RenderExpression = _loggedIn
                                   ? new RenderPartialExpression(_viewPage, _renderer).Using<USERCONTROL>()
                                   : RenderExpression;

            return this;
        }

        public LoginStatusExpression WhenLoggedOutShow<USERCONTROL>()
            where USERCONTROL : AltOxiteUserControl<ViewModel>
        {
            RenderExpression = ! _loggedIn
                       ? new RenderPartialExpression(_viewPage, _renderer).Using<USERCONTROL>()
                       : RenderExpression;
            return this;
        }

        public override string ToString()
        {
            return RenderExpression.For(_viewPage.Model);
        }
    }
}