using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Identity;
using DotnetCoreTemplate.WebAPI.Services;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;

public static class IdentityContainerExtensions
{
	public static Container RegisterIdentity(this Container container, IConfiguration configuration)
	{
		container.Register<IUserContext, AspNetUserContextAdapter>(Lifestyle.Scoped);

		container.Register<IIdentityProvider, IdentityProvider>(Lifestyle.Scoped);

		container.Register<IUserRetriever, UserRetriever>(Lifestyle.Scoped);

		container.Register<ITokenProvider, TokenProvider>(Lifestyle.Scoped);

		container.RegisterTokenSettings(configuration);

		return container;
	}

	private static Container RegisterTokenSettings(this Container container, IConfiguration configuration)
	{
		var settings = new TokenProvider.TokenSettings();

		configuration.GetSection("TokenSettings").Bind(settings);

		container.RegisterInstance(settings);

		return container;
	}
}