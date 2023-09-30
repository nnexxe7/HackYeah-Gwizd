using MongoDB.Driver;

namespace GWIZD.Core.Data;

public class MongoDbAccess : IMongoDbAccess
{
	public const string MongoDbDatabaseName = "HackYeah";
	private readonly IMongoClient _mongoClient;
	private readonly IMongoDbIndexBuilder _mongoDbIndexBuilder;


	public MongoDbAccess(ISettingsProvider connectionStringsProvider, IMongoDbIndexBuilder mongoDbIndexBuilder)
	{
		_mongoDbIndexBuilder = mongoDbIndexBuilder;
		string connectionString = connectionStringsProvider.Get("connection_string");
		_mongoClient = new MongoClient(connectionString);
	}

	public IMongoDatabase GetDatabase(string name)
	{
		return _mongoClient.GetDatabase(name);
	}

	public IMongoCollection<T> GetCollection<T>(string collectionName = null)
	{
		IMongoDatabase database = _mongoClient.GetDatabase(MongoDbDatabaseName);
		IMongoCollection<T> collection = GetCollection<T>(database, collectionName);
		_mongoDbIndexBuilder.ProcessIndexes(collection);

		return collection;
	}

	public IClientSessionHandle StartSession()
	{
		return _mongoClient.StartSession();
	}


	private static IMongoCollection<T> GetCollection<T>(IMongoDatabase db, string customCollectionName = null)
	{
		Type type = typeof(T);
		string collectionName = customCollectionName ?? BuildCollectionName(type);

		return db.GetCollection<T>(collectionName);
	}

	private static string BuildCollectionName(Type type)
	{
		return type.Name;
	}
}