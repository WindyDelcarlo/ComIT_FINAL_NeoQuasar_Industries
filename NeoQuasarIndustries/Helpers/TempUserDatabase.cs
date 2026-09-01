using System.Text;

namespace NeoQuasarIndustries;

public static class TempUserDatabase
{
    public static List<User> userList = new List<User>();
    public static Dictionary <User,string> userPasswordCheck = new Dictionary<User, string>();

    public static string EncryptPassword(string password)
    {
        List<int> charList = [.. password];
        List<int> output = new List<int>();
        for (int i = 0; i < charList.Count; i++)
        {
            charList[i] *= 2;
            charList[i] += 11 + i;
            output.Prepend(charList[i]);
        }
        StringBuilder sb = new StringBuilder();
        foreach (int i in output)
        {
            sb.Append(i);
        }
        return sb.ToString();
    }

    public static void NewUser(User newUser,string password)
    {
        userList.Add(newUser);
        userPasswordCheck.Add(newUser,password);
    }

    public static void Initialize()
    {
        NewUser(new DefaultUser(),"xxxxxxxx");
        NewUser(UserFactory.Create("Marianne Rey", "mrey","Internal","NEOQUASAR-001","MariNEO",".nqi/neo.marianne", User.Role.ADMIN),EncryptPassword("123abcdwhoops"));
        NewUser(UserFactory.Create("Silva Astera","StarlightPrincess","Astera, Altair, Capital Center-001","VEN0-A112-01","StarlightPrincess","ven0/kari:astera4",null),EncryptPassword("ETisGreat"));
        NewUser(UserFactory.Create("Lexi Astera","TachyonCascade","Research & Development, Nebula Complex","TSA-RD01121","TachyonCascade","ark0/veris.tachcasc0",null),EncryptPassword("0xF1ABC011"));
    }
}