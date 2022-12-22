using DotnetCoreTemplate.WebAPI.CompositionRoot;
using DotnetCoreTemplate.WebAPI.Middlewares;
using SimpleInjector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var container = new Container();
var configurator = new SimpleInjectorServiceConfigurator(builder.Services, builder.Configuration, container);
configurator.Configure();

var app = builder.Build();

app.Services.UseSimpleInjector(container);

container.Verify();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.UseMiddleware<ExceptionHandlingMiddleware>(container);

app.Run();