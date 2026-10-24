using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Data;

public class NeoQuasarDBContext : DbContext
{
    public DbSet<StockItemData> StockItems {get;set;}
    public DbSet<CartLineData> CartLines {get;set;}
    public DbSet<OrderData> Orders {get;set;}
    public DbSet<OrderLineData> OrderLines {get;set;}
    public DbSet<ReceiptData> Receipts {get;set;}
    public DbSet<ReceiptLineData> ReceiptLines {get;set;}
    public DbSet<ManufacturerData> Manufacturers {get;set;}
    public DbSet<UserData> Users {get;set;}
    public DbSet<PaymentDetailData> PaymentDetails {get;set;}
    public DbSet<FileData> Files {get;set;}
    public DbSet<FileContentData> FileContent {get;set;}
    public DbSet<DimensionData> Dimensions {get;set;}
    public DbSet<DimensionLOCData> DimensionLOCs {get;set;}


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