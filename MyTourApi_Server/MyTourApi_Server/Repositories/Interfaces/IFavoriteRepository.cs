using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTourApi_Server.Repositories.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<bool> AddAsync(FavoriteRequestDto request);
        Task<List<Favorite>> GetByMemberIdAsync(int memberId);
        Task<bool> DeleteAsync(int favoriteId);
    }
}