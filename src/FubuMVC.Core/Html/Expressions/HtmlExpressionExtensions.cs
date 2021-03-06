using System.Reflection;

namespace FubuMVC.Core.Html.Expressions
{
    public static class HtmlExpressionExtensions
    {
        public static EXPRESSION Attributes<EXPRESSION>(this EXPRESSION expression, object attributes)
            where EXPRESSION : HtmlExpressionBase
        {
            foreach (PropertyInfo prop in attributes.GetType().GetProperties())
            {
                expression.HtmlAttributes.Add(prop.Name, prop.GetValue(attributes, null).ToString());
            }

            return expression;
        }

        public static EXPRESSION Attr<EXPRESSION>(this EXPRESSION expression, string name, string value)
            where EXPRESSION : HtmlExpressionBase
        {
            expression.HtmlAttributes.Add(name, value);
            return expression;
        }

        public static EXPRESSION ReadOnly<EXPRESSION>(this EXPRESSION expression) where EXPRESSION : HtmlExpressionBase
        {
            expression.HtmlAttributes.Add("disabled", "disabled");
            return expression;
        }

        public static EXPRESSION Required<EXPRESSION>(this EXPRESSION expression) where EXPRESSION : HtmlExpressionBase
        {
            expression.CssClasses.Add("x-required");
            return expression;
        }

        public static EXPRESSION Class<EXPRESSION>(this EXPRESSION expression, string className)
            where EXPRESSION : HtmlExpressionBase
        {
            expression.CssClasses.Add(className);
            return expression;
        }

        public static EXPRESSION ElementId<EXPRESSION>(this EXPRESSION expression, string id)
            where EXPRESSION : HtmlExpressionBase
        {
            expression.HtmlAttributes.Add("id", id);
            return expression;
        }

        public static EXPRESSION Width<EXPRESSION>(this EXPRESSION expression, int pixels)
            where EXPRESSION : HtmlExpressionBase
        {
            expression.CustomStyles.Add("width", pixels + "px");
            return expression;
        }

        public static EXPRESSION WidthPercent<EXPRESSION>(this EXPRESSION expression, int percent)
            where EXPRESSION : HtmlExpressionBase
        {
            expression.CustomStyles.Add("width", percent + "%");
            return expression;
        }

        public static EXPRESSION Height<EXPRESSION>(this EXPRESSION expression, int pixels)
            where EXPRESSION : HtmlExpressionBase
        {
            expression.CustomStyles.Add("height", pixels + "px");
            return expression;
        }

        public static EXPRESSION Style<EXPRESSION>(this EXPRESSION expression, string name, string value)
            where EXPRESSION : HtmlExpressionBase
        {
            expression.CustomStyles.Add(name, value);
            return expression;
        }

        public static string WithLabel<EXPRESSION>(this EXPRESSION expression, string label)
        {
            return string.Format("<label>{0}:</label>{1}", label, expression);
        }

        public static EXPRESSION VisibilityFrom<EXPRESSION>(this EXPRESSION expression, bool visibility)
            where EXPRESSION : HtmlExpressionBase
        {
            if (!visibility)
            {
                expression.CustomStyles.Add("display", "none");
            }

            return expression;
        }
    }
}