using eShopSolution.ViewModels.System.Users;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public interface IUserService
    {
        //Trả về trạng thái có đăng nhập được hay không
        Task<string> Authencate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);

    }
}
