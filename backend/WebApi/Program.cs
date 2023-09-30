using GWIZD.Core;
using GWIZD.Core.Data;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Ninject;
using Service;

namespace WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			IKernel kernel = BuildKernel();
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.AddSingleton<IControllerActivator>(new NinjectControllerActivator(kernel));
			builder.Services.AddSingleton<IViewComponentActivator>(new NinjectViewComponentActivator(kernel));
			builder.WebHost.UseUrls("http://0.0.0.0:5000");

			var app = builder.Build();

			// Configure the HTTP request pipeline.

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}

		private static IKernel BuildKernel()
		{
			var kernel = new StandardKernel(
				new CoreNinjectModule(),
				new DataNinjectModule(),
				new ServiceNinjectModule());

			//kernel.Bind(typeof(ILog<>)).To(typeof(NLogBasedLogger<>));

			return kernel;
		}
	}
}