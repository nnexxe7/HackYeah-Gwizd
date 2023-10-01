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
		Bind<ISubscriptionsRepository>().To<SubscriptionsRepository>().InSingletonScope();
		Bind<IUsersService>().To<UsersService>().InSingletonScope();
		Bind<ITootsDuplicateDetector>().To<TootsDuplicateDetector>().InSingletonScope();
		Bind<IImagesService>().To<ImagesService>().InSingletonScope();
		Bind<INotificationsSender>().To<NotificationsSender>().InSingletonScope();
		Bind<INotificationsProcessor>().To<NotificationsProcessor>().InSingletonScope();
	}
}