using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ZLMediaServerManagent.Commons;
using ZLMediaServerManagent.DataBase;
using ZLMediaServerManagent.Filters;
using ZLMediaServerManagent.Hubs;
using ZLMediaServerManagent.Implements;
using ZLMediaServerManagent.Interface;
using ZLMediaServerManagent.Models.Dto;

namespace ZLMediaServerManagent
{
    public class Startup
    {

        public IApplicationBuilder App { get; set; }
        private static Startup _instance;
        public static Startup Instance { get => _instance; }


        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.Configure<CookiePolicyOptions>(option =>
            {
                option.CheckConsentNeeded = context => false;
            });

            services.AddControllersWithViews(option =>
            {
                option.Filters.Add<ActionFilter>();
            }).AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    // options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.ContractResolver = new CustomContractResolver();
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                }
                ).AddRazorRuntimeCompilation()
                    .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            // services.AddAuthentication();
            // services.AddAuthenticationCore();
            // services.AddAuthorization();
            // services.AddAuthorizationCore();
            services.AddAuthentication(options =>
                 {
                     options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                     
                 }).AddCookie(opt => { opt.LoginPath = new PathString("/Home/Login"); });


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IDomainAndAppService, DomainAndAppService>();
            services.AddScoped<IZLServerService, ZLServerService>();

            services.AddAutoMapper(typeof(MappingProfile));
            services.AddSignalR();




            initDataBase(ref services);
            services.AddHostedService<ZLBackGroundTask>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.App = app;

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
            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            // app.UseCookiePolicy();
            app.UseRouting();

            //开启身份认证
            app.UseAuthentication();
            //开启授权验证([Authorize])
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<MessageHub>("/Hubs/MessageHub");
            });
        }

        /// <summary>
        /// InitDataBase    初始化数据库
        /// </summary>
        private void initDataBase(ref IServiceCollection services)
        {
            var dbType = Configuration["DataBase:DbType"];
            var dbConnectionString = Configuration["DataBase:DbConnectionString"];
            var optionsBuilder = new DbContextOptionsBuilder<ZLDataBaseContext>();
            switch (dbType)
            {
                case "sqlite":
                    optionsBuilder.UseSqlite(dbConnectionString);
                    services.AddDbContext<ZLDataBaseContext>(o => o.UseSqlite(dbConnectionString));
                    break;
                case "sqlserver":
                    optionsBuilder.UseSqlServer(dbConnectionString);
                    services.AddDbContext<ZLDataBaseContext>(o => o.UseSqlServer(dbConnectionString));
                    break;
                case "oracle":
                    optionsBuilder.UseOracle(dbConnectionString);
                    services.AddDbContext<ZLDataBaseContext>(o => { o.UseOracle(dbConnectionString); });
                    break;
                case "mysql":
                    optionsBuilder.UseMySQL(dbConnectionString);
                    services.AddDbContext<ZLDataBaseContext>(o => o.UseMySQL(dbConnectionString));
                    break;
                case "postgres":
                    optionsBuilder.UseNpgsql(dbConnectionString);
                    services.AddDbContext<ZLDataBaseContext>(o => o.UseNpgsql(dbConnectionString));
                    break;
            }
            try
            {

                using (var dbContext = new ZLDataBaseContext(optionsBuilder.Options))
                {
                    Console.WriteLine("开始初始化数据库....");
                    if (dbContext.Database.EnsureCreated())
                    {
                        //初始化数据,等用户访问/Home/Init
                    }
                    else
                    {
                        //从数据库里读到GloableCache中
                        ReadDataBase2Cache(dbContext);
                    }
                }
                Console.WriteLine("数据库校验通过!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("创建数据库异常:" + ex.Message);
                Console.WriteLine("程序即将退出...");
                Environment.Exit(0);
            }
        }

        public static void ReadDataBase2Cache(ZLDataBaseContext dataBaseContext)
        {
            DataBaseCache.Configs = dataBaseContext.Configs.ToList();
            DataBaseCache.MenuRoles = dataBaseContext.MenuRoles.ToList();
            DataBaseCache.Menus = dataBaseContext.Menus.ToList();
            DataBaseCache.Roles = dataBaseContext.Roles.ToList();
            DataBaseCache.UserRoles = dataBaseContext.UserRoles.ToList();
            DataBaseCache.Users = dataBaseContext.Users.ToList();
            DataBaseCache.Domains = dataBaseContext.Domains.ToList();
            DataBaseCache.Applications = dataBaseContext.Applications.ToList();
            DataBaseCache.StreamProxies = dataBaseContext.StreamProxies.ToList();

            if (GloableCache.IsInitServer)
            {
                var ip = DataBaseCache.Configs.Where(p => p.ConfigKey == ConfigKeys.ZLMediaServerIp).FirstOrDefault().ConfigValue;
                var port = DataBaseCache.Configs.Where(p => p.ConfigKey == ConfigKeys.ZLMediaServerPort).FirstOrDefault().ConfigValue;
                var sec = DataBaseCache.Configs.Where(p => p.ConfigKey == ConfigKeys.ZLMediaServerSecret).FirstOrDefault().ConfigValue;
                var client = new STRealVideo.Lib.ZLClient("http://" + ip + ":" + port, sec);
                var configs = client.getServerConfig();
                if (configs.code == -300)
                {
                    GloableCache.ZLServerOnline = false;
                }
                else
                {
                    GloableCache.ZLClient = client;
                    GloableCache.ZLMediaServerConfig = configs.data.FirstOrDefault();
                    GloableCache.ZLServerOnline = true;
                }

            }

        }
    }
}
