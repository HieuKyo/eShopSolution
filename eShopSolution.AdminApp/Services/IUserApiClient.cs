using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    //Service để tích hợp với backend API
    public interface IUserApiClient
    {
        //Login
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        //PageView làm việc với Api bên AdminApp.UserApiClient
        Task<ApiResult<PagedResult<UserViewModel>>> GetUsersPagings(GetUserPagingRequest request);

        Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequest);

        Task<ApiResult<bool>> UpdateUser(Guid id,UserUpdateRequest request);

        Task<ApiResult<UserViewModel>> GetById(Guid id);
    }
}
