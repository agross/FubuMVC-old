using System;
using System.Linq;
using AltOxite.Core.Config;
using AltOxite.Core.Domain;
using NUnit.Framework;
using FubuMVC.Core;
using FubuMVC.Core.Util;

namespace AltOxite.Tests.Config
{
    [TestFixture]
    public class SiteConfigDTOTester
    {
        [Test]
        public void should_be_serializeable_from_json()
        {
            var id = Guid.NewGuid();
            var name = "test";
            var host1 = "host1";
            var host2 = "host2";

            var json = "{{id: '{0}', name: '{1}', aliases:[{{host: '{2}'}}, {{host: '{3}'}}]}}".ToFormat(id, name, host1, host2);

            var dto = JsonUtil.Get<SiteConfigDTO>(json);

            dto.id.ShouldEqual(id);
            dto.name.ShouldEqual(name);
            dto.aliases[0].host.ShouldEqual(host1);
            dto.aliases[1].host.ShouldEqual(host2);
        }

        [Test]
        public void should_apply_defaults()
        {
            new SiteConfigDTO().pageTitleSeparator.ShouldEqual(" - ");
        }

        [Test]
        public void should_convert_to_full_domain_entity()
        {
            var id = Guid.NewGuid();
            var name = "test";
            var dto = new SiteConfigDTO {id = id, name = name};

            var config = new SiteConfiguration();
            dto.ToSiteConfiguration(config);

            config.ID.ShouldEqual(id);
            config.Name.ShouldEqual(name);
        }

        [Test]
        public void should_convert_sub_aliases()
        {
            var host1 = "host1";
            var host2 = "host2";
            var dto = new SiteConfigDTO {aliases = new[] {new AliasDTO {host = host1}, new AliasDTO {host = host2}}};

            var config = new SiteConfiguration();
            dto.ToSiteConfiguration(config);
            var aliases = config.GetAliases();

            aliases.First().Host.ShouldEqual(host1);
            aliases.Skip(1).Single().Host.ShouldEqual(host2);
        }
    }
}