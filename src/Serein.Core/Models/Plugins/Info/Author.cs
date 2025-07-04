namespace Serein.Core.Models.Plugins.Info;

public sealed record Author(string Name, string? Description = null)
{
    public override string ToString()
    {
        return string.IsNullOrEmpty(Description) ? Name : $"{Name}({Description})";
    }
}
