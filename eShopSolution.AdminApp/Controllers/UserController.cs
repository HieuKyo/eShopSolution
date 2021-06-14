using System;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Services;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace eShopSolution.AdminApp.Controllers
{
    public class UserController : BaseController
    {
        //Khởi tạo 1 lần trong Constructor thôi, nên dùng private only
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        //private readonly IRoleApiClient _roleApiClient;
        public UserController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            //Từ token này ta sẽ get 1 cái request bao gồm BearerToken, keyword
            var request = new GetUserPagingRequest()
            {
                //default paramenter
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            //Sau đó gọi UserClient API
            var data = await _userApiClient.GetUsersPagings(request);
            //Truyền về 1 cái ViewBag để lưu giá trị đã tìm trên thanh search
            ViewBag.Keyword = keyword;
            return View(data.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            //Đến controller User
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult Create()
        {
            //Trả về 1 view bình thường
            return View();
        }

        //Trả về 1 view bình thường
        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.RegisterUser(request);
            //Nếu có đăng ký user thì trả về Index
            if (result.IsSuccessed)
                return RedirectToAction("Index");
            //Không thì trả về View Bình thường thôi
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //Thằng này lấy ra user thôi
            var result = await _userApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var user = result.ResultObj;
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Id = id
                };
                return View(updateRequest);
            }
            //Ra trang Error luon nêu k tồn tại
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.UpdateUser(request.Id, request);
            //Nếu có đăng ký user thì trả về Index
            if (result.IsSuccessed)
                return RedirectToAction("Index");
            //Không thì trả về View Bình thường thôi
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        //Detail
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            //Trả về result của Id. ResultObj là 1 kiểu result View Model
            return View(result.ResultObj);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {

            return View(new UserDeleteRequest()
            {
                Id = id
            }
            );
        }
        //Trả về 1 view bình thường
        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.Delete(request.Id);
            //Nếu có đăng ký user thì trả về Index
            if (result.IsSuccessed)
                return RedirectToAction("Index");
            //Không thì trả về View Bình thường thôi
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
    }
}
