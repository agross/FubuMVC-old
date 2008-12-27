using System;
using System.Linq.Expressions;
using OpinionatedMVC.Core.Util;

namespace OpinionatedMVC.Core.Html
{
    public class TextBoxExpression<VIEWMODEL> : BoundExpression<VIEWMODEL, TextBoxExpression<VIEWMODEL>>
        where VIEWMODEL : class
    {
        private readonly Accessor accessor;
        private bool _dayHourMin;
        private bool _multiLine;
        private bool _password;

        public TextBoxExpression(string prefix, VIEWMODEL viewModel, Expression<Func<VIEWMODEL, object>> expression)
            : base(viewModel, expression, prefix)
        {
            accessor = ReflectionHelper.GetAccessor(expression);

            //TODO: This kind of stuff should be convention based
            if (accessor.PropertyType == typeof (DateTime) || accessor.PropertyType == typeof (DateTime?))
            {
                CssClasses.Add("DatePicker");
            }
        }

        public TextBoxExpression<VIEWMODEL> MultilineMode()
        {
            _multiLine = true;
            return this;
        }

        public TextBoxExpression<VIEWMODEL> PasswordMode()
        {
            _password = true;
            return this;
        }

        public TextBoxExpression<VIEWMODEL> DayHourMinuteMask()
        {
            _dayHourMin = true;
            return this;
        }

        public override string ToString()
        {
            if (_dayHourMin)
            {
                return DayHourMinuteToString();
            }
            string htmlFormat = _multiLine
                                    ? "<textarea name=\"{1}\"{3} >{2}</textarea>"
                                    : "<input type=\"{0}\" name=\"{1}\" value=\"{2}\"{3} />";

            string inputType = _password ? "password" : "text";
            object textBoxValue = getValue();

            return string.Format(htmlFormat, inputType, name, textBoxValue, GetHtmlAttributesString());
        }

        //TODO: This sort of thing should be a convention
        private string DayHourMinuteToString()
        {
            object textBoxValue = getValue();
            string masked = "<input type=\"text\" id=\"{0}MaskedInput\" value=\"{1}\"{2} />";
            masked += "<input type=\"hidden\" name=\"{0}\" id=\"{0}\">";
            masked +=
                " <script type=\"text/javascript\">$(document).ready(function(){{$(\"#{0}MaskedInput\").daysHoursMinutes(\"#{0}\");}});</script>";
            return string.Format(masked, name, textBoxValue, GetHtmlAttributesString());
        }

        protected override TextBoxExpression<VIEWMODEL> thisInstance()
        {
            return this;
        }
    }
}