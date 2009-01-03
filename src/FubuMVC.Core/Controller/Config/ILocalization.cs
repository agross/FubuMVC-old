using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace FubuMVC.Core.Controller.Config
{
    public interface ILocalization
    {
        void Configure(IDictionary<string, object> requestData);
    }

    public class Localization : ILocalization
    {
        // TODO: This is a lot of parsing to do here, especially
        // since HttpContext exposes a "UserLanguages" property that pre-parses
        // the strings. It's not much, but DRY, right?

        // Also, this currently only sets the culture info for loading resources.
        // Setting the culture for currency and number formats is still TBD.

        private IList<CultureInfo> GetCulturesFromStrings(IEnumerable<string> cultures)
        {
            var cultureInfos = new List<CultureInfo>();
            if (cultures == null) { return cultureInfos; }
            cultures.Each(c =>
            {
                try
                {
                    cultureInfos.Add(new CultureInfo(c));
                }
                catch (ArgumentException)
                {
                    // Eat it, this data can be "tweaked"
                    // and we don't want bad data to cause 
                    // problems just for localization.
                }
            });
            return cultureInfos;
        }

        private IList<CultureInfo> GetCulturesFromRequest(IDictionary<string, object> requestData)
        {
            // TODO: Where should strings like this be maintained? In-place like this?
            string rawHeaderValue = requestData["HTTP_ACCEPT_LANGUAGE"] as string;
            if (String.IsNullOrEmpty(rawHeaderValue))
            {
                return new List<CultureInfo>();
            }

            // One line of code to do virtually the same thing as System.Web.HttpRequest.ParseMultivalueHeader :)
            return GetCulturesFromStrings(rawHeaderValue.Trim().Split(',').Each(x => x.Trim()));
        }

        public void Configure(IDictionary<string, object> requestData)
        {
            var cultures = GetCulturesFromRequest(requestData);
            if (cultures.Count > 0)
            {
                Thread.CurrentThread.CurrentUICulture = cultures[0];
            }
        }
    }
}
