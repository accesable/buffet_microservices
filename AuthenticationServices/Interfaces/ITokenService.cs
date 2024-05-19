using Shared.Authentication;

namespace AuthenticationServices.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user, IList<string>? roles);
    }
}
