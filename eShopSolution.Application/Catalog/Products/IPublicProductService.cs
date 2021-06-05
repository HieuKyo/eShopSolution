using eShopSolution.Application.Catalog.Products.DTOS;
using eShopSolution.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Application.Catalog.Products
{
    //Phần này bên ngoài, cho khách hàng đọc
    public interface IPublicProductService
    {
        public PagedViewModel<ProductViewModel> GetAllByCategoryID(int categoryId, int pageIndex, int pageSize);
    }
}
