using GWIZD.Model;

namespace Service;

public interface ITootsDuplicateDetector
{
	bool CanSubmit(Toot toot, out Toot? foundDuplicate);
}