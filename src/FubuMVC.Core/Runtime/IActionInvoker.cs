using System.Collections.Generic;

namespace FubuMVC.Core.Runtime
{
    public interface IActionInvoker
    {
        void Invoke(IDictionary<string, object> dictionary);
    }
}