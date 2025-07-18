﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using iNKORE.UI.WPF.Modern.Controls;
using Serein.Core.Models.Permissions;
using Serein.Core.Services.Permissions;
using Serein.Plus.Dialogs;
using Serein.Plus.ViewModels;

namespace Serein.Plus.Windows;

public partial class PermissionGroupEditor : Window
{
    private readonly PermissionManager _permissionManager;
    private readonly GroupManager _groupManager;

    public string Id { get; set; }
    public Group Group { get; }
    public ListViewPageViewModel ViewModel { get; }

    public PermissionGroupEditor(
        PermissionManager permissionManager,
        GroupManager groupManager,
        Group group,
        string? id = null
    )
    {
        _permissionManager = permissionManager;
        _groupManager = groupManager;
        Group = group;
        ViewModel = new();
        Id = id ?? string.Empty;
        DataContext = this;

        InitializeComponent();

        IdTextBox.IsEnabled = string.IsNullOrEmpty(id);

        foreach (var userId in Group.Users)
        {
            MemberListView.Items.Add(userId);
        }

        foreach ((var key, var value) in group.Nodes)
        {
            PermissionListView.Items.Add(
                new PermissionItemViewModel
                {
                    Value = value,
                    Node = key,
                    Description = _permissionManager.Nodes.TryGetValue(key, out var description)
                        ? description
                        : string.Empty,
                }
            );
        }
    }

    private void MemberMenuItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuItem)?.Tag as string)
        {
            case "Add":
                var dialog = new IdEditorDialog(0);
                dialog
                    .ShowAsync()
                    .ContinueWith(
                        (task) =>
                        {
                            if (task.Result == ContentDialogResult.Primary)
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    if (!MemberListView.Items.Contains(dialog.Id))
                                    {
                                        MemberListView.Items.Add(dialog.Id);
                                    }
                                    else
                                    {
                                        DialogHelper.ShowSimpleDialog(
                                            "添加失败",
                                            "已经添加过此用户Id"
                                        );
                                    }
                                });
                            }
                        }
                    );
                break;

            case "Remove":
                if (MemberListView.SelectedIndex >= 0)
                {
                    DialogHelper
                        .ShowDeleteConfirmation(
                            $"你确定要删除\"{MemberListView.SelectedItem}\"吗？"
                        )
                        .ContinueWith(
                            (task) =>
                            {
                                if (task.Result)
                                {
                                    Dispatcher.Invoke(
                                        () =>
                                            MemberListView.Items.RemoveAt(
                                                MemberListView.SelectedIndex
                                            )
                                    );
                                }
                            }
                        );
                }
                break;
        }
    }

    private void PermissionMenuItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuItem)?.Tag as string)
        {
            case "Add":
                var dialog1 = new PermissionEditorDialog(_permissionManager);
                dialog1
                    .ShowAsync()
                    .ContinueWith(
                        (task) =>
                        {
                            if (task.Result == ContentDialogResult.Primary)
                            {
                                Dispatcher.Invoke(
                                    () =>
                                        PermissionListView.Items.Add(
                                            new PermissionItemViewModel
                                            {
                                                Node = dialog1.Node,
                                                Value = dialog1.Value,
                                                Description = _permissionManager.Nodes.TryGetValue(
                                                    dialog1.Node,
                                                    out var description
                                                )
                                                    ? description
                                                    : string.Empty,
                                            }
                                        )
                                );
                            }
                        }
                    );
                break;

            case "Edit":
                if (PermissionListView.SelectedItem is not PermissionItemViewModel viewModel)
                {
                    break;
                }

                var dialog2 = new PermissionEditorDialog(
                    _permissionManager,
                    viewModel.Node,
                    viewModel.Value
                );
                dialog2
                    .ShowAsync()
                    .ContinueWith(
                        (task) =>
                        {
                            if (task.Result == ContentDialogResult.Primary)
                            {
                                viewModel.Value = dialog2.Value;
                                viewModel.Node = dialog2.Node;
                                viewModel.Description = _permissionManager.Nodes.TryGetValue(
                                    dialog2.Node,
                                    out var description
                                )
                                    ? description
                                    : string.Empty;
                            }
                        }
                    );
                break;

            case "Remove":
                if (PermissionListView.SelectedIndex >= 0)
                {
                    DialogHelper
                        .ShowDeleteConfirmation(
                            $"你确定要删除\"{(PermissionListView.SelectedItem as PermissionItemViewModel)?.Node}\"吗？"
                        )
                        .ContinueWith(
                            (task) =>
                            {
                                if (task.Result)
                                {
                                    Dispatcher.Invoke(
                                        () =>
                                            PermissionListView.Items.RemoveAt(
                                                PermissionListView.SelectedIndex
                                            )
                                    );
                                }
                            }
                        );
                }
                break;
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            GroupManager.ValidateGroupId(Id);

            if (IdTextBox.IsEnabled && _groupManager.Ids.Contains(Id))
            {
                throw new InvalidOperationException("此Id已被占用");
            }

            Group.Users = [.. MemberListView.Items.OfType<string>().Distinct()];

            var dict = new Dictionary<string, bool?>();
            foreach (var item in PermissionListView.Items.OfType<PermissionItemViewModel>())
            {
                if (!string.IsNullOrEmpty(item.Node))
                {
                    dict.TryAdd(item.Node, item.Value);
                }
            }
            Group.Nodes = dict;
            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            DialogHelper.ShowSimpleDialog("保存失败", ex.Message);
        }
    }

    private void PermissionListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.IsSelected = PermissionListView.SelectedIndex >= 0;
    }
}
