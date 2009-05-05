using System;
using System.Collections.Generic;

namespace FubuMVC.Core.Controller.Invokers
{
    public interface IControllerActionInvoker
    {
        void Invoke(Delegate actionDelegate, IDictionary<string, object> requestData);
    }
}