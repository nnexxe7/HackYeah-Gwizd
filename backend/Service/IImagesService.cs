using GWIZD.Model;

namespace Service;

public interface IImagesService
{
	string UploadContent(byte[] fileContent, string fileName);

	void DeleteContent(Toot toot);
}