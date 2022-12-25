using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace DotnetCoreTemplate.Infrastructure.Identity;

public class IdentityProvider : IIdentityProvider
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly SignInManager<ApplicationUser> _signInManager;

	public IdentityProvider(
		UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager)
	{
		_userManager = userManager;
		_signInManager = signInManager;
	}

	public async Task<Result> RegisterUsertAsync(
		string email, string password, SecurityRole role, CancellationToken cancellation = default)
	{
		var user = new ApplicationUser
		{
			Id = Guid.NewGuid().ToString(),
			Email = email,
			UserName = email,
		};
		return await CreateUser(user, password, role);
	}

	private async Task<Result> CreateUser(ApplicationUser user, string password, SecurityRole role)
	{
		var result = await _userManager.CreateAsync(user, password);
		if (!result.Succeeded)
		{
			return result.AsApplicationResult();
		}

		return await AddToRole(user, role);
	}

	private async Task<Result> AddToRole(ApplicationUser user, SecurityRole role)
	{
		var result = await _userManager.AddToRoleAsync(user, role.ToString());

		return result.AsApplicationResult();
	}

	public async Task<Result> SignInAsync(
		string userId, string password, CancellationToken cancellation = default)
	{
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
		{
			return Result.Fail(new[] { $"User '{userId}' was not found." });
		}

		return await SignIn(user, password);
	}

	private async Task<Result> SignIn(ApplicationUser user, string password)
	{
		var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

		if (!result.Succeeded)
		{
			return Result.Fail(result.GetSignInErros());
		}

		return Result.Succeed();
	}
}