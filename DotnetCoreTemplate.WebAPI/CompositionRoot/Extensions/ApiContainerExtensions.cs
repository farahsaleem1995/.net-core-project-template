using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Utils;
using DotnetCoreTemplate.WebAPI.Services;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Extensions;

public static class ApiContainerExtensions
{
	public static Container RegisterWebApi(this Container container)
	{
		container.Register<IUserContext, AspNetUserContextAdapter>(Lifestyle.Scoped);

		container.Register<IDirector, Director>(Lifestyle.Singleton);

		container.Register(typeof(ILocalCache<,>), typeof(ConcurrentLocalCache<,>), Lifestyle.Singleton);

		container.Register(typeof(Application.Shared.Interfaces.ILogger<>), typeof(AspNetLogger<>), Lifestyle.Singleton);

		return container;
	}
}