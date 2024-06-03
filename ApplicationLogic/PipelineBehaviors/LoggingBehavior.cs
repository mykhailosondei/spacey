using CustomMediator.Pipelines;

namespace ApplicationLogic.PipelineBehaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Handling request of type {request.GetType().Name} with response of type {typeof(TResponse).Name}");
        var response = await next();
        Console.WriteLine($"Handled request of type {request.GetType().Name} with response of type {typeof(TResponse).Name}");
        return response;
    }
}