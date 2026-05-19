namespace MyTourApi_Server.DTOs.Request
{
    public class FavoriteRequestDto
    {
        public int MemberId { get; set; }

        //  "TouristSpot" 또는 "Accommodation" 문자열이 들어옵니다.
        public string TargetType { get; set; } = string.Empty;

        //  관광지 ID(SpotId) 또는 숙소 ID(AccomId)
        public int TargetId { get; set; }
    }
}
