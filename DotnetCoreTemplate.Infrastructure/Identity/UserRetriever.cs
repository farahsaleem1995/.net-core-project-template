using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace DotnetCoreTemplate.Infrastructure.Identity;

public class UserRetriever : IUserRetriever
{
	private readonly UserManager<ApplicationUser> _userManager;

	public UserRetriever(UserManager<ApplicationUser> userManager)
	{
		_userManager = userManager;
	}

	public async Task<User?> GetUserById(string userId, CancellationToken cancellation = default)
	{
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
		{
			return null;
		}

		return await UserWithRole(user);
	}

	private async Task<User> UserWithRole(ApplicationUser user)
	{
		var roles = await _userManager.GetRolesAsync(user);

		return new User(user.Id, user.Email!, Enum.Parse<UserRole>(roles.First()));
	}

	public async Task<User?> GetUserByEmai(string email, CancellationToken cancellation = default)
	{
		var user = await _userManager.FindByEmailAsync(email);
		if (user == null)
		{
			return null;
		}

		return await UserWithRole(user);
	}
}