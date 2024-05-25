using System.Runtime.CompilerServices;
using CustomMediator.Pipelines;

namespace CustomMediator;

public class Mediator : IMediator
{
    private readonly Func<Type, object> _serviceFactory;
    private readonly IDictionary<Type, Type> _handlers;

    public Mediator(Func<Type, object> serviceFactory, IDictionary<Type, Type> handlers)
    {
        _serviceFactory = serviceFactory;
        _handlers = handlers;
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        var requestType = request.GetType();
        var handlerType = _handlers[requestType];
        if (handlerType == null)
        {
            throw new InvalidOperationException($"Handler not found for request type {requestType}");
        }
        var handler = _serviceFactory(handlerType);
        var handlerWrapperType = typeof(RequestHandlerWrapper<,>).MakeGenericType(requestType, typeof(TResponse));
        var handlerWrapper = Activator.CreateInstance(handlerWrapperType)!;
        var handlerWrapperMethod = handlerWrapperType.GetMethod("Handle")!;
        
        return await (Task<TResponse>)handlerWrapperMethod.Invoke(handlerWrapper, new [] { request, handler, _serviceFactory, CancellationToken.None })!;
    }
    
    public async Task SendAsync(IRequest request)
    {
        var requestType = request.GetType();
        var handlerType = _handlers[requestType];
        if (handlerType == null)
        {
            throw new InvalidOperationException($"Handler not found for request type {requestType}");
        }
        var handler = _serviceFactory(handlerType);
        var handlerWrapperType = typeof(RequestHandlerWrapper<>).MakeGenericType(requestType);
        var handlerWrapper = Activator.CreateInstance(handlerWrapperType)!;
        var handlerWrapperMethod = handlerWrapperType.GetMethod("Handle")!;
        
        await (Task)handlerWrapperMethod.Invoke(handlerWrapper, new [] { request, handler, _serviceFactory, CancellationToken.None })!;
    }

    public static IDictionary<Type, Type> InitializeHandlerDictionary(Type[] markers)
    {
        var dictionary = new Dictionary<Type, Type>();
        
        foreach (var marker in markers)
        {
            var handlerTypes = marker.Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));
            foreach (var handlerType in handlerTypes)
            {
                var requestType = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)).GetGenericArguments()[0];
                dictionary.Add(requestType, handlerType);
            }
            var nonGenericHandlerTypes = marker.Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<>)));
            foreach (var handlerType in nonGenericHandlerTypes)
            {
                var requestType = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<>)).GetGenericArguments()[0];
                dictionary.Add(requestType, handlerType);
            }
        }
        
        return dictionary;
    }
}