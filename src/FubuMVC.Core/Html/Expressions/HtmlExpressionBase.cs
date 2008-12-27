using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace FubuMVC.Core.Html.Expressions
{
    public abstract class HtmlExpressionBase
    {
        public IDictionary<string, string> HtmlAttributes { get; set; }
        public IList<string> CssClasses { get; set; }
        public IDictionary<string, string> CustomStyles { get; set; }

        protected HtmlExpressionBase()
        {
            HtmlAttributes = new Dictionary<string, string>();
            CssClasses = new List<string>();
            CustomStyles = new Dictionary<string, string>();
        }

        protected IDictionary<string, string> GetAllHtmlAttributes()
        {
            var attributes = new Dictionary<string, string>(HtmlAttributes);

            var builder = new StringBuilder();

            addCustomStyles(builder, attributes);
            addCssClasses(builder, attributes);

            return attributes;
        }

        private void addCssClasses(StringBuilder builder, IDictionary<string, string> attributes)
        {
            if (CssClasses.Count > 0)
            {
                CssClasses.Each(name=> builder.AppendFormat("{0}", name));

                attributes.Add("class", string.Join(" ", CssClasses.ToArray()));

                builder.Length = 0;
            }
        }

        private void addCustomStyles(StringBuilder builder, IDictionary<string, string> attributes)
        {
            if (CustomStyles.Count > 0)
            {
                var separator = "";

                CustomStyles.Each(style =>
                    {
                        builder.AppendFormat("{0}{1}: {2};", separator, style.Key, style.Value);
                        separator = " ";
                    });

                attributes.Add("style", builder.ToString());

                builder.Length = 0;
            }
        }

        protected string GetHtmlAttributesString()
        {
            var attributes = GetAllHtmlAttributes();

            if (attributes.Count == 0)
            {
                return "";
            }

            var builder = new StringBuilder();

            attributes.Each(pair => builder.AppendFormat(
                                        CultureInfo.InvariantCulture, " {0}=\"{1}\"",
                                        pair.Key,
                                        HttpUtility.HtmlAttributeEncode(pair.Value)));

            return builder.ToString();
        }
    }
}