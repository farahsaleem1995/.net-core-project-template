using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotnetCoreTemplate.WebAPI.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
	private readonly IDictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

	public ExceptionHandlingMiddleware()
	{
		// Register known exception types and handlers.
		_exceptionHandlers = new Dictionary<Type, Func<HttpContext, Exception, Task>>
		{
			{typeof(NotFoundException), HandleNotFoundException},
			{typeof(SecurityException), HandleSecurityException},
			{typeof(ValidationException), HandleValidationException},
			{typeof(DomainException), HandleDomainException},
		};
	}

	private async Task HandleNotFoundException(HttpContext context, Exception exception)
	{
		var details = new ProblemDetails()
		{
			Status = StatusCodes.Status404NotFound,
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
			Title = "NotFound",
			Detail = exception?.Message,
		};

		await WriteResponse(context, details, StatusCodes.Status404NotFound);
	}

	private async Task HandleSecurityException(HttpContext context, Exception exception)
	{
		var securtyException = exception as SecurityException;
		var stausCode = securtyException!.ErrorType == SecurityError.Unauthorized
			? StatusCodes.Status401Unauthorized
			: StatusCodes.Status403Forbidden;

		var details = new ProblemDetails()
		{
			Status = stausCode,
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
			Title = "SecurityError",
			Detail = exception?.Message,
		};

		await WriteResponse(context, details, stausCode);
	}

	private async Task HandleValidationException(HttpContext context, Exception exception)
	{
		var validationException = exception as ValidationException;
		var errors = validationException!.Errors.ToDictionary(e => e.Key, e => e.Value.ToArray());

		var details = new ValidationProblemDetails(errors)
		{
			Status = StatusCodes.Status400BadRequest,
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
			Title = "BadRequest",
			Detail = exception?.Message,
		};

		await WriteResponse(context, details, StatusCodes.Status400BadRequest);
	}

	private async Task HandleDomainException(HttpContext context, Exception exception)
	{
		var details = new ProblemDetails()
		{
			Status = StatusCodes.Status400BadRequest,
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
			Title = "BadRequest",
			Detail = exception?.Message,
		};

		await WriteResponse(context, details, StatusCodes.Status400BadRequest);
	}

	private static async Task WriteResponse(HttpContext context, ProblemDetails problemDetails, int statusCode)
	{
		var response = JsonConvert.SerializeObject(problemDetails);

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = statusCode;

		await context.Response.WriteAsync(response);
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (DomainException e)
		{
			await _exceptionHandlers[e.GetType()].Invoke(context, e);
		}
		catch
		{
			await HandleUnknownException(context);
		}
	}

	private static async Task HandleUnknownException(HttpContext context)
	{
		var details = new ProblemDetails()
		{
			Status = StatusCodes.Status500InternalServerError,
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
			Title = "InternalError",
			Detail = "An error occurred while processing your request."
		};

		await WriteResponse(context, details, StatusCodes.Status500InternalServerError);
	}
}