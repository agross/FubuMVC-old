using System.Linq;
using System.Security.Principal;
using NUnit.Framework;
using FubuMVC.Container.StructureMap.Config;
using FubuMVC.Core.Security;
using FubuMVC.Core.Web.Security;
using Rhino.Mocks;
using StructureMap;

namespace FubuMVC.Tests.StructureMap
{
    [TestFixture]
    public class StructureMapServiceLocatorTester
    {
        private string _testInstanceKey;
        private ISecurityContext _mockSecurityContext;

        [SetUp]
        public void SetUp()
        {
            _testInstanceKey = "test";

            _mockSecurityContext = MockRepository.GenerateStub<ISecurityContext>();

            ObjectFactory.Configure(x =>
            {
                x.ForRequestedType<ISecurityContext>().TheDefault.IsThis(_mockSecurityContext);
                x.ForRequestedType<ISecurityContext>().AddInstances(
                    s => s.OfConcreteType<WebSecurityContext>().WithName(_testInstanceKey));
            });
        }

        [Test]
        public void should_resolve_unnamed_instances()
        {
            new StructureMapServiceLocator().GetInstance(typeof(ISecurityContext))
                .ShouldBeTheSameAs(_mockSecurityContext);
        }

        [Test]
        public void should_resolve_named_instances()
        {
            new StructureMapServiceLocator().GetInstance(typeof(ISecurityContext), _testInstanceKey)
                .ShouldBeOfType<WebSecurityContext>();
        }

        [Test]
        public void should_get_all_instances_for_a_given_type()
        {
            var instances = new StructureMapServiceLocator()
                .GetAllInstances(typeof (ISecurityContext));

            instances.Single(i => i.GetType() == typeof (WebSecurityContext));
            instances.Single(i => ReferenceEquals(i, _mockSecurityContext));
        }
    }
}