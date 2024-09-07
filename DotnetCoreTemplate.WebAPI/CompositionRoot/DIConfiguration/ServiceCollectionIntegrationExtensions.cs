using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Identity;
using DotnetCoreTemplate.Infrastructure.Persistence;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Factories;
using DotnetCoreTemplate.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using SimpleInjector;
using SimpleInjector.Integration.ServiceCollection;
using System.Text;
using System.Text.Json.Serialization;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;

public static class ServiceCollectionIntegrationExtensions
{
	public static Container IntegrateWithServiceCollection(this Container container,
		IServiceCollection services,
		IConfiguration configuration,
		Action<SimpleInjectorAddOptions> setupAction)
	{
		services.AddWebApi(configuration);
		services.AddDataAceess(configuration);
		services.AddIdentity();
		services.AddAuthentication(configuration);
		services.AddQuartz();

		services.AddAutoMapper(typeof(ISender).Assembly);

		services.Configure(configuration);

		services.AddSimpleInjector(container, setupAction);

		return container;
	}

	private static void AddWebApi(this IServiceCollection services, IConfiguration configuration)
	{
		services
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

		services.AddRazorPages();
		services.AddEndpointsApiExplorer();

		services.AddLogging();

		services.AddHttpContextAccessor();

		services.AddLocalization(options => options.ResourcesPath = "Resources");

		services.AddSwagger();

		services.AddCors(options =>
		{
			options.AddPolicy(configuration["CorsPolicy"] ?? throw new InvalidOperationException("Invalid CORS Configuration"),
				policy =>
				{
					policy.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials()
					.SetIsOriginAllowed(_ => true);
				});
		});
	}

	private static void AddSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(c =>
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
	}

	private static void AddDataAceess(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<ApplicationDbContext>(options =>
		{
			var connectionString = configuration.GetConnectionString("Default");
			options.UseSqlServer(connectionString, sql =>
			{
				sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
			});
		});
	}

	private static void AddIdentity(this IServiceCollection services)
	{
		services.AddIdentity<ApplicationUser, ApplicationRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

		services.Configure<IdentityOptions>(options =>
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
	}

	private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				var tokenSettings = configuration.GetOrThrow<TokenProvider.Settings>("TokenSettings");

				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new()
				{
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = tokenSettings.Issuer,
					ValidateIssuer = tokenSettings.ValidateIssuer,
					ValidAudience = tokenSettings.Audience,
					ValidateAudience = tokenSettings.ValidateAudience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Key)),
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
	}

	private static void AddQuartz(this IServiceCollection services)
	{
		services.AddQuartz(quartz =>
		{
			quartz.UseJobFactory<SimpleInjectorJobFactory>();
		});

		services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
	}

	private static void Configure(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
	}
}