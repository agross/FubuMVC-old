using VsTemplate.Core.Config;
using VsTemplate.Core.Domain;
using VsTemplate.Core.Security;
using FubuMVC.Core.Security;
using StructureMap.Configuration.DSL;

namespace VsTemplate.Web
{
    public class FubuMvcRegistry : Registry
    {
        protected override void configure()
        {
            ForRequestedType<IPrincipalFactory>().TheDefault.Is.OfConcreteType<FubuMvcPrincipalFactory>();

            ForRequestedType<SiteConfiguration>()
                .AsSingletons()
                .TheDefault.Is.ConstructedBy(() =>
                    new SiteConfiguration()
                    .FromAppSetting("FubuMvc.SiteConfiguration"));
        }
    }
}