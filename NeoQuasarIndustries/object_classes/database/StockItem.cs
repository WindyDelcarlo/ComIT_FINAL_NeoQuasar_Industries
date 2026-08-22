namespace NeoQuasarIndustries.object_classes.database
{
    public class StockItem
    {
        private string Name {get;set;}
        private string NeoSKU {get;set;}
        private string ManSKU {get;set;}
        public Manufacturer Manufacturer {get;set;}
        private string Description {get;set;}
        public int Price {get; private set;}

        public StockItem (string name, string neoSKU, string manSKU, string manufacturer, string description, int price)
        {
            Name = name;
            NeoSKU = neoSKU;
            ManSKU = manSKU;
            Manufacturer = LookUpHelpers.ManufacturerByString(manufacturer);
            Description = description;
            Price = price;
        }

    }
}
