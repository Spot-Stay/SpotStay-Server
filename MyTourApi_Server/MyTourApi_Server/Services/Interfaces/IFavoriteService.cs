using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTourApi_Server.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<bool> AddFavoriteAsync(FavoriteRequestDto request);

        Task<List<Favorite>> GetMemberFavoritesAsync(int memberId);

        Task<bool> RemoveFavoriteAsync(int favoriteId);
    }
}