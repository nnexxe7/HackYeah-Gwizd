﻿using GWIZD.Model;
using Service.Repositories;

namespace Service;

public class UsersService : IUsersService
{
	private readonly IUsersRepository _usersRepository;


	public UsersService(IUsersRepository usersRepository)
	{
		_usersRepository = usersRepository;
	}

	public User EnsureUserExists(string username)
	{
		if (username == null) throw new ArgumentNullException(nameof(username));

		User user = _usersRepository.Get(username.ToLower());
		if (user == null)
		{
			_usersRepository.Save(new User { Id = username.ToLower() });
		}

		return user;
	}

	public void IncreasePoints(string username, int points)
	{
		if (username == null) throw new ArgumentNullException(nameof(username));

		User user = _usersRepository.Get(username.ToLower());
		if (user == null) throw new ArgumentNullException(nameof(username));
		user.Points += points;

		_usersRepository.Save(user);
	}
}