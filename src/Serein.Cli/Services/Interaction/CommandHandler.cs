namespace Serein.Cli.Services.Interaction;

public abstract class CommandHandler()
{
    public abstract void Invoke(string[] args);
}
