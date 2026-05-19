using MyTourApi.DTOs.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTourApi.Services.Interfaces
{
    public interface ITouristSpotService
    {
        Task<List<TouristSpotResponseDto>> SearchSpotsAsync(string keyword, string region);
        Task<TouristSpotResponseDto?> GetSpotDetailAsync(int id);
    }
}