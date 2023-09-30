using Ninject.Modules;

namespace GWIZD.Core;

public class CoreNinjectModule : NinjectModule
{
	public override void Load()
	{
		Bind(typeof(ILog<>)).To(typeof(NLogBasedLogger<>));
		Bind<ISettingsProvider>().To<FileBasedSettingsProvider>().InSingletonScope();
	}
}