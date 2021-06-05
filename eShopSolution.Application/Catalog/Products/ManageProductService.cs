using eShopSolution.Application.Catalog.Products.DTOS;
using eShopSolution.Application.Catalog.Products.DTOS.Manage;
using eShopSolution.Application.DTOS;
using eShopSolution.Data.Entities;
using eShopSolution.Data.Entities_Framework;
using eShopSolution.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        //Cần 2 cái Class ManageProductService PublicProductService
        //Cải tiến button để đơn giản hơn
        //Tạo 1 constructor ManageProductService

        //Cần khai báo 1 biến context private để đón nó
        private readonly eShopDBContext _context;
        public ManageProductService(eShopDBContext context)
        {
            //Cần đầu vào là 1 DbContext cái đã khai báo ở eShop.Data
            //Cần add reference từ tầng Data lên, qua cáu Dependencies
            _context = context;
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            //Tạo mới product = Entity framework
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                //Ghi đè thằng DateTime luôn
                DateCreated = DateTime.Now,
                //Thêm Translation theo dạng cha con
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        //Thêm thông qua khoá ngoại
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        //Thêm bản ghi đúng bằng cái Language đó
                        LanguageId = request.LanguageId,
                    }
                }
            };
            //Sau khi thêm xong Add cái product này vào SaveChange
            _context.Products.Add(product);
            //Do xài SaveChangesAsync nên phải đổi kiểu của int trong Create thành Task<int>
            //await nó, cho nó chạy background thay vì phải chờ, giảm thời gian chờ
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Can't find a product: {productId}");

            _context.Products.Remove(product);
            //Còn tìm dc thì sẽ trả về số bản ghi Delete
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        //Phương thức GetPaging
        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            //Dùng cách query trong LinQ dễ hơn
            //1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.Name.Contains(request.Keyword)
                        select new { p, pt, pic};
            
            //2. filter
            //Kiểm tra nếu keyword khác rỗng mới tìm
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            //Có bất cứ tìm kiếm nào
            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(p => request.CategoryIds.Contains(p.pic.CategoryId));
            }
            
            //3. Paging
            //Bước này ta phải lấy ra dc totalRow lấy dc tổng số bản ghi
            int totalRow = await query.CountAsync();

            //Ví dụ: Ở bản ghi ở trang 1. 1 - 1 = 0, 0 * 10 = 0
                               //trang 2. 2 - 1 = 1, 1 * 10 = 10 Bỏ qua 10 bản ghi đầu thì sẽ lấy trang số 2.
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x=> new ProductViewModel()
                { 
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            //4 Select and projection
            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            throw new NotImplementedException();
        }
    }
}
