using System.Threading.Tasks;
using VmsApi.Data.Models;
using VmsApi.ViewModels.GetModels;

namespace VmsApi.Services
{
    public interface ITokenGenerator
    {
        Task<LoginJwtTokenDto> GetTokenAsync(User user);
    }
}