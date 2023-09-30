using MongoDB.Driver;

namespace GWIZD.Core.Data;

public interface IMongoDbIndexBuilder
{
	void ProcessIndexes<T>(IMongoCollection<T> collection);
}