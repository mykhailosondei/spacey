using CustomMediator.Pipelines;

namespace CustomMediator;

public class RequestHandlerWrapper<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, object handler, Func<Type, object> serviceFactory,
        CancellationToken token = default)
    {
        var method = handler.GetType().GetMethod("Handle")!;
        Task<TResponse> Handler() => (Task<TResponse>)method.Invoke(handler, new object?[] { request, token })!;
        var pipelineBehaviours = (IEnumerable<IPipelineBehavior<TRequest, TResponse>>)serviceFactory(typeof(IEnumerable<IPipelineBehavior<TRequest, TResponse>>));
        
        return await pipelineBehaviours
            .Reverse()
            .Aggregate((RequestHandlerDelegate<TResponse>) Handler, (next, pipeline) => () => pipeline.Handle(request, next, token))();
    }
}

public class RequestHandlerWrapper<TRequest> where TRequest : notnull
{
    public async Task Handle(TRequest request, object handler, Func<Type, object> serviceFactory,
        CancellationToken token = default)
    {
        var method = handler.GetType().GetMethod("Handle")!;
        Task Handler() => (Task)method.Invoke(handler, new object?[] { request, token })!;
        var pipelineBehaviours = (IEnumerable<IPipelineBehavior<TRequest>>)serviceFactory(typeof(IEnumerable<IPipelineBehavior<TRequest>>));
        
        await pipelineBehaviours
            .Reverse()
            .Aggregate((RequestHandlerDelegate) Handler, (next, pipeline) => () => pipeline.Handle(request, next, token))();
    }
}