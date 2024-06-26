﻿using System.Windows.Forms;

using Serein.Core;
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
        "https://www.minecraft.net/zh-hans/eula".OpenInBrowser();
    }

    private void DeclarationLinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        "https://account.mojang.com/documents/commercial_guidelines".OpenInBrowser();
    }

    private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        "https://github.com/SereinDev/Serein".OpenInBrowser();
    }

    private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        "https://sereindev.github.io/".OpenInBrowser();
    }

    private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        "https://jq.qq.com/?_wv=1027&k=XNZqPSPv".OpenInBrowser();
    }
}
