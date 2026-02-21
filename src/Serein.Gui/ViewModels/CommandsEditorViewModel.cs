using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Force.DeepCloner;
using iNKORE.UI.WPF.Modern.Controls;
using Serein.Core.Models.Commands;
using Serein.Gui.Commands;
using Serein.Gui.Dialogs;
using Serein.Gui.Utils;

namespace Serein.Gui.ViewModels;

public class CommandsEditorViewModel : ListViewPageViewModel
{
    public ObservableCollection<Command> Commands { get; }

    public CommandsEditorViewModel(ObservableCollection<Command> commands)
    {
        Commands = commands;

        AddCommand = new(Add);
        ImportCommand = new(Import);
        RemoveCommand = new(Remove);
        EditCommand = new(Edit);
    }

    public RelayCommand AddCommand { get; }

    public RelayCommand ImportCommand { get; }

    public RelayCommand<IList> RemoveCommand { get; }

    public RelayCommand<Command> EditCommand { get; }

    private async void Add()
    {
        try
        {
            var command = new Command();
            var dialog = new CommandEditorDialog(command);

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                Commands.Add(command);
            }
        }
        catch (Exception e)
        {
            await MessageBoxEx.ShowExceptionAsync(e, "添加命令时发生错误");
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

            var itemsToRemove = selectedItems.Cast<Command>().ToArray();
            foreach (var command in itemsToRemove)
            {
                Commands.Remove(command);
            }
        }
        catch (Exception e)
        {
            await MessageBoxEx.ShowExceptionAsync(e, "删除命令时发生错误");
        }
    }

    private static async void Edit(Command? command)
    {
        try
        {
            if (command == null)
            {
                return;
            }

            var copy = new Command(command);
            var dialog = new CommandEditorDialog(copy);

            if (await dialog.ShowAsync() != ContentDialogResult.Primary)
            {
                return;
            }

            command.Type = copy.Type;
            command.Arguments = copy.Arguments;
            command.Body = copy.Body.DeepClone();
        }
        catch (Exception e)
        {
            await MessageBoxEx.ShowExceptionAsync(e, "编辑命令时发生错误");
        }
    }

    private static async void Import()
    {
        try
        {
            var textBox = new TextBox();
            var contentDialog = new ContentDialog
            {
                Title = "旧版命令编辑器",
                PrimaryButtonText = "保存",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary,
                Content = textBox,
            };

            if (await contentDialog.ShowAsync() != ContentDialogResult.Primary)
            {
                return;
            }
        }
        catch (Exception e)
        {
            await MessageBoxEx.ShowExceptionAsync(e, "导入命令时发生错误");
        }
    }
}
