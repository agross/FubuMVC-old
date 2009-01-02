using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.UI;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Util;
using FubuMVC.Core.View;
using FubuMVC.Core.View.WebForms;

namespace FubuMVC.Core.Html.Expressions
{
    public interface IRenderPartialForScope
    {
        string For<VIEWMODEL, PARTIALVIEWMODEL>(
            Expression<Func<VIEWMODEL, PARTIALVIEWMODEL>> expression)
            where VIEWMODEL : class
            where PARTIALVIEWMODEL : class;

        string For(object model);

        string ForEachOf<PARTIALVIEWMODEL>(IEnumerable<PARTIALVIEWMODEL> items)
            where PARTIALVIEWMODEL : class;
    }

    public class RenderPartialExpression
    {
        private readonly IFubuViewWithModel _parentPage;
        private readonly IWebFormsViewRenderer _renderer;
        private readonly FubuConventions _conventions;

        public RenderPartialExpression(IFubuViewWithModel parentPage, IWebFormsViewRenderer renderer, FubuConventions conventions)
        {
            _renderer = renderer;
            _parentPage = parentPage;
            _conventions = conventions;
        }

        public IRenderPartialForScope Using<PARTIALVIEW>()
            where PARTIALVIEW : Control, IFubuViewWithModel
        {
            return Using<PARTIALVIEW>(null);
        }

        public IRenderPartialForScope Using<PARTIALVIEW>(Action<PARTIALVIEW> optionAction)
            where PARTIALVIEW : Control, IFubuViewWithModel
        {
            return new RenderPartialForScope<PARTIALVIEW>()
                {
                    Renderer = _renderer,
                    ParentPage = _parentPage,
                    SetupAction = optionAction,
                    Conventions = _conventions
                };
        }

        public class RenderPartialForScope<PARTIALVIEW> : IRenderPartialForScope
            where PARTIALVIEW : Control, IFubuViewWithModel
        {
            private readonly Type _viewType = typeof(PARTIALVIEW);

            public Action<PARTIALVIEW> SetupAction;
            public IFubuViewWithModel ParentPage;
            public IWebFormsViewRenderer Renderer;
            public FubuConventions Conventions;

            private object _partialModel;
            private int render_multiple_item_count = 0;
            

            public string For<VIEWMODEL, PARTIALVIEWMODEL>(Expression<Func<VIEWMODEL, PARTIALVIEWMODEL>> expression) 
                where VIEWMODEL : class
                where PARTIALVIEWMODEL : class
            {
                var accessor = ReflectionHelper.GetAccessor(expression);
                var model = ((IFubuView<VIEWMODEL>) ParentPage).Model;
                if (model != null)
                {
                    _partialModel = accessor.GetValue(model);
                }
                
                return ToString();
            }

            public string For(object model)
            {
                _partialModel = model;

                return ToString();
            }

            public string ForEachOf<PARTIALVIEWMODEL>(IEnumerable<PARTIALVIEWMODEL> items)
                where PARTIALVIEWMODEL : class
            {
                var list = items.ToList();

                _partialModel = list;
                render_multiple_item_count = list.Count;

                return ToString();
            }

            public override string ToString()
            {
                if (render_multiple_item_count > 0)
                {
                    var builder = new StringBuilder();

                    builder.Append(Conventions.PartialForEachOfHeader(_partialModel, render_multiple_item_count));

                    var current = 0;

                    foreach (var item in (IEnumerable) _partialModel)
                    {
                        var before = Conventions.PartialForEachOfBeforeEachItem(item, current,
                                                                                render_multiple_item_count);
                        var renderedItem = RenderItem(item);
                        var after = Conventions.PartialForEachOfAfterEachItem(item, current, render_multiple_item_count);
                        builder.AppendFormat("{0}{1}{2}", before, renderedItem, after);

                        current++;
                    }

                    builder.Append(Conventions.PartialForEachOfFooter(_partialModel, render_multiple_item_count));

                    return builder.ToString();
                }

                return RenderItem(_partialModel);
            }

            private string RenderItem(object modelItem)
            {
                return Renderer.RenderView(
                    Conventions.DefaultPathToPartialView(_viewType), 
                    modelItem,
                    SetupAction);
            }
        }
    }
}