using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Serein.Lite.Ui;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new Container();
        ToolStripMenuItem ServerToolStripMenuItem;
        ToolStripSeparator ToolStripSeparator1;
        ToolStripMenuItem FunctionsToolStripMenuItem;
        ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
        ServerConsoleToolStripMenuItem = new ToolStripMenuItem();
        ServerAddToolStripMenuItem = new ToolStripMenuItem();
        ServerEditToolStripMenuItem = new ToolStripMenuItem();
        ServerRemoveToolStripMenuItem = new ToolStripMenuItem();
        ServerImportToolStripMenuItem = new ToolStripMenuItem();
        ConnectionToolStripMenuItem = new ToolStripMenuItem();
        MatchToolStripMenuItem = new ToolStripMenuItem();
        ScheduleToolStripMenuItem = new ToolStripMenuItem();
        PluginToolStripMenuItem = new ToolStripMenuItem();
        MenuStrip = new MenuStrip();
        更多ToolStripMenuItem = new ToolStripMenuItem();
        设置ToolStripMenuItem = new ToolStripMenuItem();
        关于ToolStripMenuItem = new ToolStripMenuItem();
        NotifyIcon = new NotifyIcon(components);
        ServerToolStripMenuItem = new ToolStripMenuItem();
        ToolStripSeparator1 = new ToolStripSeparator();
        FunctionsToolStripMenuItem = new ToolStripMenuItem();
        MenuStrip.SuspendLayout();
        SuspendLayout();
        // 
        // ServerToolStripMenuItem
        // 
        ServerToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ServerConsoleToolStripMenuItem, ToolStripSeparator1, ServerAddToolStripMenuItem, ServerEditToolStripMenuItem, ServerRemoveToolStripMenuItem, ServerImportToolStripMenuItem });
        ServerToolStripMenuItem.Name = "ServerToolStripMenuItem";
        ServerToolStripMenuItem.Size = new Size(106, 38);
        ServerToolStripMenuItem.Text = "服务器";
        ServerToolStripMenuItem.DropDownOpening += ServerToolStripMenuItem_DropDownOpening;
        // 
        // ServerConsoleToolStripMenuItem
        // 
        ServerConsoleToolStripMenuItem.Name = "ServerConsoleToolStripMenuItem";
        ServerConsoleToolStripMenuItem.Size = new Size(359, 44);
        ServerConsoleToolStripMenuItem.Text = "控制台...";
        ServerConsoleToolStripMenuItem.Click += ServerConsoleToolStripMenuItem_Click;
        // 
        // ToolStripSeparator1
        // 
        ToolStripSeparator1.Name = "ToolStripSeparator1";
        ToolStripSeparator1.Size = new Size(356, 6);
        // 
        // ServerAddToolStripMenuItem
        // 
        ServerAddToolStripMenuItem.Name = "ServerAddToolStripMenuItem";
        ServerAddToolStripMenuItem.Size = new Size(359, 44);
        ServerAddToolStripMenuItem.Text = "添加";
        ServerAddToolStripMenuItem.Click += ServerAddToolStripMenuItem_Click;
        // 
        // ServerEditToolStripMenuItem
        // 
        ServerEditToolStripMenuItem.Name = "ServerEditToolStripMenuItem";
        ServerEditToolStripMenuItem.Size = new Size(359, 44);
        ServerEditToolStripMenuItem.Text = "修改";
        // 
        // ServerRemoveToolStripMenuItem
        // 
        ServerRemoveToolStripMenuItem.Name = "ServerRemoveToolStripMenuItem";
        ServerRemoveToolStripMenuItem.Size = new Size(359, 44);
        ServerRemoveToolStripMenuItem.Text = "删除";
        // 
        // ServerImportToolStripMenuItem
        // 
        ServerImportToolStripMenuItem.Name = "ServerImportToolStripMenuItem";
        ServerImportToolStripMenuItem.Size = new Size(359, 44);
        ServerImportToolStripMenuItem.Text = "导入...";
        ServerImportToolStripMenuItem.Click += ServerImportToolStripMenuItem_Click;
        // 
        // FunctionsToolStripMenuItem
        // 
        FunctionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ConnectionToolStripMenuItem, MatchToolStripMenuItem, ScheduleToolStripMenuItem, PluginToolStripMenuItem });
        FunctionsToolStripMenuItem.Name = "FunctionsToolStripMenuItem";
        FunctionsToolStripMenuItem.Size = new Size(82, 38);
        FunctionsToolStripMenuItem.Text = "功能";
        // 
        // ConnectionToolStripMenuItem
        // 
        ConnectionToolStripMenuItem.Name = "ConnectionToolStripMenuItem";
        ConnectionToolStripMenuItem.Size = new Size(243, 44);
        ConnectionToolStripMenuItem.Text = "连接";
        // 
        // MatchToolStripMenuItem
        // 
        MatchToolStripMenuItem.Name = "MatchToolStripMenuItem";
        MatchToolStripMenuItem.Size = new Size(243, 44);
        MatchToolStripMenuItem.Text = "匹配";
        MatchToolStripMenuItem.Click += MatchToolStripMenuItem_Click;
        // 
        // ScheduleToolStripMenuItem
        // 
        ScheduleToolStripMenuItem.Name = "ScheduleToolStripMenuItem";
        ScheduleToolStripMenuItem.Size = new Size(243, 44);
        ScheduleToolStripMenuItem.Text = "定时任务";
        // 
        // PluginToolStripMenuItem
        // 
        PluginToolStripMenuItem.Name = "PluginToolStripMenuItem";
        PluginToolStripMenuItem.Size = new Size(243, 44);
        PluginToolStripMenuItem.Text = "插件";
        // 
        // MenuStrip
        // 
        MenuStrip.ImageScalingSize = new Size(32, 32);
        MenuStrip.Items.AddRange(new ToolStripItem[] { ServerToolStripMenuItem, FunctionsToolStripMenuItem, 更多ToolStripMenuItem });
        MenuStrip.Location = new Point(0, 0);
        MenuStrip.Name = "MenuStrip";
        MenuStrip.Size = new Size(1280, 42);
        MenuStrip.TabIndex = 0;
        MenuStrip.Text = "menuStrip1";
        // 
        // 更多ToolStripMenuItem
        // 
        更多ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 设置ToolStripMenuItem, 关于ToolStripMenuItem });
        更多ToolStripMenuItem.Name = "更多ToolStripMenuItem";
        更多ToolStripMenuItem.Size = new Size(82, 38);
        更多ToolStripMenuItem.Text = "更多";
        // 
        // 设置ToolStripMenuItem
        // 
        设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
        设置ToolStripMenuItem.Size = new Size(195, 44);
        设置ToolStripMenuItem.Text = "设置";
        // 
        // 关于ToolStripMenuItem
        // 
        关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
        关于ToolStripMenuItem.Size = new Size(195, 44);
        关于ToolStripMenuItem.Text = "关于";
        // 
        // NotifyIcon
        // 
        NotifyIcon.Icon = (Icon)resources.GetObject("NotifyIcon.Icon");
        NotifyIcon.Text = "Serein.Lite";
        NotifyIcon.Visible = true;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(14F, 31F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1280, 761);
        Controls.Add(MenuStrip);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MainMenuStrip = MenuStrip;
        MinimumSize = new Size(1280, 800);
        Name = "MainForm";
        Text = "Serein.Lite";
        MenuStrip.ResumeLayout(false);
        MenuStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip MenuStrip;
    private NotifyIcon NotifyIcon;
    private ToolStripMenuItem MatchToolStripMenuItem;
    private ToolStripMenuItem ScheduleToolStripMenuItem;
    private ToolStripMenuItem ConnectionToolStripMenuItem;
    private ToolStripMenuItem PluginToolStripMenuItem;
    private ToolStripMenuItem 更多ToolStripMenuItem;
    private ToolStripMenuItem 设置ToolStripMenuItem;
    private ToolStripMenuItem 关于ToolStripMenuItem;
    private ToolStripMenuItem ServerAddToolStripMenuItem;
    private ToolStripMenuItem ServerEditToolStripMenuItem;
    private ToolStripMenuItem ServerRemoveToolStripMenuItem;
    private ToolStripMenuItem ServerImportToolStripMenuItem;
    private ToolStripMenuItem ServerConsoleToolStripMenuItem;
}
