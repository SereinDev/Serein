using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using iNKORE.UI.WPF.Modern.Controls;

using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Utils.Extensions;
using Serein.Plus.Dialogs;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages.Settings;

public partial class ReactionSettingPage : Page
{
    private readonly SettingProvider _settingProvider;

    public ReactionSettingPage(SettingProvider settingProvider)
    {
        _settingProvider = settingProvider;

        InitializeComponent();
        DataContext = settingProvider;

        var list = new List<DisplayedItem>();
        foreach (var type in Enum.GetValues<ReactionType>())
            list.Add(new(type));
        ReactionTypeListView.ItemsSource = list;
        ReactionTypeListView.SelectedIndex = 0;
    }

    private void ReactionTypeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ReactionTypeListView.SelectedItem is not DisplayedItem item)
            return;

        CommandListView.Items.Clear();
        if (_settingProvider.Value.Reactions.TryGetValue(item.Type, out var commands))
            foreach (var command in commands)
                CommandListView.Items.Add(command);
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as MenuItem)?.Tag?.ToString();

        if (tag == "Add")
        {
            var dialog = new CommandEditorDialog();
            dialog.ShowAsync().ContinueWith((task) => Dispatcher.Invoke(() =>
            {
                if (task.Result != ContentDialogResult.Primary)
                    return;

                CommandListView.Items.Add(dialog.Command);
                Save();
            }));
        }
        else if (tag == "Remove")
        {
            DialogHelper.ShowDeleteConfirmation("ȷ��Ҫɾ����ѡ������").ContinueWith((task) =>
            {
                if (task.Result)
                    Dispatcher.Invoke(() =>
                    {
                        foreach (var command in CommandListView.SelectedItems.OfType<string>().ToArray())
                            CommandListView.Items.Remove(command);

                        Save();
                    });
            });
        }
    }

    private void Save()
    {
        if (ReactionTypeListView.SelectedItem is not DisplayedItem item)
            return;

        _settingProvider.Value.Reactions[item.Type] = CommandListView.Items.OfType<string>().ToArray();
        _settingProvider.SaveAsyncWithDebounce();
    }

    private void CommandListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        RemoveMenuItem.IsEnabled = CommandListView.SelectedIndex >= 0;
    }

    private class DisplayedItem
    {
        public ReactionType Type { get; }

        public string Name { get; }

        public DisplayedItem(ReactionType reactionType)
        {
            Type = reactionType;
            Name = Type switch
            {
                ReactionType.ServerStart => "����������",
                ReactionType.ServerExitedNormally => "�������رգ������˳�",
                ReactionType.ServerExitedUnexpectedly => "�������رգ��������˳�",
                ReactionType.GroupIncreased => "Ⱥ��������",
                ReactionType.GroupDecreased => "Ⱥ��������",
                ReactionType.GroupPoke => "Ⱥ��һ��",
                ReactionType.PermissionDeniedFromPrivateMsg => "Ȩ�޲��㣺˽��",
                ReactionType.PermissionDeniedFromGroupMsg => "Ȩ�޲��㣺Ⱥ��",
                ReactionType.SereinCrash => "Serein����",
                _ => throw new NotSupportedException(),
            };
        }

        public override string ToString() => Name;
    }
}