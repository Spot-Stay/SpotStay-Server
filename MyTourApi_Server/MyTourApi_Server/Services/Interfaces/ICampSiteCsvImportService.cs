namespace MyTourApi_Server.Services.Interfaces
{
    public interface ICampSiteCsvImportService
    {
        int ImportAllCsv(Stream csvStream);
    }
}