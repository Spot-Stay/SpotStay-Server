using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Models;
using MyTourApi_Server.Repositories.Interfaces;
using MyTourApi_Server.Services.Interfaces;

namespace MyTourApi_Server.Services.Impls
{
    public class CampSiteService : ICampSiteService
    {
        private readonly ICampSiteRepository campSiteRepository;

        public CampSiteService(ICampSiteRepository campSiteRepository)
        {
            this.campSiteRepository = campSiteRepository;
        }

        public CampSiteSearchResponse GetCampSites(string? parkName)
        {
            List<CampSite> list = campSiteRepository.SelectCampSites(parkName);

            return new CampSiteSearchResponse
            {
                ParkName = parkName,
                Count = list.Count,
                Items = list
            };
        }

        public CampSite? GetCampSiteById(int campId)
        {
            return campSiteRepository.SelectCampSiteById(campId);
        }
    }
}