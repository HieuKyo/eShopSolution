using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Services;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;


namespace eShopSolution.AdminApp.Controllers
{
    public class UserController : Controller
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login() 
        {
            //Chưa kịp LogOut mà lại vào trang LogIn thì phải
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        //Nếu tên phương thức trùng với tên của View thì ta k cần truyền cái nào vào đây nữa
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (ModelState.IsValid)
                return View(ModelState);

            var token = await _userApiClient.Authenticate(request);
            //Sau khi đăng nhập thành công, lấy dc token rồi thì ta giải mã token ra, coi có những claim nào dc set ở bên kia
            //Sau đó chúng ta cũng set authenticate cookie = HttpContext.SignIn
            //Chuyển token này sang UserPrincipal
            var userPrincipal = this.ValidateToken(token);
            //Rồi chuyển sang 1 tập Properties của Cookie
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            //Sau đó dùng Signing httpContext
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    authProperties
            );
            //Sau khi login thành công, => về trang chủ

            return RedirectToAction("Index","Home");
        }

        //Hàm giải mã token
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            //2 thằng Issuer giải mã ra Token:Key
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            //Thì nó sẽ Valid được Token và truyền thôgn tin của token vào principal sang đói tượng Claim principal
            //Phải có 1 cái key bên appsettings.json giống ApiBackEnd để giải mã
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken); 
            
            return principal;
        }

        [HttpPost]
        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Đến controller User
            return RedirectToAction("Login", "User");
        }
    }
}
