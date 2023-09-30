using GWIZD.Model;

namespace Service;

public interface IUsersService
{
	User EnsureUserExists(string username);

	void IncreasePoints(string username, int points);
}