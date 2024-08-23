using System;

using Serein.Pro.Views;

namespace Serein.Pro.ViewModels;

public class ShellViewModel
{
    public Type HomePage { get; } = typeof(HomePage);
    public Type ServerPage { get; } = typeof(ServerPage);
    public Type MatchPage { get; } = typeof(MatchPage);
}
