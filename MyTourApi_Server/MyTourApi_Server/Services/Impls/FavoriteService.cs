using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.Models;
using MyTourApi_Server.Repositories.Interfaces;
using MyTourApi_Server.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTourApi_Server.Services.Impls
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public Task<bool> AddFavoriteAsync(FavoriteRequestDto request)
        {
            return _favoriteRepository.AddAsync(request);
        }

        public Task<List<Favorite>> GetMemberFavoritesAsync(int memberId)
        {
            return _favoriteRepository.GetByMemberIdAsync(memberId);
        }

        public Task<bool> RemoveFavoriteAsync(int favoriteId)
        {
            return _favoriteRepository.DeleteAsync(favoriteId);
        }
    }
}