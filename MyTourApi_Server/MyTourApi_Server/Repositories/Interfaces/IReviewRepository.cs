using MyTourApi_Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyTourApi_Server.DTOs.Request;

namespace MyTourApi_Server.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        // 1. 리뷰 등록
        Task<bool> AddAsync(ReviewRequestDto request);

        // 2. 관광지 또는 숙소 상세페이지용 리뷰 목록 조회
        Task<List<Review>> GetByTargetAsync(string targetType, int targetId);

        // 3. 마이페이지용 특정 회원의 리뷰 목록 조회
        Task<List<Review>> GetByMemberIdAsync(int memberId);

        // 4. 리뷰 수정
        Task<bool> UpdateAsync(int reviewId, ReviewUpdateRequestDto request);

        // 5. 리뷰 삭제
        Task<bool> DeleteAsync(int reviewId);
    }
}