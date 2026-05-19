namespace MyTourApi_Server.DTOs.Request
{
    public class LoginRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}