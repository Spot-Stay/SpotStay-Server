using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyTourApi_Server.Models;
using MyTourApi_Server.Services;

namespace MyTourApi_Server.Controllers
{
    [ApiController]
    [Route("api/naver")]
    public class NaverLocalController : ControllerBase
    {
        private readonly NaverLocalApiService naverLocalApiService;

        public NaverLocalController(NaverLocalApiService naverLocalApiService)
        {
            this.naverLocalApiService = naverLocalApiService;
        }

        // 네이버가 돌려주는 가공되지 않은 생 원본 JSON 확인용
        // GET /api/naver/raw?keyword=강릉 숙소
        [HttpGet("raw")]
        public async Task<IActionResult> GetRaw([FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return Ok(ApiResponse<object>.Fail("검색어를 입력해 주세요."));
                }

                string json = await naverLocalApiService.SearchLocalRawAsync(keyword);

                return Ok(ApiResponse<string>.Ok(json, "네이버 지역 검색 원본 JSON 조회 성공"));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Fail($"네이버 지역 검색 원본 JSON 조회 실패: {ex.Message}"));
            }
        }

        // HTML 태그(<b> 등)가 전부 제거되고 정제된 데이터 확인용
        // GET /api/naver/search?keyword=강릉 숙소
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return Ok(ApiResponse<object>.Fail("검색어를 입력해 주세요."));
                }

                List<NaverLocalItem> result = await naverLocalApiService.SearchLocalAsync(keyword);

                return Ok(ApiResponse<List<NaverLocalItem>>.Ok(result, "네이버 지역 검색 성공", result.Count));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Fail($"네이버 지역 검색 실패: {ex.Message}"));
            }
        }
    }
}