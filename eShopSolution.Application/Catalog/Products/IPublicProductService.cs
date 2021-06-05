using eShopSolution.Application.Catalog.Products.DTOS;
using eShopSolution.Application.Catalog.Products.DTOS.Public;
using eShopSolution.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    //Phần này bên ngoài, cho khách hàng đọc
    public interface IPublicProductService
    {
        Task <PagedResult<ProductViewModel>> GetAllByCategoryID(GetProductPagingRequest request);
    }
}
