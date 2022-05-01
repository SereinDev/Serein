
namespace Serein
{
    partial class Ui
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ui));
            this.PluginContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PluginContextMenuStripAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.PluginContextMenuStripRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.PluginContextMenuStripEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.PluginContextMenuStripDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.PluginContextMenuStripRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.SereinIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.MainTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.Panel = new System.Windows.Forms.TabPage();
            this.PanelTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.PanelInfo = new System.Windows.Forms.GroupBox();
            this.PanelInfoCPU2 = new System.Windows.Forms.Label();
            this.PanelInfoPort2 = new System.Windows.Forms.Label();
            this.PanelInfoTime2 = new System.Windows.Forms.Label();
            this.PanelInfoDifficulty2 = new System.Windows.Forms.Label();
            this.PanelInfoVersion2 = new System.Windows.Forms.Label();
            this.PanelInfoCPU = new System.Windows.Forms.Label();
            this.PanelInfoTime = new System.Windows.Forms.Label();
            this.PanelInfoPort = new System.Windows.Forms.Label();
            this.PanelInfoDifficulty = new System.Windows.Forms.Label();
            this.PanelInfoVersion = new System.Windows.Forms.Label();
            this.PanelInfoStatus2 = new System.Windows.Forms.Label();
            this.PanelInfoStatus = new System.Windows.Forms.Label();
            this.PanelControls = new System.Windows.Forms.GroupBox();
            this.PanelControlKill = new System.Windows.Forms.Button();
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
            this.columnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Regular = new System.Windows.Forms.TabPage();
            this.RegexList = new System.Windows.Forms.ListView();
            this.RegexListRegex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RegexListArea = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RegexListIsAdmin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RegexListRemark = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RegexListCommand = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RegexContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RegexContextMenuStripAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.RegexContextMenuStripEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.RegexContextMenuStripDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.RegexContextMenuStripClear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.RegexContextMenuStripRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.Task = new System.Windows.Forms.TabPage();
            this.TaskList = new System.Windows.Forms.ListView();
            this.Bot = new System.Windows.Forms.TabPage();
            this.BotTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.BotWebBrowser = new System.Windows.Forms.WebBrowser();
            this.BotInfo = new System.Windows.Forms.GroupBox();
            this.BotWebsocket = new System.Windows.Forms.GroupBox();
            this.BotClose = new System.Windows.Forms.Button();
            this.BotConnect = new System.Windows.Forms.Button();
            this.Setting = new System.Windows.Forms.TabPage();
            this.SettingPanel = new System.Windows.Forms.Panel();
            this.SettingSerein = new System.Windows.Forms.GroupBox();
            this.SettingSereinVersion = new System.Windows.Forms.Label();
            this.SettingSereinEnableGetAnnouncement = new System.Windows.Forms.CheckBox();
            this.SettingSereinEnableGetUpdate = new System.Windows.Forms.CheckBox();
            this.SettingBot = new System.Windows.Forms.GroupBox();
            this.SettingBotClearCache = new System.Windows.Forms.Button();
            this.SettingBotPermission = new System.Windows.Forms.Label();
            this.SettingBotGroup = new System.Windows.Forms.Label();
            this.SettingBotPermissionList = new System.Windows.Forms.TextBox();
            this.SettingBotGroupList = new System.Windows.Forms.TextBox();
            this.SettingBotPortLabel = new System.Windows.Forms.Label();
            this.SettingBotPort = new System.Windows.Forms.NumericUpDown();
            this.SettingBotSupportedLabel = new System.Windows.Forms.Label();
            this.SettingBotSupportedLink = new System.Windows.Forms.LinkLabel();
            this.SettingBotGivePermissionToAllAdmin = new System.Windows.Forms.CheckBox();
            this.SettingBotEnableLog = new System.Windows.Forms.CheckBox();
            this.SettingServer = new System.Windows.Forms.GroupBox();
            this.SettingServerOutputStyleLabel = new System.Windows.Forms.Label();
            this.SettingServerEnableLog = new System.Windows.Forms.CheckBox();
            this.SettingServerOutputStyle = new System.Windows.Forms.ComboBox();
            this.SettingServerEnableOutputCommand = new System.Windows.Forms.CheckBox();
            this.SettingServerEnableRestart = new System.Windows.Forms.CheckBox();
            this.SettingServerPathLabel = new System.Windows.Forms.Label();
            this.SettingServerPathSelect = new System.Windows.Forms.Button();
            this.SettingServerPath = new System.Windows.Forms.TextBox();
            this.PluginContextMenuStrip.SuspendLayout();
            this.MainTableLayout.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.Panel.SuspendLayout();
            this.PanelTableLayout.SuspendLayout();
            this.PanelInfo.SuspendLayout();
            this.PanelControls.SuspendLayout();
            this.PanelConsole.SuspendLayout();
            this.PanelConsolePanel2.SuspendLayout();
            this.PanelConsolePanel1.SuspendLayout();
            this.Plugin.SuspendLayout();
            this.Regular.SuspendLayout();
            this.RegexContextMenuStrip.SuspendLayout();
            this.Task.SuspendLayout();
            this.Bot.SuspendLayout();
            this.BotTableLayoutPanel.SuspendLayout();
            this.BotWebsocket.SuspendLayout();
            this.Setting.SuspendLayout();
            this.SettingPanel.SuspendLayout();
            this.SettingSerein.SuspendLayout();
            this.SettingBot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SettingBotPort)).BeginInit();
            this.SettingServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // PluginContextMenuStrip
            // 
            this.PluginContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.PluginContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PluginContextMenuStripAdd,
            this.PluginContextMenuStripRemove,
            this.toolStripSeparator1,
            this.PluginContextMenuStripEnable,
            this.PluginContextMenuStripDisable,
            this.toolStripSeparator2,
            this.PluginContextMenuStripRefresh});
            this.PluginContextMenuStrip.Name = "PluginContextMenuStrip";
            resources.ApplyResources(this.PluginContextMenuStrip, "PluginContextMenuStrip");
            this.PluginContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.PluginContextMenuStrip_Opening);
            // 
            // PluginContextMenuStripAdd
            // 
            this.PluginContextMenuStripAdd.Name = "PluginContextMenuStripAdd";
            resources.ApplyResources(this.PluginContextMenuStripAdd, "PluginContextMenuStripAdd");
            this.PluginContextMenuStripAdd.Click += new System.EventHandler(this.PluginContextMenuStripAdd_Click);
            // 
            // PluginContextMenuStripRemove
            // 
            this.PluginContextMenuStripRemove.Name = "PluginContextMenuStripRemove";
            resources.ApplyResources(this.PluginContextMenuStripRemove, "PluginContextMenuStripRemove");
            this.PluginContextMenuStripRemove.Click += new System.EventHandler(this.PluginContextMenuStripRemove_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // PluginContextMenuStripEnable
            // 
            this.PluginContextMenuStripEnable.Name = "PluginContextMenuStripEnable";
            resources.ApplyResources(this.PluginContextMenuStripEnable, "PluginContextMenuStripEnable");
            this.PluginContextMenuStripEnable.Click += new System.EventHandler(this.PluginContextMenuStripEnable_Click);
            // 
            // PluginContextMenuStripDisable
            // 
            this.PluginContextMenuStripDisable.Name = "PluginContextMenuStripDisable";
            resources.ApplyResources(this.PluginContextMenuStripDisable, "PluginContextMenuStripDisable");
            this.PluginContextMenuStripDisable.Click += new System.EventHandler(this.PluginContextMenuStripDisable_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // PluginContextMenuStripRefresh
            // 
            this.PluginContextMenuStripRefresh.Name = "PluginContextMenuStripRefresh";
            resources.ApplyResources(this.PluginContextMenuStripRefresh, "PluginContextMenuStripRefresh");
            this.PluginContextMenuStripRefresh.Click += new System.EventHandler(this.PluginContextMenuStripRefresh_Click);
            // 
            // SereinIcon
            // 
            resources.ApplyResources(this.SereinIcon, "SereinIcon");
            this.SereinIcon.BalloonTipClicked += new System.EventHandler(this.SereinIcon_BalloonTipClicked);
            this.SereinIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SereinIcon_MouseClick);
            // 
            // MainTableLayout
            // 
            resources.ApplyResources(this.MainTableLayout, "MainTableLayout");
            this.MainTableLayout.Controls.Add(this.tabControl, 0, 0);
            this.MainTableLayout.Name = "MainTableLayout";
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
            this.PanelInfo.Controls.Add(this.PanelInfoCPU2);
            this.PanelInfo.Controls.Add(this.PanelInfoPort2);
            this.PanelInfo.Controls.Add(this.PanelInfoTime2);
            this.PanelInfo.Controls.Add(this.PanelInfoDifficulty2);
            this.PanelInfo.Controls.Add(this.PanelInfoVersion2);
            this.PanelInfo.Controls.Add(this.PanelInfoCPU);
            this.PanelInfo.Controls.Add(this.PanelInfoTime);
            this.PanelInfo.Controls.Add(this.PanelInfoPort);
            this.PanelInfo.Controls.Add(this.PanelInfoDifficulty);
            this.PanelInfo.Controls.Add(this.PanelInfoVersion);
            this.PanelInfo.Controls.Add(this.PanelInfoStatus2);
            this.PanelInfo.Controls.Add(this.PanelInfoStatus);
            resources.ApplyResources(this.PanelInfo, "PanelInfo");
            this.PanelInfo.Name = "PanelInfo";
            this.PanelInfo.TabStop = false;
            // 
            // PanelInfoCPU2
            // 
            resources.ApplyResources(this.PanelInfoCPU2, "PanelInfoCPU2");
            this.PanelInfoCPU2.Name = "PanelInfoCPU2";
            // 
            // PanelInfoPort2
            // 
            resources.ApplyResources(this.PanelInfoPort2, "PanelInfoPort2");
            this.PanelInfoPort2.Name = "PanelInfoPort2";
            // 
            // PanelInfoTime2
            // 
            resources.ApplyResources(this.PanelInfoTime2, "PanelInfoTime2");
            this.PanelInfoTime2.Name = "PanelInfoTime2";
            // 
            // PanelInfoDifficulty2
            // 
            resources.ApplyResources(this.PanelInfoDifficulty2, "PanelInfoDifficulty2");
            this.PanelInfoDifficulty2.Name = "PanelInfoDifficulty2";
            // 
            // PanelInfoVersion2
            // 
            resources.ApplyResources(this.PanelInfoVersion2, "PanelInfoVersion2");
            this.PanelInfoVersion2.Name = "PanelInfoVersion2";
            // 
            // PanelInfoCPU
            // 
            resources.ApplyResources(this.PanelInfoCPU, "PanelInfoCPU");
            this.PanelInfoCPU.Name = "PanelInfoCPU";
            // 
            // PanelInfoTime
            // 
            resources.ApplyResources(this.PanelInfoTime, "PanelInfoTime");
            this.PanelInfoTime.Name = "PanelInfoTime";
            // 
            // PanelInfoPort
            // 
            resources.ApplyResources(this.PanelInfoPort, "PanelInfoPort");
            this.PanelInfoPort.Name = "PanelInfoPort";
            // 
            // PanelInfoDifficulty
            // 
            resources.ApplyResources(this.PanelInfoDifficulty, "PanelInfoDifficulty");
            this.PanelInfoDifficulty.Name = "PanelInfoDifficulty";
            // 
            // PanelInfoVersion
            // 
            resources.ApplyResources(this.PanelInfoVersion, "PanelInfoVersion");
            this.PanelInfoVersion.Name = "PanelInfoVersion";
            // 
            // PanelInfoStatus2
            // 
            resources.ApplyResources(this.PanelInfoStatus2, "PanelInfoStatus2");
            this.PanelInfoStatus2.Name = "PanelInfoStatus2";
            // 
            // PanelInfoStatus
            // 
            resources.ApplyResources(this.PanelInfoStatus, "PanelInfoStatus");
            this.PanelInfoStatus.Name = "PanelInfoStatus";
            // 
            // PanelControls
            // 
            this.PanelControls.Controls.Add(this.PanelControlKill);
            this.PanelControls.Controls.Add(this.PanelControlRestart);
            this.PanelControls.Controls.Add(this.PanelControlStop);
            this.PanelControls.Controls.Add(this.PanelControlStart);
            resources.ApplyResources(this.PanelControls, "PanelControls");
            this.PanelControls.Name = "PanelControls";
            this.PanelControls.TabStop = false;
            // 
            // PanelControlKill
            // 
            resources.ApplyResources(this.PanelControlKill, "PanelControlKill");
            this.PanelControlKill.Name = "PanelControlKill";
            this.PanelControlKill.UseVisualStyleBackColor = true;
            this.PanelControlKill.Click += new System.EventHandler(this.PanelControlKill_Click);
            // 
            // PanelControlRestart
            // 
            resources.ApplyResources(this.PanelControlRestart, "PanelControlRestart");
            this.PanelControlRestart.Name = "PanelControlRestart";
            this.PanelControlRestart.UseVisualStyleBackColor = true;
            this.PanelControlRestart.Click += new System.EventHandler(this.PanelControlRestart_Click);
            // 
            // PanelControlStop
            // 
            resources.ApplyResources(this.PanelControlStop, "PanelControlStop");
            this.PanelControlStop.Name = "PanelControlStop";
            this.PanelControlStop.UseVisualStyleBackColor = true;
            this.PanelControlStop.Click += new System.EventHandler(this.PanelControlStop_Click);
            // 
            // PanelControlStart
            // 
            resources.ApplyResources(this.PanelControlStart, "PanelControlStart");
            this.PanelControlStart.Name = "PanelControlStart";
            this.PanelControlStart.UseVisualStyleBackColor = true;
            this.PanelControlStart.Click += new System.EventHandler(this.PanelControlStart_Click);
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
            this.PanelConsoleWebBrowser.AllowWebBrowserDrop = false;
            resources.ApplyResources(this.PanelConsoleWebBrowser, "PanelConsoleWebBrowser");
            this.PanelConsoleWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.PanelConsoleWebBrowser.Name = "PanelConsoleWebBrowser";
            this.PanelConsoleWebBrowser.ScriptErrorsSuppressed = true;
            this.PanelConsoleWebBrowser.ScrollBarsEnabled = false;
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
            this.PanelConsoleEnter.Click += new System.EventHandler(this.PanelConsoleEnter_Click);
            // 
            // PanelConsoleInput
            // 
            resources.ApplyResources(this.PanelConsoleInput, "PanelConsoleInput");
            this.PanelConsoleInput.Name = "PanelConsoleInput";
            this.PanelConsoleInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PanelConsoleInput_KeyDown);
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
            this.PluginList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PluginList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader});
            this.PluginList.ContextMenuStrip = this.PluginContextMenuStrip;
            resources.ApplyResources(this.PluginList, "PluginList");
            this.PluginList.HideSelection = false;
            this.PluginList.Name = "PluginList";
            this.PluginList.UseCompatibleStateImageBehavior = false;
            this.PluginList.View = System.Windows.Forms.View.SmallIcon;
            // 
            // Regular
            // 
            this.Regular.Controls.Add(this.RegexList);
            resources.ApplyResources(this.Regular, "Regular");
            this.Regular.Name = "Regular";
            this.Regular.UseVisualStyleBackColor = true;
            // 
            // RegexList
            // 
            this.RegexList.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.RegexList.BackColor = System.Drawing.SystemColors.Window;
            this.RegexList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.RegexListRegex,
            this.RegexListArea,
            this.RegexListIsAdmin,
            this.RegexListRemark,
            this.RegexListCommand});
            this.RegexList.ContextMenuStrip = this.RegexContextMenuStrip;
            this.RegexList.Cursor = System.Windows.Forms.Cursors.Arrow;
            resources.ApplyResources(this.RegexList, "RegexList");
            this.RegexList.FullRowSelect = true;
            this.RegexList.GridLines = true;
            this.RegexList.HideSelection = false;
            this.RegexList.MultiSelect = false;
            this.RegexList.Name = "RegexList";
            this.RegexList.UseCompatibleStateImageBehavior = false;
            this.RegexList.View = System.Windows.Forms.View.Details;
            // 
            // RegexListRegex
            // 
            resources.ApplyResources(this.RegexListRegex, "RegexListRegex");
            // 
            // RegexListArea
            // 
            resources.ApplyResources(this.RegexListArea, "RegexListArea");
            // 
            // RegexListIsAdmin
            // 
            resources.ApplyResources(this.RegexListIsAdmin, "RegexListIsAdmin");
            // 
            // RegexListRemark
            // 
            resources.ApplyResources(this.RegexListRemark, "RegexListRemark");
            // 
            // RegexListCommand
            // 
            resources.ApplyResources(this.RegexListCommand, "RegexListCommand");
            // 
            // RegexContextMenuStrip
            // 
            this.RegexContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.RegexContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RegexContextMenuStripAdd,
            this.RegexContextMenuStripEdit,
            this.RegexContextMenuStripDelete,
            this.RegexContextMenuStripClear,
            this.toolStripSeparator3,
            this.RegexContextMenuStripRefresh});
            this.RegexContextMenuStrip.Name = "RegexMenuStrip";
            resources.ApplyResources(this.RegexContextMenuStrip, "RegexContextMenuStrip");
            this.RegexContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.RegexContextMenuStrip_Opening);
            // 
            // RegexContextMenuStripAdd
            // 
            this.RegexContextMenuStripAdd.Name = "RegexContextMenuStripAdd";
            resources.ApplyResources(this.RegexContextMenuStripAdd, "RegexContextMenuStripAdd");
            this.RegexContextMenuStripAdd.Click += new System.EventHandler(this.RegexContextMenuStripAdd_Click);
            // 
            // RegexContextMenuStripEdit
            // 
            this.RegexContextMenuStripEdit.Name = "RegexContextMenuStripEdit";
            resources.ApplyResources(this.RegexContextMenuStripEdit, "RegexContextMenuStripEdit");
            this.RegexContextMenuStripEdit.Click += new System.EventHandler(this.RegexContextMenuStripEdit_Click);
            // 
            // RegexContextMenuStripDelete
            // 
            this.RegexContextMenuStripDelete.Name = "RegexContextMenuStripDelete";
            resources.ApplyResources(this.RegexContextMenuStripDelete, "RegexContextMenuStripDelete");
            this.RegexContextMenuStripDelete.Click += new System.EventHandler(this.RegexContextMenuStripDelete_Click);
            // 
            // RegexContextMenuStripClear
            // 
            this.RegexContextMenuStripClear.Name = "RegexContextMenuStripClear";
            resources.ApplyResources(this.RegexContextMenuStripClear, "RegexContextMenuStripClear");
            this.RegexContextMenuStripClear.Click += new System.EventHandler(this.RegexContextMenuStripClear_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // RegexContextMenuStripRefresh
            // 
            this.RegexContextMenuStripRefresh.Name = "RegexContextMenuStripRefresh";
            resources.ApplyResources(this.RegexContextMenuStripRefresh, "RegexContextMenuStripRefresh");
            this.RegexContextMenuStripRefresh.Click += new System.EventHandler(this.RegexContextMenuStripRefresh_Click);
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
            this.BotTableLayoutPanel.Controls.Add(this.BotWebsocket, 0, 1);
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
            // 
            // BotInfo
            // 
            resources.ApplyResources(this.BotInfo, "BotInfo");
            this.BotInfo.Name = "BotInfo";
            this.BotInfo.TabStop = false;
            // 
            // BotWebsocket
            // 
            this.BotWebsocket.Controls.Add(this.BotClose);
            this.BotWebsocket.Controls.Add(this.BotConnect);
            resources.ApplyResources(this.BotWebsocket, "BotWebsocket");
            this.BotWebsocket.Name = "BotWebsocket";
            this.BotWebsocket.TabStop = false;
            // 
            // BotClose
            // 
            resources.ApplyResources(this.BotClose, "BotClose");
            this.BotClose.Name = "BotClose";
            this.BotClose.UseVisualStyleBackColor = true;
            this.BotClose.Click += new System.EventHandler(this.BotClose_Click);
            // 
            // BotConnect
            // 
            resources.ApplyResources(this.BotConnect, "BotConnect");
            this.BotConnect.Name = "BotConnect";
            this.BotConnect.UseVisualStyleBackColor = true;
            this.BotConnect.Click += new System.EventHandler(this.BotConnect_Click);
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
            this.SettingSerein.Controls.Add(this.SettingSereinVersion);
            this.SettingSerein.Controls.Add(this.SettingSereinEnableGetAnnouncement);
            this.SettingSerein.Controls.Add(this.SettingSereinEnableGetUpdate);
            resources.ApplyResources(this.SettingSerein, "SettingSerein");
            this.SettingSerein.Name = "SettingSerein";
            this.SettingSerein.TabStop = false;
            // 
            // SettingSereinVersion
            // 
            resources.ApplyResources(this.SettingSereinVersion, "SettingSereinVersion");
            this.SettingSereinVersion.Name = "SettingSereinVersion";
            // 
            // SettingSereinEnableGetAnnouncement
            // 
            resources.ApplyResources(this.SettingSereinEnableGetAnnouncement, "SettingSereinEnableGetAnnouncement");
            this.SettingSereinEnableGetAnnouncement.Name = "SettingSereinEnableGetAnnouncement";
            this.SettingSereinEnableGetAnnouncement.UseVisualStyleBackColor = true;
            this.SettingSereinEnableGetAnnouncement.CheckedChanged += new System.EventHandler(this.SettingSereinEnableGetAnnouncement_CheckedChanged);
            this.SettingSereinEnableGetAnnouncement.MouseHover += new System.EventHandler(this.SettingSereinEnableGetAnnouncement_MouseHover);
            // 
            // SettingSereinEnableGetUpdate
            // 
            resources.ApplyResources(this.SettingSereinEnableGetUpdate, "SettingSereinEnableGetUpdate");
            this.SettingSereinEnableGetUpdate.Name = "SettingSereinEnableGetUpdate";
            this.SettingSereinEnableGetUpdate.UseVisualStyleBackColor = true;
            this.SettingSereinEnableGetUpdate.CheckedChanged += new System.EventHandler(this.SettingSereinEnableGetUpdate_CheckedChanged);
            this.SettingSereinEnableGetUpdate.MouseHover += new System.EventHandler(this.SettingSereinEnableGetUpdate_MouseHover);
            // 
            // SettingBot
            // 
            this.SettingBot.Controls.Add(this.SettingBotClearCache);
            this.SettingBot.Controls.Add(this.SettingBotPermission);
            this.SettingBot.Controls.Add(this.SettingBotGroup);
            this.SettingBot.Controls.Add(this.SettingBotPermissionList);
            this.SettingBot.Controls.Add(this.SettingBotGroupList);
            this.SettingBot.Controls.Add(this.SettingBotPortLabel);
            this.SettingBot.Controls.Add(this.SettingBotPort);
            this.SettingBot.Controls.Add(this.SettingBotSupportedLabel);
            this.SettingBot.Controls.Add(this.SettingBotSupportedLink);
            this.SettingBot.Controls.Add(this.SettingBotGivePermissionToAllAdmin);
            this.SettingBot.Controls.Add(this.SettingBotEnableLog);
            resources.ApplyResources(this.SettingBot, "SettingBot");
            this.SettingBot.Name = "SettingBot";
            this.SettingBot.TabStop = false;
            // 
            // SettingBotClearCache
            // 
            resources.ApplyResources(this.SettingBotClearCache, "SettingBotClearCache");
            this.SettingBotClearCache.Name = "SettingBotClearCache";
            this.SettingBotClearCache.UseVisualStyleBackColor = true;
            this.SettingBotClearCache.MouseHover += new System.EventHandler(this.SettingBotClearCache_MouseHover);
            // 
            // SettingBotPermission
            // 
            resources.ApplyResources(this.SettingBotPermission, "SettingBotPermission");
            this.SettingBotPermission.Name = "SettingBotPermission";
            this.SettingBotPermission.MouseHover += new System.EventHandler(this.SettingBotPermission_MouseHover);
            // 
            // SettingBotGroup
            // 
            resources.ApplyResources(this.SettingBotGroup, "SettingBotGroup");
            this.SettingBotGroup.Name = "SettingBotGroup";
            this.SettingBotGroup.MouseHover += new System.EventHandler(this.SettingBotGroup_MouseHover);
            // 
            // SettingBotPermissionList
            // 
            resources.ApplyResources(this.SettingBotPermissionList, "SettingBotPermissionList");
            this.SettingBotPermissionList.Name = "SettingBotPermissionList";
            this.SettingBotPermissionList.TextChanged += new System.EventHandler(this.SettingBotPermissionList_TextChanged);
            this.SettingBotPermissionList.MouseHover += new System.EventHandler(this.SettingBotPermissionList_MouseHover);
            // 
            // SettingBotGroupList
            // 
            resources.ApplyResources(this.SettingBotGroupList, "SettingBotGroupList");
            this.SettingBotGroupList.Name = "SettingBotGroupList";
            this.SettingBotGroupList.TextChanged += new System.EventHandler(this.SettingBotGroupList_TextChanged);
            this.SettingBotGroupList.MouseHover += new System.EventHandler(this.SettingBotGroupList_MouseHover);
            // 
            // SettingBotPortLabel
            // 
            resources.ApplyResources(this.SettingBotPortLabel, "SettingBotPortLabel");
            this.SettingBotPortLabel.Name = "SettingBotPortLabel";
            this.SettingBotPortLabel.MouseHover += new System.EventHandler(this.SettingBotPortLabel_MouseHover);
            // 
            // SettingBotPort
            // 
            resources.ApplyResources(this.SettingBotPort, "SettingBotPort");
            this.SettingBotPort.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.SettingBotPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SettingBotPort.Name = "SettingBotPort";
            this.SettingBotPort.Value = new decimal(new int[] {
            6700,
            0,
            0,
            0});
            this.SettingBotPort.ValueChanged += new System.EventHandler(this.SettingBotPort_ValueChanged);
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
            this.SettingBotGivePermissionToAllAdmin.CheckedChanged += new System.EventHandler(this.SettingBotGivePermissionToAllAdmin_CheckedChanged);
            this.SettingBotGivePermissionToAllAdmin.MouseHover += new System.EventHandler(this.SettingBotGivePermissionToAllAdmin_MouseHover);
            // 
            // SettingBotEnableLog
            // 
            resources.ApplyResources(this.SettingBotEnableLog, "SettingBotEnableLog");
            this.SettingBotEnableLog.Name = "SettingBotEnableLog";
            this.SettingBotEnableLog.UseVisualStyleBackColor = true;
            this.SettingBotEnableLog.CheckedChanged += new System.EventHandler(this.SettingBotEnableLog_CheckedChanged);
            this.SettingBotEnableLog.MouseHover += new System.EventHandler(this.SettingBotEnableLog_MouseHover);
            // 
            // SettingServer
            // 
            this.SettingServer.Controls.Add(this.SettingServerOutputStyleLabel);
            this.SettingServer.Controls.Add(this.SettingServerEnableLog);
            this.SettingServer.Controls.Add(this.SettingServerOutputStyle);
            this.SettingServer.Controls.Add(this.SettingServerEnableOutputCommand);
            this.SettingServer.Controls.Add(this.SettingServerEnableRestart);
            this.SettingServer.Controls.Add(this.SettingServerPathLabel);
            this.SettingServer.Controls.Add(this.SettingServerPathSelect);
            this.SettingServer.Controls.Add(this.SettingServerPath);
            resources.ApplyResources(this.SettingServer, "SettingServer");
            this.SettingServer.Name = "SettingServer";
            this.SettingServer.TabStop = false;
            // 
            // SettingServerOutputStyleLabel
            // 
            resources.ApplyResources(this.SettingServerOutputStyleLabel, "SettingServerOutputStyleLabel");
            this.SettingServerOutputStyleLabel.Name = "SettingServerOutputStyleLabel";
            this.SettingServerOutputStyleLabel.MouseHover += new System.EventHandler(this.SettingServerOutputStyleLabel_MouseHover);
            // 
            // SettingServerEnableLog
            // 
            resources.ApplyResources(this.SettingServerEnableLog, "SettingServerEnableLog");
            this.SettingServerEnableLog.Name = "SettingServerEnableLog";
            this.SettingServerEnableLog.UseVisualStyleBackColor = true;
            this.SettingServerEnableLog.CheckedChanged += new System.EventHandler(this.SettingServerEnableLog_CheckedChanged);
            this.SettingServerEnableLog.MouseHover += new System.EventHandler(this.SettingServerEnableLog_MouseHover);
            // 
            // SettingServerOutputStyle
            // 
            this.SettingServerOutputStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SettingServerOutputStyle.FormattingEnabled = true;
            this.SettingServerOutputStyle.Items.AddRange(new object[] {
            resources.GetString("SettingServerOutputStyle.Items"),
            resources.GetString("SettingServerOutputStyle.Items1"),
            resources.GetString("SettingServerOutputStyle.Items2")});
            resources.ApplyResources(this.SettingServerOutputStyle, "SettingServerOutputStyle");
            this.SettingServerOutputStyle.Name = "SettingServerOutputStyle";
            this.SettingServerOutputStyle.SelectedIndexChanged += new System.EventHandler(this.SettingServerOutputStyle_SelectedIndexChanged);
            this.SettingServerOutputStyle.MouseHover += new System.EventHandler(this.SettingServerOutputStyle_MouseHover);
            // 
            // SettingServerEnableOutputCommand
            // 
            resources.ApplyResources(this.SettingServerEnableOutputCommand, "SettingServerEnableOutputCommand");
            this.SettingServerEnableOutputCommand.Name = "SettingServerEnableOutputCommand";
            this.SettingServerEnableOutputCommand.UseVisualStyleBackColor = true;
            this.SettingServerEnableOutputCommand.CheckedChanged += new System.EventHandler(this.SettingServerEnableOutputCommand_CheckedChanged);
            this.SettingServerEnableOutputCommand.MouseHover += new System.EventHandler(this.SettingServerEnableOutputCommand_MouseHover);
            // 
            // SettingServerEnableRestart
            // 
            resources.ApplyResources(this.SettingServerEnableRestart, "SettingServerEnableRestart");
            this.SettingServerEnableRestart.Name = "SettingServerEnableRestart";
            this.SettingServerEnableRestart.UseVisualStyleBackColor = true;
            this.SettingServerEnableRestart.CheckedChanged += new System.EventHandler(this.SettingServerEnableRestart_CheckedChanged);
            this.SettingServerEnableRestart.MouseHover += new System.EventHandler(this.SettingServerEnableRestart_MouseHover);
            // 
            // SettingServerPathLabel
            // 
            resources.ApplyResources(this.SettingServerPathLabel, "SettingServerPathLabel");
            this.SettingServerPathLabel.Name = "SettingServerPathLabel";
            this.SettingServerPathLabel.MouseHover += new System.EventHandler(this.SettingServerPathLabel_MouseHover);
            // 
            // SettingServerPathSelect
            // 
            resources.ApplyResources(this.SettingServerPathSelect, "SettingServerPathSelect");
            this.SettingServerPathSelect.Name = "SettingServerPathSelect";
            this.SettingServerPathSelect.UseVisualStyleBackColor = true;
            this.SettingServerPathSelect.Click += new System.EventHandler(this.SettingServerPathSelect_Click);
            // 
            // SettingServerPath
            // 
            resources.ApplyResources(this.SettingServerPath, "SettingServerPath");
            this.SettingServerPath.Name = "SettingServerPath";
            this.SettingServerPath.ReadOnly = true;
            this.SettingServerPath.MouseHover += new System.EventHandler(this.SettingServerPath_MouseHover);
            // 
            // Ui
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.MainTableLayout);
            this.Name = "Ui";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Serein_FormClosing);
            this.PluginContextMenuStrip.ResumeLayout(false);
            this.MainTableLayout.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.Panel.ResumeLayout(false);
            this.PanelTableLayout.ResumeLayout(false);
            this.PanelTableLayout.PerformLayout();
            this.PanelInfo.ResumeLayout(false);
            this.PanelInfo.PerformLayout();
            this.PanelControls.ResumeLayout(false);
            this.PanelConsole.ResumeLayout(false);
            this.PanelConsolePanel2.ResumeLayout(false);
            this.PanelConsolePanel1.ResumeLayout(false);
            this.PanelConsolePanel1.PerformLayout();
            this.Plugin.ResumeLayout(false);
            this.Regular.ResumeLayout(false);
            this.RegexContextMenuStrip.ResumeLayout(false);
            this.Task.ResumeLayout(false);
            this.Bot.ResumeLayout(false);
            this.BotTableLayoutPanel.ResumeLayout(false);
            this.BotWebsocket.ResumeLayout(false);
            this.Setting.ResumeLayout(false);
            this.SettingPanel.ResumeLayout(false);
            this.SettingSerein.ResumeLayout(false);
            this.SettingSerein.PerformLayout();
            this.SettingBot.ResumeLayout(false);
            this.SettingBot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SettingBotPort)).EndInit();
            this.SettingServer.ResumeLayout(false);
            this.SettingServer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel MainTableLayout;
        private System.Windows.Forms.ContextMenuStrip PluginContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem PluginContextMenuStripAdd;
        private System.Windows.Forms.ToolStripMenuItem PluginContextMenuStripRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem PluginContextMenuStripDisable;
        private System.Windows.Forms.ToolStripMenuItem PluginContextMenuStripEnable;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem PluginContextMenuStripRefresh;
        public System.Windows.Forms.NotifyIcon SereinIcon;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage Panel;
        private System.Windows.Forms.TableLayoutPanel PanelTableLayout;
        private System.Windows.Forms.GroupBox PanelInfo;
        private System.Windows.Forms.Label PanelInfoCPU2;
        private System.Windows.Forms.Label PanelInfoPort2;
        private System.Windows.Forms.Label PanelInfoTime2;
        private System.Windows.Forms.Label PanelInfoDifficulty2;
        private System.Windows.Forms.Label PanelInfoVersion2;
        private System.Windows.Forms.Label PanelInfoCPU;
        private System.Windows.Forms.Label PanelInfoTime;
        private System.Windows.Forms.Label PanelInfoPort;
        private System.Windows.Forms.Label PanelInfoDifficulty;
        private System.Windows.Forms.Label PanelInfoVersion;
        private System.Windows.Forms.Label PanelInfoStatus2;
        private System.Windows.Forms.Label PanelInfoStatus;
        private System.Windows.Forms.GroupBox PanelControls;
        private System.Windows.Forms.Button PanelControlKill;
        private System.Windows.Forms.Button PanelControlRestart;
        private System.Windows.Forms.Button PanelControlStop;
        private System.Windows.Forms.Button PanelControlStart;
        private System.Windows.Forms.GroupBox PanelConsole;
        private System.Windows.Forms.Panel PanelConsolePanel2;
        private System.Windows.Forms.WebBrowser PanelConsoleWebBrowser;
        private System.Windows.Forms.Panel PanelConsolePanel1;
        private System.Windows.Forms.Button PanelConsoleEnter;
        private System.Windows.Forms.TextBox PanelConsoleInput;
        private System.Windows.Forms.TabPage Plugin;
        public System.Windows.Forms.ListView PluginList;
        private System.Windows.Forms.ColumnHeader columnHeader;
        private System.Windows.Forms.TabPage Regular;
        private System.Windows.Forms.ListView RegexList;
        private System.Windows.Forms.TabPage Task;
        private System.Windows.Forms.ListView TaskList;
        private System.Windows.Forms.TabPage Bot;
        private System.Windows.Forms.TableLayoutPanel BotTableLayoutPanel;
        private System.Windows.Forms.WebBrowser BotWebBrowser;
        private System.Windows.Forms.GroupBox BotInfo;
        private System.Windows.Forms.GroupBox BotWebsocket;
        private System.Windows.Forms.Button BotClose;
        private System.Windows.Forms.Button BotConnect;
        private System.Windows.Forms.TabPage Setting;
        private System.Windows.Forms.Panel SettingPanel;
        private System.Windows.Forms.GroupBox SettingSerein;
        private System.Windows.Forms.Label SettingSereinVersion;
        private System.Windows.Forms.CheckBox SettingSereinEnableGetAnnouncement;
        private System.Windows.Forms.CheckBox SettingSereinEnableGetUpdate;
        private System.Windows.Forms.GroupBox SettingBot;
        private System.Windows.Forms.Button SettingBotClearCache;
        private System.Windows.Forms.Label SettingBotPermission;
        private System.Windows.Forms.Label SettingBotGroup;
        private System.Windows.Forms.TextBox SettingBotPermissionList;
        private System.Windows.Forms.TextBox SettingBotGroupList;
        private System.Windows.Forms.Label SettingBotPortLabel;
        private System.Windows.Forms.NumericUpDown SettingBotPort;
        private System.Windows.Forms.Label SettingBotSupportedLabel;
        private System.Windows.Forms.LinkLabel SettingBotSupportedLink;
        private System.Windows.Forms.CheckBox SettingBotGivePermissionToAllAdmin;
        private System.Windows.Forms.CheckBox SettingBotEnableLog;
        private System.Windows.Forms.GroupBox SettingServer;
        private System.Windows.Forms.Label SettingServerOutputStyleLabel;
        private System.Windows.Forms.CheckBox SettingServerEnableLog;
        private System.Windows.Forms.ComboBox SettingServerOutputStyle;
        private System.Windows.Forms.CheckBox SettingServerEnableOutputCommand;
        private System.Windows.Forms.CheckBox SettingServerEnableRestart;
        private System.Windows.Forms.Label SettingServerPathLabel;
        private System.Windows.Forms.Button SettingServerPathSelect;
        private System.Windows.Forms.TextBox SettingServerPath;
        private System.Windows.Forms.ContextMenuStrip RegexContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem RegexContextMenuStripAdd;
        private System.Windows.Forms.ToolStripMenuItem RegexContextMenuStripDelete;
        private System.Windows.Forms.ToolStripMenuItem RegexContextMenuStripClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem RegexContextMenuStripRefresh;
        private System.Windows.Forms.ColumnHeader RegexListArea;
        private System.Windows.Forms.ColumnHeader RegexListRegex;
        private System.Windows.Forms.ColumnHeader RegexListRemark;
        private System.Windows.Forms.ColumnHeader RegexListCommand;
        private System.Windows.Forms.ColumnHeader RegexListIsAdmin;
        private System.Windows.Forms.ToolStripMenuItem RegexContextMenuStripEdit;
    }
}

