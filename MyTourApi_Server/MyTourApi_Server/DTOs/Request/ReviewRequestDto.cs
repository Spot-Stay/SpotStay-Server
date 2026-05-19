namespace MyTourApi_Server.DTOs.Request
{
    public class ReviewRequestDto
    {
        public int MemberId { get; set; }
        public string TargetType { get; set; } = string.Empty; // "TouristSpot" 또는 "Accommodation"
        public int TargetId { get; set; }
        public int Rating { get; set; }
        public string? Content { get; set; }
    }
}