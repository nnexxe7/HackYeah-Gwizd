using GWIZD.Model;
using GWIZD.Service;
using MongoDB.Driver;
using Service.Repositories;

namespace Service;

public class TootsDuplicateDetector : ITootsDuplicateDetector
{
	private readonly ITootsRepository _tootsRepository;


	public TootsDuplicateDetector(ITootsRepository tootsRepository)
	{
		_tootsRepository = tootsRepository;
	}

	public bool CanSubmit(Toot toot, out Toot? foundDuplicate)
	{
		if (toot == null) throw new ArgumentNullException(nameof(toot));

		FilterDefinitionBuilder<Toot>? b = Builders<Toot>.Filter;
		FilterDefinition<Toot>? query = b.And(b.Eq(n => n.Type, toot.Type), b.Eq(n => n.RelatedAnimal, toot.RelatedAnimal));

		List<Toot> allToots = _tootsRepository.Find(query); //This can be optimized. POC

		if (allToots.Count == 0)
		{
			foundDuplicate = null;
			return true;
		}
		Toot? duplicate = allToots.FirstOrDefault(n => n.Location != null && n.Location.CalculateDistance(toot.Location) < Consts.SingularityPrecision);
		foundDuplicate = duplicate;

		return duplicate == null;
	}
}