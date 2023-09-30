using Microsoft.AspNetCore.Mvc.ViewComponents;
using Ninject;

namespace WebApi;

public class NinjectViewComponentActivator : IViewComponentActivator
{
	private readonly IKernel _kernel;

	public NinjectViewComponentActivator(IKernel kernel)
	{
		_kernel = kernel;
	}

	public object Create(ViewComponentContext context)
	{
		return _kernel.Get(context.ViewComponentDescriptor.TypeInfo.AsType());

	}

	public void Release(ViewComponentContext context, object viewComponent)
	{
	}
}