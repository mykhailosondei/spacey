namespace ApplicationLogic.Jwt;

public interface ITokenGenerator
{
    Task<AccessToken> GenerateAccessToken(Guid id, string userName, string email);
}