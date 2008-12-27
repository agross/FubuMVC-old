using System;
using System.Linq.Expressions;
using System.Web.UI;
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
    }

    public class RenderPartialExpression
    {
        private readonly IFubuViewWithModel _parentPage;
        private readonly IWebFormsViewRenderer _renderer;

        public RenderPartialExpression(IFubuViewWithModel parentPage, IWebFormsViewRenderer renderer)
        {
            _renderer = renderer;
            _parentPage = parentPage;
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
                    SetupAction = optionAction
                };
        }

        public class RenderPartialForScope<PARTIALVIEW> : IRenderPartialForScope
            where PARTIALVIEW : Control, IFubuViewWithModel
        {
            public Action<PARTIALVIEW> SetupAction;
            public IFubuViewWithModel ParentPage;
            private object _partialModel;
            public IWebFormsViewRenderer Renderer;

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

            public override string ToString()
            {
                //TODO: TOTAL HACK for now, until we can come up with a better solution of
                // user control location besides just /controller/action

                return Renderer.RenderView("~/Views/Shared/{0}.ascx".ToFormat(typeof(PARTIALVIEW).Name), _partialModel, SetupAction);
            }
        }
    }
}