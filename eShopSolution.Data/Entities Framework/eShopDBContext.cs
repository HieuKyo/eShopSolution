using eShopSolution.Data.Configurations;
using eShopSolution.Data.Entities;
using eShopSolution.Data.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities_Framework
{
    // Cái : là kế thừa (Kế thừa từ FrameworkCore)
    // DBContext kế thừa từ Identity nên nó sẽ có kèm theo 3 thuộc tính
    // User, Role, String
    public class eShopDBContext : IdentityDbContext <AppUser,AppRole,Guid>
    {
        //Được gợi ý truyền vào DbContexOption
        public eShopDBContext(DbContextOptions options) : base(options)
        {
        
        }
        //Phương thức OnModelCreating giúp ta làm 1 số việc khi tạo DbContext
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Config = Fluent API
            //Tạo data mẫu để xài
            //Configure using Fluent API
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new AppConfigConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductInCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());

            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryTranslationConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTranslationConfiguration());
            modelBuilder.ApplyConfiguration(new PromotionConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());

            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());

            //Config = Identity - tạo migration ASP.Net core Identity database
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            //UserRole này có 2 key nên là phải xài ngoặc nhọn, Ctrl Chuột phải để coi key
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new {x.UserId,x.RoleId });
            //UserLogin này cần 1 khoá chính, là Haskey -> key nó là 
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            //UserToken
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);

            //Data seeding
            modelBuilder.Seed();

            //base.OnModelCreating(modelBuilder);
        }
        //Tên thuộc tính với DbSet thì phải chữ hoa và số nhièu
        //Quản lý object, table thông qua các Proxy thành các class trong C#
        //Mọi Config đều nằm trong DbSet
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AppConfig> AppConfigs { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<ProductInCategory> ProductInCategories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
