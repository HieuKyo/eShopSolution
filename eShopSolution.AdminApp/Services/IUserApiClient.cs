using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    //Service để tích hợp với backend API
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);
    }
}
