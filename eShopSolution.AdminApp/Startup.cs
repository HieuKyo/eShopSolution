using eShopSolution.AdminApp.Services;
using eShopSolution.ViewModels.System.Users;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Cấu hình toàn bộ thành phần được include vào và gọi trong file Program.cs
namespace eShopSolution.AdminApp
{
    //Cách Login
    //Login và sẽ gọi 1 web API, để API trả về 1 token, sau đó thực hiện tạo cái claims ở dưới và login như bình thường
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            //AddAuthentication cho nó
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
            {
                options.LoginPath = "/User/Login/";
                //Truy cập bị từ chối thì sẽ trở về trang Forbidden
                options.AccessDeniedPath = "/User/Forbidden/";
            });

            services.AddControllersWithViews()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

            services.AddSession(option =>
            {
                //Sesstion tồn tại trong vòng 30p
                option.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddTransient<IUserApiClient, UserApiClient>();

            IMvcBuilder builder = services.AddRazorPages();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            #if DEBUG
            if (environment == Environments.Development)
                {
                    builder.AddRazorRuntimeCompilation();
                }
            #endif
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
            //Trình tự đăng nhập. 
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //Authen xong rồi Routing rồi mới vào Author
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
