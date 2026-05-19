using MyTourApi_Server.Models;
using System.Threading.Tasks;

namespace MyTourApi_Server.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member?> GetByUserIdAsync(string userId);
        Task<Member?> GetProfileByIdAsync(int memberId);
    }
}