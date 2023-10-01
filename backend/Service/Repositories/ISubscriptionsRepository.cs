using GWIZD.Core.Data;
using GWIZD.Model;
using WebApi.Model;

namespace Service.Repositories;

public interface ISubscriptionsRepository : IGenericRepository<Subscription, Guid>
{
	List<Subscription> GetMatching(TootType tootType, AnimalType? animalType, Location location);
}