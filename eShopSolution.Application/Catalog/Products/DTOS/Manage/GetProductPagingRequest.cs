﻿using eShopSolution.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Application.Catalog.Products.DTOS.Manage
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        //Tìm kiếm = List (Mảng) các Category và keyword kể tìm kiếm
        public string Keyword { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
