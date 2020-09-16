using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.CodeAnalysis.Options;

namespace SSHZHAKAR
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string connectionString = "Data Source=DESKTOP-G92CTF5;Initial Catalog=ZhakarSSH;Integrated Security=True;" + "MultipleActiveResultSets=True";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<AppDbContext>(config => {
                config.UseSqlServer(connectionString);
            });

            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(connectionString));
                
            //services.AddIdentity<IdentityUser, IdentityRole>(config => {
            //    config.Password.RequiredLength = 4;
            //    config.Password.RequireDigit = false;
            //    config.Password.RequireNonAlphanumeric = false;
            //    config.Password.RequireUppercase = false;
            //    config.Password.RequireLowercase = false;

            //})
            //    .AddEntityFrameworkStores<AppDbContext>()
            //    .AddDefaultTokenProviders();
            services.AddDefaultIdentity<IdentityUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<AppDbContext>();
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/login";
            });
            services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "Resources";
            });


            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMvc()
                .AddViewLocalization(opt => { opt.ResourcesPath = "Resources"; })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            services.Configure<RequestLocalizationOptions>(opts =>
            {

                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru"),

                };
                opts.DefaultRequestCulture = new RequestCulture("en");

                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Program> logger)
        {
            logger.LogInformation("Processing request {0}", "AYYYYYYYF");
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

            var options = app.ApplicationServices.GetService<Option<RequestLocalizationOptions>>();
            var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru"),

                };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }




    }
}
