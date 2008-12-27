using System;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;
using FubuMVC.Core.Behaviors;

namespace AltOxite.Core.Web.Behaviors
{
    public class set_the_current_logged_in_user_on_the_output_viewmodel : behavior_base_for_convenience
    {
        private readonly IRepository _repository;

        public set_the_current_logged_in_user_on_the_output_viewmodel(IRepository repository)
        {
            _repository = repository;
        }

        public void UpdateModel(ViewModel model)
        {
            if (model == null) return;

            var prin = AltOxitePrincipal.Current;

            if (prin == null) return;

            model.CurrentUser = _repository.Load<User>(prin.UserId);
        }

        public override void ModifyOutput<OUTPUT>(OUTPUT output)
        {
            UpdateModel(output as ViewModel);
        }
    }
}