using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using iNKORE.UI.WPF.Modern.Controls;
using iNKORE.UI.WPF.Modern.Controls.Helpers;
using Serein.Core.Models.Network.Web.WebAuthentication;
using Serein.Core.Services.Data;
using Serein.Gui.Commands;
using Serein.Gui.Utils;

namespace Serein.Gui.ViewModels;

public sealed class WebAuthenticationEditorViewModel : ListViewPageViewModel
{
    private readonly WebAuthenticationProvider _provider;
    private AuthenticationBase? _selectedItem;

    internal WebAuthenticationEditorViewModel(WebAuthenticationProvider webAuthenticationProvider)
    {
        _provider = webAuthenticationProvider;
        Items = _provider.Value;

        AddCommand = new(Add);
        EditCommand = new(Edit);
        RemoveCommand = new(Remove);
        RefreshCommand = new(Refresh);
    }

    public RelayCommand<string> AddCommand { get; }
    public RelayCommand<AuthenticationBase> EditCommand { get; }
    public RelayCommand<AuthenticationBase> RemoveCommand { get; }
    public RelayCommand RefreshCommand { get; }

    public ObservableCollection<AuthenticationBase> Items { get; }

    public void UpdateSelection(AuthenticationBase? item)
    {
        _selectedItem = item;
        UpdateSelection(item is null ? [] : new[] { item });
    }

    private async void Add(string? type)
    {
        try
        {
            switch (type)
            {
                case "token":
                    await AddTokenAsync();
                    break;

                case "user":
                    await AddUserAsync();
                    break;
            }
        }
        catch (Exception e)
        {
            await MessageBoxEx.ShowExceptionAsync(e, "添加认证失败");
        }
    }

    private async void Edit(AuthenticationBase? item)
    {
        item ??= _selectedItem;
        if (item is null)
        {
            return;
        }

        try
        {
            switch (item)
            {
                case TokenAuthentication token:
                    await EditTokenAsync(token);
                    break;

                case UserAuthentication user:
                    await EditUserAsync(user);
                    break;
            }
        }
        catch (Exception e)
        {
            await MessageBoxEx.ShowExceptionAsync(e, "编辑认证失败");
        }
    }

    private void Refresh()
    {
        try
        {
            _provider.Read();
        }
        catch (Exception e)
        {
            MessageBoxEx.ShowException(e, "刷新认证失败");
        }
    }

    private async void Remove(AuthenticationBase? item)
    {
        item ??= _selectedItem;
        if (item is null)
        {
            return;
        }

        try
        {
            if (!await DialogFactory.ShowDeleteConfirmation("确定要删除所选访问凭证吗？"))
            {
                return;
            }

            _provider.Value.Remove(item);
            _provider.SaveAsyncWithDebounce();
        }
        catch (Exception e)
        {
            await MessageBoxEx.ShowExceptionAsync(e, "删除认证失败");
        }
    }

    private async Task AddTokenAsync()
    {
        var descriptionTextBox = new TextBox();
        var tokenTextBox = new TextBox();

        var content = BuildTokenEditorContent(descriptionTextBox, tokenTextBox);

        var dialog = new ContentDialog
        {
            Title = "添加令牌凭证",
            PrimaryButtonText = "添加",
            CloseButtonText = "取消",
            DefaultButton = ContentDialogButton.Primary,
            Content = content,
        };

        if (await dialog.ShowAsync() != ContentDialogResult.Primary)
        {
            return;
        }

        Items.Add(
            new TokenAuthentication
            {
                Description = descriptionTextBox.Text,
                Token = tokenTextBox.Text,
            }
        );

        _provider.SaveAsyncWithDebounce();
    }

    private async Task AddUserAsync()
    {
        var descriptionTextBox = new TextBox();
        var usernameTextBox = new TextBox();
        var passwordBox = new PasswordBox();

        var content = BuildUserEditorContent(descriptionTextBox, usernameTextBox, passwordBox);

        var dialog = new ContentDialog
        {
            Title = "添加用户凭证",
            PrimaryButtonText = "添加",
            CloseButtonText = "取消",
            DefaultButton = ContentDialogButton.Primary,
            Content = content,
        };

        if (await dialog.ShowAsync() != ContentDialogResult.Primary)
        {
            return;
        }

        Items.Add(
            new UserAuthentication
            {
                Description = descriptionTextBox.Text,
                Username = usernameTextBox.Text,
                Password = passwordBox.Password,
            }
        );

        _provider.SaveAsyncWithDebounce();
    }

    private async Task EditTokenAsync(TokenAuthentication tokenAuthentication)
    {
        var descriptionTextBox = new TextBox { Text = tokenAuthentication.Description };
        var tokenTextBox = new TextBox
        {
            Text = tokenAuthentication.Token,
            Margin = new(0, 12, 0, 12),
        };

        var dialog = new ContentDialog
        {
            Title = "编辑令牌凭证",
            PrimaryButtonText = "保存",
            CloseButtonText = "取消",
            DefaultButton = ContentDialogButton.Primary,
            Content = BuildTokenEditorContent(descriptionTextBox, tokenTextBox),
        };

        if (await dialog.ShowAsync() != ContentDialogResult.Primary)
        {
            return;
        }

        tokenAuthentication.Description = descriptionTextBox.Text;
        tokenAuthentication.Token = tokenTextBox.Text;

        _provider.SaveAsyncWithDebounce();
    }

    private async Task EditUserAsync(UserAuthentication userAuthentication)
    {
        var descriptionTextBox = new TextBox { Text = userAuthentication.Description };
        var usernameTextBox = new TextBox
        {
            Text = userAuthentication.Username,
            Margin = new(0, 12, 0, 12),
        };
        var passwordBox = new PasswordBox { Password = userAuthentication.Password };

        var dialog = new ContentDialog
        {
            Title = "编辑用户凭证",
            PrimaryButtonText = "保存",
            CloseButtonText = "取消",
            DefaultButton = ContentDialogButton.Primary,
            Content = BuildUserEditorContent(descriptionTextBox, usernameTextBox, passwordBox),
        };

        if (await dialog.ShowAsync() != ContentDialogResult.Primary)
        {
            return;
        }

        userAuthentication.Description = descriptionTextBox.Text;
        userAuthentication.Username = usernameTextBox.Text;
        userAuthentication.Password = passwordBox.Password;

        _provider.SaveAsyncWithDebounce();
    }

    private static StackPanel BuildTokenEditorContent(
        TextBox descriptionTextBox,
        TextBox tokenTextBox
    )
    {
        ControlHelper.SetHeader(descriptionTextBox, "描述");
        ControlHelper.SetHeader(tokenTextBox, "令牌");

        return new() { Children = { descriptionTextBox, tokenTextBox } };
    }

    private static StackPanel BuildUserEditorContent(
        TextBox descriptionTextBox,
        TextBox usernameTextBox,
        PasswordBox passwordBox
    )
    {
        ControlHelper.SetHeader(descriptionTextBox, "描述");
        ControlHelper.SetHeader(usernameTextBox, "用户名");
        ControlHelper.SetHeader(passwordBox, "密码");

        return new() { Children = { descriptionTextBox, usernameTextBox, passwordBox } };
    }
}
