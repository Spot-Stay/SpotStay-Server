namespace MyTourApi_Server.Models
{
    public class Favorite
    {
        public int FavoriteId { get; set; }
        public int MemberId { get; set; }
        public string? UserId { get; set; }
        public string TargetType { get; set; } = string.Empty;
        public int TargetId { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? TargetName { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
    }
}
