using CsvHelper.Configuration.Attributes;

namespace MyTourApi_Server.Models
{
    public class CampSiteCsv
    {
        [Name("관리번호(OBJECTID)")]
        public int OBJECTID { get; set; }

        [Name("국립공원관리번호(ID_CD)")]
        public string? ID_CD { get; set; }

        [Name("공원사무소코드(PO_CD)")]
        public string? PO_CD { get; set; }

        [Name("분류코드(CLASS_CD)")]
        public string? CLASS_CD { get; set; }

        [Name("일련번호(SEQNO)")]
        public string? SEQNO { get; set; }

        [Name("명칭_한글(KOR_NM)")]
        public string? KOR_NM { get; set; }

        [Name("명칭_영어(ENG_NM)")]
        public string? ENG_NM { get; set; }

        [Name("주소_지번(LNM_ADRES)")]
        public string? LNM_ADRES { get; set; }

        [Name("주소_새주소(RDNMADR)")]
        public string? RDNMADR { get; set; }

        [Name("전화번호(TELNO)")]
        public string? TELNO { get; set; }

        [Name("야영동수(CMP_NUM)")]
        public int? CMP_NUM { get; set; }

        [Name("야영료징수여부(CMP_CHR)")]
        public string? CMP_CHR { get; set; }

        [Name("사용여부(USE_YN)")]
        public string? USE_YN { get; set; }

        [Name("고도(ELEVATION)")]
        public double? ELEVATION { get; set; }

        [Name("경도(LONGITUDE)")]
        public double? LONGITUDE { get; set; }

        [Name("위도(LATITUDE)")]
        public double? LATITUDE { get; set; }

        [Name("심볼코드(SYMBOL_CD)")]
        public string? SYMBOL_CD { get; set; }
    }
}