using System.Configuration;
using VsTemplate.Core.Domain;
using FubuMVC.Core.Util;

namespace VsTemplate.Core.Config
{
    public static class ConfigExtensions
    {
        public static SiteConfiguration FromAppSetting(this SiteConfiguration config, string appSettingName)
        {
            var json = ConfigurationManager.AppSettings[appSettingName];
            var dto = JsonUtil.Get<SiteConfigDTO>(json);
            dto.ToSiteConfiguration(config);
            return config;
        }
    }
}