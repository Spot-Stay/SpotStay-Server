using Microsoft.AspNetCore.Mvc;
using MyTourApi_Server.Models;
using MyTourApi_Server.Services.Interfaces;

namespace MyTourApi_Server.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IViewPointService viewPointService;

        public AdminController(IViewPointService viewPointService)
        {
            this.viewPointService = viewPointService;
        }

        // POST /api/admin/viewpoint/upload
        [HttpPost("viewpoint/upload")]
        [Consumes("multipart/form-data")]
        public IActionResult UploadViewPoint(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(ApiResponse<object>.Fail(
                    "CSV 파일을 업로드해주세요."
                ));
            }

            if (!file.FileName.EndsWith(".csv"))
            {
                return BadRequest(ApiResponse<object>.Fail(
                    "CSV 파일만 업로드 가능합니다."
                ));
            }

            try
            {
                using Stream stream = file.OpenReadStream();

                object result = viewPointService.UploadViewPointCsv(stream);

                return Ok(ApiResponse<object>.Ok(
                    result,
                    "조망점 데이터 저장 완료"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail(
                    $"서버 오류: {ex.Message}"
                ));
            }
        }
    }
}