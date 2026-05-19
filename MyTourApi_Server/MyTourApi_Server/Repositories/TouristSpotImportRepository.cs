using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using MyTourApi_Server.Models;

namespace MyTourApi_Server.Repositories
{
    public class TouristSpotImportRepository
    {
        private readonly Db db;

        public TouristSpotImportRepository(Db db)
        {
            this.db = db;
        }

        public int SaveTouristSpots(List<TouristSpot> spots)
        {
            int count = 0;

            foreach (TouristSpot spot in spots)
            {
                if (InsertIfNotExists(spot))
                    count++;
            }

            return count;
        }

        private bool InsertIfNotExists(TouristSpot spot)
        {
            // 종혁님의 jjh.TouristSpot 테이블 스키마 규칙 반영
            string sql = @"
IF NOT EXISTS (
    SELECT 1
    FROM jjh.TouristSpot
    WHERE 
        (ContentId = @ContentId)
        OR
        (Name = @Name AND ISNULL(Address, '') = ISNULL(@Address, ''))
)
BEGIN
    INSERT INTO jjh.TouristSpot
    (
        ContentId,
        Name,
        Address,
        Category,
        Description,
        Phone,
        Homepage,
        ImageUrl,
        Latitude,
        Longitude,
        RegionSido,
        RegionSigungu
    )
    VALUES
    (
        @ContentId,
        @Name,
        @Address,
        @Category,
        @Description,
        @Phone,
        @Homepage,
        @ImageUrl,
        @Latitude,
        @Longitude,
        @RegionSido,
        @RegionSigungu
    )
END";

            using SqlConnection conn = db.GetConnection();
            using SqlCommand cmd = new SqlCommand(sql, conn);

            // 데이터가 null일 경우를 대비해 DBNull.Value 처리 안전장치 추가
            cmd.Parameters.AddWithValue("@ContentId", spot.ContentId);
            cmd.Parameters.AddWithValue("@Name", spot.Name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", spot.Address ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Category", spot.Category ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", spot.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", spot.Phone ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Homepage", spot.Homepage ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ImageUrl", spot.ImageUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Latitude", spot.Latitude);
            cmd.Parameters.AddWithValue("@Longitude", spot.Longitude);
            cmd.Parameters.AddWithValue("@RegionSido", spot.RegionSido ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@RegionSigungu", spot.RegionSigungu ?? (object)DBNull.Value);

            conn.Open();

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}