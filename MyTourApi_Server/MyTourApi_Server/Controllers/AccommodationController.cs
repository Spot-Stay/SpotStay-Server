using Microsoft.AspNetCore.Mvc;
using MyTourApi_Server.Models;
using MyTourApi_Server.Services;
using System;
using System.Collections.Generic;

namespace MyTourApi_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccommodationController : ControllerBase
    {
        private readonly AccommodationService _service;

        public AccommodationController(AccommodationService service)
        {
            _service = service;
        }

        // GET /api/accommodation/nearby?lat=37.5796&lng=126.9770
        [HttpGet("nearby")]
        public IActionResult GetNearby(double lat, double lng, int top = 20)
        {
            if (lat == 0 || lng == 0)
                return BadRequest(ApiResponse<object>.Fail("위도/경도를 입력해주세요."));

            try
            {
                var result = _service.GetNearby(lat, lng, top);
                return Ok(ApiResponse<List<AccommodationWithDistance>>.Ok(
                    result,
                    $"주변 숙소 {result.Count}개 조회 성공",
                    result.Count
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }
        // GET /api/Accommodation/3
        [HttpGet("{id}")]
        public IActionResult GetAccomDetail(int id)
        {
            try
            {
                var result = _service.GetAccomDetail(id);

                if (result == null)
                    return NotFound(ApiResponse<object>.Fail("해당 숙소를 찾을 수 없습니다."));

                return Ok(ApiResponse<Accommodation>.Ok(result, "숙소 상세 정보 조회 성공"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }
    }
}