
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    //Phần này bên ngoài, cho khách hàng đọc
    public interface IPublicProductService
    {
        Task <PagedResult<ProductViewModel>> GetAllByCategoryID(GetManageProductPagingRequest request);
        //Lấy list sản phẩm ra
        Task<List<ProductViewModel>> GetAll();
    }
}
