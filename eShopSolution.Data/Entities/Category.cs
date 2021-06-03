using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
        public class Category
    {
        public int Id { set; get; }
        public int SortOrder { set; get; }
        public bool IsShowOnHome { set; get; }
        public int? ParentID { set; get; }
        //Kiểu Enums Trong class Enums sẽ cho ra 2 kiểu Active và InActive giống bool True False
        public Status Status { set; get; }

    }
}
