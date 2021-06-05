using eShopSolution.Application.Catalog.Products.DTOS;
using eShopSolution.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    //Phần này cho Admin, chỉ thêm sửa xoá thôi
    //SOLID thì có 5 nguyên tắc D có nghĩa là Dependency Inverse
    //Để triển khai Dependency Inverse thì có Dependency Injection
    //Nguyên tắc đảo ngược sự phụ thuộc, không phụ thuộc trực tiếp với nhau, mà dùng cơ chế button
    //Để tiêm, truyền các thành phân phụ thuộc vào
    //Interface bắt đầu bằng I
    public interface IManageProductService
    {
        //Định nghĩa các phương thức liên quan cho nó
        //Create truyền vào 1 cái DTO - Data Transfer object
        //Khi mà tạo mới 1 sản phẩm thì sẽ truyền vào Product Create Resquest
        //Trả về kiểu int là mã sản phẩm
        Task<int> Create(ProductCreateRequest request);
        //Dùng Task - SaveChangesAsync để tận dụng được nhiều Thread 1 lúc
        Task<int> Update(ProductEditRequest request);

        Task<int> Delete(int productId);
        //Add về 1 cái List
        Task<List <ProductViewModel>> GetAll();
        //Truyền vào 1 số request, keyword để tìm kiếm, stt trang, và pagesize để phân trang
        Task<PagedViewModel <ProductViewModel>> GetAllPaging(string keyword, int pageIndex, int pageSize);
    }
}
