using System.Text;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Routing;
using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Controller.Results
{
    public class RenderDebugInformationResult : IInvocationResult
    {
        private readonly FubuConventions _conventions;
        private readonly FubuConfiguration _configuration;
        private readonly string _contentType;
        public static readonly string CONTENT_TYPE = "text/html; charset=utf-8";

        public RenderDebugInformationResult(FubuConventions conventions, FubuConfiguration configuration, string contentType)
        {
            _conventions = conventions;
            _configuration = configuration;
            _contentType = contentType;
        }

        public void Execute(IServiceLocator locator)
        {
            var writer = locator.GetInstance<IOutputWriter>();

            StringBuilder stringBuilder = new StringBuilder();

            var controllerActions = _configuration.GetControllerActionConfigs();
            controllerActions.Each(c => AddControllerAction(stringBuilder, c));

            //Controllers = controllers,
            //ViewFileBasePath = _conventions.ViewFileBasePath,
            //LayoutViewFileBasePath = _conventions.LayoutViewFileBasePath,
            //SharedViewFileBasePath = _conventions.SharedViewFileBasePath,

            writeDebugInformationToOutput(writer, stringBuilder.ToString());
        }

        private static void AddControllerAction(StringBuilder stringBuilder, ControllerActionConfig controllerActionConfig)
        {
            stringBuilder.Append("PrimaryUrl = {0}<br />".ToFormat(controllerActionConfig.PrimaryUrl));
            stringBuilder.Append("ControllerType = {0}<br />".ToFormat(StripAllUpToAndIncludingTheLastDot(controllerActionConfig.ControllerType.ToString())));
            stringBuilder.Append("ActionFunc = {0}<br />".ToFormat(controllerActionConfig.ActionMethod.Name));

            var InputType = StripAllUpToAndIncludingTheLastDot(controllerActionConfig.ActionMethod.ToString());
            stringBuilder.Append("InputType = {0}<br />".ToFormat(InputType.Substring(0, InputType.IndexOf(")"))));
            string outputType = StripAllUpToAndIncludingTheLastDot(controllerActionConfig.ActionMethod.ReturnType.ToString());
            stringBuilder.Append("OutputType = {0}<br />".ToFormat(outputType));

            var actionName = StripAllUpToAndIncludingTheLastDot(controllerActionConfig.ActionMethod.Name);
            var parenLoc = actionName.IndexOf("(");
            if (parenLoc >= 0)
            {
                actionName = actionName.Substring(0, parenLoc);
            }
            stringBuilder.Append("MethodSignature = public {0} {1}({2} object);<br />".ToFormat(outputType, actionName, InputType));

            stringBuilder.Append("Behaviors<br />");
            controllerActionConfig.GetBehaviors().Each(b => stringBuilder.Append("- {0}<br />".ToFormat(b.ToString())));

            stringBuilder.Append("Other URLs<br />");
            controllerActionConfig.GetOtherUrls().Each(u => stringBuilder.Append("- {0}<br />".ToFormat(u.ToString())));
            stringBuilder.Append("<br /><br />");
        }

        private static string StripAllUpToAndIncludingTheLastDot(string input)
        {
            return input.Substring(input.LastIndexOf(".") + 1);
        }

        protected virtual void writeDebugInformationToOutput(IOutputWriter writer, string debugInformation)
        {
            writer.Write(_contentType, debugInformation);
        }
    }
}