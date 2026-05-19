using Microsoft.AspNetCore.Mvc;
using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTourApi_Server.Controllers
{
    [ApiController]
    [Route("api/touristspots")]
    public class TouristSpotController : ControllerBase
    {
        private readonly ITouristSpotService _touristSpotService;

        public TouristSpotController(ITouristSpotService touristSpotService)
        {
            _touristSpotService = touristSpotService;
        }

        // GET api/touristspots/search
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword = "", [FromQuery] string region = "")
        {
            var results = await _touristSpotService.SearchSpotsAsync(keyword, region);
            return Ok(results);
        }

        // GET api/touristspots/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var result = await _touristSpotService.GetSpotDetailAsync(id);
            if (result == null)
            {
                return NotFound(new { message = "해당 관광지를 찾을 수 없습니다." });
            }
            return Ok(result);
        }
    }
}