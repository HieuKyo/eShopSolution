using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace eShopSolution.ViewModels.Common
{
    public class ApiErrorResult<T> :  ApiResult<T>
    {
        public string[] ValidationErrors { get; set; }
        public ApiErrorResult(string message)
        {
            IsSuccessed = false;
            Message = message;
        }

        public ApiErrorResult(string[] validationErrors)
        {
            IsSuccessed = false;
            ValidationErrors = validationErrors;
        }
        public ApiErrorResult()
        {
        }
    }
}
