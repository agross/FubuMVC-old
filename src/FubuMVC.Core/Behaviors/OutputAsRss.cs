using System;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;
using FubuMVC.Core.Routing;
using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Behaviors
{
    public class OutputAsRss : behavior_base_for_convenience
    {
        private readonly IServiceLocator _locator;
        private readonly ICurrentRequest _currentRequest;
        private readonly FubuConventions _conventions;

        public OutputAsRss(IServiceLocator locator, ICurrentRequest currentRequest, FubuConventions conventions)
        {
            _locator = locator;
            _currentRequest = currentRequest;
            _conventions = conventions;
        }

        public override OUTPUT AfterInvocation<OUTPUT>(OUTPUT output, IInvocationResult insideResult)
        {
            if (!_currentRequest.GetUrl().ToString().EndsWith(_conventions.DefaultRssExtension))
            {
                Result = insideResult;
                return output;
            }
            try
            {
                var feedConvertor = _locator.GetInstance<IFeedConverterFor<OUTPUT>>();
                Result = ResultOverride.IfAvailable(output) ??
                         new RenderRssResult(feedConvertor.ConvertModel(output));
            }
            catch (Exception)
            {
                Result = ResultOverride.IfAvailable(output) ?? new RedirectResult("404");
            }

            return output;
        }
    }
}