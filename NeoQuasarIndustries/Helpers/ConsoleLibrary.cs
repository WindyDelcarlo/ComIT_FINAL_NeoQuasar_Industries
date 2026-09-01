using System.Text;

namespace NeoQuasarIndustries;

public static class ConsoleLibrary {
    /// <summary>
    /// Inserts spaces between two strings based on wide the space they should be taking up is.
    /// </summary>
    /// <param name="in1">String 1</param>
    /// <param name="in2">String 2</param>
    /// <param name="width">Console Width</param>
    /// <returns></returns>
    public static string VariableSpacing(string in1, string in2, int width)
    {
        int spacing = width - in1.Length - in2.Length;

        StringBuilder sb = new StringBuilder();
        sb.Append(in1);
        for (int i = 0; i < spacing; i++)
        {
            sb.Append(" ");
        }
        sb.Append(in2);
        return sb.ToString();

    }

    /// <summary>
    /// Centers a string in whitespace.
    /// </summary>
    /// <param name="input">String</param>
    /// <param name="width">Console Width</param>
    /// <returns></returns>
    public static string CenterText(string input, int width)
    {
        StringBuilder sb = new StringBuilder();
        int spaces = width - input.Length;
        if (spaces % 2 != 0)
        {
            sb.Append(" ");
            spaces--;
        }
        spaces /= 2;
        for (int i = 0; i < spaces; i++)
        {
            sb.Append(" ");
        }
        sb.Append(input);
        for (int i = 0; i < spaces; i++)
        {
            sb.Append(" ");
        }
        return sb.ToString();
    }

    public static string Line(string input, int width)
    {
        StringBuilder sb = new StringBuilder();
        int inputSize = input.Length;
        for (int i = 0; i < width; i++)
        {
            sb.Append(input[i%inputSize]);
        }
        return sb.ToString();
    }
}