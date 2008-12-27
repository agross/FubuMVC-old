using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using OpinionatedMVC.Core.Util;
using OpinionatedMVC.Core.View;

namespace OpinionatedMVC.Core.Html
{
    public class RenderPartialExpression<VIEWMODEL>
        where VIEWMODEL : class
    {
        private readonly IView _parentPage;
        private bool _multiMode;
    	private Type _multiModeModelType;
        private object _partialModel;
        private string _prefix;
        private readonly IPartialRenderer _renderer;
        private IView _partialView;

        public RenderPartialExpression(IView parentPage, IPartialRenderer renderer)
        {
            _renderer = renderer;
            _parentPage = parentPage;
        }
        
        public RenderPartialExpression<VIEWMODEL> Using<PARTIALVIEW>()
            where PARTIALVIEW : IView
        {
            return Using<PARTIALVIEW>(null);
        }

        public RenderPartialExpression<VIEWMODEL> Using<PARTIALVIEW>(Action<PARTIALVIEW> optionAction)
            where PARTIALVIEW : IView
        {
            _partialView = _renderer.CreateControl(typeof (PARTIALVIEW));
            
            if (optionAction != null)
            {
                optionAction((PARTIALVIEW)_partialView);
            }

            return this;
        }

        public RenderPartialExpression<VIEWMODEL> For<PARTIALVIEWMODEL>(
            Expression<Func<VIEWMODEL, PARTIALVIEWMODEL>> expression )
            where PARTIALVIEWMODEL : class
        {
            SetupModelFromAccessor(ReflectionHelper.GetAccessor(expression));

            return this;
        }

        public RenderPartialExpression<VIEWMODEL> For(object model)
        {
            _partialModel = model;
            _prefix = string.Empty;

            return this;
        }

        public RenderPartialExpression<VIEWMODEL> ForEachOf<PARTIALVIEWMODEL>(
            Expression<Func<VIEWMODEL, IEnumerable<PARTIALVIEWMODEL>>> expression)
            where PARTIALVIEWMODEL : class
        {
            SetupModelFromAccessor(ReflectionHelper.GetAccessor(expression));

            _multiMode = true;
			_multiModeModelType = typeof(PARTIALVIEWMODEL);

            return this;
        }

        private void SetupModelFromAccessor(Accessor accessor)
        {
            _partialModel = accessor.GetValue(_parentPage.GetViewModel());
            _prefix = accessor.Name;
        }

        public override string ToString()
        {
            if (!_multiMode)
            {
                return _renderer.Render(_partialView, _partialModel, _prefix);
            }
            
            var idx = 0;
            var builder = new StringBuilder();

			var fullPrefix = _prefix + "_" + _multiModeModelType.Name;

			var containerName = "_container" + fullPrefix;
			var templateName = "_template" + fullPrefix;
			var blankName = "_blank" + fullPrefix;

            renderRepeaterControls(builder, containerName);

            builder.AppendFormat("<div class=\"{0}\">", containerName);

            foreach (var model in (IEnumerable) _partialModel)
            {
            	builder.AppendFormat("<div class=\"_{0}{1}\" style=\"display: none;\">", idx, fullPrefix);
                builder.Append(_renderer.Render(_partialView, model, "_" + idx + fullPrefix));
                builder.AppendFormat("<input type=\"hidden\" name=\"{0}Deleted\"/>", "_" + idx + fullPrefix);
				builder.Append("</div>");
                idx++;
            }

            renderTemplateBlock(builder, templateName, blankName);

			renderScriptWireBlock(builder, containerName, fullPrefix, idx, templateName);

            builder.Append("</div>");

            return builder.ToString();
        }

    	private static void renderRepeaterControls(StringBuilder builder, string containerName)
        {
            builder.AppendFormat(
                @"
                <div class=""repeaterControlsFor{0}"">
                    <a class=""repeaterPrevious"" href=""javascript:void(0);"">&lt; &lt; Prev</a>
                    <a class=""repeaterNext"" href=""javascript:void(0);"">Next &gt; &gt;</a>
                    &nbsp;&nbsp;
                    <a class=""repeaterDelete"" href=""javascript:void(0);"">Delete</a>
                    <a class=""repeaterUnDelete"" style=""display: hidden"" href=""javascript:void(0);"">Un-Delete</a>
                    &nbsp;&nbsp;
                    <a class=""repeaterAddNew"" href=""javascript:void(0);"">Add New</a>
                    
                </div>",
                containerName);
        }

        private static void renderScriptWireBlock(StringBuilder builder, string containerName, string prefix, int idx, string templateName)
        {
            builder.AppendFormat(
                @"<script type=""text/javascript"">
                        $.dovetail.editableRepeater({{
                            templateClass: '{0}', 
                            prefix: '{1}', 
                            containerClass: '{2}',
                            existingItemCount: {3}
                        }});
                  </script>",
                templateName,
                prefix,
                containerName,
                idx);
        }

        private void renderTemplateBlock(StringBuilder builder, string templateName, string blankName)
        {
            builder.AppendFormat(@"
<div class=""{0}"" style=""display: none;"">
    {1}
    <input type=""hidden"" name=""{2}Deleted""/>
</div>",
                                 templateName,
                                 _renderer.Render(_partialView, null, blankName),
                                 blankName);
        }
    }
}