namespace MyTourApi_Server.DTOs.Request
{
    public class NaverLocalSearchRequest
    {
        public string Keyword { get; set; } = "";
        public int Display { get; set; } = 5;
    }
}
