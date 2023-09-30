using Ninject.Modules;

namespace GWIZD.Core.Data;

public class DataNinjectModule : NinjectModule
{
	public override void Load()
	{
		Bind<IMongoDbIndexBuilder>().To<MongoDbIndexBuilder>().InSingletonScope();
		Bind<IMongoDbAccess>().To<MongoDbAccess>().InSingletonScope();
		Bind<ISettingsProvider>().To<FileBasedSettingsProvider>().InSingletonScope();
	}
}