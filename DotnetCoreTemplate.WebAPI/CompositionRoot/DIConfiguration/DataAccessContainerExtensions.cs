using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;
using DotnetCoreTemplate.Infrastructure.Persistence.Services;
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
			Lifestyle.Transient.CreateRegistration(typeof(EFIncludeEvaluator), container),
			Lifestyle.Transient.CreateRegistration(typeof(EFFilterEvaluator), container),
			Lifestyle.Transient.CreateRegistration(typeof(EFOrderEvaluator), container),
			Lifestyle.Transient.CreateRegistration(typeof(EFPaginationEvaluator), container),
		});

		container.Register<ISpecificationEvaluator, EFSpecificationEvaluator>();

		return container;
	}
}