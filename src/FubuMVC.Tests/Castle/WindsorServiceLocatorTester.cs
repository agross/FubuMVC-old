using System.Linq;

using Castle.MicroKernel.Registration;
using Castle.Windsor;

using FubuMVC.Container.Castle.Config;
using FubuMVC.Core.Security;
using FubuMVC.Core.Web.Security;

using NUnit.Framework;

using Rhino.Mocks;

namespace FubuMVC.Tests.Castle
{
	[TestFixture]
	public class WindsorServiceLocatorTester
	{
		#region Setup/Teardown
		[SetUp]
		public void SetUp()
		{
			_testInstanceKey = "test";

			_mockSecurityContext = MockRepository.GenerateStub<ISecurityContext>();

			_container = new WindsorContainer();
			_container.Register(Component.For<ISecurityContext>().Instance(_mockSecurityContext),
			                    Component.For<ISecurityContext>().ImplementedBy<WebSecurityContext>().Named(_testInstanceKey));
		}
		#endregion

		string _testInstanceKey;
		ISecurityContext _mockSecurityContext;
		IWindsorContainer _container;

		[Test]
		public void should_get_all_instances_for_a_given_type()
		{
			var instances = new WindsorServiceLocator(_container)
				.GetAllInstances(typeof(ISecurityContext));

			instances.Single(i => i.GetType() == typeof(WebSecurityContext));
			instances.Single(i => ReferenceEquals(i, _mockSecurityContext));
		}

		[Test]
		public void should_resolve_named_instances()
		{
			new WindsorServiceLocator(_container).GetInstance(typeof(ISecurityContext), _testInstanceKey)
				.ShouldBeOfType<WebSecurityContext>();
		}

		[Test]
		public void should_resolve_unnamed_instances()
		{
			new WindsorServiceLocator(_container).GetInstance(typeof(ISecurityContext))
				.ShouldBeTheSameAs(_mockSecurityContext);
		}
	}
}