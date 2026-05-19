using Microsoft.AspNetCore.Mvc;
using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Models;
using MyTourApi_Server.Services.Interfaces;

namespace MyTourApi_Server.Controllers
{
    [ApiController]
    [Route("api/naver")]
    public class NaverLocalController : ControllerBase
    {
        private readonly INaverLocalApiService naverLocalApiService;

        public NaverLocalController(INaverLocalApiService naverLocalApiService)
        {
            this.naverLocalApiService = naverLocalApiService;
        }

        // GET /api/naver/raw?keyword=강남맛집
        [HttpGet("raw")]
        public async Task<IActionResult> GetRaw([FromQuery] string keyword)
        {
            try
            {
                string json = await naverLocalApiService.SearchLocalRawAsync(keyword);

                return Ok(ApiResponse<object>.Ok(
                    json,
                    "네이버 지역 검색 원본 JSON 조회 성공"
                ));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Fail(
                    "네이버 지역 검색 원본 JSON 조회 실패 : " + ex.Message
                ));
            }
        }

        // GET /api/naver/search?keyword=강남맛집
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            try
            {
                NaverLocalSearchResponse result =
                    await naverLocalApiService.SearchLocalResponseAsync(keyword);

                return Ok(ApiResponse<object>.Ok(
                    result,
                    $"네이버 검색 결과 {result.Count}건"
                ));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Fail(
                    "네이버 지역 검색 실패 : " + ex.Message
                ));
            }
        }
    }
}