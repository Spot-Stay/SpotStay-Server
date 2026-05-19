using Microsoft.AspNetCore.Mvc;
using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.Models;
using MyTourApi_Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTourApi_Server.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        // 1. 즐겨찾기 추가 (POST /api/favorites)
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] FavoriteRequestDto request)
        {
            try
            {
                if (request.MemberId <= 0)
                {
                    return Ok(ApiResponse<object>.Fail("memberId는 로그인 성공 시 받은 회원 번호를 입력해야 합니다."));
                }

                if (request.TargetType != "TouristSpot" && request.TargetType != "Accommodation")
                {
                    return Ok(ApiResponse<object>.Fail("targetType은 TouristSpot 또는 Accommodation만 가능합니다."));
                }

                bool success = await _favoriteService.AddFavoriteAsync(request);
                if (!success)
                {
                    return Ok(ApiResponse<object>.Fail("이미 즐겨찾기에 등록되어 있거나 등록에 실패했습니다."));
                }

                return Ok(ApiResponse<object?>.Ok(null, "즐겨찾기 등록 성공"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }

        // 2. 마이페이지 회원별 즐겨찾기 조회 (GET /api/favorites?memberId=3)
        [HttpGet]
        public async Task<IActionResult> GetByMemberId([FromQuery] int memberId)
        {
            try
            {
                if (memberId <= 0)
                {
                    return Ok(ApiResponse<object>.Fail("memberId는 로그인 성공 시 받은 회원 번호를 입력해야 합니다."));
                }

                var list = await _favoriteService.GetMemberFavoritesAsync(memberId);

                return Ok(ApiResponse<List<Favorite>>.Ok(list, "즐겨찾기 조회 성공"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }

        // 3. 즐겨찾기 해제 (DELETE /api/favorites/5)
        [HttpDelete("{favoriteId}")]
        public async Task<IActionResult> Delete(int favoriteId)
        {
            try
            {
                bool success = await _favoriteService.RemoveFavoriteAsync(favoriteId);
                if (!success)
                {
                    return Ok(ApiResponse<object>.Fail("즐겨찾기 정보를 찾을 수 없습니다."));
                }

                return Ok(ApiResponse<object?>.Ok(null, "즐겨찾기 삭제 성공"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }
    }
}