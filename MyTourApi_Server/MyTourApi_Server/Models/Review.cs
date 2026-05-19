namespace MyTourApi_Server.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int MemberId { get; set; }
        public string TargetType { get; set; } = string.Empty;
        public int TargetId { get; set; }
        public byte Rating { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // 조인 결과물을 담기 위한 확장 프로퍼티들
        public string? UserId { get; set; }
        public string? MemberName { get; set; }
        public string? TargetName { get; set; } // 마이페이지 조회용 (관광지명 또는 숙소명)
    }
}