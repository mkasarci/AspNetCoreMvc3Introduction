using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvc3.Introduction.Models;
using AspNetCoreMvc3.Introduction.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace AspNetCoreMvc3.Introduction
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();  //Configured first service. I'm not sure if it is really necessary at 3.1. The code still working without this line.
            services.AddRazorPages();   //We gonna use RazorPages.
            var connection = @"Server=(localdb)\MSSQLLocalDB; Database=SchoolDb; Trusted_Connection=true ";
            services.AddDbContext<SchoolContext>(options => options.UseSqlServer(connection));
            services.AddSingleton<ICalculator, Calculator18>();
            services.AddSession();
            services.AddDistributedMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(
                        Directory.GetCurrentDirectory(),@"node_modules")),
                RequestPath = new PathString("/node_modules")
            });   //Added for use npm packages.

            //env.EnvironmentName = EnvironmentName.Production;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

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
        }

        private void ConfigureRoutes(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder.MapControllerRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            routeBuilder.MapControllerRoute("MyRoute", "{controller=Employee}/{action=Add}/{id?}");
            
            routeBuilder.MapRazorPages();

            routeBuilder.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
        }
    }
}
