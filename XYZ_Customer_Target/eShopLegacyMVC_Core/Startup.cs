using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.Extensions.Hosting; 
using Microsoft.Extensions.Logging; 
using Autofac; 
using Autofac.Extensions.DependencyInjection; 
using eShopLegacyMVC_Core.Modules; 
using eShopLegacyMVC_Core.Models; 
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Identity; 
using System; 
namespace eShopLegacyMVC_Core 
{ 
    public class Startup 
    { 
        public Startup(IConfiguration configuration) 
        { 
            Configuration = configuration; 
        } 
        public IConfiguration Configuration { get; } 
        public void ConfigureServices(IServiceCollection services) 
        { 
            services.AddDbContext<CatalogDBContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("CatalogDBContext"))); 
            services.AddDefaultIdentity<IdentityUser>() 
                .AddEntityFrameworkStores<CatalogDBContext>(); 
            services.AddControllersWithViews(); 
            services.AddRazorPages(); 
            services.AddAutofac(); 
            // Log the configuration loading for debugging purposes 
            var useMockData = Configuration.GetValue<bool>("UseMockData"); 
            var useCustomizationData = Configuration.GetValue<bool>("UseCustomizationData"); 
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole()); 
            var logger = loggerFactory.CreateLogger<Startup>(); 
            logger.LogInformation("UseMockData: {UseMockData}, UseCustomizationData: {UseCustomizationData}", useMockData, useCustomizationData); 
        } 
        public void ConfigureContainer(ContainerBuilder builder) 
        { 
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole()); 
            builder.RegisterModule(new ApplicationModule(Configuration, loggerFactory)); 
        } 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) 
        { 
            // Global exception handler 
            app.Use(async (context, next) => 
            { 
                try 
                { 
                    await next(); 
                } 
                catch (Exception ex) 
                { 
                    logger.LogError(ex, "An unhandled exception occurred."); 
                    context.Response.Redirect("/Home/Error"); 
                } 
            }); 
            if (env.IsDevelopment()) 
            { 
                app.UseDeveloperExceptionPage(); 
            } 
            else 
            { 
                app.UseExceptionHandler("/Home/Error"); 
                app.UseHsts(); 
            } 
            app.UseHttpsRedirection(); 
            app.UseStaticFiles(); 
            app.UseRouting(); 
            app.UseAuthentication(); // Add this line for enabling authentication 
            app.UseAuthorization(); 
            app.UseEndpoints(endpoints => 
            { 
                endpoints.MapControllerRoute( 
                    name: "default", 
                    pattern: "{controller=Catalog}/{action=Index}/{id?}"); 
                endpoints.MapRazorPages(); // Add this line to map Razor Pages 
            }); 
            logger.LogInformation("Application started and configured."); 
        } 
    } 
}