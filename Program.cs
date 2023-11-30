using DinkToPdf.Contracts;
using DinkToPdf;
using GraduaitionProjectITI.Models.Context;
using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.Reprository.BrandRepository;
using GraduaitionProjectITI.Reprository.CategoryReprositry;
using GraduaitionProjectITI.Reprository.OrderReprository;
using GraduaitionProjectITI.Reprository.ProductReprository;
using GraduaitionProjectITI.Reprository.ReviewReprositry;
using GraduaitionProjectITI.Reprository.SubOrderReprository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GraduaitionProjectITI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<ECcontext>(option=>
            
            option.UseSqlServer(builder.Configuration.GetConnectionString("con"))
            );
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ECcontext>();

            builder.Services.AddScoped<ICategoryReprositry, CategoryReprository>();
            builder.Services.AddScoped<IProductReprository, ProductReprository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IOrderReprository, OrderReprository>();
            builder.Services.AddScoped<ISubOrderReprository, SubOrderReprository>();
            builder.Services.AddScoped<IReviewReprositry, ReviewReprositry>();
            builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            builder.Services.AddSession();
            var app = builder.Build();

            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}" );
     
            app.MapControllerRoute(
                name: "Dashbord",
                pattern: "{controller=Dashbord}/{action=Index}");


            app.Run();
        }
    }
}