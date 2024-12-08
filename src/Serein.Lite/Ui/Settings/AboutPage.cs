using System;
using System.Windows.Forms;
using Serein.Core;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;

namespace Serein.Lite.Ui.Settings;

public partial class AboutPage : UserControl
{
    public AboutPage()
    {
        InitializeComponent();

        VersionLabel.Text += SereinApp.Version;
        DetailedVersionLabel.Text += SereinApp.FullVersion;
        AssemblyLabel.Text += typeof(Program).Assembly.ToString();
    }

    private void DeclarationLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        UrlConstants.Eula.OpenInBrowser();
    }

    private void DeclarationLinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        UrlConstants.CommercialGuidelines.OpenInBrowser();
    }

    private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        UrlConstants.Repository.OpenInBrowser();
    }

    private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        UrlConstants.Docs.OpenInBrowser();
    }

    private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        UrlConstants.Group.OpenInBrowser();
    }

    private void CopiableLabel_Click(object sender, EventArgs e)
    {
        if (sender is Label label)
        {
            Clipboard.SetText(label.Text);
            MessageBox.Show("复制成功", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
