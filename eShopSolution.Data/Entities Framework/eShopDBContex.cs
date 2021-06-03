using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities_Framework
{
    // Cái : là kế thừa (Kế thừa từ FrameworkCore)
    public class eShopDBContex : DbContext
    {
        //Được gợi ý truyền vào DbContexOption
        public eShopDBContex(DbContextOptions options) : base(options)
        {
        
        }
        //Tên thuộc tính với DbSet thì phải chữ hoa và số nhièu
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
