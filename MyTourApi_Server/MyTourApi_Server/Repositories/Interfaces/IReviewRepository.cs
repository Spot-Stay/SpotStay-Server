using MyTourApi_Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyTourApi_Server.DTOs.Request;

namespace MyTourApi_Server.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<bool> AddAsync(ReviewRequestDto request);
        Task<List<Review>> GetByTargetAsync(string targetType, int targetId);

        Task<List<Review>> GetByMemberIdAsync(int memberId);

        Task<bool> UpdateAsync(int reviewId, ReviewUpdateRequestDto request);

        Task<bool> DeleteAsync(int reviewId);
    }
}