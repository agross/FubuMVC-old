using Castle.MicroKernel.Facilities;

namespace FubuMVC.Container.Castle.ForIoC
{
	public class ArrayDependencyFacility : AbstractFacility
	{
		#region Overrides of AbstractFacility
		protected override void Init()
		{
			Kernel.Resolver.AddSubResolver(new ArraySubDependencyResolver(Kernel));
		}
		#endregion
	}
}