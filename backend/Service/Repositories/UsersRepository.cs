using GWIZD.Core.Data;
using GWIZD.Model;
using MongoDB.Driver;

namespace Service.Repositories;

public class UsersRepository : MongoDbGenericRepository<User, string>, IUsersRepository
{
	public UsersRepository(IMongoDbAccess dbAccess) : base(dbAccess)
	{
	}

	public List<User> GetUsersWithHighestPoints(int count)
	{
		SortDefinition<User> sort = Builders<User>.Sort.Descending(n => n.Points);
		IMongoCollection<User> collection = DbAccess.GetCollection<User>();
		List<User> result = collection.Find(Builders<User>.Filter.Empty).Sort(sort).Limit(count).ToList();

		return result;
	}
}