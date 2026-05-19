using MyTourApi_Server.Models;
using MyTourApi_Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyTourApi_Server.Services
{
    public class AccommodationService
    {
        private readonly AccommodationRepository _repo;

        public AccommodationService(AccommodationRepository repo)
        {
            _repo = repo;
        }

        // 거리순 숙소 추천 메인 메서드
        public List<AccommodationWithDistance> GetNearby(double lat, double lng, int top = 20)
        {
            var allAccoms = _repo.GetAll();

            var result = allAccoms
                .Select(a => new AccommodationWithDistance
                {
                    AccomId = a.AccomId,
                    Name = a.Name,
                    Address = a.Address,
                    AccomType = a.AccomType,
                    Phone = a.Phone,
                    ImageUrl = a.ImageUrl,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    BookingUrl = a.BookingUrl,
                    DistanceKm = CalcDistance(lat, lng, a.Latitude, a.Longitude)
                })
                .OrderBy(a => a.DistanceKm)   // 가까운 순 정렬
                .Take(top)                     // 상위 개수만큼만
                .ToList();

            return result;
        }

        // 하버사인 공식으로 두 좌표 사이 거리 계산 (km)
        private double CalcDistance(double lat1, double lng1, double lat2, double lng2)
        {
            const double R = 6371; // 지구 반지름 (km)

            double dLat = ToRad(lat2 - lat1);
            double dLng = ToRad(lng2 - lng1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                     + Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2))
                     * Math.Sin(dLng / 2) * Math.Sin(dLng / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return Math.Round(R * c, 2);
        }

        private double ToRad(double degree) => degree * Math.PI / 180;
        public Accommodation? GetAccomDetail(int id)
        {
            return _repo.GetById(id);
        }
    }
}