using System.Collections.Generic;

namespace FubuMVC.Core.Controller.Invokers
{
    public interface IControllerActionInvoker
    {
        void Invoke(IDictionary<string, object> requestData);
    }
}