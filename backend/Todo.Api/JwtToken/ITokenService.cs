namespace Todo.Api.JwtToken
{
    public interface ITokenService
    {
        string CreateToken(string userName);
    }
}
