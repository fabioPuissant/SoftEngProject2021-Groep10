using System.Diagnostics.CodeAnalysis;

namespace VmsApi.ViewModels.GetModels
{
    [ExcludeFromCodeCoverage]
    public class LoginJwtTokenDto
    {
        public LoginJwtTokenDto(string token)
        {
            JwtBearerToken = token;
        }
        public string JwtBearerToken { get; }
    }
}