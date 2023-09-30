using MongoDB.Driver;

namespace GWIZD.Core.Data
{
	public interface IMongoDbAccess
	{
		IMongoDatabase GetDatabase(string name);

		IMongoCollection<T> GetCollection<T>(string collectionName = null);

		IClientSessionHandle StartSession();
	}
}