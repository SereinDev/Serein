using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Serein.Core.Models.Commands;
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
        {
            _eventListBox.Items.Add(new DisplayedItem(type));
        }

        _commandListView.SetExploreTheme();
        _settingProvider = settingProvider;
    }

    private void SaveData()
    {
        if (_eventListBox.SelectedItem is not DisplayedItem displayedItem)
        {
            return;
        }

        _settingProvider.Value.Reactions[displayedItem.Type] =
        [
            .. _commandListView.Items.Cast<ListViewItem>().Select((item) => item.Text),
        ];
        _settingProvider.SaveAsyncWithDebounce();
    }

    private void EventListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (
            _eventListBox.SelectedItem is DisplayedItem displayedItem
            && _settingProvider.Value.Reactions.TryGetValue(displayedItem.Type, out var commands)
        )
        {
            _commandListView.BeginUpdate();
            _commandListView.Items.Clear();

            foreach (var command in commands)
            {
                _commandListView.Items.Add(command);
            }

            _commandListView.EndUpdate();
        }
        else
        {
            _commandListView.Items.Clear();
        }
    }

    private void CommandListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
        SaveData();
    }

    private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
        _deleteToolStripMenuItem.Enabled = _commandListView.SelectedItems.Count == 1;
    }

    private void AddToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (_commandListView.SelectedItems.Count == 1)
        {
            _commandListView.Items.Insert(_commandListView.SelectedItems[0].Index, string.Empty);
        }
        else
        {
            _commandListView.Items.Add(string.Empty);
        }
        SaveData();
    }

    private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (_commandListView.SelectedItems.Count == 1)
        {
            _commandListView.Items.Remove(_commandListView.SelectedItems[0]);
            SaveData();
        }
    }

    private sealed class DisplayedItem
    {
        public ReactionType Type { get; }

        public string Name { get; }

        public DisplayedItem(ReactionType reactionType)
        {
            Type = reactionType;
            Name = Type switch
            {
                ReactionType.ServerStart => "服务器启动",
                ReactionType.ServerExitedNormally => "服务器关闭：正常退出",
                ReactionType.ServerExitedUnexpectedly => "服务器关闭：不正常退出",
                ReactionType.GroupIncreased => "群人数增加",
                ReactionType.GroupDecreased => "群人数减少",
                ReactionType.GroupPoke => "群戳一戳",
                ReactionType.BindingSucceeded => "绑定成功",
                ReactionType.UnbindingSucceeded => "解绑成功",
                ReactionType.PermissionDeniedFromPrivateMsg => "权限不足：私聊",
                ReactionType.PermissionDeniedFromGroupMsg => "权限不足：群聊",
                ReactionType.SereinCrash => "Serein崩溃",
                _ => throw new NotSupportedException(),
            };
        }

        public override string ToString() => Name;
    }
}
