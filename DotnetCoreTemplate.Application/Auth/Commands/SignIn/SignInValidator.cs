using FluentValidation;

namespace DotnetCoreTemplate.Application.Auth.Commands.SignIn;

public class SignInValidator : AbstractValidator<SignInCommand>
{
	public SignInValidator()
	{
		RuleFor(v => v.Email).NotEmpty();
		RuleFor(v => v.Password).NotEmpty();
	}
}