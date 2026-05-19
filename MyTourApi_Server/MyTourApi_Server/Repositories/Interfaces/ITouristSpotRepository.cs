using MyTourApi_Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTourApi_Server.Repositories.Interfaces
{
    public interface ITouristSpotRepository
    {
        Task<List<TouristSpot>> SearchSpotsAsync(string keyword, string regionSido);
    }
}
