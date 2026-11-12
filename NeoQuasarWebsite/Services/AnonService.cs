using System.Text.Json;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Services;

public interface IAnonService
{
    public bool AddToCart(AnonUserDTO user, string id, int quantity);

    public bool RemoveFromCart(AnonUserDTO user, string id, int quantity, out bool lineZeroed);

    public List<CartLineData> GetCart(AnonUserDTO user);
    
    public void ResetCart(AnonUserDTO user);
}

public class AnonService : IAnonService
{
    private readonly NeoQuasarDBContext database;
    private readonly IHttpContextAccessor _httpContext;

    public AnonService(NeoQuasarDBContext dbContext, IHttpContextAccessor httpContext)
    {
        database = dbContext;
        _httpContext = httpContext;
    }

    public bool AddToCart(AnonUserDTO user, string id, int quantity)
    {
        if (user != null)
        {
            foreach (CartLineDTO line in user.lines)
            {
                if (line.NeoSKU == id)
                {
                    line.Quantity += quantity;
                    return true;
                }
            }
            user.lines.Add(new CartLineDTO
            {
               NeoSKU = id,
               Quantity = quantity
            });
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<CartLineData> GetCart(AnonUserDTO user)
    {
        List<CartLineData> list = new List<CartLineData>();
        foreach(CartLineDTO line in user.lines)
        {
            list.Add(new CartLineData
            {
                ItemSKU = line.NeoSKU,
                Quantity = line.Quantity
            });
        }
        return list;
    }

    public bool RemoveFromCart(AnonUserDTO user, string id, int quantity, out bool lineZeroed)
    {
        if (user != null)
        {
            bool removed = false;
            CartLineDTO? lineToDelete = null;
            foreach (CartLineDTO line in user.lines)
            {
                if (line.NeoSKU == id)
                {
                    line.Quantity -= quantity;
                    if (line.Quantity <= 0)
                    {
                        lineToDelete = line;
                    }
                    removed = true;
                }
            }
            if (lineToDelete != null)
            {
                user.lines.Remove(lineToDelete);
                lineZeroed = true;
            }
            else 
            {
                lineZeroed = false;
            }
            return removed;

        }
        lineZeroed = false;
        return false;
    }

    public void ResetCart(AnonUserDTO user)
    {
        user.lines = new List<CartLineDTO>();
    }

}