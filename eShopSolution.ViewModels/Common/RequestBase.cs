using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    //Tất cả các request đều phải có 
    public class RequestBase
    {
        public string BearerToken { get; set; }
    }
}
