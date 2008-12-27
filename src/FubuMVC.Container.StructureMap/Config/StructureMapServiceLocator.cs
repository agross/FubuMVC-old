using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using FubuMVC.Core;
using StructureMap;

namespace FubuMVC.Container.StructureMap.Config
{
    public class StructureMapServiceLocator : ServiceLocatorImplBase {
        
        protected override object DoGetInstance(Type serviceType, string key)
        {
            return key.IsEmpty()
                       ? ObjectFactory.GetInstance(serviceType)
                       : ObjectFactory.GetNamedInstance(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType) {
            return ObjectFactory.GetAllInstances(serviceType).Cast<object>().AsEnumerable();
        }
    }
}