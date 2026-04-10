namespace Serein.Core.Models;

public sealed record GitInfo
{
    public static GitInfo Current { get; } = new();

    private GitInfo() { }

    public string Branch { get; } = ThisAssembly.Git.Branch;

    public string CommitHash { get; } = ThisAssembly.Git.Commit;

    public string Root { get; } = ThisAssembly.Git.Root;

    public string Sha { get; } = ThisAssembly.Git.Sha;

    public string Url { get; } = ThisAssembly.Git.Url;
}
