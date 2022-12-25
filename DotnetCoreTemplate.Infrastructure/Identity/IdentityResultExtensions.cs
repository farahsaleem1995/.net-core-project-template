using DotnetCoreTemplate.Application.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace DotnetCoreTemplate.Infrastructure.Identity;

public static class IdentityResultExtensions
{
	public static Result AsApplicationResult(this IdentityResult result)
	{
		if (!result.Succeeded)
		{
			return Result.Fail(result.Errors.Select(e => e.Description).ToArray());
		}

		return Result.Succeed();
	}

	public static string[] GetSignInErros(this SignInResult result)
	{
		var errors = new List<string>();

		if (result.IsLockedOut)
			errors.Add("User was locked out.");

		if (result.IsNotAllowed)
			errors.Add("User is not allowed to sign in.");

		if (result.RequiresTwoFactor)
			errors.Add("Two factor authentication is required.");

		if (!errors.Any())
			errors.Add("Sign in failed.");

		return errors.ToArray();
	}
}