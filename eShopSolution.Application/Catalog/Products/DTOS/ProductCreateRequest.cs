using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Application.Catalog.Products.DTOS
{
    public class ProductCreateRequest
    {
        //Thêm request
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
