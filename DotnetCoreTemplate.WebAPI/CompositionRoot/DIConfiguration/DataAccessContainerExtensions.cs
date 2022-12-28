using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Persistence.Decorator;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;
using DotnetCoreTemplate.Infrastructure.Persistence.Services;
using DotnetCoreTemplate.Infrastructure.Services;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;

public static class DataAccessContainerExtensions
{
	public static Container RegisterDataAccess(this Container container)
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

		return container;
	}
}