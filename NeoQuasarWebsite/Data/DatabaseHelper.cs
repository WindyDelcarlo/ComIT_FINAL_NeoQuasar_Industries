using System.Text;
using Microsoft.AspNetCore.Identity;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Data;

public static class DatabaseHelper
{
    public static void Initialize(NeoQuasarDBContext database, IPasswordHasher<UserData> passwordHasher)
    {
        if (!database.Users.Any(u => u.UserID == -1))
        {
            database.Add(new UserData
            {
                UserID = -1,
                Username = "anon",
                PassHash = "",
                Role = "USER"
            });
        }

        foreach (UserData ud in users)
        {
            if (!database.Users.Any(u => u.Username == ud.Username))
            {
                string? password = null;
                if (userPasswords.TryGetValue(ud.Username,out password)){
                ud.PassHash = passwordHasher.HashPassword(ud,password);
                database.Add(ud);
                }
            }
            database.SaveChanges();
        }
        
        foreach (ManufacturerData md in manufacturers)
        {
            if (!database.Manufacturers.Any(m => m.Name == md.Name)){
                database.Add(md);
            }
            database.SaveChanges();
        }

        foreach (StockItemData sid in stockItems)
        {
            if (!database.StockItems.Any(si => si.Name == sid.Name))
            {
                string? manufacturer = null;
                if (stockManufacturers.TryGetValue(sid.ManufacturerSKU, out manufacturer))
                {
                    ManufacturerData? md = database.Manufacturers.FirstOrDefault(m => m.Name == manufacturer);
                    if (md != null)
                    {
                        sid.ManufacturerID = md.ManufacturerID;
                        sid.ImageLocation = "blankitem.png";
     
                        string dimCode = md != null ? md.DimCode.ToUpper() : ".NQI";
                        string abbreviation = md != null ? md.Abbreviation : "";;
                        int stockCount = 1 + database.StockItems.Where(si => si.ManufacturerID == md.ManufacturerID).Count();
                        bool valid = false;

                        if (dimCode.StartsWith("."))
                        {
                            dimCode = dimCode.Substring(1);
                        }
                        
                        StringBuilder sb = new StringBuilder();
                        while (!valid){
                            sb.Clear();
                            sb.Append(dimCode);
                            sb.Append(abbreviation);
                            sb.Append("-");

                            string number = stockCount.ToString().PadLeft(3,'0');
                            sb.Append(number);

                            if (database.StockItems.Any(si => si.NeoSKU == sb.ToString()))
                            {
                                stockCount++;
                            }
                            else
                            {
                                valid = true;
                            }

                        }

                        sid.NeoSKU = sb.ToString();
                    
                        database.Add(sid);
                    }
                }
            }
            database.SaveChanges();
        }


        foreach (PaymentDetailData pdd in paymentDetails)
        {
            if (!database.PaymentDetails.Any(pd => pd.CardNumber == pdd.CardNumber && pd.UserID == pdd.UserID)
            && database.Users.Any(u => u.UserID == pdd.UserID))
            {
                database.Add(pdd);
            }
            database.SaveChanges();
        }

        foreach (OrderData od in orders)
        {
            if (!database.Orders.Any(o => o.OrderID == od.OrderID))
            {
                database.Add(od);
            }
            database.SaveChanges();
        }
    }

    // Requires entry on UserPasswords
    private static UserData[] users = [
        new UserData{
            Name = "Marianne Rey",
            Username = "mrey",
            Address = "Internal",
            LocatorCode = "NEOQUASAR-001",
            SlingName = "MariNEO",
            StellarNet = ".nqi/neo.marianne",
            Role = UserRole.ADMIN.ToString()
        },
        new UserData{
            Name = "Lexi Astera",
            Username = "TachyonCascade",
            Address = "Research & Development, Nebula Complex",
            LocatorCode = "TSA-RD01121",
            SlingName = "TachyonCascade",
            StellarNet = "ark0/veris.tachcasc0",
            Role = UserRole.USER.ToString()
        },
        new UserData{
            Name = "Silva Astera",
            Username = "StarlitePrincess",
            Address = "Astera, Altair, Capital Center-001",
            LocatorCode = "VEN0-A112-01",
            SlingName = "StarlightPrincess",
            StellarNet = "ven0/kari.astera4",
            Role = UserRole.USER.ToString()
        },
        new UserData{
            Name = "Rianne Astera",
            Username = "TransientStar",
            Address = "Research & Development, Nebula Complex",
            LocatorCode = "TSA-RD01121",
            SlingName = "StarlightSoul",
            StellarNet = null,
            Role = UserRole.SHIPPER.ToString()
        }
    ];

    // [Username] = Password
    private static Dictionary<string,string> userPasswords = new ()
    {
        ["mrey"] = "WriteOnTime",
        ["StarlitePrincess"] = "ET4Ever",
        ["TachyonCascade"] = "CCIKissedAGirl",
        ["TransientStar"] = "ax151NovaengineeR"
    };

    private static ManufacturerData[] manufacturers = [
        new ManufacturerData{Name = "NeoQuasar", DimCode = ".nqi", Abbreviation = "NQ", ContactAddress = ".nqi/neo.marianne"},
        new ManufacturerData{Name = "Martin Industries", DimCode = "ven0", Abbreviation = "MI", ContactAddress = "ven0/martin.erikas"},
        new ManufacturerData{Name = "Stillmetal Laboratories",  DimCode = "oria", Abbreviation = "SL", ContactAddress = "FL-022-1334"},
        new ManufacturerData{Name = "Corry Industrial",  DimCode = "ven0", Abbreviation = "CI", ContactAddress = "ven0/kari.ci.sarrik"},
        new ManufacturerData{Name = "EsterTech", DimCode =  "ven0", Abbreviation = "ET", ContactAddress = "ven0/estert.mnevva"},
        new ManufacturerData{Name = "Silweave", DimCode =  "ven0", Abbreviation = "SW", ContactAddress = "ven0/kari.sw.mordire"},
        new ManufacturerData{Name = "StarTamers", DimCode =  "tri0", Abbreviation = "ST", ContactAddress = "StarTamers:LAmar281"},
        new ManufacturerData{Name = "Levin Starlight", DimCode =  "ark0", Abbreviation = "LS", ContactAddress = "ark0/ryl.cormigni"},
        new ManufacturerData{Name = "EnTech", DimCode =  ".egd", Abbreviation = "ET", ContactAddress = "1.734.2233-001"},
        new ManufacturerData{Name = "Karisou Industries", DimCode = "ven0", Abbreviation = "KI", ContactAddress = "ven0/kari.virela"}
    ];

    // Requires an entry on stockManufacturers
    private static StockItemData[] stockItems = [
        new StockItemData{Name = "Crystal Reflux Generator", ManufacturerSKU = "CRG-0001", StockCategory = "REACTOR", Description = "Flow generator producing 600 An/s in dimspace.", Price = 12500, Stock = 2, DontRestock = false},
        new StockItemData{Name = "CRG Heating Coil", ManufacturerSKU = "CRG-0102", StockCategory = "REACTOR_PART", Description = "Replacement heating coil for standard-sized crystal reflux generators. 3kW.", Price = 1000, Stock = 12, DontRestock = false},
        new StockItemData{Name = "Inertial Emulator Card", ManufacturerSKU = "QC-1002", StockCategory = "QUANTUM_COMPUTER", Description = "Accepts flickerspace information input and sends signals to realsim about how the ship is moving.", Price = 7500, Stock = 110, DontRestock = false},
        new StockItemData{Name = "IL-2 Quantum Core", ManufacturerSKU = "QC-0100", StockCategory = "QUANTUM_COMPUTER", Description = "Quantum computer capable of processing 3 thousand flickerspace operations per second.", Price = 22000, Stock = 3, DontRestock = false},
        new StockItemData{Name = "Archaemin Drive Simple", ManufacturerSKU = "archaed-2", StockCategory = "ARCANOHARMONIC_DRIVE", Description = "Use flow to travel flickerspace with the equivalent of 5k FSO/s.", Price = 16330, Stock = 1, DontRestock = false},
        new StockItemData{Name = "Arcanoharmonic Spectrometer", ManufacturerSKU = "SEN-H-1", StockCategory = "SENSOR", Description = "Reads information about surrounding arcanoharmonics.", Price = 6000, Stock = 6, DontRestock = false},
        new StockItemData{Name = "Dummy Item", ManufacturerSKU = "TEST-1", StockCategory = "MISC", Description = "An item for dummies.", Price = 1, Stock = 0, DontRestock = true},
        new StockItemData{Name = "Star Plushie", ManufacturerSKU = "PLSH-STAR", StockCategory = "COMFORT", Description = "A plush star.", Price = 20, Stock = 55, DontRestock = false},
        new StockItemData{Name = "EVA Suit - Astera Surplus", ManufacturerSKU = "EVA-ASTR", StockCategory = "MISC", Description = "An EVA suit designed for use by the Asteran navy. Disarmed.", Price = 25000, Stock = 2, DontRestock = false},
        new StockItemData{Name = "*ShipLife* Oxygen Circulation System", ManufacturerSKU = "CLS-011", StockCategory = "LIFE_SUPPORT", Description = "Designed for clean air in flickerspace realsim.", Price = 10500, Stock = 100, DontRestock = false},
        new StockItemData{Name = "Flow Evaporation Field", ManufacturerSKU = "FS-DEF-00112", StockCategory = "COMBAT", Description = "A field that absorbs and dissipates magical attacks.", Price = 9000, Stock = 4, DontRestock = false},
        new StockItemData{Name = "CrossTalk Sr5", ManufacturerSKU = "CT5", StockCategory = "COMFORT", Description = "A personal smartphone. Plan not sold here.", Price = 400, Stock = 800, DontRestock = false},
        new StockItemData{Name = "Gorgon 360/180 Railcannon", ManufacturerSKU = "3K", StockCategory = "COMBAT", Description = "Railcannon designed to fire standard ammunition.", Price = 20000, Stock = 5, DontRestock = false},
        new StockItemData{Name = "Vector Impulse Thruster", ManufacturerSKU = "VIT1", StockCategory = "DIMSPACE_THRUSTERS", Description = "Flow-based thrusters for traveling in dimensional space. Sold individually", Price = 6400, Stock = 12, DontRestock = false}
    ];

    // [ManufacturerID] = ManufacturerName
    private static Dictionary<string,string> stockManufacturers = new (){
        ["QC-1002"] = "Martin Industries",
        ["CRG-0102"] = "Martin Industries",
        ["CRG-0001"] = "Martin Industries",
        ["QC-0100"] = "Martin Industries",
        ["archaed-2"] = "Stillmetal Laboratories",
        ["SEN-H-1"] = "Corry Industrial",
        ["PLSH-STAR"] = "Silweave",
        ["EVA-ASTR"] = "Silweave",
        ["CLS-011"] = "StarTamers",
        ["FS-DEF-00112"] = "Levin Starlight",
        ["CT5"] = "Karisou Industries",
        ["3K"] = "EsterTech",
        ["VIT1"] = "EnTech" ,
        ["TEST-1"] = "NeoQuasar"  
    };

    private static PaymentDetailData[] paymentDetails = [new PaymentDetailData{
        CardName = "Marianne Rey",
        CardType = "STELLARNET_DEBIT",
        CardNumber = "nq1441-258",
        UserID = 1
    }];

    private static OrderData[] orders = [new OrderData{
        OrderID = "PR00000",
        UserID = -1,
        Shipper = "Martin Industries",
        Direction = "INCOMING",
        Date = "Dummy Order",
        Completed = false,
        ShippingOption = "Standard",
        ContactAddress = "mrey"
    }];


}