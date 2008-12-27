using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OpinionatedMVC.Core.Html
{
    public class DropDownListExpression<VIEWMODEL> : BoundExpression<VIEWMODEL, DropDownListExpression<VIEWMODEL>> where VIEWMODEL : class
    {
        private readonly VIEWMODEL _viewModel;
        private readonly DropdownWriter _writer;


        public DropDownListExpression(VIEWMODEL viewModel, Expression<Func<VIEWMODEL, object>> expression, string prefix)
            : base(viewModel, expression, prefix)
        {
            _viewModel = viewModel;
            var dropdownValue = Convert.ToString(rawValue);

            _writer = new DropdownWriter(name, dropdownValue);
        }

        public DropDownListExpression<VIEWMODEL> FillWith<LISTITEM>(
            Func<VIEWMODEL, IEnumerable<LISTITEM>> func, Func<LISTITEM, string> displaySelector, Func<LISTITEM, string> valueSelector)
        {
            IEnumerable<LISTITEM> items = func(_viewModel);

            if (items == null) return this;

            foreach (var item in items)
            {
                _writer.WriteOption(displaySelector(item), valueSelector(item));
            }
            return this;
        }

        public override string ToString()
        {
            return _writer.WithAttributes(GetAllHtmlAttributes()).ToString();
        }

        protected override DropDownListExpression<VIEWMODEL> thisInstance()
        {
            return this;
        }
    }
}