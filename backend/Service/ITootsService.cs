using GWIZD.Model;

namespace Service;

public interface ITootsService
{
	List<Toot> FindAll();

	List<Toot> FindExpired();

	List<Toot> FindByPoint(Location location, double radius);

	Guid? Submit(Toot toot);

	void Delete(Toot toot);

	void AddPhotoAttachment(Guid tootId, string submittedBy, byte[] fileContent, string extension);
}