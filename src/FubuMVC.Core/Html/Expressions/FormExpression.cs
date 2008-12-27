
namespace FubuMVC.Core.Html.Expressions
{
    public class FormExpression : HtmlExpressionBase
    {
        private readonly string _actionUrl;

        public FormExpression(string actionUrl)
        {
            _actionUrl = actionUrl;
        }

        public override string ToString()
        {
            var html = "<form action=\"{0}\" method=\"post\" {1}>".ToFormat(_actionUrl, GetHtmlAttributesString());
            return html;
        }
    }
}