using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTourApi_Server.Services.Interfaces
{
    public interface IFavoriteService
    {
        // 1. 즐겨찾기 등록 계약
        Task<bool> AddFavoriteAsync(FavoriteRequestDto request);

        // 2. 특정 회원의 즐겨찾기 목록 조회 계약
        Task<List<Favorite>> GetMemberFavoritesAsync(int memberId);

        // 3. 즐겨찾기 삭제 계약
        Task<bool> RemoveFavoriteAsync(int favoriteId);
    }
}