using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Ninject;
using Ninject.Parameters;

namespace WebApi;

public class NinjectControllerActivator : IControllerActivator
{
	private readonly IKernel _kernel;

	public NinjectControllerActivator(IKernel kernel)
	{
		_kernel = kernel;
	}

	public object Create(ControllerContext context)
	{
		var t = context.ActionDescriptor.ControllerTypeInfo.AsType();
		return _kernel.Get(t, new IParameter[0]);
	}

	public void Release(ControllerContext context, object controller)
	{
	}
}