using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Shared;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;
using SimpleInjector;
using System.Collections.Concurrent;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public class Director : ISender, IDispatcher
{
	private static readonly ConcurrentDictionary<Type, RequestHandlerWrapperBase> _requestHandlers = new();
	private static readonly ConcurrentDictionary<Type, EventHandlerWrapperBase> _eventHandlers = new();

	private readonly Container _container;

	public Director(Container container)
	{
		_container = container;
	}

	public async Task<TResult> Send<TResult>(IRequest<TResult> request, CancellationToken cancellation)
	{
		var requestType = request.GetType();

		var wrapper = GetOrAddRequestHandler<TResult>(requestType);

		return await wrapper.Handle(_container, request, cancellation);
	}

	public RequestHandlerWrapper<TResult> GetOrAddRequestHandler<TResult>(Type requestType)
	{
		return (RequestHandlerWrapper<TResult>)_requestHandlers.GetOrAdd(requestType, _ =>
		{
			var wrapper = Activator.CreateInstance(typeof(RequestHandlerWrapperImpl<,>)
				.MakeGenericType(requestType, typeof(TResult)));

			if (wrapper == null)
			{
				throw new InvalidOperationException($"Could not create wrapper type for {requestType}");
			}

			return (RequestHandlerWrapperBase)wrapper;
		});
	}

	public async Task Dispatch(DomainEvent domainEvent, CancellationToken cancellation)
	{
		var eventType = domainEvent.GetType();

		var wrapper = GetOrAddEventHandler(eventType);

		await wrapper.Handle(_container, domainEvent, cancellation);
	}

	public EventHandlerWrapperBase GetOrAddEventHandler(Type eventType)
	{
		return _eventHandlers.GetOrAdd(eventType, _ =>
		{
			var wrapper = Activator.CreateInstance(typeof(EventHandlerWrapperImpl<>).MakeGenericType(eventType));

			if (wrapper == null)
			{
				throw new InvalidOperationException($"Could not create wrapper type for {eventType}");
			}

			return (EventHandlerWrapperBase)wrapper;
		});
	}
}