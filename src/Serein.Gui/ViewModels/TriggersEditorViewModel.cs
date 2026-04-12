using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using iNKORE.UI.WPF.Modern.Controls;
using Serein.Core.Models.Automations.Triggers;
using Serein.Gui.Commands;
using Serein.Gui.Dialogs;
using Serein.Gui.Utils;

namespace Serein.Gui.ViewModels;

public class TriggersEditorViewModel : ListViewPageViewModel
{
    public ObservableCollection<TriggerBase> Triggers { get; }

    public TriggersEditorViewModel(ObservableCollection<TriggerBase> triggers)
    {
        Triggers = triggers;

        AddEventTriggerCommand = new(AddEventTrigger);
        AddMatchTriggerCommand = new(AddMatchTrigger);
        AddScheduleTriggerCommand = new(AddScheduleTrigger);
        AddPluginTriggerCommand = new(AddPluginTrigger);
        EditCommand = new(Edit);
        RemoveCommand = new(Remove);
    }

    public RelayCommand AddEventTriggerCommand { get; }

    public RelayCommand AddMatchTriggerCommand { get; }

    public RelayCommand AddScheduleTriggerCommand { get; }

    public RelayCommand AddPluginTriggerCommand { get; }

    public RelayCommand<TriggerBase> EditCommand { get; }

    public RelayCommand<IList> RemoveCommand { get; }

    private void AddEventTrigger()
    {
        AddByType(TriggerType.Event);
    }

    private void AddMatchTrigger()
    {
        AddByType(TriggerType.Match);
    }

    private void AddScheduleTrigger()
    {
        AddByType(TriggerType.Schedule);
    }

    private void AddPluginTrigger()
    {
        AddByType(TriggerType.Plugin);
    }

    private async void AddByType(TriggerType type)
    {
        try
        {
            var trigger = CreateTrigger(type);
            if (await ShowDialogByType(trigger, isEdit: false) == ContentDialogResult.Primary)
            {
                Triggers.Add(trigger);
            }
        }
        catch (Exception e)
        {
            await MessageBoxEx.ShowExceptionAsync(e, "添加触发器时发生错误");
        }
    }

    private async void Edit(TriggerBase? trigger)
    {
        try
        {
            if (trigger == null)
            {
                return;
            }

            var copy = CloneTrigger(trigger);
            if (await ShowDialogByType(copy, isEdit: true) != ContentDialogResult.Primary)
            {
                return;
            }

            ApplyTriggerChanges(trigger, copy);
        }
        catch (Exception e)
        {
            await MessageBoxEx.ShowExceptionAsync(e, "编辑触发器时发生错误");
        }
    }

    private async void Remove(IList? selectedItems)
    {
        try
        {
            if (
                selectedItems == null
                || selectedItems.Count == 0
                || !await DialogFactory.ShowDeleteConfirmation("确定要删除所选项吗？")
            )
            {
                return;
            }

            var itemsToRemove = selectedItems.Cast<TriggerBase>().ToArray();
            foreach (var trigger in itemsToRemove)
            {
                Triggers.Remove(trigger);
            }
        }
        catch (Exception e)
        {
            await MessageBoxEx.ShowExceptionAsync(e, "删除触发器时发生错误");
        }
    }

    private static TriggerBase CreateTrigger(TriggerType type)
    {
        return type switch
        {
            TriggerType.Event => new EventTrigger(),
            TriggerType.Match => new MatchTrigger(),
            TriggerType.Schedule => new ScheduleTrigger(),
            TriggerType.Plugin => new PluginTrigger(),
            _ => throw new ArgumentOutOfRangeException(nameof(type)),
        };
    }

    private static TriggerBase CloneTrigger(TriggerBase source)
    {
        return source switch
        {
            EventTrigger trigger => new EventTrigger
            {
                IsEnabled = trigger.IsEnabled,
                Event = trigger.Event,
            },
            MatchTrigger trigger => new MatchTrigger
            {
                IsEnabled = trigger.IsEnabled,
                FieldType = trigger.FieldType,
                IsRegex = trigger.IsRegex,
                Pattern = trigger.Pattern,
                RequireAdminPermission = trigger.RequireAdminPermission,
            },
            ScheduleTrigger trigger => new ScheduleTrigger
            {
                IsEnabled = trigger.IsEnabled,
                CronExpression = trigger.CronExpression,
            },
            PluginTrigger trigger => new PluginTrigger
            {
                IsEnabled = trigger.IsEnabled,
                Key = trigger.Key,
            },
            _ => throw new ArgumentOutOfRangeException(nameof(source)),
        };
    }

    private static void ApplyTriggerChanges(TriggerBase target, TriggerBase source)
    {
        switch (target)
        {
            case EventTrigger eventTrigger when source is EventTrigger copy:
                eventTrigger.IsEnabled = copy.IsEnabled;
                eventTrigger.Event = copy.Event;
                break;

            case MatchTrigger matchTrigger when source is MatchTrigger copy:
                matchTrigger.IsEnabled = copy.IsEnabled;
                matchTrigger.FieldType = copy.FieldType;
                matchTrigger.IsRegex = copy.IsRegex;
                matchTrigger.Pattern = copy.Pattern;
                matchTrigger.RequireAdminPermission = copy.RequireAdminPermission;
                break;

            case ScheduleTrigger scheduleTrigger when source is ScheduleTrigger copy:
                scheduleTrigger.IsEnabled = copy.IsEnabled;
                scheduleTrigger.CronExpression = copy.CronExpression;
                break;

            case PluginTrigger pluginTrigger when source is PluginTrigger copy:
                pluginTrigger.IsEnabled = copy.IsEnabled;
                pluginTrigger.Key = copy.Key;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(target));
        }
    }

    private static Task<ContentDialogResult> ShowDialogByType(TriggerBase trigger, bool isEdit)
    {
        return trigger switch
        {
            EventTrigger e => new EventTriggerDialog(e, isEdit).ShowAsync(),
            MatchTrigger m => new MatchTriggerDialog(m, isEdit).ShowAsync(),
            ScheduleTrigger s => new ScheduleTriggerDialog(s, isEdit).ShowAsync(),
            PluginTrigger p => new PluginTriggerDialog(p, isEdit).ShowAsync(),
            _ => throw new ArgumentOutOfRangeException(nameof(trigger)),
        };
    }
}
