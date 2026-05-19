using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Models;
using MyTourApi_Server.Services.Interfaces;
using System.Text.Json;

namespace MyTourApi_Server.Services.Impls
{
    public class NaverLocalApiService : INaverLocalApiService
    {
        private readonly HttpClient httpClient;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string baseUrl;

        public NaverLocalApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            httpClient = httpClientFactory.CreateClient();

            clientId = configuration["ExternalApis:Naver:ClientId"] ?? "";
            clientSecret = configuration["ExternalApis:Naver:ClientSecret"] ?? "";
            baseUrl = configuration["ExternalApis:Naver:BaseUrl"]
                ?? "https://openapi.naver.com/v1/search/local.json";
        }

        public async Task<string> SearchLocalRawAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new Exception("검색어를 입력하세요.");

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                throw new Exception("네이버 API 키가 설정되지 않았습니다.");

            httpClient.DefaultRequestHeaders.Remove("X-Naver-Client-Id");
            httpClient.DefaultRequestHeaders.Remove("X-Naver-Client-Secret");

            httpClient.DefaultRequestHeaders.Add("X-Naver-Client-Id", clientId);
            httpClient.DefaultRequestHeaders.Add("X-Naver-Client-Secret", clientSecret);

            string encodedKeyword = Uri.EscapeDataString(keyword);

            string url =
                baseUrl +
                $"?query={encodedKeyword}" +
                "&display=5" +
                "&start=1" +
                "&sort=random";

            HttpResponseMessage response = await httpClient.GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("네이버 지역 검색 API 호출 실패 : " + json);
            }

            return json;
        }

        public async Task<List<NaverLocalItem>> SearchLocalAsync(string keyword)
        {
            string json = await SearchLocalRawAsync(keyword);

            return ParseLocalItems(json);
        }

        public async Task<NaverLocalSearchResponse> SearchLocalResponseAsync(string keyword)
        {
            List<NaverLocalItem> items = await SearchLocalAsync(keyword);

            return new NaverLocalSearchResponse
            {
                Keyword = keyword,
                Count = items.Count,
                Items = items
            };
        }

        private List<NaverLocalItem> ParseLocalItems(string json)
        {
            List<NaverLocalItem> list = new List<NaverLocalItem>();

            using JsonDocument doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("items", out JsonElement items))
                return list;

            foreach (JsonElement item in items.EnumerateArray())
            {
                NaverLocalItem localItem = new NaverLocalItem
                {
                    Title = RemoveHtmlTags(GetString(item, "title")),
                    Link = GetString(item, "link"),
                    Category = GetString(item, "category"),
                    Description = RemoveHtmlTags(GetString(item, "description")),
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