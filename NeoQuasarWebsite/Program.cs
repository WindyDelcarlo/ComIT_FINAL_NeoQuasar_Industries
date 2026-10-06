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

        builder.Services.AddScoped<IPasswordHasher<UserData>, PasswordHasher<UserData>>();
        builder.Services.AddScoped<IAccountService, AccountService>();

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        DatabasePrime();

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

    public static NeoQuasarDBContext Database()
    {
        return nqiDatabase;
    }

    public static void DatabasePrime()
    {

        PasswordHasher<UserData> passwordHasher = new PasswordHasher<UserData>();

        List<UserModel> userDefaults = new List<UserModel>{
            UserFactory.Create("Marianne Rey", "mrey", "Internal", "NEOQUASAR-001", "MariNEO", ".nqi/neo.marianne", UserRole.ADMIN),
            UserFactory.Create("Silva Astera", "StarlitePrincess", "Astera, Altair, Capital Center-001", "VEN0-A112-01", "StarlightPrincess", "ven0/kari.astera4", null),
            UserFactory.Create("Lexi Astera", "TachyonCascade", "Research & Development, Nebula Complex", "TSA-RD01121", "TachyonCascade", "ark0/veris.tachcasc0", null)
        };
        List<string> userPasswords = new List<string>
        {
            "WriteOnTime",
            "ET4Ever",
            "IKissedAGirlAndILikedIt"
        };

        for (int i = 0; i < 3; i++)
        {
            UserData user = new UserData
            {
                Username = userDefaults[i].Username,
                Name = userDefaults[i].Name,
                Address = userDefaults[i].Address,
                LocatorCode = userDefaults[i].LocatorCode,
                StellarNet = userDefaults[i].StellarNet,
                SlingName = userDefaults[i].SlingName,
                Role = userDefaults[i].UserRole.ToString()
            };
            user.PassHash = passwordHasher.HashPassword(user,userPasswords[i]);
            nqiDatabase.Users.Add(user);
        }

        Manufacturer.DatabaseAdd("NeoQuasar", ".nqi", "NQ", ".nqi/neo.marianne");
        Manufacturer.DatabaseAdd("Martin Industries", "ven0", "MI", "ven0/martin.erikas");
        Manufacturer.DatabaseAdd("Stillmetal Laboratories", "oria", "SL", "FL-022-1334");
        Manufacturer.DatabaseAdd("Corry Industrial", "ven0", "CI", "ven0/kari.ci.sarrik");
        Manufacturer.DatabaseAdd("EsterTech", "ven0", "ET", "ven0/estert.mnevva");
        Manufacturer.DatabaseAdd("Silweave", "ven0", "SW", "ven0/kari.sw.mordire");
        Manufacturer.DatabaseAdd("StarTamers", "tri0", "ST", "StarTamers:LAmar281");
        Manufacturer.DatabaseAdd("Levin Starlight", "ark0", "LS", "ark0/ryl.cormigni");
        Manufacturer.DatabaseAdd("EnTech", ".egd", "ET", "1.734.2233-001");
        Manufacturer.DatabaseAdd("Karisou Industries", "ven0", "KI", "ven0/kari.virela");

        StockItem.DatabaseAdd(StockItemFactory.Create("Crystal Reflux Generator", "CRG-0001", "Martin Industries", StockCategory.REACTOR, "Flow generator producing 600 An/s in dimspace.", 12500, 0));
        StockItem.DatabaseAdd(StockItemFactory.Create("CRG Heating Coil", "CRG-0102", "Martin Industries", StockCategory.REACTOR_PART, "Replacement heating coil for standard-sized crystal reflux generators. 3kW.", 1000, 0));
        StockItem.DatabaseAdd(StockItemFactory.Create("Inertial Emulator Card", "QC-1002", "Martin Industries", StockCategory.QUANTUM_COMPUTER, "Accepts flickerspace information input and sends signals to realsim about how the ship is moving.", 7500, 0));
        StockItem.DatabaseAdd(StockItemFactory.Create("IL-2 Quantum Core", "QC-0100", "Martin Industries", StockCategory.QUANTUM_COMPUTER, "Quantum computer capable of processing 3 thousand flickerspace operations per second.", 22000, 0));
        StockItem.DatabaseAdd(StockItemFactory.Create("Archaemin Drive Simple", "archaed-2", "Stillmetal Laboratories", StockCategory.ARCANOHARMONIC_DRIVE, "Use flow to travel flickerspace with the equivalent of 5k FSO/s.", 16330, 0));
        StockItem.DatabaseAdd(StockItemFactory.Create("Arcanoharmonic Spectrometer", "SEN-H-1", "Corry Industrial", StockCategory.SENSOR, "Reads information about surrounding arcanoharmonics.", 6000, 0));
        StockItem.DatabaseAdd(StockItemFactory.Create("Dummy Item", "TEST-1", "EsterTech", StockCategory.MISC, "An item for dummies.", 1, 0));
        StockItem.DatabaseAdd(StockItemFactory.Create("Star Plushie", "PLSH-STAR", "Silweave", StockCategory.COMFORT, "A plush star.", 20, 0));
        StockItem.DatabaseAdd(StockItemFactory.Create("EVA Suit - Astera Surplus", "EVA-ASTR", "Silweave", StockCategory.MISC, "An EVA suit designed for use by the Asteran navy. Disarmed.", 25000, 0));
        StockItem.DatabaseAdd(StockItemFactory.Create("*ShipLife* Oxygen Circulation System", "CLS-011", "StarTamers", StockCategory.LIFE_SUPPORT, "Designed for clean air in flickerspace realsim.", 10500, 0));
        StockItem.DatabaseAdd(StockItemFactory.Create("Flow Evaporation Field", "FS-DEF-00112", "Levin Starlight", StockCategory.COMBAT, "A field that absorbs and dissipates magical attacks.", 9000, 0));
        StockItem.DatabaseAdd(StockItemFactory.Create("CrossTalk Sr5", "CT5", "Karisou Industries", StockCategory.COMFORT, "A personal smartphone. Plan not sold here.", 400, 0));
        StockItem.DatabaseAdd(StockItemFactory.Create("Gorgon 360/180 Railcannon", "3K", "EsterTech", StockCategory.COMBAT, "Railcannon designed to fire standard ammunition.", 20000, 0));
        StockItem.DatabaseAdd(StockItemFactory.Create("Vector Impulse Thruster", "VIT1", "EnTech", StockCategory.DIMSPACE_THRUSTERS, "Flow-based thrusters for traveling in dimensional space. Sold individually", 6400, 0));


        new UserModel(UserModel.GetDatabase().Find(1)).AddPaymentMethod(new PaymentDetails(PaymentDetails.Type.STELLARNET_DEBIT, "MARIANNE REY", "nq1441-258"));
        OrderFactory.Create(Order.Direction.INCOMING, 1, new List<LineItem> { new LineItem(StockItem.GetDatabase().ToList()[2].NeoSKU, 3), new LineItem(StockItem.GetDatabase().ToList()[1].NeoSKU, 2) }, Manufacturer.GetDatabase().Find(Manufacturer.ManufacturerByString("Martin Industries")),ShippingOptions.STANDARD);
        new UserModel(UserModel.GetDatabase().Find(1)).AddToCart(StockItem.GetDatabase().ToList()[2].NeoSKU, 2);
        new UserModel(UserModel.GetDatabase().Find(1)).AddToCart(StockItem.GetDatabase().ToList()[5].NeoSKU, 1);
        new UserModel(UserModel.GetDatabase().Find(1)).AddToCart(StockItem.GetDatabase().ToList()[1].NeoSKU, 6);

    }

    public static string Date()
    {
        double day = DateTime.Now.DayOfYear;
        day += DateTime.Now.Hour/24;
        day += DateTime.Now.Minute/1440;
        day += DateTime.Now.Second/86400;
        double dayDecimal = day/365;
        StringBuilder sb = new StringBuilder();
        sb.Append(dayDecimal);
        string decimalString = sb.ToString().Substring(2,6);
        return $"{DateTime.Now.Year}.{decimalString}";
    }
}
