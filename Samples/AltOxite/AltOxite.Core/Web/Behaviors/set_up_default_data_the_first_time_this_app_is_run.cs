using AltOxite.Core.Config;
using FubuMVC.Core.Behaviors;

namespace AltOxite.Core.Web.Behaviors
{
    public class set_up_default_data_the_first_time_this_app_is_run : behavior_base_for_convenience
    {
        private readonly IApplicationFirstRunHandler _firstRunHandler;

        public set_up_default_data_the_first_time_this_app_is_run(IApplicationFirstRunHandler handler)
        {
            _firstRunHandler = handler;
        }

        public override void PrepareInput<INPUT>(INPUT input)
        {
            _firstRunHandler.InitializeIfNecessary();
        }
    }
}