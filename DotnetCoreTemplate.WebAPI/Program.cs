using DotnetCoreTemplate.WebAPI.CompositionRoot;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Extensions;
using DotnetCoreTemplate.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var composer = new SimpleInjectorComposer(builder.Services);

composer.Compose(builder.Configuration);

var app = builder.Build();

app.Services.UseComposer(composer);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(builder.Configuration["CorsPolicy"] ?? throw new InvalidOperationException("Invalid CORS Configuration"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>(composer);

app.Run();