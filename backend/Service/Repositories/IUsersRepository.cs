using GWIZD.Core.Data;
using GWIZD.Model;

namespace Service.Repositories;

public interface IUsersRepository : IGenericRepository<User, string>
{
	List<User> GetUsersWithHighestPoints(int count);
}