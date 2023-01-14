using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;
using DotnetCoreTemplate.Infrastructure.Persistence.Services;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Composites;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;

public static class DataAccessContainerExtensions
{
	public static Container RegisterDataAccess(this Container container)
	{
		container.Register<IUnitOfWork, EFUnitOfWork>();

		container.Register(typeof(IRepository<>), typeof(EFRepository<>));

		container.Collection.Register<IEvaluator>(new[]
		{
			Lifestyle.Transient.CreateRegistration(typeof(EFFilterEvaluator), container),
			Lifestyle.Transient.CreateRegistration(typeof(EFIncludeEvaluator), container),
			Lifestyle.Transient.CreateRegistration(typeof(EFOrderEvaluator), container),
		});
		container.Register<IEvaluator, CompositeSpecificationEvaluator>();

		container.Register<IProjector, EFProjector>();

		container.Register<IPaginator, EFPaginator>();

		return container;
	}
}