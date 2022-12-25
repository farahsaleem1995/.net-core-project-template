using DotnetCoreTemplate.Application.Shared.Decorators;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Services;
using DotnetCoreTemplate.Infrastructure.Identity;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimpleInjector;
using System.Text;
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
		_services.AddSwaggerGen(c =>
		{
			c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
			{
				Name = "Authorization",
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer",
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
				Description = @"JWT Authorization header using the Bearer scheme.
					Enter 'Bearer' [space] and then your token in the text input below.
					'Bearer 12345abcdef'",
			});

			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					Array.Empty<string>()
				}
			});
		});
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

		_services.AddIdentity<ApplicationUser, ApplicationRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

		_services.Configure<IdentityOptions>(options =>
		{
			options.User.RequireUniqueEmail = true;

			options.Password.RequireDigit = true;
			options.Password.RequireLowercase = false;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;
			options.Password.RequiredLength = 6;
			options.Password.RequiredUniqueChars = 1;

			options.Lockout.AllowedForNewUsers = true;
			options.Lockout.MaxFailedAccessAttempts = 5;
			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

			options.SignIn.RequireConfirmedEmail = false;
		});

		_services.Configure<TokenConfig>(_configuration.GetSection("TokenConfig"));

		_services
			.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new()
				{
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = _configuration["TokenConfig:Issuer"],
					ValidateIssuer = _configuration.GetValue<bool>("TokenConfig:ValidateIssuer"),
					ValidAudience = _configuration["TokenConfig:Audience"],
					ValidateAudience = _configuration.GetValue<bool>("TokenConfig:ValidateAudience"),
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenConfig:Key"]!)),
				};
				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var accessToken = context.Request.Query["access_token"];
						if (!string.IsNullOrEmpty(accessToken))
						{
							context.Token = accessToken;
						}

						return Task.CompletedTask;
					}
				};
			});

		_services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

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
		_container.Register(typeof(IRequestHandler<>), typeof(UnitCommandAdapter<>));
		_container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(TransactionRequestHandlerDecorator<,>));
		_container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(ValidationRequestHandlerDecorator<,>));
		_container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(SecurityRequestHandlerDecorator<,>));
		_container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(ExceptionLogRequestHandlerDecorator<,>));

		_container.Collection.Register(typeof(IEventHandler<>), typeof(IEventHandler<>).Assembly);
		_container.Register(typeof(IEventHandler<>), typeof(CompositeEventHandler<>));
		_container.Register<IEventDispatcher, EventDispatcher>();

		_container.Collection.Register(typeof(FluentValidation.IValidator<>), typeof(IRequestHandler<,>).Assembly);
		_container.Register(typeof(IValidator<>), typeof(FluentValidator<>));
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

		_container.Register<IIdentityProvider, IdentityProvider>(Lifestyle.Scoped);

		_container.Register<IUserRetriever, UserRetriever>(Lifestyle.Scoped);

		_container.Register<ITokenProvider, TokenProvider>(Lifestyle.Scoped);
	}
}