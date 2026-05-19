using MyTourApi_Server.Models;

namespace MyTourApi_Server.Repositories.Interfaces
{
    public interface ICampSiteRepository
    {
        int InsertCampSites(List<CampSite> campsites);

        List<CampSite> SelectCampSites(string? parkName);

        CampSite? SelectCampSiteById(int campId);
    }
}