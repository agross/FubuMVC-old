using System;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace VsTemplate.Core
{
    public class FubuMvcPrincipal : IPrincipal
    {
        private readonly IIdentity _identity;
        private readonly Guid _userId;

        public FubuMvcPrincipal(IIdentity identity)
        {
            _identity = identity;
            _userId = new Guid(_identity.Name);
        }

        public bool IsInRole(string role)
        {
            //TODO: Implement roles for ASP.NET security stuff
            object TODO_IMPLEMENT_PRINCIPAL_ROLES = null;
            return true;
        }

        public Guid UserId { get { return _userId; } }

        IIdentity IPrincipal.Identity { get { return _identity; } }


        public static FubuMvcPrincipal Current
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.User as FubuMvcPrincipal;
                }

                return Thread.CurrentPrincipal as FubuMvcPrincipal;
            }
        }
    }
}