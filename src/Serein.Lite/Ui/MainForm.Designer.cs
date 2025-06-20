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
        ToolStripMenuItem serverToolStripMenuItem;
        ToolStripMenuItem serverConsoleToolStripMenuItem;
        ToolStripSeparator serverToolStripSeparator;
        ToolStripMenuItem serverAddToolStripMenuItem;
        ToolStripMenuItem serverImportToolStripMenuItem;
        ToolStripMenuItem functionsToolStripMenuItem;
        ToolStripMenuItem connectionToolStripMenuItem;
        ToolStripMenuItem matchToolStripMenuItem;
        ToolStripMenuItem scheduleToolStripMenuItem;
        ToolStripMenuItem pluginToolStripMenuItem;
        ToolStripMenuItem settingToolStripMenuItem;
        ContextMenuStrip notifyIconContextMenuStrip;
        ToolStripSeparator toolStripSeparator1;
        ToolStripMenuItem exitToolStripMenuItem;
        ToolStripMenuItem membersToolStripMenuItem;
        ToolStripMenuItem bindingToolStripMenuItem;
        ToolStripMenuItem permissionGroupToolStripMenuItem;
        MenuStrip menuStrip;
        ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
        _serverEditToolStripMenuItem = new ToolStripMenuItem();
        _serverRemoveToolStripMenuItem = new ToolStripMenuItem();
        _hideToolStripMenuItem = new ToolStripMenuItem();
        _topMostToolStripMenuItem = new ToolStripMenuItem();
        _notifyIcon = new NotifyIcon(components);
        _childrenPanel = new Panel();
        serverToolStripMenuItem = new ToolStripMenuItem();
        serverConsoleToolStripMenuItem = new ToolStripMenuItem();
        serverToolStripSeparator = new ToolStripSeparator();
        serverAddToolStripMenuItem = new ToolStripMenuItem();
        serverImportToolStripMenuItem = new ToolStripMenuItem();
        functionsToolStripMenuItem = new ToolStripMenuItem();
        connectionToolStripMenuItem = new ToolStripMenuItem();
        matchToolStripMenuItem = new ToolStripMenuItem();
        scheduleToolStripMenuItem = new ToolStripMenuItem();
        pluginToolStripMenuItem = new ToolStripMenuItem();
        settingToolStripMenuItem = new ToolStripMenuItem();
        notifyIconContextMenuStrip = new ContextMenuStrip(components);
        toolStripSeparator1 = new ToolStripSeparator();
        exitToolStripMenuItem = new ToolStripMenuItem();
        membersToolStripMenuItem = new ToolStripMenuItem();
        bindingToolStripMenuItem = new ToolStripMenuItem();
        permissionGroupToolStripMenuItem = new ToolStripMenuItem();
        menuStrip = new MenuStrip();
        notifyIconContextMenuStrip.SuspendLayout();
        menuStrip.SuspendLayout();
        SuspendLayout();
        // 
        // serverToolStripMenuItem
        // 
        serverToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { serverConsoleToolStripMenuItem, serverToolStripSeparator, serverAddToolStripMenuItem, _serverEditToolStripMenuItem, _serverRemoveToolStripMenuItem, serverImportToolStripMenuItem });
        serverToolStripMenuItem.Name = "serverToolStripMenuItem";
        serverToolStripMenuItem.Size = new Size(106, 35);
        serverToolStripMenuItem.Text = "服务器";
        serverToolStripMenuItem.DropDownOpening += ServerToolStripMenuItem_DropDownOpening;
        // 
        // serverConsoleToolStripMenuItem
        // 
        serverConsoleToolStripMenuItem.Name = "serverConsoleToolStripMenuItem";
        serverConsoleToolStripMenuItem.Size = new Size(359, 44);
        serverConsoleToolStripMenuItem.Text = "控制台";
        serverConsoleToolStripMenuItem.Click += ServerConsoleToolStripMenuItem_Click;
        // 
        // serverToolStripSeparator
        // 
        serverToolStripSeparator.Name = "serverToolStripSeparator";
        serverToolStripSeparator.Size = new Size(356, 6);
        // 
        // serverAddToolStripMenuItem
        // 
        serverAddToolStripMenuItem.Name = "serverAddToolStripMenuItem";
        serverAddToolStripMenuItem.Size = new Size(359, 44);
        serverAddToolStripMenuItem.Text = "添加...";
        serverAddToolStripMenuItem.Click += ServerAddToolStripMenuItem_Click;
        // 
        // _serverEditToolStripMenuItem
        // 
        _serverEditToolStripMenuItem.Name = "_serverEditToolStripMenuItem";
        _serverEditToolStripMenuItem.Size = new Size(359, 44);
        _serverEditToolStripMenuItem.Text = "编辑...";
        _serverEditToolStripMenuItem.Click += ServerEditToolStripMenuItem_Click;
        // 
        // _serverRemoveToolStripMenuItem
        // 
        _serverRemoveToolStripMenuItem.Name = "_serverRemoveToolStripMenuItem";
        _serverRemoveToolStripMenuItem.Size = new Size(359, 44);
        _serverRemoveToolStripMenuItem.Text = "删除";
        _serverRemoveToolStripMenuItem.Click += ServerRemoveToolStripMenuItem_Click;
        // 
        // serverImportToolStripMenuItem
        // 
        serverImportToolStripMenuItem.Name = "serverImportToolStripMenuItem";
        serverImportToolStripMenuItem.Size = new Size(359, 44);
        serverImportToolStripMenuItem.Text = "导入...";
        serverImportToolStripMenuItem.Click += ServerImportToolStripMenuItem_Click;
        // 
        // functionsToolStripMenuItem
        // 
        functionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { connectionToolStripMenuItem, matchToolStripMenuItem, scheduleToolStripMenuItem, pluginToolStripMenuItem });
        functionsToolStripMenuItem.Name = "functionsToolStripMenuItem";
        functionsToolStripMenuItem.Size = new Size(82, 35);
        functionsToolStripMenuItem.Text = "功能";
        // 
        // connectionToolStripMenuItem
        // 
        connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
        connectionToolStripMenuItem.Size = new Size(359, 44);
        connectionToolStripMenuItem.Text = "连接";
        connectionToolStripMenuItem.Click += ConnectionToolStripMenuItem_Click;
        // 
        // matchToolStripMenuItem
        // 
        matchToolStripMenuItem.Name = "matchToolStripMenuItem";
        matchToolStripMenuItem.Size = new Size(359, 44);
        matchToolStripMenuItem.Text = "匹配";
        matchToolStripMenuItem.Click += MatchToolStripMenuItem_Click;
        // 
        // scheduleToolStripMenuItem
        // 
        scheduleToolStripMenuItem.Name = "scheduleToolStripMenuItem";
        scheduleToolStripMenuItem.Size = new Size(359, 44);
        scheduleToolStripMenuItem.Text = "定时任务";
        scheduleToolStripMenuItem.Click += ScheduleToolStripMenuItem_Click;
        // 
        // pluginToolStripMenuItem
        // 
        pluginToolStripMenuItem.Name = "pluginToolStripMenuItem";
        pluginToolStripMenuItem.Size = new Size(359, 44);
        pluginToolStripMenuItem.Text = "插件";
        pluginToolStripMenuItem.Click += PluginToolStripMenuItem_Click;
        // 
        // settingToolStripMenuItem
        // 
        settingToolStripMenuItem.Name = "settingToolStripMenuItem";
        settingToolStripMenuItem.Size = new Size(82, 35);
        settingToolStripMenuItem.Text = "设置";
        settingToolStripMenuItem.Click += SettingToolStripMenuItem_Click;
        // 
        // notifyIconContextMenuStrip
        // 
        notifyIconContextMenuStrip.ImageScalingSize = new Size(32, 32);
        notifyIconContextMenuStrip.Items.AddRange(new ToolStripItem[] { _hideToolStripMenuItem, _topMostToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
        notifyIconContextMenuStrip.Name = "NotifyIconContextMenuStrip";
        notifyIconContextMenuStrip.Size = new Size(301, 168);
        // 
        // _hideToolStripMenuItem
        // 
        _hideToolStripMenuItem.Name = "_hideToolStripMenuItem";
        _hideToolStripMenuItem.Size = new Size(300, 38);
        _hideToolStripMenuItem.Text = "隐藏";
        _hideToolStripMenuItem.Click += HideToolStripMenuItem_Click;
        // 
        // _topMostToolStripMenuItem
        // 
        _topMostToolStripMenuItem.Name = "_topMostToolStripMenuItem";
        _topMostToolStripMenuItem.Size = new Size(300, 38);
        _topMostToolStripMenuItem.Text = "置顶";
        _topMostToolStripMenuItem.Click += TopMostToolStripMenuItem_Click;
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(297, 6);
        // 
        // exitToolStripMenuItem
        // 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.Size = new Size(300, 38);
        exitToolStripMenuItem.Text = "退出";
        exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
        // 
        // membersToolStripMenuItem
        // 
        membersToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { bindingToolStripMenuItem, permissionGroupToolStripMenuItem });
        membersToolStripMenuItem.Name = "membersToolStripMenuItem";
        membersToolStripMenuItem.Size = new Size(82, 35);
        membersToolStripMenuItem.Text = "成员";
        // 
        // bindingToolStripMenuItem
        // 
        bindingToolStripMenuItem.Name = "bindingToolStripMenuItem";
        bindingToolStripMenuItem.Size = new Size(359, 44);
        bindingToolStripMenuItem.Text = "绑定";
        bindingToolStripMenuItem.Click += BindingToolStripMenuItem_Click;
        // 
        // permissionGroupToolStripMenuItem
        // 
        permissionGroupToolStripMenuItem.Name = "permissionGroupToolStripMenuItem";
        permissionGroupToolStripMenuItem.Size = new Size(359, 44);
        permissionGroupToolStripMenuItem.Text = "权限组";
        permissionGroupToolStripMenuItem.Click += PermissionGroupToolStripMenuItem_Click;
        // 
        // menuStrip
        // 
        menuStrip.ImageScalingSize = new Size(32, 32);
        menuStrip.Items.AddRange(new ToolStripItem[] { serverToolStripMenuItem, functionsToolStripMenuItem, membersToolStripMenuItem, settingToolStripMenuItem });
        menuStrip.Location = new Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(1254, 39);
        menuStrip.TabIndex = 0;
        menuStrip.Text = "menuStrip1";
        // 
        // _notifyIcon
        // 
        _notifyIcon.ContextMenuStrip = notifyIconContextMenuStrip;
        _notifyIcon.Icon = (Icon)resources.GetObject("_notifyIcon.Icon");
        _notifyIcon.Text = "Serein.Lite";
        _notifyIcon.Visible = true;
        _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
        // 
        // _childrenPanel
        // 
        _childrenPanel.Dock = DockStyle.Fill;
        _childrenPanel.Location = new Point(0, 39);
        _childrenPanel.Name = "_childrenPanel";
        _childrenPanel.Size = new Size(1254, 690);
        _childrenPanel.TabIndex = 1;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(14F, 31F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1254, 729);
        Controls.Add(_childrenPanel);
        Controls.Add(menuStrip);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MainMenuStrip = menuStrip;
        MinimumSize = new Size(800, 600);
        Name = "MainForm";
        Text = "Serein.Lite";
        DragDrop += MainForm_DragDrop;
        notifyIconContextMenuStrip.ResumeLayout(false);
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private NotifyIcon _notifyIcon;
    private ToolStripMenuItem _serverEditToolStripMenuItem;
    private ToolStripMenuItem _serverRemoveToolStripMenuItem;
    private Panel _childrenPanel;
    private ToolStripMenuItem _hideToolStripMenuItem;
    private ToolStripMenuItem _topMostToolStripMenuItem;
}
