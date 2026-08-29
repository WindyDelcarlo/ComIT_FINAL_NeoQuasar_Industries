using System.Text;

namespace NeoQuasarIndustries.object_classes.database
{
    public class StockItem
    {
        public string Name {get;set;}
        public string NeoSKU {get;set;}
        public string ManSKU {get;set;}
        public Manufacturer Manufacturer {get;set;}
        public string Description {get;set;}
        public int Price {get; set;}
        public StockCategory Category {get;set;}
        public string ImageLocation {get;set;}

        public StockItem (string name, string neoSKU, string manSKU, string manufacturer, StockCategory category, string description, int price, string img)
        {
            Name = name;
            NeoSKU = neoSKU;
            ManSKU = manSKU;
            Manufacturer = LookUpHelpers.ManufacturerByString(manufacturer);
            Category = category;
            Description = description;
            Price = price;
            ImageLocation = img;
        }

        public StockItem (string name, string neoSKU, string manSKU, string manufacturer, StockCategory category, string description, int price) : this(name,neoSKU,manSKU,manufacturer,category,description,price,"")
        {
            
        }

        public static StockCategory CatFromInt(int i)
        {
            List<StockCategory> list = [.. Enum.GetValues(typeof(StockCategory)).Cast<StockCategory>()];
            return list[i];
        }

        public enum StockCategory
        {
            REACTOR,
            REACTOR_PART,
            QUANTUM_COMPUTER,
            ARCANOHARMONIC_DRIVE,
            FLOWTECH,
            SENSOR,
            COMPUTER,
            DIMSPACE_THRUSTERS,
            MECHANICAL_PART,
            FOOD_AND_DRINK,
            COMFORT,
            COMBAT,
            LIFE_SUPPORT,
            TOOL,
            MISC

        }

        public static List<StockItem> dummyList()
        {
            List<StockItem> list =
            [
                new StockItem("Crystal Reflux Generator", "VEN0MI-001", "CRG-0001","Martin Industries",StockCategory.REACTOR,"Flow generator producing 600 An/s in dimspace.",12500),
                new StockItem("CRG Heating Coil", "VEN0MI-002", "CRG-0102","Martin Industries",StockCategory.REACTOR_PART,"Replacement heating coil for standard-sized crystal reflux generators. 3kW.",1000),
                new StockItem("Inertial Emulator Card", "VEN0MI-006", "QC-1002","Martin Industries",StockCategory.QUANTUM_COMPUTER,"Accepts flickerspace information input and sends signals to realsim about how the ship is moving.",7500),
                new StockItem("IL-2 Quantum Core", "VEN0MI-005", "QC-0100","Martin Industries",StockCategory.QUANTUM_COMPUTER,"Quantum computer capable of processing 3 thousand flickerspace operations per second.",22000),
                new StockItem("Archaemin Drive Simple", "ORIASL-001", "archaed-2","Stillmetal Laboratories",StockCategory.ARCANOHARMONIC_DRIVE,"Use flow to travel flickerspace with the equivalent of 5k FSO/s.",16330),
                new StockItem("Arcanoharmonic Spectrometer", "VEN0CI-001", "SEN-H-1","Corry Industrial",StockCategory.SENSOR,"Reads information about surrounding arcanoharmonics.",6000),
                new StockItem("Dummy Item", "TESTTS-001", "TEST-1","EsterTech",StockCategory.MISC,"An item for dummies.",1),
                new StockItem("Star Plushie", "VEN0SW-001", "PLSH-STAR","Silweave",StockCategory.COMFORT,"A plush star.",20),
                new StockItem("EVA Suit - Astera Surplus", "VEN0SW-001", "EVA-ASTR","Silweave",StockCategory.MISC,"An EVA suit designed for use by the Asteran navy. Disarmed.",25000),
                new StockItem("*ShipLife* Oxygen Circulation System", "TRI0ST-020", "CLS-011","StarTamers",StockCategory.LIFE_SUPPORT,"Designed for clean air in flickerspace realsim.",10500),
                new StockItem("Flow Evaporation Field", "ARK0LS-001", "FS-DEF-00112","Levin Starlight",StockCategory.COMBAT,"A field that absorbs and dissipates magical attacks.",9000),
                new StockItem("CrossTalk Sr5", "VEN0KS-001", "CT5","Karisou Industries",StockCategory.COMFORT,"A personal smartphone. Plan not sold here.",400),
                new StockItem("Gorgon 360/180 Railcannon", "VEN0ET-001", "3K","EsterTech",StockCategory.COMBAT,"Railcannon designed to fire standard ammunition.",20000),
                new StockItem("Vector Impulse Thruster", "ENT0ET-001", "VIT1","EnTech",StockCategory.DIMSPACE_THRUSTERS,"Flow-based thrusters for traveling in dimensional space. Sold individually",6400),
        

            ];

            return list;

        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            StockItem? testItem = obj as StockItem;
            if (testItem == null)
            {
                return false;
            }
            else if (testItem.NeoSKU != NeoSKU)
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{Name.ToUpper()} - {Category.ToString().Replace("_"," ")}");
            sb.AppendLine($"NEO #{NeoSKU} :: {Manufacturer}");
            sb.AppendLine(Description);
            sb.Append($"{Price} SIC");
            return sb.ToString();
        }
    }
}
