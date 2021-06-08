using eShopSolution.Application.Catalog.Products;
using eShopSolution.Application.Common;
using eShopSolution.Data.Entities_Framework;
using eShopSolution.Utilities.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi
{
    public class Startup
    {
        //WEb API chỉ cần Module và Controller
        //Startup gồm 2 thành phần, 2 phương thức chính
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<eShopDBContext>(option => option.UseSqlServer(Configuration.GetConnectionString(SystemConstants.MainConnectionString)));

            //Khai báo DI, đối với thằng IManageProduct thì nó sẽ chạy ra gì
            //AddTransient - mỗi lần request thì nó sẽ tạo mới
            //Nó sẽ chỉ cho thằng DI biết là
            //Nếu chúng ta yêu cầu 1 đối tượng IPublicProductService thì nó sẽ instance về cho ta 1 đối tượng của class PublicProductService
            services.AddTransient<IPublicProductService, PublicProductService>();
            //Cứ instance kiểu IStorageService thì sẽ tạo ra 1 instance FileStorageService
            services.AddTransient<IStorageService, FileStorageService>();
            services.AddTransient<IManageProductService, ManageProductService>();

            services.AddControllersWithViews();
            //Tạo phương thức Swagger rồi sử dụng ở bên dưới
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger eShop Solution", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //Use Swagger và sử dụng cấu hình Swagger UI
            //Show được ra các menthod của api
            //Vào launchSetting để add cái đường dẫn launchURL
            //CÁi Swagger này dùng để xem các phương thức API của project, có thể Excute để trả và result
            //Add Swagger vào để xem được danh sách, mỗi 1 con controller thì nó sẽ có những pương thức nào
            //Đầu vào đầu ra, danh sách tham số, mã lỗi trả về.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger eShopSolution v1");
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
