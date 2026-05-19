using MyTourApi_Server.Models;
using System.Threading.Tasks;

namespace MyTourApi.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        // UserId로 회원 정보를 찾아오는 함수 규칙 정의
        Task<Member?> GetByUserIdAsync(string userId);
    }
}