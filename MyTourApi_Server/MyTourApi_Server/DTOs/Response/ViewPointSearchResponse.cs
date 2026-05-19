using MyTourApi_Server.Models;

namespace MyTourApi_Server.DTOs.Response
{
    public class ViewPointSearchResponse
    {
        public string ParkName { get; set; } = "";

        public int Count { get; set; }

        public List<ViewPoint> Items { get; set; } = new List<ViewPoint>();
    }
}
