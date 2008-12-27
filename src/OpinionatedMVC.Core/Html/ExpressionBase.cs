using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;

namespace OpinionatedMVC.Core.Html
{
    public abstract class ExpressionBase
    {
        protected string DefaultId { get; set; }

        private Dictionary<string, string> _htmlAttributes = new Dictionary<string, string>();
        private List<string> _cssClasses = new List<string>();
        private Dictionary<string, string> _customStyles = new Dictionary<string, string>();

        public IDictionary<string, string> HtmlAttributes { get { return _htmlAttributes; } }
        public IList<string> CssClasses { get { return _cssClasses; } }
        public IDictionary<string, string> CustomStyles { get { return _customStyles; } }

        protected IDictionary<string, string> GetAllHtmlAttributes()
        {
            var attributes = new Dictionary<string, string>(HtmlAttributes);

            var builder = new StringBuilder();

            addCustomStyles(builder, attributes);
            addCssClasses(builder, attributes);

            if (!attributes.ContainsKey("id") && DefaultId.IsNotEmpty()) attributes.Add("id", DefaultId);

            return attributes;
        }

        private void addCssClasses(StringBuilder builder, IDictionary<string, string> attributes)
        {
            if (CssClasses.Count > 0)
            {
                foreach (string className in CssClasses)
                {
                    builder.AppendFormat("{0}", className);
                }

                attributes.Add("class", string.Join(" ", _cssClasses.ToArray()));

                builder.Length = 0;
            }
        }

		private void addCustomStyles(StringBuilder builder, IDictionary<string, string> attributes)
        {
            if (CustomStyles.Count > 0)
            {
                string separator = "";

                foreach (var stylePair in CustomStyles)
                {
                    builder.AppendFormat("{0}{1}: {2};", separator, stylePair.Key, stylePair.Value);
                    separator = " ";
                }

                attributes.Add("style", builder.ToString());

                builder.Length = 0;
            }
        }

        protected string GetHtmlAttributesString()
        {
            IDictionary<string, string> attributes = GetAllHtmlAttributes();

            if (attributes.Count == 0)
            {
                return "";
            }

            var builder = new StringBuilder();

            foreach (var pair in attributes)
            {
                builder.AppendFormat(CultureInfo.InvariantCulture, " {0}=\"{1}\"",
                                     pair.Key,
                                     HttpUtility.HtmlAttributeEncode(pair.Value));
            }

            return builder.ToString();
        }
    }
}