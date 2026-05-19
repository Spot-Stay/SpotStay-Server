using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Models;
using MyTourApi_Server.Repositories.Interfaces;
using MyTourApi_Server.Services.Interfaces;

namespace MyTourApi_Server.Services.Impls
{
    public class ViewPointService : IViewPointService
    {
        private readonly IViewPointRepository viewPointRepository;

        public ViewPointService(IViewPointRepository viewPointRepository)
        {
            this.viewPointRepository = viewPointRepository;
        }

        public ViewPointSearchResponse GetByParkName(string parkName)
        {
            if (string.IsNullOrWhiteSpace(parkName))
                throw new Exception("국립공원명을 입력해주세요.");

            List<ViewPoint> list = viewPointRepository.GetByParkName(parkName);

            if (list.Count == 0)
                throw new Exception($"'{parkName}'의 조망점 데이터가 없습니다.");

            return new ViewPointSearchResponse
            {
                ParkName = parkName,
                Count = list.Count,
                Items = list
            };
        }
    }
}