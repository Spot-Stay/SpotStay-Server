using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Models;

namespace MyTourApi_Server.Services.Interfaces
{
    public interface INaverLocalApiService
    {
        Task<string> SearchLocalRawAsync(string keyword);

        Task<List<NaverLocalItem>> SearchLocalAsync(string keyword);

        Task<NaverLocalSearchResponse> SearchLocalResponseAsync(string keyword);
    }
}