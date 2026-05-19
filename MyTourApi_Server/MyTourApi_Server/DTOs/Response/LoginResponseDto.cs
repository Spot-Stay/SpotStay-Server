namespace MyTourApi_Server.DTOs.Response
{
    public class LoginResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty; // JWT 토큰 자리 (현재는 더미 토큰 반환)
        public string MemberName { get; set; } = string.Empty;
    }
}