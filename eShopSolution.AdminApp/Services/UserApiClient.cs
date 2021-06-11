using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        //Cần 1 constructor
        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            //Phong cách viết theo DI
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client =  _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync("/api/users/authenticate", httpContent);
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }

        public async Task<PagedResult<UserViewModel>> GetUsersPagings(GetUserPagingRequest request)
        {

            var client = _httpClientFactory.CreateClient();            
            //"BaseAddress lúc này là https://localhost/5001" trong cái appsetting
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            //Dùng thằng này trong Controller BackendApi
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",request.BearerToken);
            var response = await client.GetAsync($"/api/users/paging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
            //Do là binding theo FromQuery nên đúng query là nó bind ra
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<PagedResult<UserViewModel>>(body);
            return users;
        }

        public async Task<bool> RegisterUser(RegisterRequest registerRequest)
        {
            var client = _httpClientFactory.CreateClient();
            //"BaseAddress lúc này là https://localhost/5001" trong cái appsetting
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            //Sign ra 1 Http Content
            var json = JsonConvert.SerializeObject(registerRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/users", httpContent);
            return response.IsSuccessStatusCode;
        }
    }
}
