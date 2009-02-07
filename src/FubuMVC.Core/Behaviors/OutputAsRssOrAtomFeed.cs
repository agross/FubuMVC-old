using System.ServiceModel.Syndication;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;
using FubuMVC.Core.Routing;
using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Behaviors
{
    public class OutputAsRssOrAtomFeed : behavior_base_for_convenience
    {
        private readonly IServiceLocator _locator;
        private readonly ICurrentRequest _currentRequest;
        private readonly FubuConventions _conventions;
        private readonly IUrlResolver _urlResolver;

        public OutputAsRssOrAtomFeed(IServiceLocator locator, ICurrentRequest currentRequest, FubuConventions conventions, IUrlResolver urlResolver)
        {
            _locator = locator;
            _currentRequest = currentRequest;
            _conventions = conventions;
            _urlResolver = urlResolver;
        }

        public override OUTPUT AfterInvocation<OUTPUT>(OUTPUT output, IInvocationResult insideResult)
        {
            var isRss = _currentRequest.GetUrl().ToString().ToLower().EndsWith(_conventions.DefaultRssExtension.ToLower());
            var isAtom = _currentRequest.GetUrl().ToString().ToLower().EndsWith(_conventions.DefaultAtomExtension.ToLower());

            if (isRss || isAtom)
            {
                var feedConvertor = _locator.GetInstance<IFeedConverterFor<OUTPUT>>();

                SyndicationFeed syndicationFeed;
                if (feedConvertor.TryConvertModel(output, out syndicationFeed))
                {
                    Result = ResultOverride.IfAvailable(output) ?? (isRss
                        ? new RenderRssOrAtomResult(syndicationFeed.SaveAsRss20, RenderRssOrAtomResult.RSS_CONTENT_TYPE)
                        : new RenderRssOrAtomResult(syndicationFeed.SaveAsAtom10, RenderRssOrAtomResult.ATOM_CONTENT_TYPE));
                    return output;
                }

                Result = ResultOverride.IfAvailable(output) ?? new RedirectResult(_urlResolver.PageNotFoundUrl());
                return output;
            }

            Result = insideResult;
            return output;
        }
    }
}