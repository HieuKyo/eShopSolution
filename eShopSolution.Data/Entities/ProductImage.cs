using System;

namespace eShopSolution.Data.Entities
{
    //Thêm bảng Image để quản lý hình ảnh 
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        //Đường dẫn ảnh
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public bool IsDefault { get; set; }
        public DateTime DateCreated { get; set; }
        public int SortOrder { get; set; }
        public long FileSize { get; set; }
        //Khoá ngoại
        public Product Product { get; set; }
        //Xong qua tạo Configuration

    }
}
