using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltOxite.Core.Domain;
using AltOxite.Core.Web.Controllers;
using AltOxite.Core.Web.WebForms;
using FubuMVC.Core;
using FubuMVC.Core.Html.Expressions;
using FubuMVC.Core.View.WebForms;

namespace AltOxite.Core.Web.Html
{
    public class TagLinkListExpression
    {
        private readonly IAltOxitePage _viewPage;
        private readonly IWebFormsViewRenderer _renderer;

        private IEnumerable<Tag> _tags;

        public IRenderPartialForScope RenderExpression { get; private set; }

        public TagLinkListExpression(IAltOxitePage viewPage, IWebFormsViewRenderer renderer)
        {
            _viewPage = viewPage;
            _renderer = renderer;
        }

        public TagLinkListExpression ForEach(IEnumerable<Tag> tags)
        {
            _tags = tags;
            return this;
        }

        public TagLinkListExpression Display<USERCONTROL>()
            where USERCONTROL : AltOxiteUserControl<TagLinkViewModel>
        {
            RenderExpression = new RenderPartialExpression(_viewPage, _renderer).Using<USERCONTROL>();
            return this;
        }

        public override string ToString()
        {
            if (_tags.Count() == 0)
                return "";

            var output = new StringBuilder();
            _tags.Each(tag => output.Append("{0} ".ToFormat(RenderExpression.For(new TagLinkViewModel {Tag = tag}))));
            return LocalizationManager.GetTextForKey("Filed under {0} | ").ToFormat(output.ToString().Trim());
        }
    }
}