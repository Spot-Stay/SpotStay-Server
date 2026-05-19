using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MyTourApi_Server.Models;

namespace MyTourApi_Server.Services
{
    public class TourApiService
    {
        private readonly HttpClient httpClient;
        private readonly string serviceKey;

        // 주입받은 HttpClient와 IConfiguration 사용
        public TourApiService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            // 종혁님의 appsettings.json 구조인 ExternalApis:TourApi:ServiceKey에 맞게 수정
            serviceKey = configuration["ExternalApis:TourApi:ServiceKey"] ?? "";
        }

        public async Task<List<TouristSpot>> SearchTouristSpotsAsync(string keyword)
        {
            string encodedKeyword = Uri.EscapeDataString(keyword);

            // 종혁님의 appsettings.json에 들어있는 서비스 키를 활용해 URL 구성
            string url =
                "https://apis.data.go.kr/B551011/KorService2/searchKeyword2" + // KorService2 -> KorService1 규격으로 변경 가능성 고려
                $"?serviceKey={serviceKey}" +
                "&MobileOS=ETC" +
                "&MobileApp=MyTourApp" + // 모바일 앱 이름 매핑 변경
                "&_type=json" +
                "&numOfRows=50" +
                "&pageNo=1" +
                $"&keyword={encodedKeyword}";

            HttpResponseMessage response = await httpClient.GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("TourAPI 호출 실패 : " + json);
            }

            return ParseTouristSpots(json);
        }

        private List<TouristSpot> ParseTouristSpots(string json)
        {
            List<TouristSpot> list = new List<TouristSpot>();

            try
            {
                using JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;

                // 공공데이터 특유의 깊은 JSON 트리 구조 안전하게 파싱 (response -> body -> items -> item)
                if (!root.TryGetProperty("response", out JsonElement response) ||
                    !response.TryGetProperty("body", out JsonElement body) ||
                    !body.TryGetProperty("items", out JsonElement items) ||
                    items.ValueKind == JsonValueKind.String) // 검색 결과가 없을 때 빈 문자열"" 로 오는 예외 방어
                {
                    return list;
                }

                if (items.TryGetProperty("item", out JsonElement itemArray) && itemArray.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement item in itemArray.EnumerateArray())
                    {
                        TouristSpot spot = new TouristSpot
                        {
                            ContentId = int.TryParse(GetString(item, "contentid"), out int cid) ? cid : 0,
                            Name = GetString(item, "title"),
                            Address = GetString(item, "addr1"),
                            Category = ConvertContentType(GetString(item, "contenttypeid")),
                            Description = "", // 키워드 검색 결과에는 상세 설명이 안 오므로 빈 값 처리
                            Phone = GetString(item, "tel"),
                            Homepage = null,
                            ImageUrl = GetString(item, "firstimage"),
                            Latitude = GetNullableDouble(item, "mapy") ?? 0.0,  // null 방어 및 double 변환
                            Longitude = GetNullableDouble(item, "mapx") ?? 0.0, // null 방어 및 double 변환
                            RegionSido = GetString(item, "areacode"),
                            RegionSigungu = GetString(item, "sigungucode")
                        };

                        list.Add(spot);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("TourAPI JSON 파싱 중 오류 발생: " + ex.Message);
            }

            return list;
        }

        private string GetString(JsonElement item, string propertyName)
        {
            if (item.TryGetProperty(propertyName, out JsonElement value))
            {
                if (value.ValueKind == JsonValueKind.String)
                    return value.GetString() ?? "";

                return value.ToString();
            }

            return "";
        }

        private double? GetNullableDouble(JsonElement item, string propertyName)
        {
            string value = GetString(item, propertyName);

            if (double.TryParse(value, out double result))
                return result;

            return null;
        }

        private string ConvertContentType(string contentTypeId)
        {
            return contentTypeId switch
            {
                "12" => "관광지",
                "14" => "문화시설",
                "15" => "축제공연행사",
                "25" => "여행코스",
                "28" => "레포츠",
                "32" => "숙박",
                "38" => "쇼핑",
                "39" => "음식점",
                _ => contentTypeId
            };
        }
    }
}