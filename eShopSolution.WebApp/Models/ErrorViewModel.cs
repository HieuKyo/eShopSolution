using System;

//Thư mục Model chứ các model dữ liệu
namespace eShopSolution.WebApp.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
