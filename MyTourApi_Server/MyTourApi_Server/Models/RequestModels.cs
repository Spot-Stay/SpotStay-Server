using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyTourApi_Server.Models
{
    // 공공데이터포털 TourAPI 공통 JSON 파싱용 구조
    public class TourApiResponseWrapper
    {
        [JsonPropertyName("response")]
        public TourApiResponse Response { get; set; } = new();
    }

    public class TourApiResponse
    {
        [JsonPropertyName("header")]
        public TourApiHeader Header { get; set; } = new();

        [JsonPropertyName("body")]
        public TourApiBody Body { get; set; } = new();
    }

    public class TourApiHeader
    {
        [JsonPropertyName("resultCode")]
        public string ResultCode { get; set; } = string.Empty;

        [JsonPropertyName("resultMsg")]
        public string ResultMsg { get; set; } = string.Empty;
    }

    public class TourApiBody
    {
        [JsonPropertyName("items")]
        public TourApiItems Items { get; set; } = new();

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }
    }

    public class TourApiItems
    {
        [JsonPropertyName("item")]
        public List<TourApiItem> ItemList { get; set; } = new();
    }

    public class TourApiItem
    {
        [JsonPropertyName("contentid")]
        public string ContentId { get; set; } = string.Empty;

        [JsonPropertyName("contenttypeid")]
        public string ContentTypeId { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("addr1")]
        public string Addr1 { get; set; } = string.Empty;

        [JsonPropertyName("tel")]
        public string Tel { get; set; } = string.Empty;

        [JsonPropertyName("firstimage")]
        public string FirstImage { get; set; } = string.Empty;

        [JsonPropertyName("mapx")]
        public string MapX { get; set; } = string.Empty; // 경도(Lng)

        [JsonPropertyName("mapy")]
        public string MapY { get; set; } = string.Empty; // 위도(Lat)

        [JsonPropertyName("cat1")]
        public string Cat1 { get; set; } = string.Empty;

        [JsonPropertyName("cat2")]
        public string Cat2 { get; set; } = string.Empty;

        [JsonPropertyName("cat3")]
        public string Cat3 { get; set; } = string.Empty;
    }
}