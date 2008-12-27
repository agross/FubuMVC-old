using System.Security.Principal;

namespace FubuMVC.Core.Security
{
    public interface ISecurityContext
    {
        bool IsAuthenticated { get; }
        IIdentity CurrentIdentity { get; }
        IPrincipal CurrentUser { get; set; }
    }
}