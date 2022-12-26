using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Identity;
using DotnetCoreTemplate.Infrastructure.Persistence.Decorator;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;
using DotnetCoreTemplate.Infrastructure.Persistence.Services;
using DotnetCoreTemplate.Infrastructure.Services;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Extensions;

public static class InfrastructureContainerExtensions
{
	public static Container RegisterInfrastructure(this Container container)
	{
		container.Register<IUnitOfWork, EFUnitOfWork>(Lifestyle.Scoped);
		container.RegisterDecorator<IUnitOfWork, EventDispatchUnitOfWorkDecorator>(Lifestyle.Scoped);
		container.RegisterDecorator<IUnitOfWork, AuditUnitOfWorkDecorator>(Lifestyle.Scoped);

		container.Register(typeof(IRepository<>), typeof(EFRepository<>), Lifestyle.Scoped);

		container.Collection.Register<ISpecificationEvaluator>(new[]
		{
			Lifestyle.Scoped.CreateRegistration(typeof(EFFilterSpecificationEvaluator), container),
			Lifestyle.Scoped.CreateRegistration(typeof(EFIncludeSpecificationEvaluator), container),
			Lifestyle.Scoped.CreateRegistration(typeof(EFOrderSpecificationEvaluator), container),
			Lifestyle.Scoped.CreateRegistration(typeof(EFPaginationSpecificationEvaluator), container),
		});
		container.Register<ISpecificationEvaluator, EFSpecificationEvaluator>(Lifestyle.Scoped);

		container.Register<ISpecificationProjector, EFFSpecificationProjector>(Lifestyle.Scoped);

		container.Register<ITimeProvider, UtcTimeProvider>(Lifestyle.Scoped);

		container.Register<IIdentityProvider, IdentityProvider>(Lifestyle.Scoped);

		container.Register<IUserRetriever, UserRetriever>(Lifestyle.Scoped);

		container.Register<ITokenProvider, TokenProvider>(Lifestyle.Scoped);

		container.Register<IAuditTrailAppender, EFAuditTrailAppender>(Lifestyle.Scoped);

		container.Register<IAuditTrailRetriever, EFAuditTrailRetriever>(Lifestyle.Scoped);

		return container;
	}
}