using System;
using System.Security.Principal;
using AltOxite.Core;
using AltOxite.Core.Security;
using NUnit.Framework;
using Rhino.Mocks;

namespace AltOxite.Tests.Security
{
    [TestFixture]
    public class AltOxitePrincipalFactoryTester
    {
        [Test]
        public void creates_a_new_principal_with_the_ID_as_a_guid()
        {
            var identity = MockRepository.GenerateStub<IIdentity>();
            var userId = Guid.NewGuid();
            identity.Stub(i => i.Name).Return(userId.ToString());

            new AltOxitePrincipalFactory()
                .CreatePrincipal(identity)
                .ShouldBeOfType<AltOxitePrincipal>()
                .UserId.ShouldEqual(userId);
        }
    }
}