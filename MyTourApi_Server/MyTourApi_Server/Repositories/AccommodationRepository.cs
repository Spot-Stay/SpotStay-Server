using Dapper;
using Microsoft.Data.SqlClient;
using MyTourApi_Server.Models;
using System.Collections.Generic;

namespace MyTourApi_Server.Repositories
{
    public class AccommodationRepository
    {
        private readonly string _connStr;

        public AccommodationRepository(string connectionString)
        {
            _connStr = connectionString;
        }

        // DB에서 숙소 전체 조회
        public List<Accommodation> GetAll()
        {
            var list = new List<Accommodation>();

            // ⭐ jjh. 스키마 적용 완료
            string sql = @"
            SELECT AccomId, Name, Address, AccomType, 
                   Phone, ImageUrl, Latitude, Longitude, BookingUrl
            FROM jjh.Accommodation
            WHERE Latitude IS NOT NULL AND Longitude IS NOT NULL";

            using var conn = new SqlConnection(_connStr);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Accommodation
                {
                    AccomId = reader.GetInt32(0),
                    // DB가 null 허용일 경우를 대비해 GetString 안전하게 처리
                    Name = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    Address = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    AccomType = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Phone = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    ImageUrl = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Latitude = reader.GetDouble(6),
                    Longitude = reader.GetDouble(7),
                    BookingUrl = reader.IsDBNull(8) ? "" : reader.GetString(8),
                });
            }
            return list;
        }

        // 특정 ID로 숙소 1개만 상세 조회
        public Accommodation? GetById(int accomId)
        {
            string sql = @"
            SELECT AccomId, Name, Address, AccomType, 
           Phone, ImageUrl, Latitude, Longitude, BookingUrl
            FROM jjh.Accommodation
            WHERE AccomId = @AccomId";

            using var conn = new SqlConnection(_connStr);
            conn.Open();

            // Dapper의 QueryFirstOrDefault 문법을 사용해 1개만 매핑
            return conn.QueryFirstOrDefault<Accommodation>(sql, new { AccomId = accomId });
        }
    }
}