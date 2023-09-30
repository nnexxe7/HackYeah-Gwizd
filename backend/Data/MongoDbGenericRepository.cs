using MongoDB.Driver;

namespace GWIZD.Core.Data;

public abstract class MongoDbGenericRepository<T, K> : IGenericRepository<T, K> where T : IEntity<K>
{
	protected IMongoDbAccess DbAccess { get; }


	protected MongoDbGenericRepository(IMongoDbAccess dbAccess)
	{
		DbAccess = dbAccess;
	}

	public virtual T Get(K id)
	{
		FilterDefinition<T> query = Builders<T>.Filter.Eq(n => n.Id, id);
		IMongoCollection<T> collection = DbAccess.GetCollection<T>();

		return collection.Find(query).FirstOrDefault();
	}

	public virtual void Delete(K id)
	{
		FilterDefinition<T> query = Builders<T>.Filter.Eq(n => n.Id, id);
		IMongoCollection<T> collection = DbAccess.GetCollection<T>();

		collection.DeleteOne(query);
	}

	public virtual K Save(T entity)
	{
		IMongoCollection<T> collection = DbAccess.GetCollection<T>();

		collection.SaveDocument<T, K>(entity);
		return entity.Id;
	}
}