using Microsoft.AspNetCore.Mvc;
using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Models;
using MyTourApi_Server.Services.Interfaces;

namespace MyTourApi_Server.Controllers
{
    [ApiController]
    [Route("api/viewpoints")]
    public class ViewPointController : ControllerBase
    {
        private readonly IViewPointService viewPointService;

        public ViewPointController(IViewPointService viewPointService)
        {
            this.viewPointService = viewPointService;
        }

        // GET /api/viewpoints?parkName=설악산
        [HttpGet]
        public IActionResult GetByParkName([FromQuery] string parkName)
        {
            try
            {
                ViewPointSearchResponse result = viewPointService.GetByParkName(parkName);

                return Ok(ApiResponse<object>.Ok(
                    result,
                    "조망점 조회 성공"
                ));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<object>.Fail(
                    ex.Message
                ));
            }
        }
    }
}