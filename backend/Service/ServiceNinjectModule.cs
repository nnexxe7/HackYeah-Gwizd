using Ninject.Modules;
using Service.Repositories;

namespace Service;

public class ServiceNinjectModule : NinjectModule
{
	public override void Load()
	{
		Bind<ITootsRepository>().To<TootsRepository>().InSingletonScope();
		Bind<ITootsService>().To<TootsService>().InSingletonScope();
		Bind<IUsersRepository>().To<UsersRepository>().InSingletonScope();
		Bind<IUsersService>().To<UsersService>().InSingletonScope();
		Bind<ITootsDuplicateDetector>().To<TootsDuplicateDetector>().InSingletonScope();
	}
}