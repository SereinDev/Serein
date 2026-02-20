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
    private readonly PanelViewModel _viewModel;

    public PanelTabItem(PanelViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;

        Console.EnableAnsiColor();
        Console.EnableLogLevelHighlight(true);

        _viewModel.RequestClearConsole += () => Dispatcher.Invoke(Console.Clear);
        _viewModel.RequestInputTextUpdate += (text) =>
        {
            InputBox.Text = text;
            InputBox.SelectionStart = InputBox.Text.Length;
        };

        viewModel.Server.Logger.Output += Output;
    }

    private void Output(object? sender, ServerOutputEventArgs e)
    {
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
