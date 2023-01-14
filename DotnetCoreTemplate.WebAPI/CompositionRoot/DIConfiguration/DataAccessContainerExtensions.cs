using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;
using DotnetCoreTemplate.Infrastructure.Persistence.Services;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;

public static class DataAccessContainerExtensions
{
	public static Container RegisterDataAccess(this Container container)
	{
		container.Register<IUnitOfWork, EFUnitOfWork>();

		container.Register(typeof(IRepository<>), typeof(EFRepository<>));

		container.Collection.Register<ISpecificationEvaluator>(new[]
		{
			Lifestyle.Transient.CreateRegistration(typeof(EFFilterSpecificationEvaluator), container),
			Lifestyle.Transient.CreateRegistration(typeof(EFIncludeSpecificationEvaluator), container),
			Lifestyle.Transient.CreateRegistration(typeof(EFOrderSpecificationEvaluator), container),
			Lifestyle.Transient.CreateRegistration(typeof(EFPaginationSpecificationEvaluator), container),
		});
		container.Register<ISpecificationEvaluator, EFSpecificationEvaluator>();

		container.Register<ISpecificationProjector, EFFSpecificationProjector>();

		return container;
	}
}