using GWIZD.Core.Data;
using GWIZD.Model;
using MongoDB.Driver;

namespace Service.Repositories;

public class TootsRepository : MongoDbGenericRepository<Toot, Guid>, ITootsRepository
{
	public TootsRepository(IMongoDbAccess dbAccess) : base(dbAccess)
	{
	}

	public List<Toot> Find(FilterDefinition<Toot> query)
	{
		if (query == null) throw new ArgumentNullException(nameof(query));

		IMongoCollection<Toot> collection = DbAccess.GetCollection<Toot>();

		List<Toot>? result = collection.Find(query).ToList();

		return result;
	}
}