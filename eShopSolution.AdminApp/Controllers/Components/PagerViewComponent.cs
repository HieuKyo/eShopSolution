using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        //Cần 1 phương thức InvokeAsync trả về 1 ViewComponent result
        //Giờ thằng nào muốn phân trang thì truyền vô PagedResultBase
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            //Cần 1 tham số đầu vào result trả về 1 cái view tên "Default" trong Share Component
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }

    }
}
