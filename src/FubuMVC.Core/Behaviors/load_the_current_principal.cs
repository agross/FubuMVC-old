using FubuMVC.Core.Security;

namespace FubuMVC.Core.Behaviors
{
    public class load_the_current_principal : behavior_base_for_convenience
    {
        private readonly ISecurityContext _context;
        private readonly IPrincipalFactory _factory;

        public load_the_current_principal(ISecurityContext context, IPrincipalFactory factory)
        {
            _context = context;
            _factory = factory;
        }

        public override void PrepareInput<INPUT>(INPUT input)
        {
            if (!_context.IsAuthenticated) return;

            var identity = _context.CurrentIdentity;
            _context.CurrentUser = _factory.CreatePrincipal(identity);
        }
    }
}