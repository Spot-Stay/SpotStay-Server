using CsvHelper.Configuration.Attributes;

namespace MyTourApi_Server.Models
{
    public class ViewPointCsv
    {
        [Name("명칭_한글(KOR_NM)")]
        public string? KOR_NM { get; set; }

        [Name("공원사무소코드(PO_CD)")]
        public string? PO_CD { get; set; }

        [Name("고도(ELEVATION)")]
        public string? ELEVATION { get; set; }

        [Name("위도(LATITUDE)")]
        public string? LATITUDE { get; set; }

        [Name("경도(LONGITUDE)")]
        public string? LONGITUDE { get; set; }
    }
}