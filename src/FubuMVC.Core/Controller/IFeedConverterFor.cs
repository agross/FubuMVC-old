using System;
using System.ServiceModel.Syndication;

namespace FubuMVC.Core.Controller
{
    public interface IFeedConverterFor<MODEL>
    {
        bool TryConvertModel(MODEL model, out SyndicationFeed syndicationFeed);
    }

    public class DefaultFeedConverter : IFeedConverterFor<Object> 
    {
        public bool TryConvertModel(Object model, out SyndicationFeed syndicationFeed)
        {
            syndicationFeed = new SyndicationFeed();
            return false;
        }
    }
}