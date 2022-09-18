
using Core.EMS.Entities;
using Core.Utils.Entities;
using Core.Utils.Interfaces;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        Task<BaseResponse> Login(string username, string password, string requestIP, string requestBrowser);
        Task<BaseResponse> Save(User entity);
    }
}