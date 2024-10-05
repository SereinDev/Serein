using System;

using iNKORE.UI.WPF.Modern.Controls;

using Serein.Core.Models.Commands;

namespace Serein.Plus.Dialogs;

public partial class MatchEditorDialog : ContentDialog
{
    private readonly Match _match;

    public MatchEditorDialog(Match match)
    {
        _match = match;

        InitializeComponent();
        DataContext = _match;
        Closing += OnClosing;
        match.PropertyChanged += UpdateTip;
    }

    private void UpdateTip(object? sender, EventArgs e)
    {
        InfoBar.IsOpen = !string.IsNullOrEmpty(_match.LastError);
    }

    private void OnClosing(ContentDialog sender, ContentDialogClosingEventArgs e)
    {
        _match.PropertyChanged -= UpdateTip;
    }
}
