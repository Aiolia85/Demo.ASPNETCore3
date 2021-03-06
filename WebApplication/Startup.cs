using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Reoository.EF;

namespace WebApplication
{
    public class ConnectionStrings
    {
        public string DefaultSQLite { get; set; }
        public string LocalDB { get; set; }
        public string LocalDB1 { get; set; }
    }
    public class AppSetting
    {
        public string AllowedHosts { get; set; }
        public string TestValue { get; set; }

        public ConnectionStrings ConnectionStrings { get; set; }
    }

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
            services.AddMvc(options => options.Filters.Add<GlobalExceptionFilter>());

            services.AddControllersWithViews();

            services.Configure<AppSetting>(Configuration);
            var appSetting = Configuration.Get<AppSetting>();

            services.AddAutoMapper(o => o.AddProfile(new AutoMapperPerfile()));

            //cookies
            services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                //这里我们将Cookie过期时间设置成了365天，你也可以通过MaxAge属性来设置，不过这里有点需要注意的是，MaxAge会替代Expiration的值
                //options.Cookie.Expiration = TimeSpan.FromDays(365);
                //options.Cookie.MaxAge = TimeSpan.FromDays(365);
                //设置我们自己的数据加密实现
                //options.DataProtectionProvider = new MyDataProtectionProvider();
                options.LoginPath = "/Account/CookieLogin";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            //windows authentication
            //services.AddAuthentication(IISDefaults.AuthenticationScheme);
            //services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();


            //#region jwt
            //var configuration = new ConfigurationBuilder()
            //        .AddJsonFile("Config/jwt.json")
            //        .Build();
            //services.Configure<JwtSetting>(configuration.GetSection("JwtSetting"));

            //var token = configuration.GetSection("JwtSetting").Get<JwtSetting>();

            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    //Token Validation Parameters
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        //获取或设置要使用的Microsoft.IdentityModel.Tokens.SecurityKey用于签名验证。
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.
            //        GetBytes(token.SecurityKey)),
            //        //获取或设置一个System.String，它表示将使用的有效发行者检查代币的发行者。
            //        ValidIssuer = token.Issuer,
            //        //获取或设置一个字符串，该字符串表示将用于检查的有效受众反对令牌的观众。
            //        ValidAudience = token.Audience,
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //    };
            //});
            //#endregion

            //entity framework core
            //services.AddDbContext<SQLiteDbContext>(options =>
            //   options.UseSqlite(
            //       appSetting.ConnectionStrings.DefaultSQLite,
            //       b => b.MigrationsAssembly(typeof(SQLiteDbContext).Assembly.FullName)));
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

            app.UseMiddleware<RequestLoggerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        //autofac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<ConfigureAutofac>();

        }
    }
}
