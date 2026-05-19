using Microsoft.AspNetCore.Mvc;
using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.Models;
using MyTourApi_Server.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTourApi_Server.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        // 1. 리뷰 작성 (POST /api/reviews)
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ReviewRequestDto request)
        {
            try
            {
                if (request.MemberId <= 0)
                    return Ok(ApiResponse<object>.Fail("memberId는 로그인 성공 시 받은 회원 번호를 입력해야 합니다."));

                if (request.TargetType != "TouristSpot" && request.TargetType != "Accommodation")
                    return Ok(ApiResponse<object>.Fail("targetType은 TouristSpot 또는 Accommodation만 가능합니다."));

                if (request.Rating < 1 || request.Rating > 5)
                    return Ok(ApiResponse<object>.Fail("별점은 1점부터 5점까지 입력 가능합니다."));

                bool result = await _reviewRepository.AddAsync(request);

                if (!result)
                    return Ok(ApiResponse<object>.Fail("리뷰 등록 실패"));

                return Ok(ApiResponse<object?>.Ok(null, "리뷰 등록 성공"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }

        // 2. 특정 관광지/숙소의 리뷰 목록 조회 (GET /api/reviews?targetType=TouristSpot&targetId=1)
        [HttpGet]
        public async Task<IActionResult> GetReviews([FromQuery] string targetType, [FromQuery] int targetId)
        {
            try
            {
                if (targetType != "TouristSpot" && targetType != "Accommodation")
                    return Ok(ApiResponse<object>.Fail("targetType은 TouristSpot 또는 Accommodation만 가능합니다."));

                List<Review> result = await _reviewRepository.GetByTargetAsync(targetType, targetId);

                return Ok(ApiResponse<List<Review>>.Ok(result, "리뷰 조회 성공", result.Count));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }

        // 3. 특정 회원의 리뷰 목록 조회 (GET /api/reviews/member/1)
        [HttpGet("member/{memberId}")]
        public async Task<IActionResult> GetByMemberId(int memberId)
        {
            try
            {
                if (memberId <= 0)
                    return Ok(ApiResponse<object>.Fail("memberId는 로그인 성공 시 받은 회원 번호를 입력해야 합니다."));

                List<Review> result = await _reviewRepository.GetByMemberIdAsync(memberId);

                return Ok(ApiResponse<List<Review>>.Ok(result, "회원 리뷰 조회 성공", result.Count));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }

        // 4. 리뷰 수정 (PUT /api/reviews/5)
        [HttpPut("{reviewId}")]
        public async Task<IActionResult> Update(int reviewId, [FromBody] ReviewUpdateRequestDto request)
        {
            try
            {
                if (request.Rating < 1 || request.Rating > 5)
                    return Ok(ApiResponse<object>.Fail("별점은 1점부터 5점까지 입력 가능합니다."));

                bool result = await _reviewRepository.UpdateAsync(reviewId, request);

                if (!result)
                    return Ok(ApiResponse<object>.Fail("리뷰를 찾을 수 없습니다."));

                return Ok(ApiResponse<object?>.Ok(null, "리뷰 수정 성공"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }

        // 5. 리뷰 삭제 (DELETE /api/reviews/5)
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> Delete(int reviewId)
        {
            try
            {
                bool result = await _reviewRepository.DeleteAsync(reviewId);

                if (!result)
                    return Ok(ApiResponse<object>.Fail("리뷰를 찾을 수 없습니다."));

                return Ok(ApiResponse<object?>.Ok(null, "리뷰 삭제 성공"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }
    }
}