using GWIZD.Core.Data;
using GWIZD.Model;
using MongoDB.Driver;

namespace Service.Repositories;

public interface ITootsRepository : IGenericRepository<Toot, Guid>
{
	List<Toot> Find(FilterDefinition<Toot> query);
}