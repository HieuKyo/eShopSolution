using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    //Chứ message result, coi seccsion đó có success k, và message trả ra là gì
    public class ApiResult<T>
    {

        public bool IsSuccessed { get; set; }
        public string Message { get; set; }
        public T ResultObj { get; set; }

    }
}
