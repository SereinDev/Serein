using System.Collections.Generic;
using System.Collections.ObjectModel;
using Force.DeepCloner;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Permissions;
using Serein.Core.Services.Data;
using Serein.Core.Services.Permissions;
using Serein.Gui.Commands;
using Serein.Gui.Utils;
using Serein.Gui.Windows;

namespace Serein.Gui.ViewModels;

public class PermissionGroupPageViewModel : NotifyPropertyChangedModelBase
{
    private readonly MainWindow _mainWindow;
    private readonly PermissionGroupProvider _permissionGroupProvider;
    private readonly PermissionManager _permissionManager;
    private readonly GroupManager _groupManager;

    private KeyValuePair<string, Group>? _selectedGroup;

    public PermissionGroupPageViewModel(
        MainWindow mainWindow,
        PermissionGroupProvider permissionGroupProvider,
        PermissionManager permissionManager,
        GroupManager groupManager
    )
    {
        _mainWindow = mainWindow;
        _permissionGroupProvider = permissionGroupProvider;
        _permissionManager = permissionManager;
        _groupManager = groupManager;

        Groups = [];

        AddCommand = new(Add);
        EditCommand = new(Edit);
        RemoveCommand = new(Remove);
        RefreshCommand = new(Refresh);

        Refresh();
    }

    public ObservableCollection<KeyValuePair<string, Group>> Groups { get; }

    public bool CanEdit { get; set; }

    public bool CanRemove { get; set; }

    public RelayCommand AddCommand { get; }

    public RelayCommand EditCommand { get; }

    public RelayCommand RemoveCommand { get; }

    public RelayCommand RefreshCommand { get; }

    public void UpdateSelection(KeyValuePair<string, Group>? selectedGroup)
    {
        _selectedGroup = selectedGroup;
        CanEdit = selectedGroup is not null;
        CanRemove = selectedGroup is { Key: not "everyone" };
    }

    private void Refresh()
    {
        _permissionGroupProvider.Read();

        Groups.Clear();
        foreach (var kv in _permissionGroupProvider.Value)
        {
            Groups.Add(kv);
        }
    }

    private void Add()
    {
        var dialog = new PermissionGroupEditor(_permissionManager, _groupManager, new())
        {
            Owner = _mainWindow,
        };

        if (dialog.ShowDialog() == true)
        {
            _groupManager.Add(dialog.Id, dialog.Group);
            Refresh();
        }
    }

    private void Edit()
    {
        if (_selectedGroup is not { } kv)
        {
            return;
        }

        var dialog = new PermissionGroupEditor(
            _permissionManager,
            _groupManager,
            kv.Value.DeepClone(),
            kv.Key
        )
        {
            Owner = _mainWindow,
        };

        if (dialog.ShowDialog() == true)
        {
            dialog.Group.DeepCloneTo(kv.Value);
            _permissionGroupProvider.Save();
            Refresh();
        }
    }

    private async void Remove()
    {
        if (_selectedGroup is not { } kv || kv.Key == "everyone")
        {
            return;
        }

        if (!await DialogFactory.ShowDeleteConfirmation($"确定要删除权限组（\"{kv.Key}\"）吗？"))
        {
            return;
        }

        _groupManager.Remove(kv.Key);
        Refresh();
    }
}
