using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Models;

namespace MyTourApi_Server.Services.Interfaces
{
    public interface ICampSiteService
    {
        CampSiteSearchResponse GetCampSites(string? parkName);

        CampSite? GetCampSiteById(int campId);
    }
}