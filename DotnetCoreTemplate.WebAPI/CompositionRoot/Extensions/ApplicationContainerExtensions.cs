using DotnetCoreTemplate.Application.Shared.Decorators;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Services;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Adapters;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Composites;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Extensions;

public static class ApplicationContainerExtensions
{
	public static Container RegisterApplication(this Container container)
	{
		container.Register(typeof(IRequestHandler<,>), typeof(IRequestHandler<,>).Assembly);
		container.Register(typeof(IRequestHandler<>), typeof(UnitCommandAdapter<>));
		container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(AuditingRequestHandlerDecorator<,>));
		container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(TransactionRequestHandlerDecorator<,>));
		container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(ValidationRequestHandlerDecorator<,>));
		container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(SecurityRequestHandlerDecorator<,>));
		container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(ExceptionLogRequestHandlerDecorator<,>));

		container.Collection.Register(typeof(IEventHandler<>), typeof(IEventHandler<>).Assembly);
		container.Register(typeof(IEventHandler<>), typeof(CompositeEventHandler<>));
		container.Register<IEventDispatcher, EventDispatcher>();

		container.Collection.Register(typeof(FluentValidation.IValidator<>), typeof(IRequestHandler<,>).Assembly);
		container.Register(typeof(IValidator<>), typeof(FluentValidator<>));

		return container;
	}
}