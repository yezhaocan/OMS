using OMS.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using System;

namespace OMS.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("sitemap.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //注入配置
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<SiteMap>(Configuration.GetSection("SiteMap"));
            //返回json日期格式化
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm";
            });
            //数据库
            services.AddDbContext<Data.Implementing.OMSContext>(options =>
                   options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], r => r.UseRowNumberForPaging()));
            //cookie
            services.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            //service
            services.AddScoped<Data.Interface.IDbAccessor, Data.Implementing.DbAccessor>();
            services.AddScoped<Core.IWorkContext, WebCore.WebWorkContext>();
            services.AddScoped<Services.Account.IUserService, Services.Account.UserService>();
            services.AddScoped<Services.Customer.ICustomerService, Services.Customer.CustomerService>();
            services.AddScoped<Services.Permissions.IMenuService, Services.Permissions.MenuService>();
            services.AddScoped<Services.Permissions.IRoleService, Services.Permissions.RoleService>();
            services.AddScoped<Services.Permissions.IUserRoleService, Services.Permissions.UserRoleService>();
            services.AddScoped<Services.Permissions.IPermissionService, Services.Permissions.PermissionService>();
            services.AddScoped<Services.Permissions.IRolePermissionService, Services.Permissions.RolePermissionService>();
            services.AddScoped<Services.Permissions.IUserPermissionService, Services.Permissions.UserPermissionService>();
            services.AddScoped<Services.Authentication.IAuthenticationService, Services.Authentication.FormsAuthenticationService>();
            services.AddScoped<Services.Common.ICommonService, Services.Common.CommonService>();
            services.AddScoped<Services.Order1.IOrderService, Services.Order1.OrderService>();
            services.AddScoped<Services.Products.IProductService, Services.Products.ProductService>();
            services.AddScoped<Services.IWareHouseService, Services.WareHouseService>();
            //定时任务
            services.AddTimedJob();
            //session
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            //nlog
            env.ConfigureNLog("nlog.config");
            loggerFactory.AddNLog();
            app.AddNLogWeb();
            LogManager.Configuration.Variables["connectionString"] = Configuration["ConnectionStrings:DefaultConnection"];

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/PageError");
            }

            app.UseAuthentication();
            app.UseSession();
            //路由
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "permissionRole",
                    template: "{controller=Permission}/{action=Role}/{id}");

                routes.MapRoute(
                    name: "permissionUser",
                    template: "{controller=Permission}/{action=User}/{id}");

                routes.MapRoute(
                    name: "UserRole",
                    template: "{controller=User}/{action=UserRole}/{id}");
            });
            //初始化map
            WebCore.AutoMapperInit.Init();
            //读取静态文件，默认wwwroot文件夹
            app.UseStaticFiles();
        }
    }
}
