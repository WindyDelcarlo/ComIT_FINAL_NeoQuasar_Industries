using Microsoft.AspNetCore.Authentication.Cookies;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite;

public class Program
{
    public static void Main(string[] args)
    {
        DatabaseQuery();

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => builder.Configuration.Bind("CookieSettings",options));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Storefront}/{id?}")
            .WithStaticAssets();

        app.Run();
    }

    public static void DatabaseQuery()
    {
        UserModel.DatabaseAdd(UserFactory.Create("Marianne Rey", "mrey","Internal","NEOQUASAR-001","MariNEO",".nqi/neo.marianne", UserRole.ADMIN),"WriteOnTime");
        UserModel.DatabaseAdd(UserFactory.Create("Silva Astera","StarlitePrincess","Astera, Altair, Capital Center-001","VEN0-A112-01","StarlightPrincess","ven0/kari.astera4",null),"ET4Ever");
        UserModel.DatabaseAdd(UserFactory.Create("Lexi Astera","TachyonCascade","Research & Development, Nebula Complex","TSA-RD01121","TachyonCascade","ark0/veris.tachcasc0",null),"IKissedAGirlAndILikedIt");
        UserModel.GetDatabase()[1].AddPaymentMethod(new PaymentDetails(PaymentDetails.Type.STELLARNET_DEBIT,"MARIANNE REY","nq1441-258"));
        OrderFactory.Create(Order.Direction.INCOMING,UserModel.GetDatabase()[1],new List<LineItem>{new LineItem(StockItem.GetDatabase()[2],3),new LineItem(StockItem.GetDatabase()[1],2)},Manufacturer.ManufacturerByString("Martin Industries"));
        UserModel.GetDatabase()[1].AddToCart(StockItem.GetDatabase()[2],2);
        UserModel.GetDatabase()[1].AddToCart(StockItem.GetDatabase()[5],1);
        UserModel.GetDatabase()[1].AddToCart(StockItem.GetDatabase()[1],6);

    }
}
