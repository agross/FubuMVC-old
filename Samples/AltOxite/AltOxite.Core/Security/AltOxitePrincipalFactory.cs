using System.Security.Principal;
using FubuMVC.Core.Security;

namespace AltOxite.Core.Security
{
    public class AltOxitePrincipalFactory : IPrincipalFactory
    {
        public IPrincipal CreatePrincipal(IIdentity identity)
        {
            return new AltOxitePrincipal(identity);
        }
    }
}