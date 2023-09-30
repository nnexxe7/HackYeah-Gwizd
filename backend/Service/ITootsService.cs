using GWIZD.Model;

namespace Service;

public interface ITootsService
{
	List<Toot> FindAll();

	List<Toot> FindByPoint(Location location, double radius);

	Guid Submit(Toot toot);

	void AddPhotoAttachment(Guid tootId, byte[] fileContent, string extension);
}