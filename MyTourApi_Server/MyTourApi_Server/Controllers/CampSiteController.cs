using Microsoft.AspNetCore.Mvc;
using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Models;
using MyTourApi_Server.Services.Interfaces;

namespace MyTourApi_Server.Controllers
{
    [ApiController]
    [Route("api/campsites")]
    public class CampSiteController : ControllerBase
    {
        private readonly ICampSiteService campSiteService;
        private readonly ICampSiteCsvImportService campSiteCsvImportService;

        public CampSiteController(
            ICampSiteService campSiteService,
            ICampSiteCsvImportService campSiteCsvImportService)
        {
            this.campSiteService = campSiteService;
            this.campSiteCsvImportService = campSiteCsvImportService;
        }

        // GET /api/campsites?parkName=설악산
        [HttpGet]
        public IActionResult GetCampSites([FromQuery] string? parkName)
        {
            try
            {
                CampSiteSearchResponse result = campSiteService.GetCampSites(parkName);

                if (result.Count == 0)
                {
                    return NotFound(ApiResponse<object>.Fail(
                        "조회된 야영장 정보가 없습니다."
                    ));
                }

                return Ok(ApiResponse<object>.Ok(
                    result,
                    "국립공원 야영장 조회 성공"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail(
                    "국립공원 야영장 조회 중 오류 발생: " + ex.Message
                ));
            }
        }

        // GET /api/campsites/1
        [HttpGet("{campId}")]
        public IActionResult GetCampSiteById(int campId)
        {
            try
            {
                CampSite? campsite = campSiteService.GetCampSiteById(campId);

                if (campsite == null)
                {
                    return NotFound(ApiResponse<object>.Fail(
                        "해당 야영장 정보를 찾을 수 없습니다."
                    ));
                }

                return Ok(ApiResponse<object>.Ok(
                    campsite,
                    "야영장 상세 조회 성공"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail(
                    "야영장 상세 조회 중 오류 발생: " + ex.Message
                ));
            }
        }

        // POST /api/campsites/import-all
        [HttpPost("import-all")]
        [Consumes("multipart/form-data")]
        public IActionResult ImportAllCampSites(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(ApiResponse<object>.Fail(
                        "CSV 파일을 선택하세요."
                    ));
                }

                using Stream stream = file.OpenReadStream();

                int insertCount = campSiteCsvImportService.ImportAllCsv(stream);

                return Ok(ApiResponse<object>.Ok(
                    new
                    {
                        insertCount = insertCount
                    },
                    "전체 야영장 CSV 데이터 저장 성공"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail(
                    "전체 야영장 CSV 데이터 저장 중 오류 발생: " + ex.Message
                ));
            }
        }
    }
}