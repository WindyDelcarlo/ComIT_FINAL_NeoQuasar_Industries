using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Data;

public class NeoQuasarDBContext : DbContext
{
    public DbSet<StockItemData> StockItems {get;set;}
    // ItemSKU | ManufacturerSKU | Name | ManufacturerID | Description | Price | StockCategory | ImageLocation | DontRestock | Stock
    public DbSet<CartLineData> CartLines {get;set;}
    // CartLineID | UserID | ItemSKU | Quantity
    public DbSet<OrderData> Orders {get;set;}
    // OrderID | UserID | Shipper | Recipient | Direction | Date | Completed
    public DbSet<OrderLineData> OrderLines {get;set;}
    // OrderID | ItemSKU | AmountOrdered | Shipped
    public DbSet<ReceiptData> Receipts {get;set;}
    // ReceiptID | OrderID | Courier | WaybillNumber
    public DbSet<ReceiptLineData> ReceiptLines {get;set;}
    // ReceiptID | ItemSKU | NumberReceived
    public DbSet<ManufacturerData> Manufacturers {get;set;}
    // ManufacturerID | Name | Abbreviation | DimCode | Contact Address
    public DbSet<UserData> Users {get;set;}
    // UserID | Username | Name | StellarNet | SlingName | Address | LOC Code | Role
    public DbSet<PaymentDetailData> PaymentDetails {get;set;}
    // PaymentDetailID | CardName | CardNumber | CardType
    public DbSet<PassHashData> PassHashes {get;set;}
    // UserID | PassHash

    public NeoQuasarDBContext()
    {
        
    }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);


        if (!optionsBuilder.IsConfigured)
        {
            
        string appDir = AppDomain.CurrentDomain.BaseDirectory;
        if (appDir.Contains("bin"))
            {
                appDir = appDir.Substring(0, appDir.IndexOf("bin"));
            }
            Console.WriteLine(appDir);
            var dbPath = Path.Combine(appDir,$"db{Path.DirectorySeparatorChar}nqi.db");
            Console.WriteLine(dbPath);
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }

}