using System.Web;

using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;

namespace FubuMVC.Container.Castle.ForIoC
{
	public class HybridLifestyleManager : AbstractLifestyleManager
	{
		readonly PerThreadLifestyleManager _perThread;
		readonly PerWebRequestLifestyleManager _perWebRequest;

		public HybridLifestyleManager()
		{
			_perWebRequest = new PerWebRequestLifestyleManager();
			_perThread = new PerThreadLifestyleManager();
		}

		public override void Init(IComponentActivator componentActivator, IKernel kernel, global::Castle.Core.ComponentModel model)
		{
			base.Init(componentActivator, kernel, model);
			
			_perThread.Init(componentActivator, kernel, model);
			_perWebRequest.Init(componentActivator, kernel, model);
		}

		public override void Dispose()
		{
			_perThread.Dispose();
			_perWebRequest.Dispose();
		}

		public override object Resolve(CreationContext context)
		{
			if (HttpContext.Current == null)
			{
				return _perThread.Resolve(context);
			}

			return _perWebRequest.Resolve(context);
		}

		public override bool Release(object instance)
		{
			if (HttpContext.Current == null)
			{
				return _perThread.Release(instance);
			}

			return _perWebRequest.Release(instance);
		}
	}
}