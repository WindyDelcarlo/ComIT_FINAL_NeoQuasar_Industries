using System.Text.Json;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Models;

public class StockEditModel
{
    public StockItemDTO Item {get;set;}
    
    public string ItemJson()
    {
        return JsonSerializer.Serialize(Item);
    }
}