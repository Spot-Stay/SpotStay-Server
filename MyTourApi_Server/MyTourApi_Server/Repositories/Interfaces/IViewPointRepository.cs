using MyTourApi_Server.Models;

namespace MyTourApi_Server.Repositories.Interfaces
{
    public interface IViewPointRepository
    {
        List<ViewPoint> GetByParkName(string parkName);
        bool ExistsByNameAndParkName(string name, string parkName);

        int InsertViewPoint(ViewPoint viewPoint);
    }
}
