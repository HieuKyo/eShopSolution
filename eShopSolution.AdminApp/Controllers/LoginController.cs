using eShopSolution.AdminApp.Services;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


//Tách hẵn Login ra 1 Controller riêng và chỉnh lại StratUp
namespace eShopSolution.AdminApp.Controllers
{
    public class LoginController : Controller
    {
        //Khởi tạo 1 lần trong Constructor thôi, nên dùng private only
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        //private readonly IRoleApiClient _roleApiClient;
        public LoginController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Chưa kịp LogOut mà lại vào trang LogIn thì phải
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        //Nếu tên phương thức trùng với tên của View thì ta k cần truyền cái nào vào đây nữa
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid)
                //return View(ModelState) thì nó hiện ra lỗi luôn
                return View(ModelState);

            var result = await _userApiClient.Authenticate(request);
            if(result.ResultObj == null)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            //Sau khi đăng nhập thành công, lấy dc token rồi thì ta giải mã token ra, coi có những claim nào dc set ở bên kia
            //Sau đó chúng ta cũng set authenticate cookie = HttpContext.SignIn
            //Chuyển token này sang UserPrincipal
            var userPrincipal = this.ValidateToken(result.ResultObj);
            //Rồi chuyển sang 1 tập Properties của Cookie
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString("Token", result.ResultObj);
            //Sau đó dùng Signing httpContext
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    authProperties
            );
            //Sau khi login thành công, => về trang chủ

            return RedirectToAction("Index", "Home");
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
    }
}
