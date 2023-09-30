using GWIZD.Model;

namespace Service;

public interface ITootsService
{
	List<Toot> FindAll();
	void AddPhotoAttachment(Guid tootId, byte[] fileContent, string extension);
}