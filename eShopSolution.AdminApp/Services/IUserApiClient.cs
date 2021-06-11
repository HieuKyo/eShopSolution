using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    //Service để tích hợp với backend API
    public interface IUserApiClient
    {
        //Login
        Task<string> Authenticate(LoginRequest request);
        //PageView làm việc với Api bên AdminApp.UserApiClient
        Task<PagedResult<UserViewModel>> GetUsersPagings(GetUserPagingRequest request);

        Task<bool> RegisterUser(RegisterRequest registerRequest);
    }
}
