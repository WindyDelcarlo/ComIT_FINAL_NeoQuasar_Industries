namespace NeoQuasarIndustries;

public class UserSorter : IComparer<User>
{
    private SortCategory cat;

    public UserSorter(SortCategory sortBy)
    {
        cat = sortBy;
    }

    public int Compare(User? x, User? y)
    {
        if (x == null || y == null)
        {
            return 0;
        }
        // returned value is how x compares to y. aka i>0 means x>y.
        if (cat == SortCategory.USERNAME)
        {
            int length;
            int nameMatch;
            if (x.Username.Length < y.Username.Length)
            {
                length = x.Username.Length;
                nameMatch = -1;
            }
            else if (x.Username.Length > y.Username.Length)
            {
                length = y.Username.Length;
                nameMatch = 1;
            }
            else
            {
                length = x.Username.Length;
                nameMatch = 0;
            }
            for (int i = 0; i < length; i++)
            {
                if (x.Username[i] < y.Username[i])
                {
                    return -1;
                }
                else if (x.Username[i] > y.Username[i])
                {
                    return 1;
                }
            }
            return nameMatch;
        } 
        else if (cat == SortCategory.NAME)
        {
            int length;
            int nameMatch;
            if (x.Name.Length < y.Name.Length)
            {
                length = x.Name.Length;
                nameMatch = -1;
            }
            else if (x.Name.Length > y.Name.Length)
            {
                length = y.Name.Length;
                nameMatch = 1;
            }
            else
            {
                length = x.Name.Length;
                nameMatch = 0;
            }
            for (int i = 0; i < length; i++)
            {
                if (x.Name[i] < y.Name[i])
                {
                    return -1;
                }
                else if (x.Name[i] > y.Name[i])
                {
                    return 1;
                }
            }
            return nameMatch;
        } 
        else if (cat == SortCategory.CONTACT)
        {
            string xTest;
            if (x.StellarNet != "")
            {
                xTest = x.StellarNet;
            }
            else
            {
                xTest = x.SlingName;
            }
            string yTest;
            if (y.StellarNet != "")
            {
                yTest = y.StellarNet;
            }
            else
            {
                yTest = y.SlingName;
            }
            int length;
            int nameMatch;
            if (xTest.Length < yTest.Length)
            {
                length = xTest.Length;
                nameMatch = -1;
            }
            else if (xTest.Length > yTest.Length)
            {
                length = yTest.Length;
                nameMatch = 1;
            }
            else
            {
                length = xTest.Length;
                nameMatch = 0;
            }
            for (int i = 0; i < length; i++)
            {
                if (xTest[i] < yTest[i])
                {
                    return -1;
                }
                else if (xTest[i] > yTest[i])
                {
                    return 1;
                }
            }
            return nameMatch;
        }
        else
        {
            return x.ID - y.ID;
        }
    }

    
    public enum SortCategory
    {
        ID = 0,
        USERNAME = 1,
        NAME = 2,
        CONTACT = 3
    }
}