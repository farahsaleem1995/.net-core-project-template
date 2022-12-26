using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.Shared.Decorators;

public class AuditingRequestHandlerDecorator<TRequest, TResult> : IRequestHandler<TRequest, TResult>
	where TRequest : ICommand<TResult>
{
	private readonly IRequestHandler<TRequest, TResult> _decoratee;
	private readonly IUserContext _userContext;
	private readonly IAuditTrailAppender _appender;

	public AuditingRequestHandlerDecorator(
		IRequestHandler<TRequest, TResult> decoratee,
		IUserContext userContext,
		IAuditTrailAppender appender)
	{
		_decoratee = decoratee;
		_userContext = userContext;
		_appender = appender;
	}

	public async Task<TResult> Handle(TRequest request, CancellationToken cancellation)
	{
		if (_userContext.Authorized)
		{
			await _appender.Append(request, _userContext.UserId, cancellation);
		}

		return await _decoratee.Handle(request, cancellation);
	}
}