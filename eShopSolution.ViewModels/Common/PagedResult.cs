using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    //Cần trả về 1 cái model mà có đầy đủ các TotalPage ,.....
    //Cần 1 DTO chung ở đây, gọi là common DTO
    public class PagedResult<T> : PagedResultBase
    {
        //Truyền List T vào các loại đối tượng khác nhau
        public List<T> Items { set; get; }
    }
}
