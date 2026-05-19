namespace MyTourApi_Server.DTOs.Response
{
    public class TouristSpotResponseDto
    {
        public int SpotId { get; set; }
        public string SpotName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}