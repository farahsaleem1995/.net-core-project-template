﻿using DotnetCoreTemplate.Application.Shared.Decorators;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Services;
using DotnetCoreTemplate.Infrastructure.Persistence;
using DotnetCoreTemplate.Infrastructure.Persistence.Decorator;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;
using DotnetCoreTemplate.Infrastructure.Persistence.Services;
using DotnetCoreTemplate.Infrastructure.Services;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Adapters;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Utils;
using DotnetCoreTemplate.WebAPI.Extensions;
using DotnetCoreTemplate.WebAPI.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;
using System.Text.Json.Serialization;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot;

public class SimpleInjectorServiceConfigurator
{
	private readonly IServiceCollection _services;
	private readonly IConfiguration _configuration;
	private readonly Container _container;

	public SimpleInjectorServiceConfigurator(
		IServiceCollection services,
		IConfiguration configuration,
		Container container)
	{
		_services = services;
		_configuration = configuration;
		_container = container;
	}

	public void Configure()
	{
		InitializeServices();

		InitializeContainer();
	}

	private void InitializeServices()
	{
		_services
			.AddControllers(options =>
			{
				options.UseGeneralRoutePrefix("api/");
				options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
				options.ModelValidatorProviders.Clear();
			})
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			});

		_services.AddRazorPages();
		_services.AddEndpointsApiExplorer();
		_services.AddSwaggerGen();
		_services.AddLogging();
		_services.AddHttpContextAccessor();
		_services.AddLocalization(options => options.ResourcesPath = "Resources");

		_services.AddDbContext<ApplicationDbContext>(options =>
		{
			var connectionString = _configuration.GetConnectionString("Default");
			options.UseSqlServer(connectionString, sql =>
			{
				sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
			});
		});

		_services.Configure<RouteOptions>(options =>
		{
			options.LowercaseUrls = true;
		});

		_services.AddSimpleInjector(_container, options =>
		{
			options.AddAspNetCore()
				.AddControllerActivation()
				.AddViewComponentActivation()
				.AddPageModelActivation()
				.AddTagHelperActivation();

			options.AddLogging();
			options.AddLocalization();
		});
	}

	private void InitializeContainer()
	{
		RegisterApplication();
		RegisterApi();
		RegisterInfrastructure();
	}

	private void RegisterApplication()
	{
		_container.Register(typeof(IRequestHandler<,>), typeof(IRequestHandler<,>).Assembly);
		_container.Register(typeof(IRequestHandler<>), typeof(UnitRequestAdapter<>));
		_container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(TransactionRequestHandlerDecorator<,>));
		_container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(ValidationRequestHandlerDecorator<,>));
		_container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(SecurityRequestHandlerDecorator<,>));
		_container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(ExceptionLogRequestHandlerDecorator<,>));

		_container.Collection.Register(typeof(IEventHandler<>), typeof(IEventHandler<>).Assembly);
		_container.Register(typeof(IEventHandler<>), typeof(CompositeEventHandler<>));
		_container.Register<IEventDispatcher, EventDispatcher>();

		_container.Collection.Register(typeof(FluentValidation.IValidator<>), typeof(IRequestHandler<,>).Assembly);
		_container.Register(typeof(Application.Shared.Interfaces.IValidator<>), typeof(FluentValidator<>));
	}

	private void RegisterApi()
	{
		_container.Register<IUserContext, AspNetUserContextAdapter>(Lifestyle.Scoped);

		_container.Register<IDirector, Director>(Lifestyle.Singleton);

		_container.Register(typeof(ILocalCache<,>), typeof(ConcurrentLocalCache<,>), Lifestyle.Singleton);

		_container.Register(typeof(Application.Shared.Interfaces.ILogger<>), typeof(AspNetLogger<>), Lifestyle.Singleton);
	}

	private void RegisterInfrastructure()
	{
		_container.Register<IUnitOfWork, EFUnitOfWork>(Lifestyle.Scoped);
		_container.RegisterDecorator<IUnitOfWork, EventDispatchUnitOfWorkDecorator>(Lifestyle.Scoped);
		_container.RegisterDecorator<IUnitOfWork, AuditUnitOfWorkDecorator>(Lifestyle.Scoped);

		_container.Register(typeof(IRepository<>), typeof(EFRepository<>), Lifestyle.Scoped);

		_container.Collection.Register<ISpecificationEvaluator>(new[]
		{
			Lifestyle.Scoped.CreateRegistration(typeof(EFFilterSpecificationEvaluator), _container),
			Lifestyle.Scoped.CreateRegistration(typeof(EFIncludeSpecificationEvaluator), _container),
			Lifestyle.Scoped.CreateRegistration(typeof(EFOrderSpecificationEvaluator), _container),
			Lifestyle.Scoped.CreateRegistration(typeof(EFPaginationSpecificationEvaluator), _container),
		});
		_container.Register<ISpecificationEvaluator, EFSpecificationEvaluator>(Lifestyle.Scoped);

		_container.Register<ISpecificationProjector, EFFSpecificationProjector>(Lifestyle.Scoped);

		_container.Register<ITimeProvider, UtcTimeProvider>(Lifestyle.Scoped);
	}
}