﻿using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Background;
using DotnetCoreTemplate.Infrastructure.Identity;
using DotnetCoreTemplate.Infrastructure.Interfaces;
using DotnetCoreTemplate.Infrastructure.Persistence.Decorator;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;
using DotnetCoreTemplate.Infrastructure.Persistence.Services;
using DotnetCoreTemplate.Infrastructure.Services;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using Quartz;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;

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

		container.Register<IQueueProvider, QuartzQueueProvider>(Lifestyle.Singleton);

		container.Register<IWorkExecutor, WorkExecutor>();

		container.RegisterQuartzJob();

		return container;
	}

	private static void RegisterQuartzJob(this Container container)
	{
		var jobTypesToRegisterOptions = new TypesToRegisterOptions
		{
			IncludeGenericTypeDefinitions = true,
			IncludeComposites = false
		};

		var assemblies = new[] { typeof(QuartzJob<>).Assembly };
		var types = container.GetTypesToRegister<IJob>(assemblies, jobTypesToRegisterOptions);
		foreach (var type in types)
		{
			container.Register(type);
		};
	}
}