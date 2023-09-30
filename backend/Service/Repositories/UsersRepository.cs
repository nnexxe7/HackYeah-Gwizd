using GWIZD.Core.Data;
using GWIZD.Model;

namespace Service.Repositories;

public class UsersRepository : MongoDbGenericRepository<User, string>
{
	public UsersRepository(IMongoDbAccess dbAccess) : base(dbAccess)
	{
	}
}