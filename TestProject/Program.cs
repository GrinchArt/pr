using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestProject.Repository;
using TestProject.Services;
namespace TestProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<TestProjectDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TestProjectDbContext") ?? throw new InvalidOperationException("Connection string 'TestProjectDbContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ITestModelRepository, TestModelRepository>();
            builder.Services.AddScoped<ITestModelService, TestModelService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=TestModels}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
