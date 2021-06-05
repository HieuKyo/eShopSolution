using eShopSolution.Application.Catalog.Products.DTOS;
using eShopSolution.Application.DTOS;
using eShopSolution.Data.Entities;
using eShopSolution.Data.Entities_Framework;
using System;
using System.Collections.Generic;
using System.Text;
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
        public async Task<int> Create(ProductCreateRequest request)
        {
            //Tạo mới product = Entity framework
            var product = new Product()
            {
                Price = request.Price,

            };
            _context.Products.Add(product);
            //Do xài SaveChangesAsync nên phải đổi kiểu của int trong Create thành Task<int>
            //await nó, cho nó chạy background thay vì phải chờ, giảm thời gian chờ
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<ProductViewModel>> GetAllPaging(string keyword, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(ProductEditRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
