namespace MyTourApi_Server.DTOs.Request
{
    public class ReviewUpdateRequestDto
    {
        public int Rating { get; set; }
        public string? Content { get; set; }
    }
}