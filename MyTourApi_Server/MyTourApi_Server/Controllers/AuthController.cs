using Microsoft.AspNetCore.Mvc;
using MyTourApi.DTOs.Request;
using MyTourApi.DTOs.Response;
using MyTourApi.Services.Interfaces;
using System.Threading.Tasks;

namespace MyTourApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // 실제 주소: api/auth
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        // 생성자를 통해 AuthService를 주입받습니다.
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new LoginResponseDto { IsSuccess = false, Message = "잘못된 요청입니다." });
            }

            var result = await _authService.LoginAsync(request);

            if (!result.IsSuccess)
            {
                return Unauthorized(result); // 401 Unauthorized 리턴
            }

            return Ok(result); // 200 OK와 함께 결과 리턴
        }
    }
}