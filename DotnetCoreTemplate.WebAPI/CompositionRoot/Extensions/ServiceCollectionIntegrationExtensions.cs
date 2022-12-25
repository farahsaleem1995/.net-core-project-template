using DotnetCoreTemplate.Infrastructure.Identity;
using DotnetCoreTemplate.Infrastructure.Persistence;
using DotnetCoreTemplate.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimpleInjector;
using System.Text;
using System.Text.Json.Serialization;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Extensions;

public static class ServiceCollectionIntegrationExtensions
{
	public static Container IntegrateWithServiceCollection(
		this Container container, IServiceCollection services, IConfiguration configuration)
	{
		services.AddWebApi();
		services.AddDataAceess(configuration);
		services.AddIdentity();
		services.AddAuthentication(configuration);
		services.Configure(configuration);

		container.IntegeateServices(services);

		return container;
	}

	private static void AddWebApi(this IServiceCollection services)
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
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new()
				{
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = configuration["TokenConfig:Issuer"],
					ValidateIssuer = configuration.GetValue<bool>("TokenConfig:ValidateIssuer"),
					ValidAudience = configuration["TokenConfig:Audience"],
					ValidateAudience = configuration.GetValue<bool>("TokenConfig:ValidateAudience"),
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenConfig:Key"]!)),
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

	private static void Configure(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<TokenConfig>(configuration.GetSection("TokenConfig"));

		services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
	}

	private static void IntegeateServices(this Container container, IServiceCollection services)
	{
		services.AddSimpleInjector(container, options =>
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
}