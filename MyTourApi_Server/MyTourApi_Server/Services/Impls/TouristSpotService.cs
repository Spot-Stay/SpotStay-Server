using MyTourApi.DTOs.Response;
using MyTourApi.Services.Interfaces;
using MyTourApi_Server.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTourApi_Server.Services.Impls
{
    public class TouristSpotService : ITouristSpotService
    {
        private readonly ITouristSpotRepository _touristSpotRepository;

        // 가짜 데이터 대신 진짜 DB 레포지토리를 주입받음
        public TouristSpotService(ITouristSpotRepository touristSpotRepository)
        {
            _touristSpotRepository = touristSpotRepository;
        }

        public async Task<List<TouristSpotResponseDto>> SearchSpotsAsync(string keyword, string region)
        {
            // 1. 진짜 DB에 가서 검색된 관광지 리스트를 긁어옴
            var spots = await _touristSpotRepository.SearchSpotsAsync(keyword, region);

            // 2. DB에서 가져온 모델 객체들을 클라이언트에 보낼 DTO 객체들로 변환(LinQ 사용)
            return spots.Select(s => new TouristSpotResponseDto
            {
                SpotId = s.SpotId,
                SpotName = s.Name,
                Category = s.Category ?? "미분류",
                Address = s.Address ?? "",
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                ImageUrl = s.ImageUrl ?? ""
            }).ToList();
        }

        public async Task<TouristSpotResponseDto?> GetSpotDetailAsync(int id)
        {
            // (상세 조회가 필요하다면 나중에 이 부분도 레포지토리에 함수를 뚫어서 연결하면 됩니다!)
            await Task.Delay(10);
            return null;
        }
    }
}