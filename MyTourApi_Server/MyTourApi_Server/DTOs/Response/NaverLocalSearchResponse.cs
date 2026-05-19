using MyTourApi_Server.Models;

namespace MyTourApi_Server.DTOs.Response
{
    public class NaverLocalSearchResponse
    {
        public string Keyword { get; set; } = "";
        public int Count { get; set; }
        public List<NaverLocalItem> Items { get; set; } = new List<NaverLocalItem>();
    }
}
