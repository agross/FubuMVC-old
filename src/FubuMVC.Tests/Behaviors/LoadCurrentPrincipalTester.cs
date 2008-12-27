using System.Security.Principal;
using NUnit.Framework;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Security;
using Rhino.Mocks;

namespace FubuMVC.Tests.Behaviors
{
    [TestFixture]
    public class LoadCurrentPrincipalTester
    {
        private load_the_current_principal _behavior;
        private ISecurityContext _context;
        private IPrincipalFactory _factory;
        private IIdentity _identity;

        [SetUp]
        public void SetUp()
        {
            _context = MockRepository.GenerateStub<ISecurityContext>();
            _factory = MockRepository.GenerateStub<IPrincipalFactory>();
            _identity = MockRepository.GenerateStub<IIdentity>();

            _context.Stub(c => c.CurrentIdentity).Return(_identity);

            _behavior = new load_the_current_principal(_context, _factory);
        }

        [Test]
        public void should_do_nothing_if_the_context_is_not_authenticated()
        {
            _behavior.PrepareInput<object>(null);
            _context.CurrentUser.ShouldBeNull();

            _factory.AssertWasNotCalled(f => f.CreatePrincipal(null), o => o.IgnoreArguments());
        }

        [Test]
        public void should_create_the_principal_from_the_current_identity_and_username()
        {
            var expectedUser = MockRepository.GenerateStub<IPrincipal>();

            _context.Stub(c=>c.IsAuthenticated).Return(true);

            _factory.Stub(f => f.CreatePrincipal(_identity)).Return(expectedUser);
            _behavior.PrepareInput<object>(null);
            _context.CurrentUser.ShouldBeTheSameAs(expectedUser);
        }
    }
}