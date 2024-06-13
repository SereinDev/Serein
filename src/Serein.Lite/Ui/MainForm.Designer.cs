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
        ToolStripSeparator ServerToolStripSeparator;
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
        MoreToolStripMenuItem = new ToolStripMenuItem();
        SettingToolStripMenuItem = new ToolStripMenuItem();
        AboutToolStripMenuItem = new ToolStripMenuItem();
        NotifyIcon = new NotifyIcon(components);
        NotifyIconContextMenuStrip = new ContextMenuStrip(components);
        HideToolStripMenuItem = new ToolStripMenuItem();
        TopMostToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        ExitToolStripMenuItem = new ToolStripMenuItem();
        ChildrenPanel = new Panel();
        ServerToolStripMenuItem = new ToolStripMenuItem();
        ServerToolStripSeparator = new ToolStripSeparator();
        FunctionsToolStripMenuItem = new ToolStripMenuItem();
        MenuStrip.SuspendLayout();
        NotifyIconContextMenuStrip.SuspendLayout();
        SuspendLayout();
        // 
        // ServerToolStripMenuItem
        // 
        ServerToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ServerConsoleToolStripMenuItem, ServerToolStripSeparator, ServerAddToolStripMenuItem, ServerEditToolStripMenuItem, ServerRemoveToolStripMenuItem, ServerImportToolStripMenuItem });
        ServerToolStripMenuItem.Name = "ServerToolStripMenuItem";
        ServerToolStripMenuItem.Size = new Size(106, 35);
        ServerToolStripMenuItem.Text = "服务器";
        ServerToolStripMenuItem.DropDownOpening += ServerToolStripMenuItem_DropDownOpening;
        // 
        // ServerConsoleToolStripMenuItem
        // 
        ServerConsoleToolStripMenuItem.Name = "ServerConsoleToolStripMenuItem";
        ServerConsoleToolStripMenuItem.Size = new Size(219, 44);
        ServerConsoleToolStripMenuItem.Text = "控制台";
        ServerConsoleToolStripMenuItem.Click += ServerConsoleToolStripMenuItem_Click;
        // 
        // ServerToolStripSeparator
        // 
        ServerToolStripSeparator.Name = "ServerToolStripSeparator";
        ServerToolStripSeparator.Size = new Size(216, 6);
        // 
        // ServerAddToolStripMenuItem
        // 
        ServerAddToolStripMenuItem.Name = "ServerAddToolStripMenuItem";
        ServerAddToolStripMenuItem.Size = new Size(219, 44);
        ServerAddToolStripMenuItem.Text = "添加";
        ServerAddToolStripMenuItem.Click += ServerAddToolStripMenuItem_Click;
        // 
        // ServerEditToolStripMenuItem
        // 
        ServerEditToolStripMenuItem.Name = "ServerEditToolStripMenuItem";
        ServerEditToolStripMenuItem.Size = new Size(219, 44);
        ServerEditToolStripMenuItem.Text = "修改";
        ServerEditToolStripMenuItem.Click += ServerEditToolStripMenuItem_Click;
        // 
        // ServerRemoveToolStripMenuItem
        // 
        ServerRemoveToolStripMenuItem.Name = "ServerRemoveToolStripMenuItem";
        ServerRemoveToolStripMenuItem.Size = new Size(219, 44);
        ServerRemoveToolStripMenuItem.Text = "删除";
        ServerRemoveToolStripMenuItem.Click += ServerRemoveToolStripMenuItem_Click;
        // 
        // ServerImportToolStripMenuItem
        // 
        ServerImportToolStripMenuItem.Name = "ServerImportToolStripMenuItem";
        ServerImportToolStripMenuItem.Size = new Size(219, 44);
        ServerImportToolStripMenuItem.Text = "导入...";
        ServerImportToolStripMenuItem.Click += ServerImportToolStripMenuItem_Click;
        // 
        // FunctionsToolStripMenuItem
        // 
        FunctionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ConnectionToolStripMenuItem, MatchToolStripMenuItem, ScheduleToolStripMenuItem, PluginToolStripMenuItem });
        FunctionsToolStripMenuItem.Name = "FunctionsToolStripMenuItem";
        FunctionsToolStripMenuItem.Size = new Size(82, 35);
        FunctionsToolStripMenuItem.Text = "功能";
        // 
        // ConnectionToolStripMenuItem
        // 
        ConnectionToolStripMenuItem.Name = "ConnectionToolStripMenuItem";
        ConnectionToolStripMenuItem.Size = new Size(243, 44);
        ConnectionToolStripMenuItem.Text = "连接";
        ConnectionToolStripMenuItem.Click += ConnectionToolStripMenuItem_Click;
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
        MenuStrip.Items.AddRange(new ToolStripItem[] { ServerToolStripMenuItem, FunctionsToolStripMenuItem, MoreToolStripMenuItem });
        MenuStrip.Location = new Point(0, 0);
        MenuStrip.Name = "MenuStrip";
        MenuStrip.Size = new Size(1280, 39);
        MenuStrip.TabIndex = 0;
        MenuStrip.Text = "menuStrip1";
        // 
        // MoreToolStripMenuItem
        // 
        MoreToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { SettingToolStripMenuItem, AboutToolStripMenuItem });
        MoreToolStripMenuItem.Name = "MoreToolStripMenuItem";
        MoreToolStripMenuItem.Size = new Size(82, 35);
        MoreToolStripMenuItem.Text = "更多";
        // 
        // SettingToolStripMenuItem
        // 
        SettingToolStripMenuItem.Name = "SettingToolStripMenuItem";
        SettingToolStripMenuItem.Size = new Size(195, 44);
        SettingToolStripMenuItem.Text = "设置";
        // 
        // AboutToolStripMenuItem
        // 
        AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
        AboutToolStripMenuItem.Size = new Size(195, 44);
        AboutToolStripMenuItem.Text = "关于";
        // 
        // NotifyIcon
        // 
        NotifyIcon.ContextMenuStrip = NotifyIconContextMenuStrip;
        NotifyIcon.Icon = (Icon)resources.GetObject("NotifyIcon.Icon");
        NotifyIcon.Text = "Serein.Lite";
        NotifyIcon.Visible = true;
        // 
        // NotifyIconContextMenuStrip
        // 
        NotifyIconContextMenuStrip.ImageScalingSize = new Size(32, 32);
        NotifyIconContextMenuStrip.Items.AddRange(new ToolStripItem[] { HideToolStripMenuItem, TopMostToolStripMenuItem, toolStripSeparator1, ExitToolStripMenuItem });
        NotifyIconContextMenuStrip.Name = "NotifyIconContextMenuStrip";
        NotifyIconContextMenuStrip.Size = new Size(137, 130);
        // 
        // HideToolStripMenuItem
        // 
        HideToolStripMenuItem.Name = "HideToolStripMenuItem";
        HideToolStripMenuItem.Size = new Size(136, 40);
        HideToolStripMenuItem.Text = "隐藏";
        HideToolStripMenuItem.Click += HideToolStripMenuItem_Click;
        // 
        // TopMostToolStripMenuItem
        // 
        TopMostToolStripMenuItem.Name = "TopMostToolStripMenuItem";
        TopMostToolStripMenuItem.Size = new Size(136, 40);
        TopMostToolStripMenuItem.Text = "置顶";
        TopMostToolStripMenuItem.Click += TopMostToolStripMenuItem_Click;
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(133, 6);
        // 
        // ExitToolStripMenuItem
        // 
        ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
        ExitToolStripMenuItem.Size = new Size(136, 40);
        ExitToolStripMenuItem.Text = "退出";
        ExitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
        // 
        // ChildrenPanel
        // 
        ChildrenPanel.Dock = DockStyle.Fill;
        ChildrenPanel.Location = new Point(0, 39);
        ChildrenPanel.Name = "ChildrenPanel";
        ChildrenPanel.Size = new Size(1280, 722);
        ChildrenPanel.TabIndex = 1;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(14F, 31F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1280, 761);
        Controls.Add(ChildrenPanel);
        Controls.Add(MenuStrip);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MainMenuStrip = MenuStrip;
        MinimumSize = new Size(1280, 800);
        Name = "MainForm";
        Text = "Serein.Lite";
        MenuStrip.ResumeLayout(false);
        MenuStrip.PerformLayout();
        NotifyIconContextMenuStrip.ResumeLayout(false);
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
    private ToolStripMenuItem MoreToolStripMenuItem;
    private ToolStripMenuItem SettingToolStripMenuItem;
    private ToolStripMenuItem AboutToolStripMenuItem;
    private ToolStripMenuItem ServerAddToolStripMenuItem;
    private ToolStripMenuItem ServerEditToolStripMenuItem;
    private ToolStripMenuItem ServerRemoveToolStripMenuItem;
    private ToolStripMenuItem ServerImportToolStripMenuItem;
    private ToolStripMenuItem ServerConsoleToolStripMenuItem;
    private Panel ChildrenPanel;
    private ContextMenuStrip NotifyIconContextMenuStrip;
    private ToolStripMenuItem HideToolStripMenuItem;
    private ToolStripMenuItem TopMostToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem ExitToolStripMenuItem;
}
