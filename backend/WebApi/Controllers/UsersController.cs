using GWIZD.Model;
using Microsoft.AspNetCore.Mvc;
using Service.Repositories;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
	private readonly IUsersRepository _usersRepository;

	public UsersController(IUsersRepository usersRepository)
	{
		_usersRepository = usersRepository;
	}

	[HttpPost("findTop")]
	public IActionResult FindTop()
	{
		List<User> result = _usersRepository.GetUsersWithHighestPoints(10);

		return Ok(result);
	}
}