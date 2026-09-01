using System.Text;

namespace NeoQuasarIndustries;

public static class ClickEmulator
{
    public static int Click(List<string> options)
    {
        int opt = options.Count; 
        if (opt > 10)
        {
            opt = 10;
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Click Emulation Requested. Please make a selection:");
        for (int i = 0; i < opt; i++){
            sb.AppendLine($"[{i}] - " + options[i]);
        }
        int selection = -1;
        while(selection < 0 || selection >= opt)
        {
            string? text = Console.ReadLine();
            if (text != null){
                int.TryParse(text,out selection);
            }
        }
        return selection;
    }


}