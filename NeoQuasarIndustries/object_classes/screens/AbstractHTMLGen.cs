using System.Text;

namespace NeoQuasarIndustries;

public abstract class AbstractHTMLGen
{
        public string Execute()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<html>");
        sb.AppendLine("<header>");
        sb.AppendLine(CreateHeader());
        sb.AppendLine("</header>");
        sb.AppendLine("<body>");
        sb.AppendLine(CreateBody());
        sb.AppendLine("</body>");
        sb.AppendLine("<footer>");
        sb.AppendLine(CreateFooter());
        sb.AppendLine("</footer>");
        sb.AppendLine("</html>");

        return sb.ToString();
        
    }
    public virtual string CreateHeader()
    {
        return string.Empty;
    }

    public virtual string CreateBody()
    {
        return string.Empty;
    }

    public virtual string CreateFooter()
    {
        return string.Empty;
    }

}