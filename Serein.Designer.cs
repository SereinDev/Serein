
namespace Serein
{
    partial class Serein
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Serein));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.Panel = new System.Windows.Forms.TabPage();
            this.PanelTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.PanelInfo = new System.Windows.Forms.GroupBox();
            this.PanelControls = new System.Windows.Forms.GroupBox();
            this.PanelControlTerminate = new System.Windows.Forms.Button();
            this.PanelControlRestart = new System.Windows.Forms.Button();
            this.PanelControlStop = new System.Windows.Forms.Button();
            this.PanelControlStart = new System.Windows.Forms.Button();
            this.PanelConsole = new System.Windows.Forms.GroupBox();
            this.PanelConsolePanel2 = new System.Windows.Forms.Panel();
            this.PanelConsoleWebBrowser = new System.Windows.Forms.WebBrowser();
            this.PanelConsolePanel1 = new System.Windows.Forms.Panel();
            this.PanelConsoleEnter = new System.Windows.Forms.Button();
            this.PanelConsoleInput = new System.Windows.Forms.TextBox();
            this.Plugin = new System.Windows.Forms.TabPage();
            this.PluginList = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Regular = new System.Windows.Forms.TabPage();
            this.RegularList = new System.Windows.Forms.ListView();
            this.Task = new System.Windows.Forms.TabPage();
            this.TaskList = new System.Windows.Forms.ListView();
            this.Bot = new System.Windows.Forms.TabPage();
            this.BotTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.BotWebBrowser = new System.Windows.Forms.WebBrowser();
            this.BotInfo = new System.Windows.Forms.GroupBox();
            this.BotControl = new System.Windows.Forms.GroupBox();
            this.BotStop = new System.Windows.Forms.Button();
            this.BotStart = new System.Windows.Forms.Button();
            this.Setting = new System.Windows.Forms.TabPage();
            this.SettingPanel = new System.Windows.Forms.Panel();
            this.SettingSerein = new System.Windows.Forms.GroupBox();
            this.SettingEnableGetAnnouncement = new System.Windows.Forms.CheckBox();
            this.SettingSereinEnableGetUpdate = new System.Windows.Forms.CheckBox();
            this.SettingBot = new System.Windows.Forms.GroupBox();
            this.SettingBotClearCache = new System.Windows.Forms.Button();
            this.SettingBotPermission = new System.Windows.Forms.Label();
            this.SettingBotGroup = new System.Windows.Forms.Label();
            this.SettingBotPermissionList = new System.Windows.Forms.TextBox();
            this.SettingBotGroupList = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.SettingBotSupportedLabel = new System.Windows.Forms.Label();
            this.SettingBotSupportedLink = new System.Windows.Forms.LinkLabel();
            this.SettingBotGivePermissionToAllAdmin = new System.Windows.Forms.CheckBox();
            this.SettingBotEnableLog = new System.Windows.Forms.CheckBox();
            this.SettingBotPathLabel = new System.Windows.Forms.Label();
            this.SettingBotPathSelect = new System.Windows.Forms.Button();
            this.SettingBotPath = new System.Windows.Forms.TextBox();
            this.SettingServer = new System.Windows.Forms.GroupBox();
            this.SettingServerOutputStyleLabel = new System.Windows.Forms.Label();
            this.SettingServerEnableLog = new System.Windows.Forms.CheckBox();
            this.SettingServerOutputStyle = new System.Windows.Forms.ComboBox();
            this.SettingServerEnableOutputCommand = new System.Windows.Forms.CheckBox();
            this.SettingServerEnableRestart = new System.Windows.Forms.CheckBox();
            this.SettingServerPathLabel = new System.Windows.Forms.Label();
            this.SettingServerPathSelect = new System.Windows.Forms.Button();
            this.SettingServerPath = new System.Windows.Forms.TextBox();
            this.SereinIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.MainTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.SettingServerOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SettingBotOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl.SuspendLayout();
            this.Panel.SuspendLayout();
            this.PanelTableLayout.SuspendLayout();
            this.PanelControls.SuspendLayout();
            this.PanelConsole.SuspendLayout();
            this.PanelConsolePanel2.SuspendLayout();
            this.PanelConsolePanel1.SuspendLayout();
            this.Plugin.SuspendLayout();
            this.Regular.SuspendLayout();
            this.Task.SuspendLayout();
            this.Bot.SuspendLayout();
            this.BotTableLayoutPanel.SuspendLayout();
            this.BotControl.SuspendLayout();
            this.Setting.SuspendLayout();
            this.SettingPanel.SuspendLayout();
            this.SettingSerein.SuspendLayout();
            this.SettingBot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SettingServer.SuspendLayout();
            this.MainTableLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.Panel);
            this.tabControl.Controls.Add(this.Plugin);
            this.tabControl.Controls.Add(this.Regular);
            this.tabControl.Controls.Add(this.Task);
            this.tabControl.Controls.Add(this.Bot);
            this.tabControl.Controls.Add(this.Setting);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // Panel
            // 
            this.Panel.Controls.Add(this.PanelTableLayout);
            resources.ApplyResources(this.Panel, "Panel");
            this.Panel.Name = "Panel";
            this.Panel.UseVisualStyleBackColor = true;
            // 
            // PanelTableLayout
            // 
            resources.ApplyResources(this.PanelTableLayout, "PanelTableLayout");
            this.PanelTableLayout.Controls.Add(this.PanelInfo, 0, 0);
            this.PanelTableLayout.Controls.Add(this.PanelControls, 0, 1);
            this.PanelTableLayout.Controls.Add(this.PanelConsole, 1, 0);
            this.PanelTableLayout.Name = "PanelTableLayout";
            // 
            // PanelInfo
            // 
            resources.ApplyResources(this.PanelInfo, "PanelInfo");
            this.PanelInfo.Name = "PanelInfo";
            this.PanelInfo.TabStop = false;
            // 
            // PanelControls
            // 
            this.PanelControls.Controls.Add(this.PanelControlTerminate);
            this.PanelControls.Controls.Add(this.PanelControlRestart);
            this.PanelControls.Controls.Add(this.PanelControlStop);
            this.PanelControls.Controls.Add(this.PanelControlStart);
            resources.ApplyResources(this.PanelControls, "PanelControls");
            this.PanelControls.Name = "PanelControls";
            this.PanelControls.TabStop = false;
            // 
            // PanelControlTerminate
            // 
            resources.ApplyResources(this.PanelControlTerminate, "PanelControlTerminate");
            this.PanelControlTerminate.Name = "PanelControlTerminate";
            this.PanelControlTerminate.UseVisualStyleBackColor = true;
            // 
            // PanelControlRestart
            // 
            resources.ApplyResources(this.PanelControlRestart, "PanelControlRestart");
            this.PanelControlRestart.Name = "PanelControlRestart";
            this.PanelControlRestart.UseVisualStyleBackColor = true;
            // 
            // PanelControlStop
            // 
            resources.ApplyResources(this.PanelControlStop, "PanelControlStop");
            this.PanelControlStop.Name = "PanelControlStop";
            this.PanelControlStop.UseVisualStyleBackColor = true;
            // 
            // PanelControlStart
            // 
            resources.ApplyResources(this.PanelControlStart, "PanelControlStart");
            this.PanelControlStart.Name = "PanelControlStart";
            this.PanelControlStart.UseVisualStyleBackColor = true;
            // 
            // PanelConsole
            // 
            resources.ApplyResources(this.PanelConsole, "PanelConsole");
            this.PanelConsole.Controls.Add(this.PanelConsolePanel2);
            this.PanelConsole.Name = "PanelConsole";
            this.PanelTableLayout.SetRowSpan(this.PanelConsole, 2);
            this.PanelConsole.TabStop = false;
            // 
            // PanelConsolePanel2
            // 
            this.PanelConsolePanel2.Controls.Add(this.PanelConsoleWebBrowser);
            this.PanelConsolePanel2.Controls.Add(this.PanelConsolePanel1);
            resources.ApplyResources(this.PanelConsolePanel2, "PanelConsolePanel2");
            this.PanelConsolePanel2.Name = "PanelConsolePanel2";
            // 
            // PanelConsoleWebBrowser
            // 
            resources.ApplyResources(this.PanelConsoleWebBrowser, "PanelConsoleWebBrowser");
            this.PanelConsoleWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.PanelConsoleWebBrowser.Name = "PanelConsoleWebBrowser";
            this.PanelConsoleWebBrowser.ScrollBarsEnabled = false;
            this.PanelConsoleWebBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // PanelConsolePanel1
            // 
            resources.ApplyResources(this.PanelConsolePanel1, "PanelConsolePanel1");
            this.PanelConsolePanel1.Controls.Add(this.PanelConsoleEnter);
            this.PanelConsolePanel1.Controls.Add(this.PanelConsoleInput);
            this.PanelConsolePanel1.Name = "PanelConsolePanel1";
            // 
            // PanelConsoleEnter
            // 
            resources.ApplyResources(this.PanelConsoleEnter, "PanelConsoleEnter");
            this.PanelConsoleEnter.Name = "PanelConsoleEnter";
            this.PanelConsoleEnter.UseVisualStyleBackColor = true;
            // 
            // PanelConsoleInput
            // 
            resources.ApplyResources(this.PanelConsoleInput, "PanelConsoleInput");
            this.PanelConsoleInput.Name = "PanelConsoleInput";
            // 
            // Plugin
            // 
            this.Plugin.Controls.Add(this.PluginList);
            resources.ApplyResources(this.Plugin, "Plugin");
            this.Plugin.Name = "Plugin";
            this.Plugin.UseVisualStyleBackColor = true;
            // 
            // PluginList
            // 
            this.PluginList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4});
            resources.ApplyResources(this.PluginList, "PluginList");
            this.PluginList.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("PluginList.Groups"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("PluginList.Groups1")))});
            this.PluginList.HideSelection = false;
            this.PluginList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("PluginList.Items"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("PluginList.Items1")))});
            this.PluginList.Name = "PluginList";
            this.PluginList.UseCompatibleStateImageBehavior = false;
            // 
            // Regular
            // 
            this.Regular.Controls.Add(this.RegularList);
            resources.ApplyResources(this.Regular, "Regular");
            this.Regular.Name = "Regular";
            this.Regular.UseVisualStyleBackColor = true;
            // 
            // RegularList
            // 
            resources.ApplyResources(this.RegularList, "RegularList");
            this.RegularList.HideSelection = false;
            this.RegularList.Name = "RegularList";
            this.RegularList.UseCompatibleStateImageBehavior = false;
            // 
            // Task
            // 
            this.Task.Controls.Add(this.TaskList);
            resources.ApplyResources(this.Task, "Task");
            this.Task.Name = "Task";
            this.Task.UseVisualStyleBackColor = true;
            // 
            // TaskList
            // 
            resources.ApplyResources(this.TaskList, "TaskList");
            this.TaskList.HideSelection = false;
            this.TaskList.Name = "TaskList";
            this.TaskList.UseCompatibleStateImageBehavior = false;
            // 
            // Bot
            // 
            this.Bot.Controls.Add(this.BotTableLayoutPanel);
            resources.ApplyResources(this.Bot, "Bot");
            this.Bot.Name = "Bot";
            this.Bot.UseVisualStyleBackColor = true;
            // 
            // BotTableLayoutPanel
            // 
            resources.ApplyResources(this.BotTableLayoutPanel, "BotTableLayoutPanel");
            this.BotTableLayoutPanel.Controls.Add(this.BotWebBrowser, 1, 0);
            this.BotTableLayoutPanel.Controls.Add(this.BotInfo, 0, 0);
            this.BotTableLayoutPanel.Controls.Add(this.BotControl, 0, 1);
            this.BotTableLayoutPanel.Name = "BotTableLayoutPanel";
            // 
            // BotWebBrowser
            // 
            this.BotWebBrowser.AllowWebBrowserDrop = false;
            resources.ApplyResources(this.BotWebBrowser, "BotWebBrowser");
            this.BotWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.BotWebBrowser.Name = "BotWebBrowser";
            this.BotTableLayoutPanel.SetRowSpan(this.BotWebBrowser, 2);
            this.BotWebBrowser.ScrollBarsEnabled = false;
            this.BotWebBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // BotInfo
            // 
            resources.ApplyResources(this.BotInfo, "BotInfo");
            this.BotInfo.Name = "BotInfo";
            this.BotInfo.TabStop = false;
            // 
            // BotControl
            // 
            this.BotControl.Controls.Add(this.BotStop);
            this.BotControl.Controls.Add(this.BotStart);
            resources.ApplyResources(this.BotControl, "BotControl");
            this.BotControl.Name = "BotControl";
            this.BotControl.TabStop = false;
            // 
            // BotStop
            // 
            resources.ApplyResources(this.BotStop, "BotStop");
            this.BotStop.Name = "BotStop";
            this.BotStop.UseVisualStyleBackColor = true;
            // 
            // BotStart
            // 
            resources.ApplyResources(this.BotStart, "BotStart");
            this.BotStart.Name = "BotStart";
            this.BotStart.UseVisualStyleBackColor = true;
            // 
            // Setting
            // 
            this.Setting.Controls.Add(this.SettingPanel);
            resources.ApplyResources(this.Setting, "Setting");
            this.Setting.Name = "Setting";
            this.Setting.UseVisualStyleBackColor = true;
            // 
            // SettingPanel
            // 
            resources.ApplyResources(this.SettingPanel, "SettingPanel");
            this.SettingPanel.Controls.Add(this.SettingSerein);
            this.SettingPanel.Controls.Add(this.SettingBot);
            this.SettingPanel.Controls.Add(this.SettingServer);
            this.SettingPanel.Name = "SettingPanel";
            // 
            // SettingSerein
            // 
            resources.ApplyResources(this.SettingSerein, "SettingSerein");
            this.SettingSerein.Controls.Add(this.SettingEnableGetAnnouncement);
            this.SettingSerein.Controls.Add(this.SettingSereinEnableGetUpdate);
            this.SettingSerein.Name = "SettingSerein";
            this.SettingSerein.TabStop = false;
            // 
            // SettingEnableGetAnnouncement
            // 
            resources.ApplyResources(this.SettingEnableGetAnnouncement, "SettingEnableGetAnnouncement");
            this.SettingEnableGetAnnouncement.Name = "SettingEnableGetAnnouncement";
            this.SettingEnableGetAnnouncement.UseVisualStyleBackColor = true;
            // 
            // SettingSereinEnableGetUpdate
            // 
            resources.ApplyResources(this.SettingSereinEnableGetUpdate, "SettingSereinEnableGetUpdate");
            this.SettingSereinEnableGetUpdate.Name = "SettingSereinEnableGetUpdate";
            this.SettingSereinEnableGetUpdate.UseVisualStyleBackColor = true;
            // 
            // SettingBot
            // 
            resources.ApplyResources(this.SettingBot, "SettingBot");
            this.SettingBot.Controls.Add(this.SettingBotClearCache);
            this.SettingBot.Controls.Add(this.SettingBotPermission);
            this.SettingBot.Controls.Add(this.SettingBotGroup);
            this.SettingBot.Controls.Add(this.SettingBotPermissionList);
            this.SettingBot.Controls.Add(this.SettingBotGroupList);
            this.SettingBot.Controls.Add(this.label2);
            this.SettingBot.Controls.Add(this.label1);
            this.SettingBot.Controls.Add(this.numericUpDown2);
            this.SettingBot.Controls.Add(this.numericUpDown1);
            this.SettingBot.Controls.Add(this.SettingBotSupportedLabel);
            this.SettingBot.Controls.Add(this.SettingBotSupportedLink);
            this.SettingBot.Controls.Add(this.SettingBotGivePermissionToAllAdmin);
            this.SettingBot.Controls.Add(this.SettingBotEnableLog);
            this.SettingBot.Controls.Add(this.SettingBotPathLabel);
            this.SettingBot.Controls.Add(this.SettingBotPathSelect);
            this.SettingBot.Controls.Add(this.SettingBotPath);
            this.SettingBot.Name = "SettingBot";
            this.SettingBot.TabStop = false;
            // 
            // SettingBotClearCache
            // 
            resources.ApplyResources(this.SettingBotClearCache, "SettingBotClearCache");
            this.SettingBotClearCache.Name = "SettingBotClearCache";
            this.SettingBotClearCache.UseVisualStyleBackColor = true;
            // 
            // SettingBotPermission
            // 
            resources.ApplyResources(this.SettingBotPermission, "SettingBotPermission");
            this.SettingBotPermission.Name = "SettingBotPermission";
            // 
            // SettingBotGroup
            // 
            resources.ApplyResources(this.SettingBotGroup, "SettingBotGroup");
            this.SettingBotGroup.Name = "SettingBotGroup";
            // 
            // SettingBotPermissionList
            // 
            resources.ApplyResources(this.SettingBotPermissionList, "SettingBotPermissionList");
            this.SettingBotPermissionList.Name = "SettingBotPermissionList";
            // 
            // SettingBotGroupList
            // 
            resources.ApplyResources(this.SettingBotGroupList, "SettingBotGroupList");
            this.SettingBotGroupList.Name = "SettingBotGroupList";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // numericUpDown2
            // 
            resources.ApplyResources(this.numericUpDown2, "numericUpDown2");
            this.numericUpDown2.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Value = new decimal(new int[] {
            5700,
            0,
            0,
            0});
            // 
            // numericUpDown1
            // 
            resources.ApplyResources(this.numericUpDown1, "numericUpDown1");
            this.numericUpDown1.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Value = new decimal(new int[] {
            8080,
            0,
            0,
            0});
            // 
            // SettingBotSupportedLabel
            // 
            resources.ApplyResources(this.SettingBotSupportedLabel, "SettingBotSupportedLabel");
            this.SettingBotSupportedLabel.Name = "SettingBotSupportedLabel";
            // 
            // SettingBotSupportedLink
            // 
            this.SettingBotSupportedLink.ActiveLinkColor = System.Drawing.Color.DarkSlateGray;
            resources.ApplyResources(this.SettingBotSupportedLink, "SettingBotSupportedLink");
            this.SettingBotSupportedLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SettingBotSupportedLink.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SettingBotSupportedLink.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.SettingBotSupportedLink.LinkColor = System.Drawing.Color.Teal;
            this.SettingBotSupportedLink.Name = "SettingBotSupportedLink";
            this.SettingBotSupportedLink.TabStop = true;
            this.SettingBotSupportedLink.VisitedLinkColor = System.Drawing.Color.Teal;
            this.SettingBotSupportedLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SettingBotSupportedLink_LinkClicked);
            // 
            // SettingBotGivePermissionToAllAdmin
            // 
            resources.ApplyResources(this.SettingBotGivePermissionToAllAdmin, "SettingBotGivePermissionToAllAdmin");
            this.SettingBotGivePermissionToAllAdmin.Name = "SettingBotGivePermissionToAllAdmin";
            this.SettingBotGivePermissionToAllAdmin.UseVisualStyleBackColor = true;
            // 
            // SettingBotEnableLog
            // 
            resources.ApplyResources(this.SettingBotEnableLog, "SettingBotEnableLog");
            this.SettingBotEnableLog.Name = "SettingBotEnableLog";
            this.SettingBotEnableLog.UseVisualStyleBackColor = true;
            // 
            // SettingBotPathLabel
            // 
            resources.ApplyResources(this.SettingBotPathLabel, "SettingBotPathLabel");
            this.SettingBotPathLabel.Name = "SettingBotPathLabel";
            // 
            // SettingBotPathSelect
            // 
            resources.ApplyResources(this.SettingBotPathSelect, "SettingBotPathSelect");
            this.SettingBotPathSelect.Name = "SettingBotPathSelect";
            this.SettingBotPathSelect.UseVisualStyleBackColor = true;
            // 
            // SettingBotPath
            // 
            resources.ApplyResources(this.SettingBotPath, "SettingBotPath");
            this.SettingBotPath.Name = "SettingBotPath";
            this.SettingBotPath.ReadOnly = true;
            // 
            // SettingServer
            // 
            resources.ApplyResources(this.SettingServer, "SettingServer");
            this.SettingServer.Controls.Add(this.SettingServerOutputStyleLabel);
            this.SettingServer.Controls.Add(this.SettingServerEnableLog);
            this.SettingServer.Controls.Add(this.SettingServerOutputStyle);
            this.SettingServer.Controls.Add(this.SettingServerEnableOutputCommand);
            this.SettingServer.Controls.Add(this.SettingServerEnableRestart);
            this.SettingServer.Controls.Add(this.SettingServerPathLabel);
            this.SettingServer.Controls.Add(this.SettingServerPathSelect);
            this.SettingServer.Controls.Add(this.SettingServerPath);
            this.SettingServer.Name = "SettingServer";
            this.SettingServer.TabStop = false;
            // 
            // SettingServerOutputStyleLabel
            // 
            resources.ApplyResources(this.SettingServerOutputStyleLabel, "SettingServerOutputStyleLabel");
            this.SettingServerOutputStyleLabel.Name = "SettingServerOutputStyleLabel";
            // 
            // SettingServerEnableLog
            // 
            resources.ApplyResources(this.SettingServerEnableLog, "SettingServerEnableLog");
            this.SettingServerEnableLog.Name = "SettingServerEnableLog";
            this.SettingServerEnableLog.UseVisualStyleBackColor = true;
            // 
            // SettingServerOutputStyle
            // 
            this.SettingServerOutputStyle.FormattingEnabled = true;
            this.SettingServerOutputStyle.Items.AddRange(new object[] {
            resources.GetString("SettingServerOutputStyle.Items"),
            resources.GetString("SettingServerOutputStyle.Items1"),
            resources.GetString("SettingServerOutputStyle.Items2"),
            resources.GetString("SettingServerOutputStyle.Items3")});
            resources.ApplyResources(this.SettingServerOutputStyle, "SettingServerOutputStyle");
            this.SettingServerOutputStyle.Name = "SettingServerOutputStyle";
            // 
            // SettingServerEnableOutputCommand
            // 
            resources.ApplyResources(this.SettingServerEnableOutputCommand, "SettingServerEnableOutputCommand");
            this.SettingServerEnableOutputCommand.Name = "SettingServerEnableOutputCommand";
            this.SettingServerEnableOutputCommand.UseVisualStyleBackColor = true;
            // 
            // SettingServerEnableRestart
            // 
            resources.ApplyResources(this.SettingServerEnableRestart, "SettingServerEnableRestart");
            this.SettingServerEnableRestart.Name = "SettingServerEnableRestart";
            this.SettingServerEnableRestart.UseVisualStyleBackColor = true;
            // 
            // SettingServerPathLabel
            // 
            resources.ApplyResources(this.SettingServerPathLabel, "SettingServerPathLabel");
            this.SettingServerPathLabel.Name = "SettingServerPathLabel";
            // 
            // SettingServerPathSelect
            // 
            resources.ApplyResources(this.SettingServerPathSelect, "SettingServerPathSelect");
            this.SettingServerPathSelect.Name = "SettingServerPathSelect";
            this.SettingServerPathSelect.UseVisualStyleBackColor = true;
            // 
            // SettingServerPath
            // 
            resources.ApplyResources(this.SettingServerPath, "SettingServerPath");
            this.SettingServerPath.Name = "SettingServerPath";
            this.SettingServerPath.ReadOnly = true;
            // 
            // SereinIcon
            // 
            resources.ApplyResources(this.SereinIcon, "SereinIcon");
            this.SereinIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // MainTableLayout
            // 
            resources.ApplyResources(this.MainTableLayout, "MainTableLayout");
            this.MainTableLayout.Controls.Add(this.tabControl, 0, 0);
            this.MainTableLayout.Name = "MainTableLayout";
            // 
            // SettingServerOpenFileDialog
            // 
            this.SettingServerOpenFileDialog.FileName = "openFileDialog1";
            resources.ApplyResources(this.SettingServerOpenFileDialog, "SettingServerOpenFileDialog");
            // 
            // SettingBotOpenFileDialog
            // 
            this.SettingBotOpenFileDialog.FileName = "openFileDialog1";
            // 
            // Serein
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayout);
            this.Name = "Serein";
            this.tabControl.ResumeLayout(false);
            this.Panel.ResumeLayout(false);
            this.PanelTableLayout.ResumeLayout(false);
            this.PanelTableLayout.PerformLayout();
            this.PanelControls.ResumeLayout(false);
            this.PanelConsole.ResumeLayout(false);
            this.PanelConsolePanel2.ResumeLayout(false);
            this.PanelConsolePanel1.ResumeLayout(false);
            this.PanelConsolePanel1.PerformLayout();
            this.Plugin.ResumeLayout(false);
            this.Regular.ResumeLayout(false);
            this.Task.ResumeLayout(false);
            this.Bot.ResumeLayout(false);
            this.BotTableLayoutPanel.ResumeLayout(false);
            this.BotControl.ResumeLayout(false);
            this.Setting.ResumeLayout(false);
            this.SettingPanel.ResumeLayout(false);
            this.SettingSerein.ResumeLayout(false);
            this.SettingSerein.PerformLayout();
            this.SettingBot.ResumeLayout(false);
            this.SettingBot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.SettingServer.ResumeLayout(false);
            this.SettingServer.PerformLayout();
            this.MainTableLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage Panel;
        private System.Windows.Forms.NotifyIcon SereinIcon;
        private System.Windows.Forms.TableLayoutPanel MainTableLayout;
        private System.Windows.Forms.TableLayoutPanel PanelTableLayout;
        private System.Windows.Forms.GroupBox PanelInfo;
        private System.Windows.Forms.GroupBox PanelControls;
        private System.Windows.Forms.GroupBox PanelConsole;
        private System.Windows.Forms.Panel PanelConsolePanel1;
        private System.Windows.Forms.TextBox PanelConsoleInput;
        private System.Windows.Forms.Button PanelConsoleEnter;
        private System.Windows.Forms.Panel PanelConsolePanel2;
        private System.Windows.Forms.WebBrowser PanelConsoleWebBrowser;
        private System.Windows.Forms.Button PanelControlStart;
        private System.Windows.Forms.Button PanelControlTerminate;
        private System.Windows.Forms.Button PanelControlRestart;
        private System.Windows.Forms.Button PanelControlStop;
        private System.Windows.Forms.TabPage Plugin;
        private System.Windows.Forms.TabPage Regular;
        private System.Windows.Forms.TabPage Setting;
        private System.Windows.Forms.ListView PluginList;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ListView RegularList;
        private System.Windows.Forms.TabPage Task;
        private System.Windows.Forms.ListView TaskList;
        private System.Windows.Forms.TabPage Bot;
        private System.Windows.Forms.TableLayoutPanel BotTableLayoutPanel;
        private System.Windows.Forms.WebBrowser BotWebBrowser;
        private System.Windows.Forms.GroupBox BotInfo;
        private System.Windows.Forms.GroupBox BotControl;
        private System.Windows.Forms.Button BotStop;
        private System.Windows.Forms.Button BotStart;
        private System.Windows.Forms.Panel SettingPanel;
        private System.Windows.Forms.GroupBox SettingServer;
        private System.Windows.Forms.Button SettingServerPathSelect;
        private System.Windows.Forms.TextBox SettingServerPath;
        private System.Windows.Forms.Label SettingServerPathLabel;
        private System.Windows.Forms.CheckBox SettingServerEnableOutputCommand;
        private System.Windows.Forms.CheckBox SettingServerEnableRestart;
        private System.Windows.Forms.OpenFileDialog SettingServerOpenFileDialog;
        private System.Windows.Forms.CheckBox SettingServerEnableLog;
        private System.Windows.Forms.ComboBox SettingServerOutputStyle;
        private System.Windows.Forms.Label SettingServerOutputStyleLabel;
        private System.Windows.Forms.GroupBox SettingBot;
        private System.Windows.Forms.Label SettingBotSupportedLabel;
        private System.Windows.Forms.LinkLabel SettingBotSupportedLink;
        private System.Windows.Forms.CheckBox SettingBotGivePermissionToAllAdmin;
        private System.Windows.Forms.CheckBox SettingBotEnableLog;
        private System.Windows.Forms.Label SettingBotPathLabel;
        private System.Windows.Forms.Button SettingBotPathSelect;
        private System.Windows.Forms.TextBox SettingBotPath;
        private System.Windows.Forms.OpenFileDialog SettingBotOpenFileDialog;
        private System.Windows.Forms.TextBox SettingBotGroupList;
        private System.Windows.Forms.TextBox SettingBotPermissionList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label SettingBotPermission;
        private System.Windows.Forms.Label SettingBotGroup;
        private System.Windows.Forms.GroupBox SettingSerein;
        private System.Windows.Forms.CheckBox SettingEnableGetAnnouncement;
        private System.Windows.Forms.CheckBox SettingSereinEnableGetUpdate;
        private System.Windows.Forms.Button SettingBotClearCache;
    }
}

