using CsvHelper;
using CsvHelper.Configuration;
using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Models;
using MyTourApi_Server.Repositories.Interfaces;
using MyTourApi_Server.Services.Interfaces;
using System.Globalization;
using System.Text;

namespace MyTourApi_Server.Services.Impls
{
    public class ViewPointService : IViewPointService
    {
        private readonly IViewPointRepository viewPointRepository;

        public ViewPointService(IViewPointRepository viewPointRepository)
        {
            this.viewPointRepository = viewPointRepository;
        }

        public ViewPointSearchResponse GetByParkName(string parkName)
        {
            if (string.IsNullOrWhiteSpace(parkName))
                throw new Exception("국립공원명을 입력해주세요.");

            List<ViewPoint> list = viewPointRepository.GetByParkName(parkName);

            if (list.Count == 0)
                throw new Exception($"'{parkName}'의 조망점 데이터가 없습니다.");

            return new ViewPointSearchResponse
            {
                ParkName = parkName,
                Count = list.Count,
                Items = list
            };
        }

        public object UploadViewPointCsv(Stream csvStream)
        {
            Dictionary<string, string> parkMap = new Dictionary<string, string>
            {
                {"101","한려해상국립공원"}, {"102","한려해상국립공원"}, {"103","한려해상국립공원"},
                {"201","계룡산국립공원"}, {"301","지리산국립공원"}, {"401","설악산국립공원"},
                {"501","속리산국립공원"}, {"601","내장산국립공원"}, {"602","내장산국립공원"},
                {"701","가야산국립공원"}, {"801","덕유산국립공원"}, {"901","오대산국립공원"},
                {"1001","주왕산국립공원"}, {"1101","태안해안국립공원"},
                {"1201","다도해해상국립공원"}, {"1202","다도해해상국립공원"},
                {"1301","북한산국립공원"}, {"1401","치악산국립공원"},
                {"1501","월악산국립공원"}, {"1502","월악산국립공원"},
                {"1601","소백산국립공원"}, {"1602","소백산국립공원"},
                {"1701","변산반도국립공원"}, {"1801","월출산국립공원"},
                {"2001","무등산국립공원"}
            };

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using StreamReader reader = new StreamReader(csvStream, Encoding.GetEncoding("euc-kr"));

            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                BadDataFound = null
            };

            using CsvReader csv = new CsvReader(reader, config);

            List<ViewPointCsv> records = csv.GetRecords<ViewPointCsv>().ToList();

            int success = 0;
            int skip = 0;
            int fail = 0;

            foreach (ViewPointCsv row in records)
            {
                if (string.IsNullOrWhiteSpace(row.KOR_NM))
                {
                    skip++;
                    continue;
                }

                if (!double.TryParse(row.LATITUDE, out double lat))
                {
                    skip++;
                    continue;
                }

                if (!double.TryParse(row.LONGITUDE, out double lng))
                {
                    skip++;
                    continue;
                }

                string parkName = parkMap.TryGetValue(row.PO_CD?.Trim() ?? "", out string? mappedParkName)
                    ? mappedParkName
                    : "국립공원";

                string description = string.IsNullOrWhiteSpace(row.ELEVATION)
                    ? "조망점"
                    : $"고도 {row.ELEVATION}m";

                bool exists = viewPointRepository.ExistsByNameAndParkName(row.KOR_NM, parkName);

                if (exists)
                {
                    skip++;
                    continue;
                }

                ViewPoint viewPoint = new ViewPoint
                {
                    Name = row.KOR_NM,
                    ParkName = parkName,
                    Description = description,
                    Latitude = lat,
                    Longitude = lng
                };

                try
                {
                    int result = viewPointRepository.InsertViewPoint(viewPoint);

                    if (result > 0)
                        success++;
                    else
                        fail++;
                }
                catch
                {
                    fail++;
                }
            }

            return new
            {
                success = success,
                skip = skip,
                fail = fail
            };
        }
    }
}