namespace MyTourApi_Server.Models
{
    public class CampSite
    {
        public int CampId { get; set; }

        public string? Name { get; set; }

        public string? ParkName { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public int? SiteCount { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}