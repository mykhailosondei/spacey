using CustomMediator.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace CustomMediator.ServiceRegisterers;

public static class MediatorInitializer
{
    public static void InitializeHandlers(IServiceCollection services, IDictionary<Type, Type> handlersDictionary)
    {
        foreach (var (_, handlerType) in handlersDictionary)
        {
            services.AddTransient(handlerType);
        }
    }
    
    private static bool TryMakeBehaviorGenericType(Type behavior, Type requestType, Type responseType, out Type behaviorType)
    {
        try
        {
            behaviorType = behavior.MakeGenericType(requestType, responseType);
            return true;
        } 
        catch
        {
            behaviorType = null;
            return false;
        }
    }
    
    private static bool TryMakeBehaviorGenericType(Type behavior, Type requestType, out Type behaviorType)
    {
        try
        {
            behaviorType = behavior.MakeGenericType(requestType);
            return true;
        } 
        catch
        {
            behaviorType = null;
            return false;
        }
    }
    
    public static void InitializePipelines(IServiceCollection services, IDictionary<Type, Type> handlersDictionary, PipelineFlow flow)
    {
        foreach (var (requestType,_) in handlersDictionary)
        {
            if(requestType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>)))
            {
                var responseType = requestType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>)).GetGenericArguments()[0];
                var pipelineType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);
                var requestTypeBehaviors = flow.GetRequestTypes();
                foreach (var behavior in requestTypeBehaviors)
                {
                    if(TryMakeBehaviorGenericType(behavior, requestType, responseType, out var behaviorType))
                        services.AddScoped(pipelineType, behaviorType);
                }
            }
            else
            {
                var unitTypeBehaviors = flow.GetUnitTypes();
                var pipelineType = typeof(IPipelineBehavior<>).MakeGenericType(requestType);
                foreach (var behavior in unitTypeBehaviors)
                {
                    if(TryMakeBehaviorGenericType(behavior, requestType, out var behaviorType))
                        services.AddScoped(pipelineType, behaviorType);
                }
            }
        }
    }
}