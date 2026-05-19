namespace MyTourApi_Server.Models
{
    public class Accommodation
    {
        public int AccomId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? AccomType { get; set; }
        public string? Phone { get; set; }
        public string? ImageUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? BookingUrl { get; set; }
    }

    // 거리 계산 후 응답할 데이터 (거리 포함)
    public class AccommodationWithDistance
    {
        public int AccomId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? AccomType { get; set; }
        public string? Phone { get; set; }
        public string? ImageUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? BookingUrl { get; set; }
        public double DistanceKm { get; set; }  // ← 거리 추가
    }
}