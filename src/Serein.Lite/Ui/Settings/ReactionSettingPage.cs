using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Lite.Utils.Native;

namespace Serein.Lite.Ui.Settings;

public partial class ReactionSettingPage : UserControl
{
    private readonly SettingProvider _settingProvider;

    public ReactionSettingPage(SettingProvider settingProvider)
    {
        InitializeComponent();

        foreach (var type in Enum.GetValues<ReactionType>())
            EventListBox.Items.Add(new DisplayedItem(type));

        CommandListView.SetExploreTheme();
        _settingProvider = settingProvider;
    }

    private void SaveData()
    {
        if (EventListBox.SelectedItem is not DisplayedItem displayedItem)
            return;

        _settingProvider.Value.Reactions[displayedItem.Type] = CommandListView
            .Items.Cast<ListViewItem>()
            .Select((item) => item.Text)
            .ToArray();
        _settingProvider.SaveAsyncWithDebounce();
    }

    private void EventListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (
            EventListBox.SelectedItem is DisplayedItem displayedItem
            && _settingProvider.Value.Reactions.TryGetValue(displayedItem.Type, out var commands)
        )
        {
            CommandListView.BeginUpdate();
            CommandListView.Items.Clear();

            foreach (var command in commands)
                CommandListView.Items.Add(command);

            CommandListView.EndUpdate();
        }
        else
            CommandListView.Items.Clear();
    }

    private void CommandListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
        SaveData();
    }

    private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
        DeleteToolStripMenuItem.Enabled = CommandListView.SelectedItems.Count == 1;
    }

    private void AddToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (CommandListView.SelectedItems.Count == 1)
        {
            CommandListView.Items.Insert(CommandListView.SelectedItems[0].Index, string.Empty);
        }
        else
            CommandListView.Items.Add(string.Empty);

        SaveData();
    }

    private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (CommandListView.SelectedItems.Count == 1)
        {
            CommandListView.Items.Remove(CommandListView.SelectedItems[0]);
            SaveData();
        }
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
                ReactionType.BinderDisable => "绑定功能被禁用",
                ReactionType.ServerStart => "服务器启动",
                ReactionType.ServerExitedNormally => "服务器关闭：正常退出",
                ReactionType.ServerExitedUnexpectedly => "服务器关闭：不正常退出",
                ReactionType.GroupIncreased => "群人数增加",
                ReactionType.GroupDecreased => "群人数减少",
                ReactionType.GroupPoke => "群戳一戳",
                ReactionType.PermissionDeniedFromPrivateMsg => "权限不足：私聊",
                ReactionType.PermissionDeniedFromGroupMsg => "权限不足：群聊",
                ReactionType.SereinCrash => "Serein崩溃",
                _ => throw new NotSupportedException(),
            };
        }

        public override string ToString() => Name;
    }
}
