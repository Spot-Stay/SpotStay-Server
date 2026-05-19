using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyTourApi_Server.Models;
using MyTourApi_Server.Services;
using MyTourApi_Server.Repositories;

namespace MyTourApi_Server.Controllers
{
    [ApiController]
    [Route("api/tourapi")]
    public class TourApiController : ControllerBase
    {
        private readonly TourApiService tourApiService;
        private readonly TouristSpotImportRepository touristSpotImportRepository;

        public TourApiController(
            TourApiService tourApiService,
            TouristSpotImportRepository touristSpotImportRepository)
        {
            this.tourApiService = tourApiService;
            this.touristSpotImportRepository = touristSpotImportRepository;
        }

        // TourAPI 실시간 검색 결과 확인 (DB 저장 안 함)
        // GET /api/tourapi/search?keyword=강릉
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return Ok(ApiResponse<object>.Fail("검색어를 입력해 주세요."));
                }

                List<TouristSpot> result = await tourApiService.SearchTouristSpotsAsync(keyword);

                return Ok(ApiResponse<List<TouristSpot>>.Ok(result, "TourAPI 관광지 검색 성공", result.Count));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Fail($"TourAPI 관광지 검색 실패: {ex.Message}"));
            }
        }

        // TourAPI 검색 후 우리 DB(jjh.TouristSpot)에 자동으로 밀어넣기
        // POST /api/tourapi/search-save?keyword=강릉
        [HttpPost("search-save")]
        public async Task<IActionResult> SearchAndSave([FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return Ok(ApiResponse<object>.Fail("저장할 검색어를 입력해 주세요."));
                }

                // 1. 공공데이터로부터 데이터 긁어오기
                List<TouristSpot> result = await tourApiService.SearchTouristSpotsAsync(keyword);

                // 2. 우리 DB에 중복체크하면서 저장 (저장된 신규 개수 반환)
                int saveCount = touristSpotImportRepository.SaveTouristSpots(result);

                var responseData = new
                {
                    SearchCount = result.Count,
                    SaveCount = saveCount,
                    Items = result
                };

                return Ok(ApiResponse<object>.Ok(responseData, "TourAPI 관광지 검색 및 DB 저장 성공"));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Fail($"TourAPI 관광지 검색 및 DB 저장 실패: {ex.Message}"));
            }
        }
    }
}