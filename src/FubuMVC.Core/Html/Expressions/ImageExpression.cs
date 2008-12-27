namespace FubuMVC.Core.Html.Expressions
{
    public class ImageExpression : HtmlExpressionBase
    {
        private readonly string _imageSrcUrl;
        private string _baseUrl;

        public ImageExpression(string imageSrcUrl)
        {
            _imageSrcUrl = imageSrcUrl;
            _baseUrl = "~/content/images/";
        }

        public override string ToString()
        {
            var fullUrl = UrlContext.Combine(_baseUrl, _imageSrcUrl).ToFullUrl();
            var html = @"<img src=""{0}""{1}/>".ToFormat(fullUrl, GetHtmlAttributesString());
            return html;
        }

        public ImageExpression BasedAt(string baseUrl)
        {
            _baseUrl = baseUrl;
            return this;
        }

        public ImageExpression Alt(string altText)
        {
            this.Attr("alt", altText);
            return this;
        }
    }
}
