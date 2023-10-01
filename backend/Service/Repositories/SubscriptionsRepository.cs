using GWIZD.Core.Data;
using GWIZD.Model;
using MongoDB.Driver;
using WebApi.Model;

namespace Service.Repositories;

public class SubscriptionsRepository : MongoDbGenericRepository<Subscription, Guid>, ISubscriptionsRepository
{
	public SubscriptionsRepository(IMongoDbAccess dbAccess) : base(dbAccess)
	{
	}

	public List<Subscription> GetMatching(TootType tootType, AnimalType? animalType, Location location)
	{
		if (location == null) throw new ArgumentNullException(nameof(location));

		FilterDefinitionBuilder<Subscription>? b = Builders<Subscription>.Filter;
		FilterDefinition<Subscription>? query = b.And(b.Eq(n => n.TootType, tootType), b.Eq(n => n.AnimalType, animalType));

		IMongoCollection<Subscription> collection = DbAccess.GetCollection<Subscription>();
		List<Subscription>? matchingSubscriptions = collection.Find(query).ToList();

		return matchingSubscriptions.Where(n => n.Location.CalculateDistance(location) <= n.Radius).ToList();
	}
}