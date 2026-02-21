using System;
using System.ComponentModel;
using Serein.Core.Models.Abstractions;
using Serein.Gui.Commands;
using Serein.Gui.Services;

namespace Serein.Gui.ViewModels;

public class MainWindowViewModel : NotifyPropertyChangedModelBase
{
    private readonly TitleUpdater _titleUpdater;
    private Action? _showWindowAction;
    private Action? _hideWindowAction;
    private Action? _closeWindowAction;
    private Action<bool>? _setTopmostAction;

    public MainWindowViewModel(TitleUpdater titleUpdater)
    {
        _titleUpdater = titleUpdater;
        _titleUpdater.Update();
        _titleUpdater.PropertyChanged += TitleUpdater_PropertyChanged;

        ShowWindowCommand = new(ShowWindow);
        ToggleHideCommand = new(ToggleHide);
        ToggleTopMostCommand = new(ToggleTopMost);
        ExitCommand = new(Exit);
    }

    public string CustomTitle => _titleUpdater.CustomTitle;

    public bool IsHideChecked { get; set; }

    public bool IsHideEnabled { get; set; } = true;

    public bool IsTopMostChecked { get; set; }

    public bool IsTopMostEnabled { get; set; } = true;

    public RelayCommand ShowWindowCommand { get; }

    public RelayCommand<bool?> ToggleHideCommand { get; }

    public RelayCommand<bool?> ToggleTopMostCommand { get; }

    public RelayCommand ExitCommand { get; }

    public void BindWindowActions(
        Action showWindowAction,
        Action hideWindowAction,
        Action closeWindowAction,
        Action<bool> setTopmostAction
    )
    {
        _showWindowAction = showWindowAction;
        _hideWindowAction = hideWindowAction;
        _closeWindowAction = closeWindowAction;
        _setTopmostAction = setTopmostAction;
    }

    public void SyncWindowVisibility(bool isVisible)
    {
        IsHideChecked = !isVisible;
        IsTopMostEnabled = isVisible;
    }

    public void OnMinimizeToTrayDueRunningServers()
    {
        IsHideEnabled = true;
        IsHideChecked = true;
        IsTopMostEnabled = false;
        IsTopMostChecked = false;
        _setTopmostAction?.Invoke(false);
    }

    private void ShowWindow()
    {
        _showWindowAction?.Invoke();
    }

    private void ToggleHide(bool? isChecked)
    {
        var hide = isChecked == true;
        IsHideChecked = hide;
        IsTopMostEnabled = !hide;

        if (hide)
        {
            _hideWindowAction?.Invoke();
        }
        else
        {
            _showWindowAction?.Invoke();
        }
    }

    private void ToggleTopMost(bool? isChecked)
    {
        var topMost = isChecked == true;
        IsTopMostChecked = topMost;
        IsHideEnabled = !topMost;
        _setTopmostAction?.Invoke(topMost);
    }

    private void Exit()
    {
        _closeWindowAction?.Invoke();
    }

    private void TitleUpdater_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        RaisePropertyChanged(nameof(CustomTitle));
    }
}
