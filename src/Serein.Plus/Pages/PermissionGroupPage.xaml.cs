using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Force.DeepCloner;

using Serein.Core.Models.Permissions;
using Serein.Core.Services.Data;
using Serein.Core.Services.Permissions;
using Serein.Plus.Dialogs;
using Serein.Plus.Windows;

namespace Serein.Plus.Pages;

public partial class PermissionGroupPage : Page
{
    private readonly MainWindow _mainWindow;
    private readonly SettingProvider _settingProvider;
    private readonly PermissionGroupProvider _permissionGroupProvider;
    private readonly PermissionManager _permissionManager;
    private readonly GroupManager _groupManager;

    public PermissionGroupPage(
        MainWindow mainWindow,
        SettingProvider settingProvider,
        PermissionGroupProvider permissionGroupProvider,
        PermissionManager permissionManager,
        GroupManager groupManager
    )
    {
        _mainWindow = mainWindow;
        _settingProvider = settingProvider;
        _permissionGroupProvider = permissionGroupProvider;
        _permissionManager = permissionManager;
        _groupManager = groupManager;

        InitializeComponent();
        Update();
    }

    private void Update()
    {
        GroupListView.Items.Clear();
        foreach (var kv in _permissionGroupProvider.Value)
            GroupListView.Items.Add(kv);
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuItem)?.Tag as string)
        {
            case "Add":
                var dialog1 = new PermissionGroupEditor(_permissionManager, _groupManager, new())
                {
                    Owner = _mainWindow,
                };

                if (dialog1.ShowDialog() == true)
                {
                    _groupManager.Add(dialog1.Id, dialog1.Group);
                }

                break;

            case "Refresh":
                _permissionGroupProvider.Read();
                break;

            case "Remove":
                if (
                    GroupListView.SelectedItem is KeyValuePair<string, Group> kv1
                    && kv1.Key != "everyone"
                )
                {
                    DialogHelper
                        .ShowDeleteConfirmation($"确定要删除权限组（\"{kv1.Key}\"）吗？")
                        .ContinueWith(
                            (task) =>
                            {
                                if (task.Result)
                                {
                                    _groupManager.Remove(kv1.Key);
                                    Dispatcher.Invoke(Update);
                                }
                            }
                        );
                }
                break;

            case "Edit":
                if (GroupListView.SelectedItem is KeyValuePair<string, Group> kv2)
                {
                    var dialog2 = new PermissionGroupEditor(
                        _permissionManager,
                        _groupManager,
                        kv2.Value.DeepClone(),
                        kv2.Key
                    )
                    {
                        Owner = _mainWindow,
                    };

                    if (dialog2.ShowDialog() == true)
                    {
                        dialog2.Group.DeepCloneTo(kv2.Value);
                        _permissionGroupProvider.Save();
                    }
                }
                break;
        }
        Update();
    }

    private void GroupListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        EditMenuItem.IsEnabled = GroupListView.SelectedItems.Count == 1;
        RemoveMenuItem.IsEnabled = GroupListView.SelectedItems.Count > 0 && !GroupListView.SelectedItems.OfType<KeyValuePair<string, Group>>().Any((kv) => kv.Key == "everyone");
    }
}
