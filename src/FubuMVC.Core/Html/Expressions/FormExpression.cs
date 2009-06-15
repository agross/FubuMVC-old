
namespace FubuMVC.Core.Html.Expressions
{
    public class FormExpression : HtmlExpressionBase
    {
        private readonly string _actionUrl;
        private bool _isGet = false;

        public FormExpression(string actionUrl)
        {
            _actionUrl = actionUrl;
        }

        public FormExpression AsGet()
        {
            _isGet = true;
            return this;
        }

        public override string ToString()
        {
            var html = "<form action=\"{0}\" method=\"{2}\" {1}>".ToFormat(_actionUrl, GetHtmlAttributesString(), _isGet ? "get" : "post");
            return html;
        }
    }
}