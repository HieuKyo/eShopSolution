using eShopSolution.Application.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //Khởi tạo đúng 1 lần (đặt dấu gạch dưới)
        private readonly IPublicProductService _publicProductService;
        //Khai báo hàm GetAll() bên Application/PublicProductServices vào
        //Mỗi lần khởi tạo thì nó sẽ gọi 1 Contructor, Contructor sẽ yêu cầu 1 đối tượng IPublicProductService
        //DI bên StartUp đã tạo ra đúng hình tượng này và truyền vào publicProductService
        //Và sẽ gắn vào _publicProductService để xài
        public ProductController(IPublicProductService publicProductService)
        {
            //Sau khi đọc 1 lần xong sẽ gắn vào cái biến đang dùng
            _publicProductService = publicProductService;
        }
        //Method
        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            //Chạy hàm GetAll() lấy dữ liệu truyền vào biến products rồi return ra
            //Ta phải khai báo bên Startup để nó mới chạy dc
            var products = await _publicProductService.GetAll();
            return Ok(products);
        }
    }
}
