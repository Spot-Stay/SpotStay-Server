using MyTourApi_Server.Models;

namespace MyTourApi_Server.DTOs.Response
{
    public class CampSiteSearchResponse
    {
        public string? ParkName { get; set; }

        public int Count { get; set; }

        public List<CampSite> Items { get; set; } = new List<CampSite>();
    }
}