using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Services.Interfaces;
using MyTourApi_Server.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTourApi_Server.Services.Impls
{
    public class TouristSpotService : ITouristSpotService
    {
        private readonly ITouristSpotRepository _touristSpotRepository;

        public TouristSpotService(ITouristSpotRepository touristSpotRepository)
        {
            _touristSpotRepository = touristSpotRepository;
        }

        public async Task<List<TouristSpotResponseDto>> SearchSpotsAsync(string keyword, string region)
        {
            var spots = await _touristSpotRepository.SearchSpotsAsync(keyword, region);

            return spots.Select(s => new TouristSpotResponseDto
            {
                SpotId = s.SpotId,
                SpotName = s.Name?? "이름 없음",
                Category = s.Category ?? "미분류",
                Address = s.Address ?? "",
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                ImageUrl = s.ImageUrl ?? ""
            }).ToList();
        }

        public async Task<TouristSpotResponseDto?> GetSpotDetailAsync(int id)
        {
            var spot = await _touristSpotRepository.GetSpotByIdAsync(id);

            if (spot == null) return null;

            return new TouristSpotResponseDto
            {
                SpotId = spot.SpotId,
                SpotName = spot.Name ?? "이름 없음",
                Category = spot.Category ?? "미분류",
                Address = spot.Address ?? "",
                Latitude = spot.Latitude,
                Longitude = spot.Longitude,
                ImageUrl = spot.ImageUrl ?? ""
            };
        }
    }
}