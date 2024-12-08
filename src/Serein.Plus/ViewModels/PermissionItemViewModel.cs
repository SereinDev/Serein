using Serein.Core.Models;

namespace Serein.Plus.ViewModels;

public class PermissionItemViewModel : NotifyPropertyChangedModelBase
{
    public string Node { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool? Value { get; set; }
}
