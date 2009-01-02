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
        private bool _asUnorderedList;
        private string _trailingPipe;

        public IRenderPartialForScope RenderExpression { get; private set; }

        public TagLinkListExpression(IAltOxitePage viewPage, IWebFormsViewRenderer renderer)
        {
            _viewPage = viewPage;
            _renderer = renderer;
            _asUnorderedList = false;
            _trailingPipe = "";
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

        public TagLinkListExpression AsUnorderedList()
        {
            _asUnorderedList = true;
            return this;
        }

        public TagLinkListExpression WithTrailingPipe()
        {
            _trailingPipe = " | ";
            return this;
        }

        public override string ToString()
        {
            if (_tags.Count() == 0)
                return "";

            var output = new StringBuilder();
            if (_asUnorderedList)
            {
                var tagCounter = 1;
                _tags.Each(tag =>
                {
                    var css =
                        (((tagCounter == 1) ? "first " : "") +
                        ((tagCounter == _tags.Count()) ? "last" : "")).Trim();
                    output.Append(
                        "<li{1}>{0}</li>".ToFormat(
                        RenderExpression.For(new TagLinkViewModel {Tag = tag}),
                        (css == "") ? "" : " class=\"{0}\"".ToFormat(css)));
                });
                return LocalizationManager.GetTextForKey("Filed under <ul class=\"tags\">{0}</ul>{1}").ToFormat(output.ToString().Trim(), _trailingPipe);
            }
            _tags.Each(tag => output.Append(RenderExpression.For(new TagLinkViewModel { Tag = tag })));
            return LocalizationManager.GetTextForKey("Filed under {0}{1}").ToFormat(output.ToString().Trim(), _trailingPipe);
        }
    }
}