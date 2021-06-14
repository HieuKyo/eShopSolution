using eShopSolution.Application.System.Users;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        //Xây dựng phươgn thức Login trong module System trong Application
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        //AllowAnonymous - chưa đăng nhập vẫn có thể gọi được 
        [AllowAnonymous]
        //Thaay đổi lại từ FromForm thành FromBody để cho cái nó ra chuõi json để truyền vào đăng nhập
        public async Task<IActionResult> Authenticate([FromBody]LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Authencate(request);
            //Check result Object vì token sẽ dc rán vào đây
            if(string.IsNullOrEmpty(result.ResultObj))
            {
                return BadRequest(result);
            }
            
            //Trả về luôn 1 token để dễ lấy
            return Ok(result);
        }

        //Mặc định Post đã là Rigister rồi, nên k cần ("register")
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           

            var result = await _userService.Register(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        //Put: https://localhost/api/users/id
        //Update cũng phải theo id
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Update(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        //http://localhost/api/users/paging?pageIndex=1&pageSize=10&keyword=
        [HttpGet("paging")]
        //FromQuery chỉ định GetPublicProductPagingRequest lấy từ 
        public async Task<IActionResult> GetAllPaging([FromQuery] GetUserPagingRequest request)
        {

            var products = await _userService.GetUsersPaging(request);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);
            return Ok(result);
        }
    }
}
/*
 * eShopSolution.AdminApp.Services.UserApiClient.UpdateUser(Guid id, UserUpdateRequest request) in UserApiClient.cs
+
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
eShopSolution.AdminApp.Controllers.UserController.Edit(UserUpdateRequest request) in UserController.cs
+
            var result = await _userApiClient.UpdateUser(request.Id, request);
*/
