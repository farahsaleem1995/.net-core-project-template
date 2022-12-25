using FluentValidation;

namespace DotnetCoreTemplate.Application.Auth.Commands.RegisterUser;

public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
	public RegisterUserValidator()
	{
		RuleFor(v => v.Email).NotEmpty().EmailAddress();
		RuleFor(v => v.Password).NotEmpty().MinimumLength(6).MaximumLength(256);
	}
}