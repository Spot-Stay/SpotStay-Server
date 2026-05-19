using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.DTOs.Response;

namespace MyTourApi_Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task<UserProfileResponseDto?> GetUserProfileAsync(int memberId);
    }
}