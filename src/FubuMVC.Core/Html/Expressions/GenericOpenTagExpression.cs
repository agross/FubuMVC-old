namespace FubuMVC.Core.Html.Expressions
{
    public class GenericOpenTagExpression : HtmlExpressionBase
    {
        private readonly string _tagName;

        public GenericOpenTagExpression(string tagName)
        {
            _tagName = tagName;
        }

        public override string ToString()
        {
            return @"<{0}{1}>".ToFormat(_tagName, GetHtmlAttributesString());
        }
    }
}