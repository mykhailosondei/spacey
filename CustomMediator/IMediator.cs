namespace CustomMediator;

public interface IMediator
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request);
    Task SendAsync(IRequest request);
}