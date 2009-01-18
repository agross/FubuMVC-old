using System.Security.Principal;
using FubuMVC.Core.Security;

namespace VsTemplate.Core.Security
{
    public class FubuMvcPrincipalFactory : IPrincipalFactory
    {
        public IPrincipal CreatePrincipal(IIdentity identity)
        {
            return new FubuMvcPrincipal(identity);
        }
    }
}