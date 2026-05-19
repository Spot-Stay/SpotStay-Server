using Microsoft.AspNetCore.Mvc;
using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Services.Interfaces;
using MyTourApi_Server.Models;
using System;

namespace MyTourApi_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        // 생성자를 통해 AuthService를 주입
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
        // GET /api/Auth/profile?memberId=1
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile([FromQuery] int memberId)
        {
            try
            {
                var result = await _authService.GetUserProfileAsync(memberId);

                if (result == null)
                    return NotFound(ApiResponse<object>.Fail("존재하지 않는 회원입니다."));

                return Ok(ApiResponse<UserProfileResponseDto>.Ok(result, "마이페이지 프로필 조회 성공"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }
    }
}