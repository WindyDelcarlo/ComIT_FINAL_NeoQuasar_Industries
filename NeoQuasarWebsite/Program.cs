using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.Models;
using NeoQuasarWebsite.Services;

namespace NeoQuasarWebsite;

public class Program
{

    public static NeoQuasarDBContext nqiDatabase;
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
        });

        nqiDatabase = new NeoQuasarDBContext();
        nqiDatabase.Database.EnsureCreated();

        builder.Services.AddDbContext<NeoQuasarDBContext>();
        builder.Services.AddSession(options =>
        {
            options.Cookie.Name = ".NeoQuasar.Session";
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddScoped<IPasswordHasher<UserData>, PasswordHasher<UserData>>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<ISearchService,SearchService>();
        builder.Services.AddScoped<IDataService,DataService>();
        builder.Services.AddScoped<IOrderService,OrderService>();
        builder.Services.AddScoped<IStockService,StockService>();
        builder.Services.AddScoped<IAnonService,AnonService>();
        builder.Services.AddScoped<IPaymentService,PaymentService>();
        builder.Services.AddScoped<IFileService,FileService>();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Generating a temporary scope to prime the database.
        using var scope = app.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<NeoQuasarDBContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<UserData>>();
        DatabaseHelper.Initialize(database,passwordHasher);
        DatabaseHelper.InitializeImages(builder.Environment.WebRootPath);


        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseRouting();
        app.UseSession();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Storefront}/{id?}")
            .WithStaticAssets();

        app.Run();

    }
   
    // This is here to decimalize the current date and time. Yes, I could just use DateTime and it'd be fine. No, I'm not doing that for this project.
    public static string Date()
    {
        double day = DateTime.Now.DayOfYear;
        day += (double) DateTime.Now.Hour/24;
        day += (double) DateTime.Now.Minute/1440;
        day += (double) DateTime.Now.Second/86400;
        double dayDecimal = day/365;
        StringBuilder sb = new StringBuilder();
        sb.Append(dayDecimal);
        int outputLength = sb.ToString().Length < 6 ? sb.ToString().Length : 6;
        string decimalString = sb.ToString().Substring(2,outputLength);
        return $"{DateTime.Now.Year}.{decimalString}";
    }
}
