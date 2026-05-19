namespace MyTourApi_Server.Models
{
    public class ViewPoint
    {
        public int ViewPointId { get; set; }

        public string? Name { get; set; }

        public string? ParkName { get; set; }

        public string? Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
