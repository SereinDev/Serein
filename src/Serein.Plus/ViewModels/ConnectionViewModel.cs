using Serein.Core.Models;

namespace Serein.Plus.ViewModels;

public class ConnectionViewModel : NotifyPropertyChangedModelBase
{
    public string Status { get; internal set; } = "未连接";
}