namespace NeoQuasarIndustries;

public interface IConsoleScreen
{
    public abstract void ConsoleClick(out bool running);

    public abstract void ConsoleRun();
    public abstract void ConsoleDisplay();
}