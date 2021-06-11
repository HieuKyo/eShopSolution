using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public interface IUserService
    {
        //Trả về trạng thái có đăng nhập được hay không
        Task<string> Authencate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
        //Phươgn thức này lấy ra được danh sách user và trả về 1 model phân trang
        Task<PagedResult<UserViewModel>> GetUsersPaging(GetUserPagingRequest request);
    }
}
