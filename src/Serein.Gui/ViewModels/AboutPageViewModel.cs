using System.Reflection;
using Serein.Core.Models.Abstractions;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Gui.Commands;

namespace Serein.Gui.ViewModels;

public class AboutPageViewModel : NotifyPropertyChangedModelBase
{
    public AboutPageViewModel()
    {
        OpenUrlCommand = new(url => url?.OpenInBrowser());
        OpenCardLinkCommand = new(tag =>
        {
            switch (tag)
            {
                case "Repo":
                    UrlConstants.Repository.OpenInBrowser();
                    break;

                case "Group":
                    UrlConstants.Group.OpenInBrowser();
                    break;

                case "Docs":
                    UrlConstants.Docs.OpenInBrowser();
                    break;
            }
        });
    }

    public string Version { get; } = ThisAssembly.Info.Version;
    public string AssemblyName { get; } = Assembly.GetExecutingAssembly().GetName().FullName;
    public string? InformationalVersion { get; } = ThisAssembly.Info.InformationalVersion;
    public string? Branch { get; } = ThisAssembly.Git.Branch;
    public string? Root { get; } = ThisAssembly.Git.Root;

    public RelayCommand<string> OpenUrlCommand { get; }
    public RelayCommand<string> OpenCardLinkCommand { get; }
}
