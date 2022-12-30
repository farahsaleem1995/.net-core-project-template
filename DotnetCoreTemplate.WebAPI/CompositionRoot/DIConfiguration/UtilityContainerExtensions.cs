using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Services;
using DotnetCoreTemplate.Infrastructure.Services;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using DotnetCoreTemplate.WebAPI.Services;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;

public static class UtilityContainerExtensions
{
	public static Container RegisterUtilities(this Container container)
	{
		container.RegisterValidoators()
			.RegisterAuditTrail();

		container.Register<ITimeProvider, UtcTimeProvider>(Lifestyle.Scoped);

		container.Register(typeof(ILocalCache<,>), typeof(ConcurrentLocalCache<,>), Lifestyle.Singleton);

		container.Register(typeof(Application.Shared.Interfaces.ILogger<>), typeof(AspNetLogger<>), Lifestyle.Singleton);

		return container;
	}

	private static Container RegisterValidoators(this Container container)
	{
		container.Collection.Register(typeof(FluentValidation.IValidator<>), typeof(IRequestHandler<,>).Assembly);

		container.Register(typeof(IValidator<>), typeof(FluentValidator<>));

		return container;
	}

	private static Container RegisterAuditTrail(this Container container)
	{
		container.Register<IAuditTrailAppender, EFAuditTrailAppender>(Lifestyle.Scoped);

		container.Register<IAuditTrailRetriever, EFAuditTrailRetriever>(Lifestyle.Scoped);

		return container;
	}
}