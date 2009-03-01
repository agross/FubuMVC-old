using FubuMVC.Core.Conventions;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public class CustomConventionExpression<TARGET>
        where TARGET : class
    {
        private readonly FubuConventions _conventions;

        public CustomConventionExpression(FubuConventions conventions)
        {
            _conventions = conventions;
        }

        public CustomConventionExpression<TARGET> Add<CONVENTION>()
            where CONVENTION : IFubuConvention<TARGET>
        {
            _conventions.AddCustomConvention<CONVENTION, TARGET>();
            
            return this;
        }
    }
}