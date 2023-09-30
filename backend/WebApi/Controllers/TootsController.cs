using GWIZD.Model;
using Microsoft.AspNetCore.Mvc;
using Service.Repositories;
using WebApi.Model;

namespace WebApi.Controllers;

[ApiController]
[Route("api/toots")]
public class TootsController : ControllerBase
{
	private readonly ITootsRepository _tootsRepository;


	public TootsController(ITootsRepository tootsRepository)
	{
		_tootsRepository = tootsRepository;
	}

	[HttpPost("find")]
	public IActionResult Find()
	{
		List<Toot> result = _tootsRepository.Find();

		return Ok(result);
	}

	[HttpPost("submit")]
	public IActionResult Submit(TootDto dto)
	{
		Toot toot = BuildFromDto(dto);

		_tootsRepository.Save(toot);

		return Ok(toot.Id);
	}

	private static Toot BuildFromDto(TootDto dto)
	{
		return new Toot
		{
			Location = dto.Location,
			Id = Guid.NewGuid(),
			Species = dto.Species,
			SubmittedAt = DateTime.UtcNow,
			Type = dto.Type
		};
	}
}