using UserService.Models;

namespace UserService.Api.JWTWebAuthentication
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(UserLogin users);
    }
}
