using MyTourApi.DTOs.Request;
using MyTourApi.DTOs.Response;
using MyTourApi.Repositories.Interfaces;
using MyTourApi.Services.Interfaces;
using System.Threading.Tasks;

namespace MyTourApi.Services.Impls
{
    public class AuthService : IAuthService
    {
        private readonly IMemberRepository _memberRepository;

        // 생성자를 통해 가짜 데이터 대신 진짜 DB 레포지토리를 주입받음
        public AuthService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            // 1. 진짜 DB에 가서 해당 가입 ID가 있는지 확인
            var member = await _memberRepository.GetByUserIdAsync(request.UserId);

            // 2. ID가 없거나 비밀번호가 틀린 경우 (※실무에선 해시 암호화 검증을 하지만 우선 평문 비교)
            if (member == null || member.Password != request.Password)
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    Message = "아이디 또는 비밀번호가 일치하지 않습니다."
                };
            }

            // 3. 인증 성공 시 DB 데이터를 DTO에 담아서 반환!
            return new LoginResponseDto
            {
                IsSuccess = true,
                Message = "로그인에 성공했습니다.",
                Token = "real-database-verified-token-1234", // 나중에 JWT 토큰 연동 가능
                MemberName = member.Name
            };
        }
    }
}