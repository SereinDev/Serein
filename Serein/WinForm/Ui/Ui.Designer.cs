
namespace Serein.Ui
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
            System.Windows.Forms.GroupBox PanelInfo;
            System.Windows.Forms.Label PanelInfoCPU;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ui));
            System.Windows.Forms.Label PanelInfoTime;
            System.Windows.Forms.Label PanelInfoLevel;
            System.Windows.Forms.Label PanelInfoDifficulty;
            System.Windows.Forms.Label PanelInfoVersion;
            System.Windows.Forms.Label PanelInfoStatus;
            System.Windows.Forms.GroupBox PanelControls;
            System.Windows.Forms.GroupBox PanelConsole;
            System.Windows.Forms.GroupBox BotInfo;
            System.Windows.Forms.Label BotInfoStatus;
            System.Windows.Forms.Label BotInfoTime;
            System.Windows.Forms.Label BotInfoMessageSent;
            System.Windows.Forms.Label BotInfoMessageReceived;
            System.Windows.Forms.Label BotInfoQQ;
            System.Windows.Forms.GroupBox BotWebsocket;
            System.Windows.Forms.SplitContainer SereinPluginsSplitContainer;
            this.PanelInfoCPU2 = new System.Windows.Forms.Label();
            this.PanelInfoLevel2 = new System.Windows.Forms.Label();
            this.PanelInfoTime2 = new System.Windows.Forms.Label();
            this.PanelInfoDifficulty2 = new System.Windows.Forms.Label();
            this.PanelInfoVersion2 = new System.Windows.Forms.Label();
            this.PanelInfoStatus2 = new System.Windows.Forms.Label();
            this.PanelControlKill = new System.Windows.Forms.Button();
            this.PanelControlRestart = new System.Windows.Forms.Button();
            this.PanelControlStop = new System.Windows.Forms.Button();
            this.PanelControlStart = new System.Windows.Forms.Button();
            this.PanelConsolePanel = new System.Windows.Forms.Panel();
            this.PanelConsoleEnter = new System.Windows.Forms.Button();
            this.PanelConsoleWebBrowser = new System.Windows.Forms.WebBrowser();
            this.PanelConsoleInput = new System.Windows.Forms.TextBox();
            this.BotInfoStatus2 = new System.Windows.Forms.Label();
            this.BotInfoMessageReceived2 = new System.Windows.Forms.Label();
            this.BotInfoMessageSent2 = new System.Windows.Forms.Label();
            this.BotInfoTime2 = new System.Windows.Forms.Label();
            this.BotInfoQQ2 = new System.Windows.Forms.Label();
            this.BotClose = new System.Windows.Forms.Button();
            this.BotConnect = new System.Windows.Forms.Button();
            this.SereinPluginsWebBrowser = new System.Windows.Forms.WebBrowser();
            this.SereinPluginsList = new System.Windows.Forms.ListView();
            this.SereinPluginsListName = new System.Windows.Forms.ColumnHeader();
            this.SereinPluginsListVersion = new System.Windows.Forms.ColumnHeader();
            this.SereinPluginsListAuthor = new System.Windows.Forms.ColumnHeader();
            this.SereinPluginsListDescription = new System.Windows.Forms.ColumnHeader();
            this.SereinPluginsListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SereinPluginsListContextMenuStrip_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.SereinPluginsListContextMenuStrip_ClearConsole = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.SereinPluginsListContextMenuStrip_Docs = new System.Windows.Forms.ToolStripMenuItem();
            this.PluginContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PluginContextMenuStripAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.PluginContextMenuStripRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.PluginContextMenuStripEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.PluginContextMenuStripDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.PluginContextMenuStripShow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.PluginContextMenuStripRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.SereinIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.MainTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.Panel = new System.Windows.Forms.TabPage();
            this.PanelTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.Plugin = new System.Windows.Forms.TabPage();
            this.PluginList = new System.Windows.Forms.ListView();
            this.columnHeader = new System.Windows.Forms.ColumnHeader();
            this.Regular = new System.Windows.Forms.TabPage();
            this.RegexList = new System.Windows.Forms.ListView();
            this.RegexListRegex = new System.Windows.Forms.ColumnHeader();
            this.RegexListArea = new System.Windows.Forms.ColumnHeader();
            this.RegexListIsAdmin = new System.Windows.Forms.ColumnHeader();
            this.RegexListRemark = new System.Windows.Forms.ColumnHeader();
            this.RegexListCommand = new System.Windows.Forms.ColumnHeader();
            this.RegexContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RegexContextMenuStrip_Add = new System.Windows.Forms.ToolStripMenuItem();
            this.RegexContextMenuStrip_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.RegexContextMenuStrip_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.RegexContextMenuStrip_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.RegexContextMenuStrip_Command = new System.Windows.Forms.ToolStripMenuItem();
            this.RegexContextMenuStrip_Variables = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.RegexContextMenuStripRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.Task = new System.Windows.Forms.TabPage();
            this.TaskList = new System.Windows.Forms.ListView();
            this.TaskListCron = new System.Windows.Forms.ColumnHeader();
            this.TaskListRemark = new System.Windows.Forms.ColumnHeader();
            this.TaskListCommand = new System.Windows.Forms.ColumnHeader();
            this.TaskContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TaskContextMenuStrip_Add = new System.Windows.Forms.ToolStripMenuItem();
            this.TaskContextMenuStrip_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.TaskContextMenuStrip_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.TaskContextMenuStrip_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.TaskContextMenuStrip_Enable = new System.Windows.Forms.ToolStripMenuItem();
            this.TaskContextMenuStrip_Disable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.TaskContextMenuStrip_Command = new System.Windows.Forms.ToolStripMenuItem();
            this.TaskContextMenuStrip_Variables = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.TaskContextMenuStrip_Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.Bot = new System.Windows.Forms.TabPage();
            this.BotTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.BotWebBrowser = new System.Windows.Forms.WebBrowser();
            this.Member = new System.Windows.Forms.TabPage();
            this.MemberList = new System.Windows.Forms.ListView();
            this.MemberListID = new System.Windows.Forms.ColumnHeader();
            this.MemberListRole = new System.Windows.Forms.ColumnHeader();
            this.MemberListNickname = new System.Windows.Forms.ColumnHeader();
            this.MemberListCard = new System.Windows.Forms.ColumnHeader();
            this.MemberListGameID = new System.Windows.Forms.ColumnHeader();
            this.MemberContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MemberContextMenuStrip_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.MemberContextMenuStrip_Remove = new System.Windows.Forms.ToolStripMenuItem();
            this.MemberContextMenuStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MemberContextMenuStrip_Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.SereinPlugins = new System.Windows.Forms.TabPage();
            this.Setting = new System.Windows.Forms.TabPage();
            this.SettingPanel = new System.Windows.Forms.Panel();
            this.SettingSerein = new System.Windows.Forms.GroupBox();
            this.SettingSereinStatement1 = new System.Windows.Forms.Label();
            this.SettingSereinShowWelcomePage = new System.Windows.Forms.Button();
            this.SettingSereinEnableDPIAware = new System.Windows.Forms.CheckBox();
            this.Splitter = new System.Windows.Forms.Label();
            this.SettingSereinDownload = new System.Windows.Forms.Label();
            this.SettingSereinStatement2 = new System.Windows.Forms.Label();
            this.SettingSereinStatement = new System.Windows.Forms.Label();
            this.SettingSereinPage = new System.Windows.Forms.Label();
            this.SettingSereinAbout = new System.Windows.Forms.Label();
            this.SettingSereinEnableGetUpdate = new System.Windows.Forms.CheckBox();
            this.SettingSereinTutorial = new System.Windows.Forms.Label();
            this.SettingSereinVersion = new System.Windows.Forms.Label();
            this.SettingEvent = new System.Windows.Forms.GroupBox();
            this.SettingEventSplitContainer = new System.Windows.Forms.SplitContainer();
            this.SettingEventTreeView = new System.Windows.Forms.TreeView();
            this.SettingEventList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.SettingEventListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SettingEventListContextMenuStrip_Add = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingEventListContextMenuStrip_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingEventListContextMenuStrip_Remove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.SettingEventListContextMenuStrip_Docs = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingBot = new System.Windows.Forms.GroupBox();
            this.SettingBotEnbaleParseAt = new System.Windows.Forms.CheckBox();
            this.SettingBotAutoEscape = new System.Windows.Forms.CheckBox();
            this.SettingBotRestart = new System.Windows.Forms.CheckBox();
            this.SettingBotAuthorization = new System.Windows.Forms.TextBox();
            this.SettingBotAuthorizationLabel = new System.Windows.Forms.Label();
            this.SettingBotUri = new System.Windows.Forms.TextBox();
            this.SettingBotEnbaleOutputData = new System.Windows.Forms.CheckBox();
            this.SettingBotPermission = new System.Windows.Forms.Label();
            this.SettingBotGroup = new System.Windows.Forms.Label();
            this.SettingBotPermissionList = new System.Windows.Forms.TextBox();
            this.SettingBotGroupList = new System.Windows.Forms.TextBox();
            this.SettingBotUriLabel = new System.Windows.Forms.Label();
            this.SettingBotSupportedLabel = new System.Windows.Forms.Label();
            this.SettingBotGivePermissionToAllAdmin = new System.Windows.Forms.CheckBox();
            this.SettingBotEnableLog = new System.Windows.Forms.CheckBox();
            this.SettingServer = new System.Windows.Forms.GroupBox();
            this.SettingServerLineTerminator = new System.Windows.Forms.TextBox();
            this.SettingServerLineTerminatorLabel = new System.Windows.Forms.Label();
            this.SettingServerOutputEncodingLabel = new System.Windows.Forms.Label();
            this.SettingServerOutputEncoding = new System.Windows.Forms.ComboBox();
            this.SettingServerTypeLabel = new System.Windows.Forms.Label();
            this.SettingServerPortLabel = new System.Windows.Forms.Label();
            this.SettingServerType = new System.Windows.Forms.ComboBox();
            this.SettingServerPort = new System.Windows.Forms.NumericUpDown();
            this.SettingServerOutputStyle = new System.Windows.Forms.ComboBox();
            this.SettingServerStopCommand = new System.Windows.Forms.TextBox();
            this.SettingServerEnableUnicode = new System.Windows.Forms.CheckBox();
            this.SettingServerEncoding = new System.Windows.Forms.ComboBox();
            this.SettingServerEncodingLabel = new System.Windows.Forms.Label();
            this.SettingServerAutoStop = new System.Windows.Forms.CheckBox();
            this.SettingServerStopCommandLabel = new System.Windows.Forms.Label();
            this.SettingServerOutputStyleLabel = new System.Windows.Forms.Label();
            this.SettingServerEnableLog = new System.Windows.Forms.CheckBox();
            this.SettingServerEnableOutputCommand = new System.Windows.Forms.CheckBox();
            this.SettingServerEnableRestart = new System.Windows.Forms.CheckBox();
            this.SettingServerPathLabel = new System.Windows.Forms.Label();
            this.SettingServerPathSelect = new System.Windows.Forms.Button();
            this.SettingServerPath = new System.Windows.Forms.TextBox();
            this.Debug = new System.Windows.Forms.TabPage();
            this.DebugTextBox = new System.Windows.Forms.TextBox();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            PanelInfo = new System.Windows.Forms.GroupBox();
            PanelInfoCPU = new System.Windows.Forms.Label();
            PanelInfoTime = new System.Windows.Forms.Label();
            PanelInfoLevel = new System.Windows.Forms.Label();
            PanelInfoDifficulty = new System.Windows.Forms.Label();
            PanelInfoVersion = new System.Windows.Forms.Label();
            PanelInfoStatus = new System.Windows.Forms.Label();
            PanelControls = new System.Windows.Forms.GroupBox();
            PanelConsole = new System.Windows.Forms.GroupBox();
            BotInfo = new System.Windows.Forms.GroupBox();
            BotInfoStatus = new System.Windows.Forms.Label();
            BotInfoTime = new System.Windows.Forms.Label();
            BotInfoMessageSent = new System.Windows.Forms.Label();
            BotInfoMessageReceived = new System.Windows.Forms.Label();
            BotInfoQQ = new System.Windows.Forms.Label();
            BotWebsocket = new System.Windows.Forms.GroupBox();
            SereinPluginsSplitContainer = new System.Windows.Forms.SplitContainer();
            PanelInfo.SuspendLayout();
            PanelControls.SuspendLayout();
            PanelConsole.SuspendLayout();
            this.PanelConsolePanel.SuspendLayout();
            BotInfo.SuspendLayout();
            BotWebsocket.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(SereinPluginsSplitContainer)).BeginInit();
            SereinPluginsSplitContainer.Panel1.SuspendLayout();
            SereinPluginsSplitContainer.Panel2.SuspendLayout();
            SereinPluginsSplitContainer.SuspendLayout();
            this.SereinPluginsListContextMenuStrip.SuspendLayout();
            this.PluginContextMenuStrip.SuspendLayout();
            this.MainTableLayout.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.Panel.SuspendLayout();
            this.PanelTableLayout.SuspendLayout();
            this.Plugin.SuspendLayout();
            this.Regular.SuspendLayout();
            this.RegexContextMenuStrip.SuspendLayout();
            this.Task.SuspendLayout();
            this.TaskContextMenuStrip.SuspendLayout();
            this.Bot.SuspendLayout();
            this.BotTableLayoutPanel.SuspendLayout();
            this.Member.SuspendLayout();
            this.MemberContextMenuStrip.SuspendLayout();
            this.SereinPlugins.SuspendLayout();
            this.Setting.SuspendLayout();
            this.SettingPanel.SuspendLayout();
            this.SettingSerein.SuspendLayout();
            this.SettingEvent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SettingEventSplitContainer)).BeginInit();
            this.SettingEventSplitContainer.Panel1.SuspendLayout();
            this.SettingEventSplitContainer.Panel2.SuspendLayout();
            this.SettingEventSplitContainer.SuspendLayout();
            this.SettingEventListContextMenuStrip.SuspendLayout();
            this.SettingBot.SuspendLayout();
            this.SettingServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SettingServerPort)).BeginInit();
            this.Debug.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelInfo
            // 
            PanelInfo.Controls.Add(PanelInfoCPU);
            PanelInfo.Controls.Add(PanelInfoTime);
            PanelInfo.Controls.Add(this.PanelInfoCPU2);
            PanelInfo.Controls.Add(this.PanelInfoLevel2);
            PanelInfo.Controls.Add(this.PanelInfoTime2);
            PanelInfo.Controls.Add(this.PanelInfoDifficulty2);
            PanelInfo.Controls.Add(this.PanelInfoVersion2);
            PanelInfo.Controls.Add(PanelInfoLevel);
            PanelInfo.Controls.Add(PanelInfoDifficulty);
            PanelInfo.Controls.Add(PanelInfoVersion);
            PanelInfo.Controls.Add(this.PanelInfoStatus2);
            PanelInfo.Controls.Add(PanelInfoStatus);
            resources.ApplyResources(PanelInfo, "PanelInfo");
            PanelInfo.Name = "PanelInfo";
            PanelInfo.TabStop = false;
            // 
            // PanelInfoCPU
            // 
            resources.ApplyResources(PanelInfoCPU, "PanelInfoCPU");
            PanelInfoCPU.Name = "PanelInfoCPU";
            // 
            // PanelInfoTime
            // 
            resources.ApplyResources(PanelInfoTime, "PanelInfoTime");
            PanelInfoTime.Name = "PanelInfoTime";
            // 
            // PanelInfoCPU2
            // 
            resources.ApplyResources(this.PanelInfoCPU2, "PanelInfoCPU2");
            this.PanelInfoCPU2.Name = "PanelInfoCPU2";
            // 
            // PanelInfoLevel2
            // 
            resources.ApplyResources(this.PanelInfoLevel2, "PanelInfoLevel2");
            this.PanelInfoLevel2.Name = "PanelInfoLevel2";
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
            // PanelInfoLevel
            // 
            resources.ApplyResources(PanelInfoLevel, "PanelInfoLevel");
            PanelInfoLevel.Name = "PanelInfoLevel";
            // 
            // PanelInfoDifficulty
            // 
            resources.ApplyResources(PanelInfoDifficulty, "PanelInfoDifficulty");
            PanelInfoDifficulty.Name = "PanelInfoDifficulty";
            // 
            // PanelInfoVersion
            // 
            resources.ApplyResources(PanelInfoVersion, "PanelInfoVersion");
            PanelInfoVersion.Name = "PanelInfoVersion";
            // 
            // PanelInfoStatus2
            // 
            resources.ApplyResources(this.PanelInfoStatus2, "PanelInfoStatus2");
            this.PanelInfoStatus2.Name = "PanelInfoStatus2";
            // 
            // PanelInfoStatus
            // 
            resources.ApplyResources(PanelInfoStatus, "PanelInfoStatus");
            PanelInfoStatus.Name = "PanelInfoStatus";
            // 
            // PanelControls
            // 
            PanelControls.Controls.Add(this.PanelControlKill);
            PanelControls.Controls.Add(this.PanelControlRestart);
            PanelControls.Controls.Add(this.PanelControlStop);
            PanelControls.Controls.Add(this.PanelControlStart);
            resources.ApplyResources(PanelControls, "PanelControls");
            PanelControls.Name = "PanelControls";
            PanelControls.TabStop = false;
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
            resources.ApplyResources(PanelConsole, "PanelConsole");
            PanelConsole.Controls.Add(this.PanelConsolePanel);
            PanelConsole.Name = "PanelConsole";
            this.PanelTableLayout.SetRowSpan(PanelConsole, 2);
            PanelConsole.TabStop = false;
            // 
            // PanelConsolePanel
            // 
            this.PanelConsolePanel.Controls.Add(this.PanelConsoleEnter);
            this.PanelConsolePanel.Controls.Add(this.PanelConsoleWebBrowser);
            this.PanelConsolePanel.Controls.Add(this.PanelConsoleInput);
            resources.ApplyResources(this.PanelConsolePanel, "PanelConsolePanel");
            this.PanelConsolePanel.Name = "PanelConsolePanel";
            // 
            // PanelConsoleEnter
            // 
            resources.ApplyResources(this.PanelConsoleEnter, "PanelConsoleEnter");
            this.PanelConsoleEnter.Name = "PanelConsoleEnter";
            this.PanelConsoleEnter.UseVisualStyleBackColor = true;
            this.PanelConsoleEnter.Click += new System.EventHandler(this.PanelConsoleEnter_Click);
            // 
            // PanelConsoleWebBrowser
            // 
            this.PanelConsoleWebBrowser.AllowWebBrowserDrop = false;
            resources.ApplyResources(this.PanelConsoleWebBrowser, "PanelConsoleWebBrowser");
            this.PanelConsoleWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.PanelConsoleWebBrowser.Name = "PanelConsoleWebBrowser";
            this.PanelConsoleWebBrowser.ScriptErrorsSuppressed = true;
            this.PanelConsoleWebBrowser.ScrollBarsEnabled = false;
            this.PanelConsoleWebBrowser.TabStop = false;
            // 
            // PanelConsoleInput
            // 
            resources.ApplyResources(this.PanelConsoleInput, "PanelConsoleInput");
            this.PanelConsoleInput.Name = "PanelConsoleInput";
            this.PanelConsoleInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PanelConsoleInput_KeyDown);
            this.PanelConsoleInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PanelConsoleInput_KeyPress);
            // 
            // BotInfo
            // 
            BotInfo.Controls.Add(this.BotInfoStatus2);
            BotInfo.Controls.Add(this.BotInfoMessageReceived2);
            BotInfo.Controls.Add(this.BotInfoMessageSent2);
            BotInfo.Controls.Add(this.BotInfoTime2);
            BotInfo.Controls.Add(BotInfoStatus);
            BotInfo.Controls.Add(this.BotInfoQQ2);
            BotInfo.Controls.Add(BotInfoTime);
            BotInfo.Controls.Add(BotInfoMessageSent);
            BotInfo.Controls.Add(BotInfoMessageReceived);
            BotInfo.Controls.Add(BotInfoQQ);
            resources.ApplyResources(BotInfo, "BotInfo");
            BotInfo.Name = "BotInfo";
            BotInfo.TabStop = false;
            // 
            // BotInfoStatus2
            // 
            resources.ApplyResources(this.BotInfoStatus2, "BotInfoStatus2");
            this.BotInfoStatus2.Name = "BotInfoStatus2";
            // 
            // BotInfoMessageReceived2
            // 
            resources.ApplyResources(this.BotInfoMessageReceived2, "BotInfoMessageReceived2");
            this.BotInfoMessageReceived2.Name = "BotInfoMessageReceived2";
            // 
            // BotInfoMessageSent2
            // 
            resources.ApplyResources(this.BotInfoMessageSent2, "BotInfoMessageSent2");
            this.BotInfoMessageSent2.Name = "BotInfoMessageSent2";
            // 
            // BotInfoTime2
            // 
            resources.ApplyResources(this.BotInfoTime2, "BotInfoTime2");
            this.BotInfoTime2.Name = "BotInfoTime2";
            // 
            // BotInfoStatus
            // 
            resources.ApplyResources(BotInfoStatus, "BotInfoStatus");
            BotInfoStatus.Name = "BotInfoStatus";
            // 
            // BotInfoQQ2
            // 
            resources.ApplyResources(this.BotInfoQQ2, "BotInfoQQ2");
            this.BotInfoQQ2.Name = "BotInfoQQ2";
            // 
            // BotInfoTime
            // 
            resources.ApplyResources(BotInfoTime, "BotInfoTime");
            BotInfoTime.Name = "BotInfoTime";
            // 
            // BotInfoMessageSent
            // 
            resources.ApplyResources(BotInfoMessageSent, "BotInfoMessageSent");
            BotInfoMessageSent.Name = "BotInfoMessageSent";
            // 
            // BotInfoMessageReceived
            // 
            resources.ApplyResources(BotInfoMessageReceived, "BotInfoMessageReceived");
            BotInfoMessageReceived.Name = "BotInfoMessageReceived";
            // 
            // BotInfoQQ
            // 
            resources.ApplyResources(BotInfoQQ, "BotInfoQQ");
            BotInfoQQ.Name = "BotInfoQQ";
            // 
            // BotWebsocket
            // 
            BotWebsocket.Controls.Add(this.BotClose);
            BotWebsocket.Controls.Add(this.BotConnect);
            resources.ApplyResources(BotWebsocket, "BotWebsocket");
            BotWebsocket.Name = "BotWebsocket";
            BotWebsocket.TabStop = false;
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
            // SereinPluginsSplitContainer
            // 
            resources.ApplyResources(SereinPluginsSplitContainer, "SereinPluginsSplitContainer");
            SereinPluginsSplitContainer.Name = "SereinPluginsSplitContainer";
            // 
            // SereinPluginsSplitContainer.Panel1
            // 
            SereinPluginsSplitContainer.Panel1.Controls.Add(this.SereinPluginsWebBrowser);
            // 
            // SereinPluginsSplitContainer.Panel2
            // 
            SereinPluginsSplitContainer.Panel2.Controls.Add(this.SereinPluginsList);
            // 
            // SereinPluginsWebBrowser
            // 
            this.SereinPluginsWebBrowser.AllowWebBrowserDrop = false;
            resources.ApplyResources(this.SereinPluginsWebBrowser, "SereinPluginsWebBrowser");
            this.SereinPluginsWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.SereinPluginsWebBrowser.Name = "SereinPluginsWebBrowser";
            this.SereinPluginsWebBrowser.ScriptErrorsSuppressed = true;
            this.SereinPluginsWebBrowser.ScrollBarsEnabled = false;
            this.SereinPluginsWebBrowser.TabStop = false;
            // 
            // SereinPluginsList
            // 
            this.SereinPluginsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SereinPluginsListName,
            this.SereinPluginsListVersion,
            this.SereinPluginsListAuthor,
            this.SereinPluginsListDescription});
            this.SereinPluginsList.ContextMenuStrip = this.SereinPluginsListContextMenuStrip;
            resources.ApplyResources(this.SereinPluginsList, "SereinPluginsList");
            this.SereinPluginsList.FullRowSelect = true;
            this.SereinPluginsList.GridLines = true;
            this.SereinPluginsList.MultiSelect = false;
            this.SereinPluginsList.Name = "SereinPluginsList";
            this.SereinPluginsList.UseCompatibleStateImageBehavior = false;
            this.SereinPluginsList.View = System.Windows.Forms.View.Details;
            // 
            // SereinPluginsListName
            // 
            resources.ApplyResources(this.SereinPluginsListName, "SereinPluginsListName");
            // 
            // SereinPluginsListVersion
            // 
            resources.ApplyResources(this.SereinPluginsListVersion, "SereinPluginsListVersion");
            // 
            // SereinPluginsListAuthor
            // 
            resources.ApplyResources(this.SereinPluginsListAuthor, "SereinPluginsListAuthor");
            // 
            // SereinPluginsListDescription
            // 
            resources.ApplyResources(this.SereinPluginsListDescription, "SereinPluginsListDescription");
            // 
            // SereinPluginsListContextMenuStrip
            // 
            this.SereinPluginsListContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.SereinPluginsListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SereinPluginsListContextMenuStrip_Reload,
            this.SereinPluginsListContextMenuStrip_ClearConsole,
            this.toolStripSeparator9,
            this.SereinPluginsListContextMenuStrip_Docs});
            this.SereinPluginsListContextMenuStrip.Name = "SereinPluginsListContextMenuStrip";
            resources.ApplyResources(this.SereinPluginsListContextMenuStrip, "SereinPluginsListContextMenuStrip");
            // 
            // SereinPluginsListContextMenuStrip_Reload
            // 
            this.SereinPluginsListContextMenuStrip_Reload.Name = "SereinPluginsListContextMenuStrip_Reload";
            resources.ApplyResources(this.SereinPluginsListContextMenuStrip_Reload, "SereinPluginsListContextMenuStrip_Reload");
            this.SereinPluginsListContextMenuStrip_Reload.Click += new System.EventHandler(this.SereinPluginsListContextMenuStrip_Reload_Click);
            // 
            // SereinPluginsListContextMenuStrip_ClearConsole
            // 
            this.SereinPluginsListContextMenuStrip_ClearConsole.Name = "SereinPluginsListContextMenuStrip_ClearConsole";
            resources.ApplyResources(this.SereinPluginsListContextMenuStrip_ClearConsole, "SereinPluginsListContextMenuStrip_ClearConsole");
            this.SereinPluginsListContextMenuStrip_ClearConsole.Click += new System.EventHandler(this.SereinPluginsListContextMenuStrip_ClearConsole_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // SereinPluginsListContextMenuStrip_Docs
            // 
            this.SereinPluginsListContextMenuStrip_Docs.Name = "SereinPluginsListContextMenuStrip_Docs";
            resources.ApplyResources(this.SereinPluginsListContextMenuStrip_Docs, "SereinPluginsListContextMenuStrip_Docs");
            this.SereinPluginsListContextMenuStrip_Docs.Click += new System.EventHandler(this.SereinPluginsListContextMenuStrip_Docs_Click);
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
            this.toolStripSeparator4,
            this.PluginContextMenuStripShow,
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
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // PluginContextMenuStripShow
            // 
            this.PluginContextMenuStripShow.Name = "PluginContextMenuStripShow";
            resources.ApplyResources(this.PluginContextMenuStripShow, "PluginContextMenuStripShow");
            this.PluginContextMenuStripShow.Click += new System.EventHandler(this.PluginContextMenuStripShow_Click);
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
            this.tabControl.Controls.Add(this.Member);
            this.tabControl.Controls.Add(this.SereinPlugins);
            this.tabControl.Controls.Add(this.Setting);
            this.tabControl.Controls.Add(this.Debug);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
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
            this.PanelTableLayout.Controls.Add(PanelInfo, 0, 0);
            this.PanelTableLayout.Controls.Add(PanelControls, 0, 1);
            this.PanelTableLayout.Controls.Add(PanelConsole, 1, 0);
            this.PanelTableLayout.Name = "PanelTableLayout";
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
            this.RegexList.AllowDrop = true;
            this.RegexList.BackColor = System.Drawing.SystemColors.Window;
            this.RegexList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.RegexListRegex,
            this.RegexListArea,
            this.RegexListIsAdmin,
            this.RegexListRemark,
            this.RegexListCommand});
            this.RegexList.ContextMenuStrip = this.RegexContextMenuStrip;
            resources.ApplyResources(this.RegexList, "RegexList");
            this.RegexList.FullRowSelect = true;
            this.RegexList.GridLines = true;
            this.RegexList.MultiSelect = false;
            this.RegexList.Name = "RegexList";
            this.RegexList.UseCompatibleStateImageBehavior = false;
            this.RegexList.View = System.Windows.Forms.View.Details;
            this.RegexList.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.RegexList_ItemDrag);
            this.RegexList.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.RegexList_ItemMouseHover);
            this.RegexList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RegexList_MouseUp);
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
            this.RegexContextMenuStrip_Add,
            this.RegexContextMenuStrip_Edit,
            this.RegexContextMenuStrip_Delete,
            this.RegexContextMenuStrip_Clear,
            this.toolStripSeparator3,
            this.RegexContextMenuStrip_Command,
            this.RegexContextMenuStrip_Variables,
            this.toolStripSeparator7,
            this.RegexContextMenuStripRefresh});
            this.RegexContextMenuStrip.Name = "RegexMenuStrip";
            resources.ApplyResources(this.RegexContextMenuStrip, "RegexContextMenuStrip");
            this.RegexContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.RegexContextMenuStrip_Opening);
            // 
            // RegexContextMenuStrip_Add
            // 
            this.RegexContextMenuStrip_Add.Name = "RegexContextMenuStrip_Add";
            resources.ApplyResources(this.RegexContextMenuStrip_Add, "RegexContextMenuStrip_Add");
            this.RegexContextMenuStrip_Add.Click += new System.EventHandler(this.RegexContextMenuStrip_Add_Click);
            // 
            // RegexContextMenuStrip_Edit
            // 
            this.RegexContextMenuStrip_Edit.Name = "RegexContextMenuStrip_Edit";
            resources.ApplyResources(this.RegexContextMenuStrip_Edit, "RegexContextMenuStrip_Edit");
            this.RegexContextMenuStrip_Edit.Click += new System.EventHandler(this.RegexContextMenuStrip_Edit_Click);
            // 
            // RegexContextMenuStrip_Delete
            // 
            this.RegexContextMenuStrip_Delete.Name = "RegexContextMenuStrip_Delete";
            resources.ApplyResources(this.RegexContextMenuStrip_Delete, "RegexContextMenuStrip_Delete");
            this.RegexContextMenuStrip_Delete.Click += new System.EventHandler(this.RegexContextMenuStrip_Delete_Click);
            // 
            // RegexContextMenuStrip_Clear
            // 
            this.RegexContextMenuStrip_Clear.Name = "RegexContextMenuStrip_Clear";
            resources.ApplyResources(this.RegexContextMenuStrip_Clear, "RegexContextMenuStrip_Clear");
            this.RegexContextMenuStrip_Clear.Click += new System.EventHandler(this.RegexContextMenuStrip_Clear_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // RegexContextMenuStrip_Command
            // 
            this.RegexContextMenuStrip_Command.Name = "RegexContextMenuStrip_Command";
            resources.ApplyResources(this.RegexContextMenuStrip_Command, "RegexContextMenuStrip_Command");
            this.RegexContextMenuStrip_Command.Click += new System.EventHandler(this.RegexContextMenuStrip_Command_Click);
            // 
            // RegexContextMenuStrip_Variables
            // 
            this.RegexContextMenuStrip_Variables.Name = "RegexContextMenuStrip_Variables";
            resources.ApplyResources(this.RegexContextMenuStrip_Variables, "RegexContextMenuStrip_Variables");
            this.RegexContextMenuStrip_Variables.Click += new System.EventHandler(this.RegexContextMenuStrip_Variables_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
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
            this.TaskList.AllowDrop = true;
            this.TaskList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TaskListCron,
            this.TaskListRemark,
            this.TaskListCommand});
            this.TaskList.ContextMenuStrip = this.TaskContextMenuStrip;
            resources.ApplyResources(this.TaskList, "TaskList");
            this.TaskList.FullRowSelect = true;
            this.TaskList.GridLines = true;
            this.TaskList.MultiSelect = false;
            this.TaskList.Name = "TaskList";
            this.TaskList.UseCompatibleStateImageBehavior = false;
            this.TaskList.View = System.Windows.Forms.View.Details;
            this.TaskList.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TaskList_ItemDrag);
            this.TaskList.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.TaskList_ItemMouseHover);
            this.TaskList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TaskList_MouseUp);
            // 
            // TaskListCron
            // 
            resources.ApplyResources(this.TaskListCron, "TaskListCron");
            // 
            // TaskListRemark
            // 
            resources.ApplyResources(this.TaskListRemark, "TaskListRemark");
            // 
            // TaskListCommand
            // 
            resources.ApplyResources(this.TaskListCommand, "TaskListCommand");
            // 
            // TaskContextMenuStrip
            // 
            this.TaskContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.TaskContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TaskContextMenuStrip_Add,
            this.TaskContextMenuStrip_Edit,
            this.TaskContextMenuStrip_Delete,
            this.TaskContextMenuStrip_Clear,
            this.toolStripSeparator6,
            this.TaskContextMenuStrip_Enable,
            this.TaskContextMenuStrip_Disable,
            this.toolStripSeparator8,
            this.TaskContextMenuStrip_Command,
            this.TaskContextMenuStrip_Variables,
            this.toolStripSeparator5,
            this.TaskContextMenuStrip_Refresh});
            this.TaskContextMenuStrip.Name = "TaskContextMenuStrip";
            resources.ApplyResources(this.TaskContextMenuStrip, "TaskContextMenuStrip");
            this.TaskContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.TaskContextMenuStrip_Opening);
            // 
            // TaskContextMenuStrip_Add
            // 
            this.TaskContextMenuStrip_Add.Name = "TaskContextMenuStrip_Add";
            resources.ApplyResources(this.TaskContextMenuStrip_Add, "TaskContextMenuStrip_Add");
            this.TaskContextMenuStrip_Add.Click += new System.EventHandler(this.TaskContextMenuStrip_Add_Click);
            // 
            // TaskContextMenuStrip_Edit
            // 
            this.TaskContextMenuStrip_Edit.Name = "TaskContextMenuStrip_Edit";
            resources.ApplyResources(this.TaskContextMenuStrip_Edit, "TaskContextMenuStrip_Edit");
            this.TaskContextMenuStrip_Edit.Click += new System.EventHandler(this.TaskContextMenuStrip_Edit_Click);
            // 
            // TaskContextMenuStrip_Delete
            // 
            this.TaskContextMenuStrip_Delete.Name = "TaskContextMenuStrip_Delete";
            resources.ApplyResources(this.TaskContextMenuStrip_Delete, "TaskContextMenuStrip_Delete");
            this.TaskContextMenuStrip_Delete.Click += new System.EventHandler(this.TaskContextMenuStrip_Delete_Click);
            // 
            // TaskContextMenuStrip_Clear
            // 
            this.TaskContextMenuStrip_Clear.Name = "TaskContextMenuStrip_Clear";
            resources.ApplyResources(this.TaskContextMenuStrip_Clear, "TaskContextMenuStrip_Clear");
            this.TaskContextMenuStrip_Clear.Click += new System.EventHandler(this.TaskContextMenuStrip_Clear_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // TaskContextMenuStrip_Enable
            // 
            this.TaskContextMenuStrip_Enable.Name = "TaskContextMenuStrip_Enable";
            resources.ApplyResources(this.TaskContextMenuStrip_Enable, "TaskContextMenuStrip_Enable");
            this.TaskContextMenuStrip_Enable.Click += new System.EventHandler(this.TaskContextMenuStrip_Enable_Click);
            // 
            // TaskContextMenuStrip_Disable
            // 
            this.TaskContextMenuStrip_Disable.Name = "TaskContextMenuStrip_Disable";
            resources.ApplyResources(this.TaskContextMenuStrip_Disable, "TaskContextMenuStrip_Disable");
            this.TaskContextMenuStrip_Disable.Click += new System.EventHandler(this.TaskContextMenuStrip_Disable_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // TaskContextMenuStrip_Command
            // 
            this.TaskContextMenuStrip_Command.Name = "TaskContextMenuStrip_Command";
            resources.ApplyResources(this.TaskContextMenuStrip_Command, "TaskContextMenuStrip_Command");
            this.TaskContextMenuStrip_Command.Click += new System.EventHandler(this.TaskContextMenuStrip_Command_Click);
            // 
            // TaskContextMenuStrip_Variables
            // 
            this.TaskContextMenuStrip_Variables.Name = "TaskContextMenuStrip_Variables";
            resources.ApplyResources(this.TaskContextMenuStrip_Variables, "TaskContextMenuStrip_Variables");
            this.TaskContextMenuStrip_Variables.Click += new System.EventHandler(this.TaskContextMenuStrip_Variables_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // TaskContextMenuStrip_Refresh
            // 
            this.TaskContextMenuStrip_Refresh.Name = "TaskContextMenuStrip_Refresh";
            resources.ApplyResources(this.TaskContextMenuStrip_Refresh, "TaskContextMenuStrip_Refresh");
            this.TaskContextMenuStrip_Refresh.Click += new System.EventHandler(this.TaskContextMenuStrip_Refresh_Click);
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
            this.BotTableLayoutPanel.Controls.Add(BotInfo, 0, 0);
            this.BotTableLayoutPanel.Controls.Add(BotWebsocket, 0, 1);
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
            this.BotWebBrowser.TabStop = false;
            // 
            // Member
            // 
            this.Member.Controls.Add(this.MemberList);
            resources.ApplyResources(this.Member, "Member");
            this.Member.Name = "Member";
            this.Member.UseVisualStyleBackColor = true;
            // 
            // MemberList
            // 
            this.MemberList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MemberListID,
            this.MemberListRole,
            this.MemberListNickname,
            this.MemberListCard,
            this.MemberListGameID});
            this.MemberList.ContextMenuStrip = this.MemberContextMenuStrip;
            resources.ApplyResources(this.MemberList, "MemberList");
            this.MemberList.FullRowSelect = true;
            this.MemberList.GridLines = true;
            this.MemberList.Name = "MemberList";
            this.MemberList.UseCompatibleStateImageBehavior = false;
            this.MemberList.View = System.Windows.Forms.View.Details;
            // 
            // MemberListID
            // 
            resources.ApplyResources(this.MemberListID, "MemberListID");
            // 
            // MemberListRole
            // 
            resources.ApplyResources(this.MemberListRole, "MemberListRole");
            // 
            // MemberListNickname
            // 
            resources.ApplyResources(this.MemberListNickname, "MemberListNickname");
            // 
            // MemberListCard
            // 
            resources.ApplyResources(this.MemberListCard, "MemberListCard");
            // 
            // MemberListGameID
            // 
            resources.ApplyResources(this.MemberListGameID, "MemberListGameID");
            // 
            // MemberContextMenuStrip
            // 
            this.MemberContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.MemberContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MemberContextMenuStrip_Edit,
            this.MemberContextMenuStrip_Remove,
            this.MemberContextMenuStripSeparator1,
            this.MemberContextMenuStrip_Refresh});
            this.MemberContextMenuStrip.Name = "PluginContextMenuStrip";
            resources.ApplyResources(this.MemberContextMenuStrip, "MemberContextMenuStrip");
            this.MemberContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.MemberContextMenuStrip_Opening);
            // 
            // MemberContextMenuStrip_Edit
            // 
            this.MemberContextMenuStrip_Edit.Name = "MemberContextMenuStrip_Edit";
            resources.ApplyResources(this.MemberContextMenuStrip_Edit, "MemberContextMenuStrip_Edit");
            this.MemberContextMenuStrip_Edit.Click += new System.EventHandler(this.MemberContextMenuStrip_Edit_Click);
            // 
            // MemberContextMenuStrip_Remove
            // 
            this.MemberContextMenuStrip_Remove.Name = "MemberContextMenuStrip_Remove";
            resources.ApplyResources(this.MemberContextMenuStrip_Remove, "MemberContextMenuStrip_Remove");
            this.MemberContextMenuStrip_Remove.Click += new System.EventHandler(this.MemberContextMenuStrip_Remove_Click);
            // 
            // MemberContextMenuStripSeparator1
            // 
            this.MemberContextMenuStripSeparator1.Name = "MemberContextMenuStripSeparator1";
            resources.ApplyResources(this.MemberContextMenuStripSeparator1, "MemberContextMenuStripSeparator1");
            // 
            // MemberContextMenuStrip_Refresh
            // 
            this.MemberContextMenuStrip_Refresh.Name = "MemberContextMenuStrip_Refresh";
            resources.ApplyResources(this.MemberContextMenuStrip_Refresh, "MemberContextMenuStrip_Refresh");
            this.MemberContextMenuStrip_Refresh.Click += new System.EventHandler(this.MemberContextMenuStrip_Refresh_Click);
            // 
            // SereinPlugins
            // 
            this.SereinPlugins.Controls.Add(SereinPluginsSplitContainer);
            resources.ApplyResources(this.SereinPlugins, "SereinPlugins");
            this.SereinPlugins.Name = "SereinPlugins";
            this.SereinPlugins.UseVisualStyleBackColor = true;
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
            this.SettingPanel.Controls.Add(this.SettingEvent);
            this.SettingPanel.Controls.Add(this.SettingBot);
            this.SettingPanel.Controls.Add(this.SettingServer);
            this.SettingPanel.Name = "SettingPanel";
            // 
            // SettingSerein
            // 
            this.SettingSerein.Controls.Add(this.SettingSereinStatement1);
            this.SettingSerein.Controls.Add(this.SettingSereinShowWelcomePage);
            this.SettingSerein.Controls.Add(this.SettingSereinEnableDPIAware);
            this.SettingSerein.Controls.Add(this.Splitter);
            this.SettingSerein.Controls.Add(this.SettingSereinDownload);
            this.SettingSerein.Controls.Add(this.SettingSereinStatement2);
            this.SettingSerein.Controls.Add(this.SettingSereinStatement);
            this.SettingSerein.Controls.Add(this.SettingSereinPage);
            this.SettingSerein.Controls.Add(this.SettingSereinAbout);
            this.SettingSerein.Controls.Add(this.SettingSereinEnableGetUpdate);
            this.SettingSerein.Controls.Add(this.SettingSereinTutorial);
            this.SettingSerein.Controls.Add(this.SettingSereinVersion);
            resources.ApplyResources(this.SettingSerein, "SettingSerein");
            this.SettingSerein.Name = "SettingSerein";
            this.SettingSerein.TabStop = false;
            // 
            // SettingSereinStatement1
            // 
            resources.ApplyResources(this.SettingSereinStatement1, "SettingSereinStatement1");
            this.SettingSereinStatement1.Name = "SettingSereinStatement1";
            // 
            // SettingSereinShowWelcomePage
            // 
            resources.ApplyResources(this.SettingSereinShowWelcomePage, "SettingSereinShowWelcomePage");
            this.SettingSereinShowWelcomePage.Name = "SettingSereinShowWelcomePage";
            this.SettingSereinShowWelcomePage.UseVisualStyleBackColor = true;
            this.SettingSereinShowWelcomePage.Click += new System.EventHandler(this.SettingSereinShowWelcomePage_Click);
            // 
            // SettingSereinEnableDPIAware
            // 
            resources.ApplyResources(this.SettingSereinEnableDPIAware, "SettingSereinEnableDPIAware");
            this.SettingSereinEnableDPIAware.Name = "SettingSereinEnableDPIAware";
            this.SettingSereinEnableDPIAware.UseVisualStyleBackColor = true;
            this.SettingSereinEnableDPIAware.CheckedChanged += new System.EventHandler(this.SettingSereinEnableDPIAware_CheckedChanged);
            this.SettingSereinEnableDPIAware.MouseHover += new System.EventHandler(this.SettingSereinEnableDPIAware_MouseHover);
            // 
            // Splitter
            // 
            resources.ApplyResources(this.Splitter, "Splitter");
            this.Splitter.Name = "Splitter";
            // 
            // SettingSereinDownload
            // 
            resources.ApplyResources(this.SettingSereinDownload, "SettingSereinDownload");
            this.SettingSereinDownload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SettingSereinDownload.ForeColor = System.Drawing.Color.RoyalBlue;
            this.SettingSereinDownload.Name = "SettingSereinDownload";
            this.SettingSereinDownload.Click += new System.EventHandler(this.SettingSereinDownload_Click);
            this.SettingSereinDownload.MouseHover += new System.EventHandler(this.SettingSereinDownload_MouseHover);
            // 
            // SettingSereinStatement2
            // 
            resources.ApplyResources(this.SettingSereinStatement2, "SettingSereinStatement2");
            this.SettingSereinStatement2.Name = "SettingSereinStatement2";
            // 
            // SettingSereinStatement
            // 
            resources.ApplyResources(this.SettingSereinStatement, "SettingSereinStatement");
            this.SettingSereinStatement.Name = "SettingSereinStatement";
            // 
            // SettingSereinPage
            // 
            resources.ApplyResources(this.SettingSereinPage, "SettingSereinPage");
            this.SettingSereinPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SettingSereinPage.ForeColor = System.Drawing.Color.RoyalBlue;
            this.SettingSereinPage.Name = "SettingSereinPage";
            this.SettingSereinPage.Click += new System.EventHandler(this.SettingSereinPage_Click);
            this.SettingSereinPage.MouseHover += new System.EventHandler(this.SettingSereinPage_MouseHover);
            // 
            // SettingSereinAbout
            // 
            resources.ApplyResources(this.SettingSereinAbout, "SettingSereinAbout");
            this.SettingSereinAbout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SettingSereinAbout.ForeColor = System.Drawing.Color.RoyalBlue;
            this.SettingSereinAbout.Name = "SettingSereinAbout";
            this.SettingSereinAbout.Click += new System.EventHandler(this.SettingSereinAbout_Click);
            this.SettingSereinAbout.MouseHover += new System.EventHandler(this.SettingSereinAbout_MouseHover);
            // 
            // SettingSereinEnableGetUpdate
            // 
            resources.ApplyResources(this.SettingSereinEnableGetUpdate, "SettingSereinEnableGetUpdate");
            this.SettingSereinEnableGetUpdate.Name = "SettingSereinEnableGetUpdate";
            this.SettingSereinEnableGetUpdate.UseVisualStyleBackColor = true;
            this.SettingSereinEnableGetUpdate.CheckedChanged += new System.EventHandler(this.SettingSereinEnableGetUpdate_CheckedChanged);
            this.SettingSereinEnableGetUpdate.MouseHover += new System.EventHandler(this.SettingSereinEnableGetUpdate_MouseHover);
            // 
            // SettingSereinTutorial
            // 
            resources.ApplyResources(this.SettingSereinTutorial, "SettingSereinTutorial");
            this.SettingSereinTutorial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SettingSereinTutorial.ForeColor = System.Drawing.Color.RoyalBlue;
            this.SettingSereinTutorial.Name = "SettingSereinTutorial";
            this.SettingSereinTutorial.Click += new System.EventHandler(this.SettingSereinTutorial_Click);
            this.SettingSereinTutorial.MouseHover += new System.EventHandler(this.SettingSereinTutorial_MouseHover);
            // 
            // SettingSereinVersion
            // 
            resources.ApplyResources(this.SettingSereinVersion, "SettingSereinVersion");
            this.SettingSereinVersion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SettingSereinVersion.Name = "SettingSereinVersion";
            this.SettingSereinVersion.Click += new System.EventHandler(this.SettingSereinVersion_Click);
            // 
            // SettingEvent
            // 
            this.SettingEvent.Controls.Add(this.SettingEventSplitContainer);
            resources.ApplyResources(this.SettingEvent, "SettingEvent");
            this.SettingEvent.Name = "SettingEvent";
            this.SettingEvent.TabStop = false;
            // 
            // SettingEventSplitContainer
            // 
            resources.ApplyResources(this.SettingEventSplitContainer, "SettingEventSplitContainer");
            this.SettingEventSplitContainer.Name = "SettingEventSplitContainer";
            // 
            // SettingEventSplitContainer.Panel1
            // 
            this.SettingEventSplitContainer.Panel1.Controls.Add(this.SettingEventTreeView);
            // 
            // SettingEventSplitContainer.Panel2
            // 
            this.SettingEventSplitContainer.Panel2.Controls.Add(this.SettingEventList);
            // 
            // SettingEventTreeView
            // 
            resources.ApplyResources(this.SettingEventTreeView, "SettingEventTreeView");
            this.SettingEventTreeView.Name = "SettingEventTreeView";
            this.SettingEventTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("SettingEventTreeView.Nodes"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("SettingEventTreeView.Nodes1"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("SettingEventTreeView.Nodes2"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("SettingEventTreeView.Nodes3"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("SettingEventTreeView.Nodes4"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("SettingEventTreeView.Nodes5")))});
            this.SettingEventTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SettingEventTreeView_AfterSelect);
            // 
            // SettingEventList
            // 
            this.SettingEventList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.SettingEventList.ContextMenuStrip = this.SettingEventListContextMenuStrip;
            resources.ApplyResources(this.SettingEventList, "SettingEventList");
            this.SettingEventList.FullRowSelect = true;
            this.SettingEventList.GridLines = true;
            this.SettingEventList.MultiSelect = false;
            this.SettingEventList.Name = "SettingEventList";
            this.SettingEventList.UseCompatibleStateImageBehavior = false;
            this.SettingEventList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // SettingEventListContextMenuStrip
            // 
            this.SettingEventListContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.SettingEventListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SettingEventListContextMenuStrip_Add,
            this.SettingEventListContextMenuStrip_Edit,
            this.SettingEventListContextMenuStrip_Remove,
            this.toolStripSeparator10,
            this.SettingEventListContextMenuStrip_Docs});
            this.SettingEventListContextMenuStrip.Name = "SettingEventListContextMenuStrip";
            resources.ApplyResources(this.SettingEventListContextMenuStrip, "SettingEventListContextMenuStrip");
            this.SettingEventListContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.SettingEventListContextMenuStrip_Opening);
            // 
            // SettingEventListContextMenuStrip_Add
            // 
            this.SettingEventListContextMenuStrip_Add.Name = "SettingEventListContextMenuStrip_Add";
            resources.ApplyResources(this.SettingEventListContextMenuStrip_Add, "SettingEventListContextMenuStrip_Add");
            this.SettingEventListContextMenuStrip_Add.Click += new System.EventHandler(this.SettingEventListContextMenuStrip_Add_Click);
            // 
            // SettingEventListContextMenuStrip_Edit
            // 
            this.SettingEventListContextMenuStrip_Edit.Name = "SettingEventListContextMenuStrip_Edit";
            resources.ApplyResources(this.SettingEventListContextMenuStrip_Edit, "SettingEventListContextMenuStrip_Edit");
            this.SettingEventListContextMenuStrip_Edit.Click += new System.EventHandler(this.SettingEventListContextMenuStrip_Edit_Click);
            // 
            // SettingEventListContextMenuStrip_Remove
            // 
            this.SettingEventListContextMenuStrip_Remove.Name = "SettingEventListContextMenuStrip_Remove";
            resources.ApplyResources(this.SettingEventListContextMenuStrip_Remove, "SettingEventListContextMenuStrip_Remove");
            this.SettingEventListContextMenuStrip_Remove.Click += new System.EventHandler(this.SettingEventListContextMenuStrip_Remove_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            // 
            // SettingEventListContextMenuStrip_Docs
            // 
            this.SettingEventListContextMenuStrip_Docs.Name = "SettingEventListContextMenuStrip_Docs";
            resources.ApplyResources(this.SettingEventListContextMenuStrip_Docs, "SettingEventListContextMenuStrip_Docs");
            this.SettingEventListContextMenuStrip_Docs.Click += new System.EventHandler(this.SettingEventListContextMenuStrip_Docs_Click);
            // 
            // SettingBot
            // 
            this.SettingBot.Controls.Add(this.SettingBotEnbaleParseAt);
            this.SettingBot.Controls.Add(this.SettingBotAutoEscape);
            this.SettingBot.Controls.Add(this.SettingBotRestart);
            this.SettingBot.Controls.Add(this.SettingBotAuthorization);
            this.SettingBot.Controls.Add(this.SettingBotAuthorizationLabel);
            this.SettingBot.Controls.Add(this.SettingBotUri);
            this.SettingBot.Controls.Add(this.SettingBotEnbaleOutputData);
            this.SettingBot.Controls.Add(this.SettingBotPermission);
            this.SettingBot.Controls.Add(this.SettingBotGroup);
            this.SettingBot.Controls.Add(this.SettingBotPermissionList);
            this.SettingBot.Controls.Add(this.SettingBotGroupList);
            this.SettingBot.Controls.Add(this.SettingBotUriLabel);
            this.SettingBot.Controls.Add(this.SettingBotSupportedLabel);
            this.SettingBot.Controls.Add(this.SettingBotGivePermissionToAllAdmin);
            this.SettingBot.Controls.Add(this.SettingBotEnableLog);
            resources.ApplyResources(this.SettingBot, "SettingBot");
            this.SettingBot.Name = "SettingBot";
            this.SettingBot.TabStop = false;
            // 
            // SettingBotEnbaleParseAt
            // 
            resources.ApplyResources(this.SettingBotEnbaleParseAt, "SettingBotEnbaleParseAt");
            this.SettingBotEnbaleParseAt.Name = "SettingBotEnbaleParseAt";
            this.SettingBotEnbaleParseAt.UseVisualStyleBackColor = true;
            this.SettingBotEnbaleParseAt.CheckedChanged += new System.EventHandler(this.SettingBotEnbaleParseAt_CheckedChanged);
            this.SettingBotEnbaleParseAt.MouseHover += new System.EventHandler(this.SettingBotEnbaleParseAt_MouseHover);
            // 
            // SettingBotAutoEscape
            // 
            resources.ApplyResources(this.SettingBotAutoEscape, "SettingBotAutoEscape");
            this.SettingBotAutoEscape.Name = "SettingBotAutoEscape";
            this.SettingBotAutoEscape.UseVisualStyleBackColor = true;
            this.SettingBotAutoEscape.CheckedChanged += new System.EventHandler(this.SettingBotAutoEscape_CheckedChanged);
            this.SettingBotAutoEscape.MouseHover += new System.EventHandler(this.SettingBotAutoEscape_MouseHover);
            // 
            // SettingBotRestart
            // 
            resources.ApplyResources(this.SettingBotRestart, "SettingBotRestart");
            this.SettingBotRestart.Name = "SettingBotRestart";
            this.SettingBotRestart.UseVisualStyleBackColor = true;
            this.SettingBotRestart.CheckedChanged += new System.EventHandler(this.SettingBotRestart_CheckedChanged);
            this.SettingBotRestart.MouseHover += new System.EventHandler(this.SettingBotRestart_MouseHover);
            // 
            // SettingBotAuthorization
            // 
            resources.ApplyResources(this.SettingBotAuthorization, "SettingBotAuthorization");
            this.SettingBotAuthorization.Name = "SettingBotAuthorization";
            this.SettingBotAuthorization.Enter += new System.EventHandler(this.SettingBotAuthorization_Enter);
            this.SettingBotAuthorization.Leave += new System.EventHandler(this.SettingBotAuthorization_Leave);
            this.SettingBotAuthorization.MouseHover += new System.EventHandler(this.SettingBotAuthorization_MouseHover);
            // 
            // SettingBotAuthorizationLabel
            // 
            resources.ApplyResources(this.SettingBotAuthorizationLabel, "SettingBotAuthorizationLabel");
            this.SettingBotAuthorizationLabel.Name = "SettingBotAuthorizationLabel";
            this.SettingBotAuthorizationLabel.MouseHover += new System.EventHandler(this.SettingBotAuthorizationLabel_MouseHover);
            // 
            // SettingBotUri
            // 
            resources.ApplyResources(this.SettingBotUri, "SettingBotUri");
            this.SettingBotUri.Name = "SettingBotUri";
            this.SettingBotUri.TextChanged += new System.EventHandler(this.SettingBotUri_TextChanged);
            this.SettingBotUri.MouseHover += new System.EventHandler(this.SettingBotUri_MouseHover);
            // 
            // SettingBotEnbaleOutputData
            // 
            resources.ApplyResources(this.SettingBotEnbaleOutputData, "SettingBotEnbaleOutputData");
            this.SettingBotEnbaleOutputData.Name = "SettingBotEnbaleOutputData";
            this.SettingBotEnbaleOutputData.UseVisualStyleBackColor = true;
            this.SettingBotEnbaleOutputData.CheckedChanged += new System.EventHandler(this.SettingBotEnbaleOutputData_CheckedChanged);
            this.SettingBotEnbaleOutputData.MouseHover += new System.EventHandler(this.SettingBotEnbaleOutputData_MouseHover);
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
            // SettingBotUriLabel
            // 
            resources.ApplyResources(this.SettingBotUriLabel, "SettingBotUriLabel");
            this.SettingBotUriLabel.Name = "SettingBotUriLabel";
            this.SettingBotUriLabel.MouseHover += new System.EventHandler(this.SettingBotUriLabel_MouseHover);
            // 
            // SettingBotSupportedLabel
            // 
            resources.ApplyResources(this.SettingBotSupportedLabel, "SettingBotSupportedLabel");
            this.SettingBotSupportedLabel.Name = "SettingBotSupportedLabel";
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
            this.SettingServer.Controls.Add(this.SettingServerLineTerminator);
            this.SettingServer.Controls.Add(this.SettingServerLineTerminatorLabel);
            this.SettingServer.Controls.Add(this.SettingServerOutputEncodingLabel);
            this.SettingServer.Controls.Add(this.SettingServerOutputEncoding);
            this.SettingServer.Controls.Add(this.SettingServerTypeLabel);
            this.SettingServer.Controls.Add(this.SettingServerPortLabel);
            this.SettingServer.Controls.Add(this.SettingServerType);
            this.SettingServer.Controls.Add(this.SettingServerPort);
            this.SettingServer.Controls.Add(this.SettingServerOutputStyle);
            this.SettingServer.Controls.Add(this.SettingServerStopCommand);
            this.SettingServer.Controls.Add(this.SettingServerEnableUnicode);
            this.SettingServer.Controls.Add(this.SettingServerEncoding);
            this.SettingServer.Controls.Add(this.SettingServerEncodingLabel);
            this.SettingServer.Controls.Add(this.SettingServerAutoStop);
            this.SettingServer.Controls.Add(this.SettingServerStopCommandLabel);
            this.SettingServer.Controls.Add(this.SettingServerOutputStyleLabel);
            this.SettingServer.Controls.Add(this.SettingServerEnableLog);
            this.SettingServer.Controls.Add(this.SettingServerEnableOutputCommand);
            this.SettingServer.Controls.Add(this.SettingServerEnableRestart);
            this.SettingServer.Controls.Add(this.SettingServerPathLabel);
            this.SettingServer.Controls.Add(this.SettingServerPathSelect);
            this.SettingServer.Controls.Add(this.SettingServerPath);
            resources.ApplyResources(this.SettingServer, "SettingServer");
            this.SettingServer.Name = "SettingServer";
            this.SettingServer.TabStop = false;
            // 
            // SettingServerOutputEncodingLabel
            // 
            resources.ApplyResources(this.SettingServerOutputEncodingLabel, "SettingServerOutputEncodingLabel");
            this.SettingServerOutputEncodingLabel.Name = "SettingServerOutputEncodingLabel";
            this.SettingServerOutputEncodingLabel.MouseHover += new System.EventHandler(this.SettingServerOutputEncodingLabel_MouseHover);
            //
            // SettingServerLineTerminator
            // 
            resources.ApplyResources(this.SettingServerLineTerminator, "SettingServerLineTerminator");
            this.SettingServerLineTerminator.Name = "SettingServerLineTerminator";
            this.SettingServerLineTerminator.TextChanged += new System.EventHandler(this.SettingServerLineTerminator_TextChanged);
            this.SettingServerLineTerminator.MouseHover += new System.EventHandler(this.SettingServerLineTerminator_MouseHover);
            // 
            // SettingServerLineTerminatorLabel
            // 
            resources.ApplyResources(this.SettingServerLineTerminatorLabel, "SettingServerLineTerminatorLabel");
            this.SettingServerLineTerminatorLabel.Name = "SettingServerLineTerminatorLabel";
            this.SettingServerLineTerminatorLabel.MouseHover += new System.EventHandler(this.SettingServerLineTerminatorLabel_MouseHover);
            // 
            // SettingServerOutputEncoding
            // 
            this.SettingServerOutputEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SettingServerOutputEncoding.FormattingEnabled = true;
            this.SettingServerOutputEncoding.Items.AddRange(new object[] {
            resources.GetString("SettingServerOutputEncoding.Items"),
            resources.GetString("SettingServerOutputEncoding.Items1"),
            resources.GetString("SettingServerOutputEncoding.Items2"),
            resources.GetString("SettingServerOutputEncoding.Items3"),
            resources.GetString("SettingServerOutputEncoding.Items4"),
            resources.GetString("SettingServerOutputEncoding.Items5"),
            resources.GetString("SettingServerOutputEncoding.Items6")});
            resources.ApplyResources(this.SettingServerOutputEncoding, "SettingServerOutputEncoding");
            this.SettingServerOutputEncoding.Name = "SettingServerOutputEncoding";
            this.SettingServerOutputEncoding.SelectedIndexChanged += new System.EventHandler(this.SettingServerOutputEncoding_SelectedIndexChanged);
            this.SettingServerOutputEncoding.MouseHover += new System.EventHandler(this.SettingServerOutputEncoding_MouseHover);
            // 
            // SettingServerTypeLabel
            // 
            resources.ApplyResources(this.SettingServerTypeLabel, "SettingServerTypeLabel");
            this.SettingServerTypeLabel.Name = "SettingServerTypeLabel";
            this.SettingServerTypeLabel.MouseHover += new System.EventHandler(this.SettingServerTypeLabel_MouseHover);
            // 
            // SettingServerPortLabel
            // 
            resources.ApplyResources(this.SettingServerPortLabel, "SettingServerPortLabel");
            this.SettingServerPortLabel.Name = "SettingServerPortLabel";
            this.SettingServerPortLabel.MouseHover += new System.EventHandler(this.SettingServerPortLabel_MouseHover);
            // 
            // SettingServerType
            // 
            this.SettingServerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SettingServerType.FormattingEnabled = true;
            this.SettingServerType.Items.AddRange(new object[] {
            resources.GetString("SettingServerType.Items"),
            resources.GetString("SettingServerType.Items1"),
            resources.GetString("SettingServerType.Items2")});
            resources.ApplyResources(this.SettingServerType, "SettingServerType");
            this.SettingServerType.Name = "SettingServerType";
            this.SettingServerType.SelectedIndexChanged += new System.EventHandler(this.SettingServerType_SelectedIndexChanged);
            this.SettingServerType.MouseHover += new System.EventHandler(this.SettingServerType_MouseHover);
            // 
            // SettingServerPort
            // 
            resources.ApplyResources(this.SettingServerPort, "SettingServerPort");
            this.SettingServerPort.Maximum = new decimal(new int[] {
            65565,
            0,
            0,
            0});
            this.SettingServerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SettingServerPort.Name = "SettingServerPort";
            this.SettingServerPort.Value = new decimal(new int[] {
            19132,
            0,
            0,
            0});
            this.SettingServerPort.ValueChanged += new System.EventHandler(this.SettingServerPort_ValueChanged);
            // 
            // SettingServerOutputStyle
            // 
            this.SettingServerOutputStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SettingServerOutputStyle.FormattingEnabled = true;
            this.SettingServerOutputStyle.Items.AddRange(new object[] {
            resources.GetString("SettingServerOutputStyle.Items"),
            resources.GetString("SettingServerOutputStyle.Items1"),
            resources.GetString("SettingServerOutputStyle.Items2"),
            resources.GetString("SettingServerOutputStyle.Items3")});
            resources.ApplyResources(this.SettingServerOutputStyle, "SettingServerOutputStyle");
            this.SettingServerOutputStyle.Name = "SettingServerOutputStyle";
            this.SettingServerOutputStyle.SelectedIndexChanged += new System.EventHandler(this.SettingServerOutputStyle_SelectedIndexChanged);
            this.SettingServerOutputStyle.MouseHover += new System.EventHandler(this.SettingServerOutputStyle_MouseHover);
            // 
            // SettingServerStopCommand
            // 
            resources.ApplyResources(this.SettingServerStopCommand, "SettingServerStopCommand");
            this.SettingServerStopCommand.Name = "SettingServerStopCommand";
            this.SettingServerStopCommand.TextChanged += new System.EventHandler(this.SettingServerStopCommand_TextChanged);
            this.SettingServerStopCommand.Leave += new System.EventHandler(this.SettingServerStopCommand_Leave);
            this.SettingServerStopCommand.MouseHover += new System.EventHandler(this.SettingServerStopCommand_MouseHover);
            // 
            // SettingServerEnableUnicode
            // 
            resources.ApplyResources(this.SettingServerEnableUnicode, "SettingServerEnableUnicode");
            this.SettingServerEnableUnicode.Name = "SettingServerEnableUnicode";
            this.SettingServerEnableUnicode.UseVisualStyleBackColor = true;
            this.SettingServerEnableUnicode.CheckedChanged += new System.EventHandler(this.SettingServerEnableUnicode_CheckedChanged);
            this.SettingServerEnableUnicode.MouseHover += new System.EventHandler(this.SettingServerEnableUnicode_MouseHover);
            // 
            // SettingServerEncoding
            // 
            this.SettingServerEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SettingServerEncoding.FormattingEnabled = true;
            this.SettingServerEncoding.Items.AddRange(new object[] {
            resources.GetString("SettingServerEncoding.Items"),
            resources.GetString("SettingServerEncoding.Items1"),
            resources.GetString("SettingServerEncoding.Items2"),
            resources.GetString("SettingServerEncoding.Items3"),
            resources.GetString("SettingServerEncoding.Items4"),
            resources.GetString("SettingServerEncoding.Items5"),
            resources.GetString("SettingServerEncoding.Items6")});
            resources.ApplyResources(this.SettingServerEncoding, "SettingServerEncoding");
            this.SettingServerEncoding.Name = "SettingServerEncoding";
            this.SettingServerEncoding.SelectedValueChanged += new System.EventHandler(this.SettingServerEncoding_SelectedValueChanged);
            this.SettingServerEncoding.MouseHover += new System.EventHandler(this.SettingServerEncoding_MouseHover);
            // 
            // SettingServerEncodingLabel
            // 
            resources.ApplyResources(this.SettingServerEncodingLabel, "SettingServerEncodingLabel");
            this.SettingServerEncodingLabel.Name = "SettingServerEncodingLabel";
            this.SettingServerEncodingLabel.MouseHover += new System.EventHandler(this.SettingServerEncodingLabel_MouseHover);
            // 
            // SettingServerAutoStop
            // 
            resources.ApplyResources(this.SettingServerAutoStop, "SettingServerAutoStop");
            this.SettingServerAutoStop.Name = "SettingServerAutoStop";
            this.SettingServerAutoStop.UseVisualStyleBackColor = true;
            this.SettingServerAutoStop.CheckedChanged += new System.EventHandler(this.SettingServerAutoStop_CheckedChanged);
            this.SettingServerAutoStop.MouseHover += new System.EventHandler(this.SettingServerAutoStop_MouseHover);
            // 
            // SettingServerStopCommandLabel
            // 
            resources.ApplyResources(this.SettingServerStopCommandLabel, "SettingServerStopCommandLabel");
            this.SettingServerStopCommandLabel.Name = "SettingServerStopCommandLabel";
            this.SettingServerStopCommandLabel.MouseHover += new System.EventHandler(this.SettingServerStopCommandLabel_MouseHover);
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
            this.SettingServerPathSelect.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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
            // Debug
            // 
            this.Debug.Controls.Add(this.DebugTextBox);
            resources.ApplyResources(this.Debug, "Debug");
            this.Debug.Name = "Debug";
            this.Debug.UseVisualStyleBackColor = true;
            // 
            // DebugTextBox
            // 
            resources.ApplyResources(this.DebugTextBox, "DebugTextBox");
            this.DebugTextBox.Name = "DebugTextBox";
            // 
            // StatusStrip
            // 
            this.StatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripStatusLabel});
            resources.ApplyResources(this.StatusStrip, "StatusStrip");
            this.StatusStrip.Name = "StatusStrip";
            // 
            // StripStatusLabel
            // 
            this.StripStatusLabel.Name = "StripStatusLabel";
            resources.ApplyResources(this.StripStatusLabel, "StripStatusLabel");
            // 
            // Ui
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.MainTableLayout);
            this.Controls.Add(this.StatusStrip);
            this.Name = "Ui";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Ui_FormClosing);
            this.Shown += new System.EventHandler(this.Ui_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Ui_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Ui_DragEnter);
            PanelInfo.ResumeLayout(false);
            PanelInfo.PerformLayout();
            PanelControls.ResumeLayout(false);
            PanelConsole.ResumeLayout(false);
            this.PanelConsolePanel.ResumeLayout(false);
            this.PanelConsolePanel.PerformLayout();
            BotInfo.ResumeLayout(false);
            BotInfo.PerformLayout();
            BotWebsocket.ResumeLayout(false);
            SereinPluginsSplitContainer.Panel1.ResumeLayout(false);
            SereinPluginsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(SereinPluginsSplitContainer)).EndInit();
            SereinPluginsSplitContainer.ResumeLayout(false);
            this.SereinPluginsListContextMenuStrip.ResumeLayout(false);
            this.PluginContextMenuStrip.ResumeLayout(false);
            this.MainTableLayout.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.Panel.ResumeLayout(false);
            this.PanelTableLayout.ResumeLayout(false);
            this.PanelTableLayout.PerformLayout();
            this.Plugin.ResumeLayout(false);
            this.Regular.ResumeLayout(false);
            this.RegexContextMenuStrip.ResumeLayout(false);
            this.Task.ResumeLayout(false);
            this.TaskContextMenuStrip.ResumeLayout(false);
            this.Bot.ResumeLayout(false);
            this.BotTableLayoutPanel.ResumeLayout(false);
            this.Member.ResumeLayout(false);
            this.MemberContextMenuStrip.ResumeLayout(false);
            this.SereinPlugins.ResumeLayout(false);
            this.Setting.ResumeLayout(false);
            this.SettingPanel.ResumeLayout(false);
            this.SettingSerein.ResumeLayout(false);
            this.SettingSerein.PerformLayout();
            this.SettingEvent.ResumeLayout(false);
            this.SettingEventSplitContainer.Panel1.ResumeLayout(false);
            this.SettingEventSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SettingEventSplitContainer)).EndInit();
            this.SettingEventSplitContainer.ResumeLayout(false);
            this.SettingEventListContextMenuStrip.ResumeLayout(false);
            this.SettingBot.ResumeLayout(false);
            this.SettingBot.PerformLayout();
            this.SettingServer.ResumeLayout(false);
            this.SettingServer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SettingServerPort)).EndInit();
            this.Debug.ResumeLayout(false);
            this.Debug.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Label PanelInfoCPU2;
        private System.Windows.Forms.Label PanelInfoLevel2;
        private System.Windows.Forms.Label PanelInfoTime2;
        private System.Windows.Forms.Label PanelInfoDifficulty2;
        private System.Windows.Forms.Label PanelInfoVersion2;
        private System.Windows.Forms.Label PanelInfoStatus2;
        private System.Windows.Forms.Button PanelControlKill;
        private System.Windows.Forms.Button PanelControlRestart;
        private System.Windows.Forms.Button PanelControlStop;
        private System.Windows.Forms.Button PanelControlStart;
        private System.Windows.Forms.Panel PanelConsolePanel;
        private System.Windows.Forms.WebBrowser PanelConsoleWebBrowser;
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
        private System.Windows.Forms.Button BotClose;
        private System.Windows.Forms.Button BotConnect;
        private System.Windows.Forms.TabPage Setting;
        private System.Windows.Forms.Panel SettingPanel;
        private System.Windows.Forms.GroupBox SettingSerein;
        private System.Windows.Forms.Label SettingSereinVersion;
        private System.Windows.Forms.GroupBox SettingBot;
        private System.Windows.Forms.Label SettingBotPermission;
        private System.Windows.Forms.Label SettingBotGroup;
        private System.Windows.Forms.TextBox SettingBotPermissionList;
        private System.Windows.Forms.TextBox SettingBotGroupList;
        private System.Windows.Forms.Label SettingBotUriLabel;
        private System.Windows.Forms.Label SettingBotSupportedLabel;
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
        private System.Windows.Forms.ToolStripMenuItem RegexContextMenuStrip_Add;
        private System.Windows.Forms.ToolStripMenuItem RegexContextMenuStrip_Delete;
        private System.Windows.Forms.ToolStripMenuItem RegexContextMenuStrip_Clear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem RegexContextMenuStripRefresh;
        private System.Windows.Forms.ColumnHeader RegexListArea;
        private System.Windows.Forms.ColumnHeader RegexListRegex;
        private System.Windows.Forms.ColumnHeader RegexListRemark;
        private System.Windows.Forms.ColumnHeader RegexListCommand;
        private System.Windows.Forms.ColumnHeader RegexListIsAdmin;
        private System.Windows.Forms.ToolStripMenuItem RegexContextMenuStrip_Edit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem PluginContextMenuStripShow;
        private System.Windows.Forms.ColumnHeader TaskListCron;
        private System.Windows.Forms.ColumnHeader TaskListRemark;
        private System.Windows.Forms.ColumnHeader TaskListCommand;
        private System.Windows.Forms.ContextMenuStrip TaskContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem TaskContextMenuStrip_Add;
        private System.Windows.Forms.ToolStripMenuItem TaskContextMenuStrip_Delete;
        private System.Windows.Forms.ToolStripMenuItem TaskContextMenuStrip_Clear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem TaskContextMenuStrip_Refresh;
        private System.Windows.Forms.ToolStripMenuItem TaskContextMenuStrip_Edit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem TaskContextMenuStrip_Enable;
        private System.Windows.Forms.ToolStripMenuItem TaskContextMenuStrip_Disable;
        private System.Windows.Forms.Label SettingSereinAbout;
        private System.Windows.Forms.Label SettingSereinPage;
        private System.Windows.Forms.Label BotInfoTime2;
        private System.Windows.Forms.Label BotInfoMessageSent2;
        private System.Windows.Forms.Label BotInfoMessageReceived2;
        private System.Windows.Forms.Label BotInfoQQ2;
        private System.Windows.Forms.Label BotInfoStatus2;
        private System.Windows.Forms.CheckBox SettingServerAutoStop;
        private System.Windows.Forms.Label SettingServerStopCommandLabel;
        private System.Windows.Forms.TextBox SettingServerStopCommand;
        private System.Windows.Forms.CheckBox SettingBotEnbaleOutputData;
        private System.Windows.Forms.TabPage Debug;
        private System.Windows.Forms.TextBox DebugTextBox;
        private System.Windows.Forms.Label SettingSereinTutorial;
        private System.Windows.Forms.ToolStripMenuItem RegexContextMenuStrip_Variables;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem TaskContextMenuStrip_Variables;
        private System.Windows.Forms.ToolStripMenuItem TaskContextMenuStrip_Command;
        private System.Windows.Forms.ToolStripMenuItem RegexContextMenuStrip_Command;
        private System.Windows.Forms.CheckBox SettingSereinEnableGetUpdate;
        private System.Windows.Forms.Label SettingSereinStatement2;
        private System.Windows.Forms.Label SettingSereinStatement;
        private System.Windows.Forms.Label SettingSereinDownload;
        private System.Windows.Forms.Label Splitter;
        private System.Windows.Forms.TextBox SettingBotAuthorization;
        private System.Windows.Forms.Label SettingBotAuthorizationLabel;
        private System.Windows.Forms.TextBox SettingBotUri;
        private System.Windows.Forms.ComboBox SettingServerEncoding;
        private System.Windows.Forms.Label SettingServerEncodingLabel;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StripStatusLabel;
        private System.Windows.Forms.CheckBox SettingServerEnableUnicode;
        private System.Windows.Forms.CheckBox SettingSereinEnableDPIAware;
        private System.Windows.Forms.Button PanelConsoleEnter;
        private System.Windows.Forms.TextBox PanelConsoleInput;
        private System.Windows.Forms.Label SettingSereinStatement1;
        private System.Windows.Forms.Button SettingSereinShowWelcomePage;
        private System.Windows.Forms.TabPage Member;
        private System.Windows.Forms.ListView MemberList;
        private System.Windows.Forms.ColumnHeader MemberListID;
        private System.Windows.Forms.ColumnHeader MemberListNickname;
        private System.Windows.Forms.ColumnHeader MemberListGameID;
        private System.Windows.Forms.ColumnHeader MemberListRole;
        private System.Windows.Forms.ColumnHeader MemberListCard;
        private System.Windows.Forms.ContextMenuStrip MemberContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem MemberContextMenuStrip_Refresh;
        private System.Windows.Forms.ToolStripMenuItem MemberContextMenuStrip_Edit;
        private System.Windows.Forms.ToolStripMenuItem MemberContextMenuStrip_Remove;
        private System.Windows.Forms.ToolStripSeparator MemberContextMenuStripSeparator1;
        private System.Windows.Forms.Label SettingServerTypeLabel;
        private System.Windows.Forms.Label SettingServerPortLabel;
        private System.Windows.Forms.ComboBox SettingServerType;
        private System.Windows.Forms.NumericUpDown SettingServerPort;
        private System.Windows.Forms.CheckBox SettingBotRestart;
        private System.Windows.Forms.TabPage SereinPlugins;
        private System.Windows.Forms.WebBrowser SereinPluginsWebBrowser;
        private System.Windows.Forms.ListView SereinPluginsList;
        private System.Windows.Forms.ColumnHeader SereinPluginsListName;
        private System.Windows.Forms.ColumnHeader SereinPluginsListVersion;
        private System.Windows.Forms.ColumnHeader SereinPluginsListAuthor;
        private System.Windows.Forms.ColumnHeader SereinPluginsListDescription;
        private System.Windows.Forms.ContextMenuStrip SereinPluginsListContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem SereinPluginsListContextMenuStrip_Reload;
        private System.Windows.Forms.CheckBox SettingBotAutoEscape;
        private System.Windows.Forms.ToolStripMenuItem SereinPluginsListContextMenuStrip_ClearConsole;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem SereinPluginsListContextMenuStrip_Docs;
        private System.Windows.Forms.GroupBox SettingEvent;
        private System.Windows.Forms.TreeView SettingEventTreeView;
        private System.Windows.Forms.ListView SettingEventList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.SplitContainer SettingEventSplitContainer;
        private System.Windows.Forms.ContextMenuStrip SettingEventListContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem SettingEventListContextMenuStrip_Add;
        private System.Windows.Forms.ToolStripMenuItem SettingEventListContextMenuStrip_Edit;
        private System.Windows.Forms.ToolStripMenuItem SettingEventListContextMenuStrip_Remove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem SettingEventListContextMenuStrip_Docs;
        private System.Windows.Forms.ComboBox SettingServerOutputEncoding;
        private System.Windows.Forms.Label SettingServerOutputEncodingLabel;
        private System.Windows.Forms.CheckBox SettingBotEnbaleParseAt;
        private System.Windows.Forms.TextBox SettingServerLineTerminator;
        private System.Windows.Forms.Label SettingServerLineTerminatorLabel;
    }
}

