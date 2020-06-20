using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvc3.Introduction.Identity;
using AspNetCoreMvc3.Introduction.Models;
using AspNetCoreMvc3.Introduction.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace AspNetCoreMvc3.Introduction
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration; //Can be readable from appsettings.json
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();  //Configured first service. I'm not sure if it is really necessary at 3.1. The code still working without this line.
            services.AddRazorPages();   //We gonna use RazorPages.
            services.AddDbContext<SchoolContext>(options => options.UseSqlServer(_configuration["dbConnection"]));
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(_configuration["dbConnection"]));
            services.AddSingleton<ICalculator, Calculator18>();
            services.AddSession();
            services.AddDistributedMemoryCache();
            
            //Added for authentication process
            services.AddIdentity<AppIdentityUser, AppIdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;

                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                options.SignIn.RequireConfirmedAccount = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Security/Login";
                options.LogoutPath = "/Security/Logout";
                options.AccessDeniedPath = "/Security/AccessDenied";
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".AspNetCoreDemo.Security.Cookie",
                    Path = "/",
                    SameSite = SameSiteMode.Lax,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //env.EnvironmentName = EnvironmentName.Production;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(
                        Directory.GetCurrentDirectory(), @"node_modules")),
                RequestPath = new PathString("/node_modules")
            });   //Added for use npm packages.

            app.UseRouting();

            ////Middleware add for Core 2.1
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template:"{controller=Home}/{action=Index}/{id?}");
            //});

            //Middleware Add for Core 3.0
            //app.UseEndpoints(builder => builder.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}"));
            //Code could be shortened like this.

            app.UseSession();

            app.UseEndpoints(ConfigureRoutes);

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "areas",
            //        template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            //    );
            //});

            app.UseAuthentication();
            app.Map("/test", builder =>
            {
                builder.Run(async context => 
                    { await context.Response.WriteAsync("Hello from test."); });

            });
        }

        private void ConfigureRoutes(IEndpointRouteBuilder routeBuilder)
        {
            //This one can be use as routeBuilder.MapDefaultControllerRoute;
            routeBuilder.MapControllerRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            
            routeBuilder.MapControllerRoute("MyRoute", "{controller=Employee}/{action=Add}/{id?}");
            
            routeBuilder.MapRazorPages();

            routeBuilder.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
        }
    }
}
