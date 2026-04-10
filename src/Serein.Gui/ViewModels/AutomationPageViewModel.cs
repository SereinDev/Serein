using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Controls;
using iNKORE.UI.WPF.Modern.Controls;
using Serein.Core.Models.Automations;
using Serein.Core.Services.Data;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Gui.Commands;
using Serein.Gui.Utils;
using Serein.Gui.Windows;

namespace Serein.Gui.ViewModels;

public class AutomationPageViewModel : ListViewPageViewModel
{
    private readonly AutomationTaskProvider _provider;
    private readonly MainWindow _mainWindow;
    private AutomationTask[] _selectedItems;

    public AutomationPageViewModel(AutomationTaskProvider provider, MainWindow mainWindow)
    {
        _provider = provider;
        _mainWindow = mainWindow;
        _selectedItems = [];

        Tasks = _provider.Value;
        Tasks.CollectionChanged += (_, _) => UpdateText();

        AddCommand = new(Add);
        RemoveCommand = new(Remove);
        EditDescriptionCommand = new(EditDescription);
        EditTriggersCommand = new(EditTriggers);
        EditCommandsCommand = new(EditCommands);
        RefreshCommand = new(Refresh);
        OpenVariablesDocCommand = new(OpenVariablesDoc);

        UpdateText();
    }

    public ObservableCollection<AutomationTask> Tasks { get; }
    public string Details { get; set; }
    public RelayCommand AddCommand { get; }
    public RelayCommand<IList> RemoveCommand { get; }
    public RelayCommand<AutomationTask> EditDescriptionCommand { get; }
    public RelayCommand<AutomationTask> EditTriggersCommand { get; }
    public RelayCommand<AutomationTask> EditCommandsCommand { get; }
    public RelayCommand RefreshCommand { get; }
    public RelayCommand OpenVariablesDocCommand { get; }

    public void UpdateSelection(AutomationTask[] selectedItems)
    {
        base.UpdateSelection(selectedItems);
        _selectedItems = selectedItems;
    }

    [MemberNotNull(nameof(Details))]
    private void UpdateText()
    {
        var count = Tasks.Count;
        var selectedCount = _selectedItems.Length;

        switch (selectedCount)
        {
            case > 1:
                Details = $"共{count}项，已选择{selectedCount}项";
                break;

            case 1:
                var index = _selectedItems[0] is { } item ? Tasks.IndexOf(item) : -1;
                Details = index >= 0 ? $"共{count}项，已选择第{index + 1}项" : $"共{count}项";
                break;

            default:
                Details = $"共{count}项";
                break;
        }
    }

    private void Add()
    {
        Tasks.Add(new());

        _provider.SaveAsyncWithDebounce();
    }

    private async void Remove(IList? items)
    {
        try
        {
            if (
                items == null
                || items.Count == 0
                || !await DialogFactory.ShowDeleteConfirmation("确定要删除所选自动化任务吗？")
            )
            {
                return;
            }

            var list = items.Cast<AutomationTask>().ToList();
            foreach (var item in list)
            {
                Tasks.Remove(item);
            }

            _provider.SaveAsyncWithDebounce();
        }
        catch (Exception e)
        {
            await MessageBoxEx.ShowExceptionAsync(e, "删除失败");
        }
    }

    private async void EditDescription(AutomationTask? task)
    {
        try
        {
            if (task == null)
            {
                return;
            }

            var textBox = new TextBox { Text = task.Description };
            var contentDialog = new ContentDialog
            {
                Title = "编辑描述",
                PrimaryButtonText = "保存",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary,
                Content = textBox,
            };

            if (await contentDialog.ShowAsync() != ContentDialogResult.Primary)
            {
                return;
            }

            task.Description = textBox.Text;
            _provider.SaveAsyncWithDebounce();
        }
        catch (Exception e)
        {
            await MessageBoxEx.ShowExceptionAsync(e, "编辑失败");
        }
    }

    private void EditTriggers(AutomationTask? task)
    {
        if (task == null)
        {
            return;
        }

        new TriggersEditor(task.Triggers) { Owner = _mainWindow }.ShowDialog();
        _provider.SaveAsyncWithDebounce();
    }

    private void EditCommands(AutomationTask? task)
    {
        if (task == null)
        {
            return;
        }

        new CommandsEditor(task.Commands) { Owner = _mainWindow }.ShowDialog();
        _provider.SaveAsyncWithDebounce();
    }

    private void Refresh()
    {
        try
        {
            _provider.Read();
        }
        catch (Exception e)
        {
            MessageBoxEx.ShowException(e, "刷新自动化任务失败");
        }
    }

    private static void OpenVariablesDoc()
    {
        UrlConstants.DocsVariables.OpenInBrowser();
    }
}
