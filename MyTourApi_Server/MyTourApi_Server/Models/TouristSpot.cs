namespace MyTourApi_Server.Models
{
    public class TouristSpot
    {
        public int SpotId { get; set; }
        public int ContentId { get; set; }
        public string? Name { get; set; }        
        public string? Address { get; set; }      
        public string? Category { get; set; }    
        public string? Description { get; set; }
        public string? Phone { get; set; }
        public string? Homepage { get; set; }
        public string? ImageUrl { get; set; }    
        public double Latitude { get; set; }     
        public double Longitude { get; set; }    
        public string? RegionSido { get; set; }
        public string? RegionSigungu { get; set; }
    }
}
    