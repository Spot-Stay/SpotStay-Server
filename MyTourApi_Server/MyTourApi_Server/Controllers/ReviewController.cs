using Microsoft.AspNetCore.Mvc;
using MyTourApi_Server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTourApi_Server.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        public ReviewController()
        {
        }

        // 1. 리뷰 작성 (POST /api/reviews)
        [HttpPost]
        public async Task<IActionResult> Add()
        {
            try
            {
                await Task.CompletedTask;
                // TODO: 리뷰 저장 로직 연동 필요
                return Ok(ApiResponse<object?>.Ok(null, "리뷰 등록 기능 구현 예정"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }

        // 2. 특정 관광지/숙소의 리뷰 목록 조회 (GET /api/reviews)
        [HttpGet]
        public async Task<IActionResult> GetReviews([FromQuery] string targetType, [FromQuery] int targetId)
        {
            try
            {
                await Task.CompletedTask;
                // TODO: 리뷰 목록 조회 로직 연동 필요
                return Ok(ApiResponse<object?>.Ok(null, "리뷰 목록 조회 기능 구현 예정"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }

        // 3. 리뷰 삭제 (DELETE /api/reviews/5)
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> Delete(int reviewId)
        {
            try
            {
                await Task.CompletedTask;
                // TODO: 리뷰 삭제 로직 연동 필요
                return Ok(ApiResponse<object?>.Ok(null, "리뷰 삭제 기능 구현 예정"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }
    }
}