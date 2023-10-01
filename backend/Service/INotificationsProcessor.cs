using GWIZD.Model;

namespace Service;

public interface INotificationsProcessor
{
	void Process(Toot toot);
}