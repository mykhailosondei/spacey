using Amazon.SecurityToken.Model;

namespace ApplicationLogic.UserIdLogic;

public class UserIdStorageProvider : IUserIdSetter, IUserIdGetter
{
    private Guid _userId;
    
    public Guid UserId
    {
        get => GetUserIdValidation(_userId);
        set => SetUserIdValidation(value);
    }

    private Guid GetUserIdValidation(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new InvalidIdentityTokenException(userId.ToString());
        }

        return userId;
    }

    private void SetUserIdValidation(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new InvalidIdentityTokenException(userId.ToString());
        }

        _userId = userId;
    }
}