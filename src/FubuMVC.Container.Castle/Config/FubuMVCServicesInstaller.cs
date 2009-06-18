using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using FubuMVC.Container.Castle.ForIoC;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;
using FubuMVC.Core.Routing;
using FubuMVC.Core.Security;
using FubuMVC.Core.SessionState;
using FubuMVC.Core.View;
using FubuMVC.Core.View.WebForms;
using FubuMVC.Core.Web.Security;

using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Container.Castle.Config
{
	public class FubuMVCServicesInstaller : IWindsorInstaller
	{
		// --------------------------------------------------------
		// --- DEFAULT/REQUIRED FRAMEWORK SERVICES FOR FUBU-MVC ---
		// --------------------------------------------------------

		// Core Services:
		//  - IRouteConfigurer
		//  - ISerivceLocator
		//  - IControllerConfigContext
		//  - IUrlResolver

		// Security/Authentication:
		//  - ISecurityContext
		//  - IAuthenticationContext

		// View Rendering Services
		//  - IViewRenderer
		//  - IWebFormsViewRenderer (for web-forms specific functionality like RenderPartialFor/RenderPartialExpression
		//  - IWebFormsControlBuilder (for Web Forms/ASPX rendering)
		//  - IOutputWriter

		#region IWindsorInstaller Members
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(DefaultRegistrations(container).ToArray());
		}
		#endregion

		public static IEnumerable<IRegistration> DefaultRegistrations(IWindsorContainer container)
		{
			yield return Component.For<IServiceLocator>()
				.LifeStyle.Singleton
				.Instance(new WindsorServiceLocator(container));

			yield return Component.For<IControllerConfigContext>()
				.LifeStyle.Custom<HybridLifestyleManager>()
				.ImplementedBy<DefaultControllerConfigContext>();

			yield return Component.For<IOutputWriter>().ImplementedBy<HttpResponseOutputWriter>();
			Component.For<ICurrentRequest>().ImplementedBy<CurrentRequest>();

			yield return Component.For(typeof(IFeedConverterFor<>)).ImplementedBy(typeof(DefaultFeedConverter<>));

			yield return Component.For<ISecurityContext>().ImplementedBy<WebSecurityContext>();
			yield return Component.For<IAuthenticationContext>().ImplementedBy<WebAuthenticationContext>();

			yield return Component.For<IViewRenderer, IWebFormsViewRenderer>().ImplementedBy<WebFormsViewRenderer>();

			yield return Component.For<HttpContextBase>().Activator<HttpContextActivator>();

			yield return Component.For<IFlash>().ImplementedBy<FlashProvider>();

			yield return Component.For<IResultOverride>()
				.LifeStyle.Custom<HybridLifestyleManager>()
				.ImplementedBy<CurrentRequestResultOverride>();

			//***  Can be replaced by DefaultConventionScanner
			//***  Left in here for now to make documentation easier later
			yield return Component.For<IRouteConfigurer>().LifeStyle.Singleton.ImplementedBy<RouteConfigurer>();
			yield return Component.For<IWebFormsControlBuilder>().ImplementedBy<WebFormsControlBuilder>();
			//yield return Component.For<IWebFormsViewRenderer>().ImplementedBy<WebFormsViewRenderer>();
			yield return Component.For<IUrlResolver>().ImplementedBy<UrlResolver>();
			yield return Component.For<ILocalization>()
				.LifeStyle.Transient
				.ImplementedBy<Localization>();
			yield return Component.For<IRequestDataProvider>().ImplementedBy<RequestDataProvider>();
			//***  

			yield return AllTypes
				.FromAssembly(Assembly.GetExecutingAssembly());

			yield return AllTypes
				.FromAssemblyContaining<FubuMVCServicesInstaller>()
				.IncludeNonPublicTypes();
		}
	}
}