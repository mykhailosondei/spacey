namespace ApplicationLogic.Jwt;

public interface ITokenGenerator
{
    Task<AccessToken> GenerateAccessToken(Guid id, Guid hostId, string userName, string email, bool isHost);
}