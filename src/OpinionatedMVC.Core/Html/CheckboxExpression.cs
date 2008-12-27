using System;
using System.Linq.Expressions;

namespace OpinionatedMVC.Core.Html
{
    public class CheckboxExpression<VIEWMODEL> : BoundExpression<VIEWMODEL, CheckboxExpression<VIEWMODEL>> where VIEWMODEL : class
    {
        private readonly bool _isChecked;
        private string _text;
        private bool _labelBeforeCheckbox;

        public CheckboxExpression(VIEWMODEL viewModel, Expression<Func<VIEWMODEL, object>> expression, string prefix)
            : base(viewModel, expression, prefix)
        {
            _isChecked = (bool) rawValue;
        }

		public CheckboxExpression<VIEWMODEL> WithLabel(string text)
		{
		    _text = text;
			return this;
		}

        public CheckboxExpression<VIEWMODEL> LabelBeforeCheckbox()
        {
            _labelBeforeCheckbox = true;
            return this;
        }

        public override string ToString()
        {
            const string template = "<input type=\"checkbox\" value=\"{0}\" name=\"{0}\"{1}{2}/>";
        	string checkedText = _isChecked ? " checked=\"checked\"" : "";

        	string tagText = string.Format(template, name, checkedText, GetHtmlAttributesString());

			if (_text != null)
			{
			    var tagFormat = "<label>{0}{1}</label>";

                return _labelBeforeCheckbox
                    ? string.Format(tagFormat, _text, tagText)
                    : string.Format(tagFormat, tagText, _text);
			}

        	return tagText;
        }

        protected override CheckboxExpression<VIEWMODEL> thisInstance()
        {
            return this;
        }
    }
}