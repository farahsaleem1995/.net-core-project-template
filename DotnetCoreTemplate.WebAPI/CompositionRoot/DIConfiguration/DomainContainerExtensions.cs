﻿using DotnetCoreTemplate.Application.Shared.Decorators;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Services;
using DotnetCoreTemplate.Infrastructure.Background;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Adapters;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Composites;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;

public static class DomainContainerExtensions
{
	public static Container RegisterDomainServices(this Container container)
	{
		container.Register<ISender, Director>();
		container.Register<IDispatcher, Director>();

		container.RegisterRequestHandlers()
			.RegisterEventHandlers()
			.RegisterWorkHandlers();

		return container;
	}

	private static Container RegisterRequestHandlers(this Container container)
	{
		container.Register(typeof(IRequestHandler<,>), typeof(IRequestHandler<,>).Assembly);
		container.Register(typeof(IRequestHandler<>), typeof(UnitCommandAdapter<>));

		container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(AuditingRequestHandlerDecorator<,>));
		container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(TransactionRequestHandlerDecorator<,>));
		container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(ValidationRequestHandlerDecorator<,>));
		container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(SecurityRequestHandlerDecorator<,>));
		container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(ExceptionLogRequestHandlerDecorator<,>));

		return container;
	}

	private static Container RegisterEventHandlers(this Container container)
	{
		container.Collection.Register(typeof(IEventHandler<>), typeof(IEventHandler<>).Assembly);
		container.Register(typeof(IEventHandler<>), typeof(CompositeEventHandler<>));

		return container;
	}

	private static Container RegisterWorkHandlers(this Container container)
	{
		container.Register(typeof(IWorkHandler<>), typeof(IWorkHandler<>).Assembly);

		return container;
	}
}