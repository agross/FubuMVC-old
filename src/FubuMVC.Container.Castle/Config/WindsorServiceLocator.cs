using System;
using System.Collections.Generic;
using System.Linq;

using Castle.Windsor;

using FubuMVC.Core;

using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Container.Castle.Config
{
	public class WindsorServiceLocator : ServiceLocatorImplBase
	{
		readonly IWindsorContainer _container;

		public WindsorServiceLocator(IWindsorContainer container)
		{
			_container = container;
		}

		protected override object DoGetInstance(Type serviceType, string key)
		{
			return key.IsEmpty()
			       	? _container.Resolve(serviceType)
			       	: _container.Resolve(key, serviceType);
		}

		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			return _container.ResolveAll(serviceType).Cast<object>().AsEnumerable();
		}
	}
}