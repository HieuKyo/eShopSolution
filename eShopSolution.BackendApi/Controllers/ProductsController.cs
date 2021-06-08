
using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //Khởi tạo đúng 1 lần (đặt dấu gạch dưới)
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;
        //Khai báo hàm GetAll() bên Application/PublicProductServices vào
        //Mỗi lần khởi tạo thì nó sẽ gọi 1 Contructor, Contructor sẽ yêu cầu 1 đối tượng IPublicProductService
        //DI bên StartUp đã tạo ra đúng hình tượng này và truyền vào publicProductService
        //Và sẽ gắn vào _publicProductService để xài
        public ProductsController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            //Sau khi đọc 1 lần xong sẽ gắn vào cái biến đang dùng
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }
/*Không ai lại đi get tất cả sản phẩm ra làm gì
        //Method
        //https://localhost:port/product
        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAll(string languageId) 
        {
            //Chạy hàm GetAll() lấy dữ liệu truyền vào biến products rồi return ra
            //Ta phải khai báo bên Startup để nó mới chạy dc
            var products = await _publicProductService.GetAll(languageId);
            return Ok(products);
        }
*/
        //2 thằng Hàm Get() giống nhau, nên k ổn => ta phải chỉ ra alias mặc định cho nó => Swagger mới chạy dc
        //https://localhost:port/product?pageIndex=1&pageSize=10&CategoryId=
        [HttpGet("{languageId}")]
        //FromQuery chỉ định GetPublicProductPagingRequest lấy từ 
        public async Task<IActionResult> GetAllPaging(string languageId, [FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _publicProductService.GetAllByCategoryId(languageId, request);
            return Ok(products);
        }

        //https://localhost:port/product/1
        [HttpGet("{productId}/{languageId}")]   //Tên đặt trên này phải trùng với cái ở dưới
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _manageProductService.GetById(productId, languageId);
            if (product == null)
                return BadRequest("Cannot find productt");
            return Ok(product);
        }

        //Phương thức Create
        //Do thằng này tạo mới, nên dùng HttpPost
        [HttpPost]
        //Do thằng này là post nên Port sẽ là FromForm
        public async Task<IActionResult> Create([FromForm]ProductCreateRequest request)
        {
            //Kiểm tra valid của thằng này có Ok k
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Post thì sẽ k có alias gì cả
            //sẽ có result trả về
            var productId = await _manageProductService.Create(request);
            if (productId == 0)   //Không tạo được gì
                return BadRequest(); //Lỗi 404

            var product = await _manageProductService.GetById(productId, request.LanguageId);
            //Created là trạng thái 201 trả về 1 URL và 1 object
            //Sau khi nhận về, truyền vào 1 đường dẫn
            //GetById là url
            //return Created(nameof(GetById), product);
            return CreatedAtAction(nameof(GetById),new { id = productId}, product);
        }

        //Phương thức Update
        //Do thằng này update, nên dùng HttpPut
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Hàm Update trả về số bản ghi đã dược sửa
            var affectedResult = await _manageProductService.Update(request);
            if (affectedResult == 0)   //Không tạo được gì
                return BadRequest(); //Lỗi 404
            return Ok();
        }

        //Phương thức Update Price
        //Do thằng này update, nên dùng HttpPut truyền vào id và new price
        //Update có 1 phần của bản ghi thôi, nên dùng HttpPatch, còn update toàn bộ thì HttpPut
        [HttpPatch("{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var isSuccessful = await _manageProductService.UpdatePrice(productId, newPrice);
            if (isSuccessful)   //Không tạo được gì
                return Ok(); //Lỗi 404
            return BadRequest();
        }

        //Phương thức Delete
        //productId phải dc khai báo trên này, đặt productId cho nó clear
        [HttpDelete("{productId}")]
        //Delete ta truyền vào 1 product Id
        public async Task<IActionResult> Delete(int productId)
        {
            //Hàm Update trả về số bản ghi đã dược sửa
            var affectedResult = await _manageProductService.Delete(productId);
            if (affectedResult == 0)   //Không tạo được gì
                return BadRequest(); //Lỗi 404
            return Ok();
        }
    }
}
