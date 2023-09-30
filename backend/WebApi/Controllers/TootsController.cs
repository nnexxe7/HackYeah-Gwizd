using GWIZD.Model;
using Microsoft.AspNetCore.Mvc;
using Service;
using WebApi.Model;

namespace WebApi.Controllers;

[ApiController]
[Route("api/toots")]
public class TootsController : ControllerBase
{
	private readonly ITootsService _tootsService;


	public TootsController(ITootsService tootsService)
	{
		_tootsService = tootsService;
	}

	[HttpPost("findAll")]
	public IActionResult FindAll()
	{
		List<Toot> result = _tootsService.FindAll();

		return Ok(result);
	}

	[HttpPost("findByPoint")]
	public IActionResult FindByPoint(FindByPointDto dto)
	{
		if (dto == null) return BadRequest();

		List<Toot> result = _tootsService.FindByPoint(dto.Location, dto.Radius);

		return Ok(result);
	}

	[HttpPost("addPhotoAttachment")]
	public IActionResult AddPhotoAttachment(Guid tootId, string submittedBy)
	{
		foreach (IFormFile file in Request.Form.Files)
		{
			byte[] fileContent = ReadFully(file.OpenReadStream());
			_tootsService.AddPhotoAttachment(tootId, submittedBy, fileContent, ".jpg");
		}

		return Ok();
	}

	[HttpPost("submit")]
	public IActionResult Submit(TootDto dto)
	{
		if (dto == null) return BadRequest();
		if (dto.Location == null) return BadRequest();

		Toot toot = BuildFromDto(dto);

		Guid? tootId = _tootsService.Submit(toot);

		return Ok(tootId);
	}


	private static byte[] ReadFully(Stream input)
	{
		byte[] buffer = new byte[16 * 1024];
		using (MemoryStream ms = new MemoryStream())
		{
			int read;
			while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
			{
				ms.Write(buffer, 0, read);
			}
			return ms.ToArray();
		}
	}

	private static Toot BuildFromDto(TootDto dto)
	{
		return new Toot
		{
			Id = Guid.NewGuid(),
			Location = dto.Location,
			Activity = dto.Activity,
			RelatedAnimal = dto.RelatedAnimal,
			Description = dto.Description,
			IsDangerous = dto.IsDangerous,
			SubmittedBy = dto.SubmittedBy,
			SubmittedAt = DateTime.UtcNow,
			Type = dto.Type
		};
	}
}