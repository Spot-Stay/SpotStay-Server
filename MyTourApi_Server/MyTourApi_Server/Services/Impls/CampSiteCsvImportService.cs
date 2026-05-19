using CsvHelper;
using CsvHelper.Configuration;
using MyTourApi_Server.Models;
using MyTourApi_Server.Repositories.Interfaces;
using MyTourApi_Server.Services.Interfaces;
using System.Globalization;
using System.Text;

namespace MyTourApi_Server.Services.Impls
{
    public class CampSiteCsvImportService : ICampSiteCsvImportService
    {
        private readonly ICampSiteRepository campSiteRepository;

        public CampSiteCsvImportService(ICampSiteRepository campSiteRepository)
        {
            this.campSiteRepository = campSiteRepository;
        }

        public int ImportAllCsv(Stream csvStream)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using StreamReader reader = new StreamReader(csvStream, Encoding.GetEncoding("euc-kr"));

            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null,
                BadDataFound = null
            };

            using CsvReader csv = new CsvReader(reader, config);

            List<CampSiteCsv> csvRecords = csv.GetRecords<CampSiteCsv>().ToList();

            List<CampSite> campsites = csvRecords.Select(x => new CampSite
            {
                Name = x.KOR_NM,
                ParkName = GetParkName(x),
                Address = string.IsNullOrWhiteSpace(x.RDNMADR) ? x.LNM_ADRES : x.RDNMADR,
                Phone = x.TELNO,
                SiteCount = x.CMP_NUM,
                Latitude = x.LATITUDE,
                Longitude = x.LONGITUDE
            }).ToList();

            return campSiteRepository.InsertCampSites(campsites);
        }

        private string GetParkName(CampSiteCsv item)
        {
            string text = $"{item.KOR_NM} {item.LNM_ADRES} {item.RDNMADR} {item.ID_CD} {item.PO_CD}";

            if (text.Contains("설악")) return "설악산";
            if (text.Contains("지리")) return "지리산";
            if (text.Contains("한라")) return "한라산";
            if (text.Contains("덕유")) return "덕유산";
            if (text.Contains("오대")) return "오대산";
            if (text.Contains("속리")) return "속리산";
            if (text.Contains("내장")) return "내장산";
            if (text.Contains("가야")) return "가야산";
            if (text.Contains("계룡")) return "계룡산";
            if (text.Contains("월악")) return "월악산";
            if (text.Contains("소백")) return "소백산";
            if (text.Contains("월출")) return "월출산";
            if (text.Contains("주왕")) return "주왕산";
            if (text.Contains("태백")) return "태백산";
            if (text.Contains("무등")) return "무등산";
            if (text.Contains("치악")) return "치악산";
            if (text.Contains("팔공")) return "팔공산";
            if (text.Contains("다도해")) return "다도해해상";
            if (text.Contains("한려")) return "한려해상";
            if (text.Contains("변산")) return "변산반도";
            if (text.Contains("태안")) return "태안해안";
            if (text.Contains("북한")) return "북한산";

            return "기타";
        }
    }
}