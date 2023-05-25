namespace UserService.Api.JWTWebAuthentication
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(Users users);
    }
}
