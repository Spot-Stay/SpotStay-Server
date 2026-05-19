using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MyTourApi_Server.Services
{
    // 네이버 검색 API 응답 객체 정의 구조 (Models에 따로 없다면 함께 배치 가능)
    public class NaverLocalItem
    {
        public string Title { get; set; } = "";
        public string Link { get; set; } = "";
        public string Category { get; set; } = "";
        public string Description { get; set; } = "";
        public string Telephone { get; set; } = "";
        public string Address { get; set; } = "";
        public string RoadAddress { get; set; } = "";
        public string MapX { get; set; } = "";
        public string MapY { get; set; } = "";
    }

    public class NaverLocalApiService
    {
        private readonly HttpClient httpClient;
        private readonly string clientId;
        private readonly string clientSecret;

        public NaverLocalApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            httpClient = httpClientFactory.CreateClient();
            // 종혁님의 appsettings.json 외부 API 설정 경로와 정확히 일치시킴
            clientId = configuration["ExternalApis:Naver:ClientId"] ?? "";
            clientSecret = configuration["ExternalApis:Naver:ClientSecret"] ?? "";
        }

        public async Task<string> SearchLocalRawAsync(string keyword, int display = 5)
        {
            if (display < 1) display = 1;
            if (display > 5) display = 5;

            httpClient.DefaultRequestHeaders.Remove("X-Naver-Client-Id");
            httpClient.DefaultRequestHeaders.Remove("X-Naver-Client-Secret");

            httpClient.DefaultRequestHeaders.Add("X-Naver-Client-Id", clientId);
            httpClient.DefaultRequestHeaders.Add("X-Naver-Client-Secret", clientSecret);

            string encodedKeyword = Uri.EscapeDataString(keyword);

            string url = "https://openapi.naver.com/v1/search/local.json" +
                        $"?query={encodedKeyword}" +
                        $"&display={display}" +
                        "&start=1" +
                        "&sort=random";

            HttpResponseMessage response = await httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<NaverLocalItem>> SearchLocalAsync(string keyword, int display = 5)
        {
            string json = await SearchLocalRawAsync(keyword, display);
            List<NaverLocalItem> list = new List<NaverLocalItem>();

            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            if (!root.TryGetProperty("items", out JsonElement items))
                return list;

            foreach (JsonElement item in items.EnumerateArray())
            {
                NaverLocalItem localItem = new NaverLocalItem
                {
                    Title = RemoveHtmlTags(GetString(item, "title")),
                    Link = GetString(item, "link"),
                    Category = GetString(item, "category"),
                    Description = GetString(item, "description"),
                    Telephone = GetString(item, "telephone"),
                    Address = GetString(item, "address"),
                    RoadAddress = GetString(item, "roadAddress"),
                    MapX = GetString(item, "mapx"),
                    MapY = GetString(item, "mapy")
                };

                list.Add(localItem);
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

        private string RemoveHtmlTags(string text)
        {
            return text
                .Replace("<b>", "")
                .Replace("</b>", "")
                .Replace("&amp;", "&")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">");
        }
    }
}