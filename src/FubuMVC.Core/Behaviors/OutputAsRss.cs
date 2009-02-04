using System;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Results;
using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Behaviors
{
    public class OutputAsRss : behavior_base_for_convenience
    {
        private readonly IServiceLocator _locator;

        public OutputAsRss(IServiceLocator locator)
        {
            _locator = locator;
        }

        public override OUTPUT AfterInvocation<OUTPUT>(OUTPUT output, IInvocationResult insideResult)
        {
            try
            {
                var feedConvertor = _locator.GetInstance<IFeedConverterFor<OUTPUT>>();
                Result = ResultOverride.IfAvailable(output) ?? new RenderRssResult(feedConvertor.ConvertModel(output));
            }
            catch (Exception)
            {
                Result = ResultOverride.IfAvailable(output) ?? new RedirectResult("404");
            }

            return output;
        }
    }
}