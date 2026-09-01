using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public class StockItemScreen : AbstractScreen
{
    private StockItem Item {get;set;}

    public StockItemScreen(User user, StockItem item) : base(user)
    {
        Item = item;
    }

    public void AddToCart(StockItem item, int qty)
    {
        this.User.AddToCart(item,qty);
    }
    
    public void RemoveFromCart(StockItem item, int qty)
    {
        this.User.RemoveFromCart(item,qty);
    }

    public void SetItemProperty(string input, int property)
    {
        int newInt;
        bool tryParse;
        if (User.UserRole == User.Role.ADMIN)
        {
            switch (property)
            {
                case 0:
                    Item.Name = input;
                    break;
                case 1:
                    Item.NeoSKU = input;
                    break;
                case 2:
                    Item.ManSKU = input;
                    break;
                case 3:
                    Item.Manufacturer = LookUpHelpers.ManufacturerByString(input);
                    break;
                case 4:
                    Item.Description = input;
                    break;
                case 5:
                    tryParse = int.TryParse(input, out newInt);
                    if (tryParse)
                    {
                        Item.Price = newInt;
                    }
                    break;
                case 6:
                    tryParse = int.TryParse(input, out newInt);
                    if (tryParse && newInt < Enum.GetValues(typeof(StockItem)).Length)
                    {
                        Item.Category = StockItem.CatFromInt(newInt);
                    }
                    break;
                case 7:
                    Item.ImageLocation = input;
                    break;
                default:
                    return;
            }
        }
    }

    


}