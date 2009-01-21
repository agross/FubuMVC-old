using System;
using System.Reflection;

namespace FubuMVC.Core.Behaviors
{
    public class copy_viewmodel_from_input_to_output<VIEWMODEL> : behavior_base_for_convenience
        where VIEWMODEL : class
    {
        private VIEWMODEL _inputViewModel;
        private readonly PropertyInfo[] viewModelProperties;

        public copy_viewmodel_from_input_to_output()
        {
            Type viewModelType = typeof(VIEWMODEL);
            viewModelProperties = viewModelType.GetProperties();
        }

        public override void PrepareInput<INPUT>(INPUT input)
        {
            _inputViewModel = input as VIEWMODEL;
        }

        public override void ModifyOutput<OUTPUT>(OUTPUT output)
        {
            if (_inputViewModel != null && output as VIEWMODEL != null)
                viewModelProperties.Each(p => p.SetValue(output, p.GetValue(_inputViewModel, null), null));
        }
    }
}