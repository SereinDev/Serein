using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Bindings;
using Serein.Core.Services.Bindings;
using Serein.Gui.Commands;

namespace Serein.Gui.ViewModels;

public class BindingPageViewModel : NotifyPropertyChangedModelBase
{
    private readonly Lazy<BindingManager> _bindingManager;

    public BindingPageViewModel(IServiceProvider serviceProvider)
    {
        _bindingManager = new(serviceProvider.GetRequiredService<BindingManager>());
        Records = [];
        RefreshCommand = new(Refresh);
    }

    public ObservableCollection<BindingRecord> Records { get; }

    public bool IsLoading { get; set; } = true;

    public bool IsListEnabled => !IsLoading;

    public RelayCommand RefreshCommand { get; }

    public async Task LoadAsync()
    {
        IsLoading = true;
        var records = await Task.Run(() => _bindingManager.Value.Records);
        Records.Clear();

        foreach (var record in records)
        {
            Records.Add(record);
        }

        IsLoading = false;
    }

    private void Refresh()
    {
        Records.Clear();
        foreach (var record in _bindingManager.Value.Records)
        {
            Records.Add(record);
        }
    }
}
