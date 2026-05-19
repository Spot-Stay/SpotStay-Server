using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Repositories.Interfaces;
using MyTourApi_Server.Services.Interfaces;


namespace MyTourApi_Server.Services.Impls
{
    public class AuthService : IAuthService
    {
        private readonly IMemberRepository _memberRepository;

        public AuthService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var member = await _memberRepository.GetByUserIdAsync(request.UserId);

            if (member == null || member.Password != request.Password)
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    Message = "아이디 또는 비밀번호가 일치하지 않습니다."
                };
            }

            return new LoginResponseDto
            {
                IsSuccess = true,
                Message = "로그인에 성공했습니다.",
                Token = "real-database-verified-token-1234", // 나중에 JWT 토큰 연동 가능
                MemberName = member.Name
            };
        }
        public async Task<UserProfileResponseDto?> GetUserProfileAsync(int memberId)
        {
            var member = await _memberRepository.GetProfileByIdAsync(memberId);

            if (member == null) return null;

            return new UserProfileResponseDto
            {
                MemberId = member.MemberId,
                UserId = member.UserId ?? "",
                MemberName = member.Name ?? "이름 없음"
            };
        }

    }
}