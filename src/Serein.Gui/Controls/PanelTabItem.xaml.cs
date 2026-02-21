using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using iNKORE.UI.WPF.Modern.Controls;
using Serein.Core.Models.Server;
using Serein.Core.Utils;
using Serein.Gui.ViewModels;

namespace Serein.Gui.Controls;

public partial class PanelTabItem : TabItem
{
    private PanelViewModel? _viewModel;

    public PanelTabItem()
    {
        InitializeComponent();

        Console.EnableAnsiColor();
        Console.EnableLogLevelHighlight(true);

        DataContextChanged += PanelTabItem_DataContextChanged;
    }

    public PanelTabItem(PanelViewModel viewModel)
        : this()
    {
        BindViewModel(viewModel);
    }

    internal void BindViewModel(PanelViewModel viewModel)
    {
        DataContext = viewModel;
    }

    private void PanelTabItem_DataContextChanged(
        object sender,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (e.OldValue is PanelViewModel oldViewModel)
        {
            oldViewModel.RequestClearConsole -= ViewModel_RequestClearConsole;
            oldViewModel.RequestInputTextUpdate -= ViewModel_RequestInputTextUpdate;
            oldViewModel.Server.Logger.Output -= Output;
        }

        if (e.NewValue is PanelViewModel newViewModel)
        {
            _viewModel = newViewModel;

            newViewModel.RequestClearConsole += ViewModel_RequestClearConsole;
            newViewModel.RequestInputTextUpdate += ViewModel_RequestInputTextUpdate;
            newViewModel.Server.Logger.Output += Output;
            return;
        }

        _viewModel = null;
    }

    private void ViewModel_RequestClearConsole()
    {
        Dispatcher.Invoke(Console.Clear);
    }

    private void ViewModel_RequestInputTextUpdate(string text)
    {
        InputBox.Text = text;
        InputBox.SelectionStart = InputBox.Text.Length;
    }

    private void Output(object? sender, ServerOutputEventArgs e)
    {
        if (_viewModel == null)
        {
            return;
        }

        switch (e.Type)
        {
            case ServerOutputType.StandardOutput:
                Dispatcher.Invoke(
                    () =>
                        Console.AppendLine(
                            _viewModel.Server.Configuration.OutputStyle == OutputStyle.Plain
                                ? OutputFilter.Clean(e.Data)
                                : e.Data
                        )
                );
                break;

            case ServerOutputType.StandardInput:
                if (_viewModel.Server.Configuration.OutputCommandUserInput)
                {
                    Dispatcher.Invoke(() => Console.AppendLine($">{e.Data}"));
                }
                break;

            case ServerOutputType.InternalInfo:
                Dispatcher.Invoke(() => Console.AppendNoticeLine(e.Data));
                break;

            case ServerOutputType.InternalError:
                Dispatcher.Invoke(() => Console.AppendErrorLine(e.Data));
                break;

            default:
                throw new NotSupportedException();
        }
    }

    private void InputBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (_viewModel == null)
        {
            return;
        }

        switch (e.Key)
        {
            case Key.Enter:
                _viewModel.Input();
                e.Handled = true;
                break;

            case Key.Up:
                _viewModel.HandleHistoryUp();
                e.Handled = true;
                break;

            case Key.Down:
                _viewModel.HandleHistoryDown();
                e.Handled = true;
                break;
        }
    }

    private void TabItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        e.Handled = Mouse.GetPosition(this).Y > 32;
    }
}
