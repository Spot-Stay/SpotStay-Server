using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Services.Interfaces;
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
                SpotName = s.Name?? "이름 없음",
                Category = s.Category ?? "미분류",
                Address = s.Address ?? "",
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                ImageUrl = s.ImageUrl ?? ""
            }).ToList();
        }

        public async Task<TouristSpotResponseDto?> GetSpotDetailAsync(int id)
        {
            // 1. 진짜 DB에 가서 해당 ID의 관광지가 있는지 가져옵니다.
            var spot = await _touristSpotRepository.GetSpotByIdAsync(id);

            // 2. 만약 DB에 데이터가 없다면 null을 반환합니다.
            if (spot == null) return null;

            // 3. 찾은 데이터가 있다면 DTO 바구니에 이쁘게 담아서 반환합니다.
            return new TouristSpotResponseDto
            {
                SpotId = spot.SpotId,
                SpotName = spot.Name ?? "이름 없음",
                Category = spot.Category ?? "미분류",
                Address = spot.Address ?? "",
                Latitude = spot.Latitude,
                Longitude = spot.Longitude,
                ImageUrl = spot.ImageUrl ?? ""
                // 💡 팁: 나중에 상세 설명(Description)이나 홈페이지 정보도 화면에 띄우고 싶다면,
                // TouristSpotResponseDto 파일에 변수를 만들고 여기에 추가로 매핑해 주면 됩니다!
            };
        }
    }
}