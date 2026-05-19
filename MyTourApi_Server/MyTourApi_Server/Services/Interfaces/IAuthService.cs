using MyTourApi.DTOs.Request;
using MyTourApi.DTOs.Response;
using System.Threading.Tasks;

namespace MyTourApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}