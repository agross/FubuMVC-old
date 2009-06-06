using System;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Results;

namespace FubuMVC.Core.Behaviors
{
    public class OutputAsJson : behavior_base_for_convenience
    {
        private bool _nonAjaxRequestDetected;

        public override void PrepareInput<INPUT>(INPUT input)
        {
            // If we know for sure that the request was NOT initiated by XMLHttpRequest, we will
            // return the JSON wrapped in an HTML textarea.
            // This enables support for the "iframe" approach to submitting forms via AJAX, 
            // as described in the jQuery Form Plugin documentation. http://malsup.com/jquery/form/#api
            var ajaxAwareOutput = input as ICanDetectAjaxRequests;
            _nonAjaxRequestDetected = ajaxAwareOutput != null && !ajaxAwareOutput.IsAjaxRequest();
        }

        public override OUTPUT AfterInvocation<OUTPUT>(OUTPUT output, IInvocationResult insideResult)
        {
            Result = (_nonAjaxRequestDetected 
                ? new RenderHtmlFriendlyJsonResult<OUTPUT>(output) 
                : new RenderJsonResult<OUTPUT>(output));
            return output;
        }
    }

    public interface ICanDetectAjaxRequests
    {
        string HTTP_X_REQUESTED_WITH { get; set; }
    }

    public static class RequestHelper
    {
        public static bool IsAjaxRequest(this ICanDetectAjaxRequests request)
        {
            return "XMLHttpRequest".Equals(request.HTTP_X_REQUESTED_WITH, StringComparison.OrdinalIgnoreCase);
        }
    }

}