using MyTourApi_Server.DTOs.Response;

namespace MyTourApi_Server.Services.Interfaces
{
    public interface IViewPointService
    {
        ViewPointSearchResponse GetByParkName(string parkName);

        object UploadViewPointCsv(Stream csvStream);
    }
}