namespace MyTourApi_Server.DTOs.Response
{
    public class UserProfileResponseDto
    {
        public int MemberId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
    }
}
