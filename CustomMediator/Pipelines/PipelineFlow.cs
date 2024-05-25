using System.Collections;

namespace CustomMediator.Pipelines;

public class PipelineFlow : IEnumerable<Type>
{
    private readonly IList<Type> _pipelines;
    
    public PipelineFlow()
    {
        _pipelines = new List<Type>();
    }
    
    private void ValidatePipeline(Type pipeline)
    {
        if (!pipeline.GetInterfaces().Any(i => i.IsGenericType 
                                               && (i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>) 
                                                   || i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<>))))
        {
            throw new InvalidOperationException($"Pipeline {pipeline} does not implement IPipelineBehavior");
        }
    }
    
    public void AddPipeline(Type pipeline)
    {
        ValidatePipeline(pipeline);
        _pipelines.Add(pipeline);
    }
    
    public IEnumerable<Type> GetUnitTypes()
    {
        return _pipelines.Where(type =>
            type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<>)));
    }
    
    public IEnumerable<Type> GetRequestTypes()
    {
        return _pipelines.Where(type =>
            type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>)));
    }
    
    public IEnumerator<Type> GetEnumerator()
    {
        return _pipelines.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}