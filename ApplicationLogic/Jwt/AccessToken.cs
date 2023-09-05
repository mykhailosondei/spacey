namespace ApplicationLogic.Jwt;

public class AccessToken
{
    public AccessToken(string token, int expiresIn)
    {
        Token = token;
        ExpiresIn = expiresIn;
    }

    public string Token { get; }
    public int ExpiresIn { get; }
}