
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
            System.Windows.Forms.GroupBox ServerPanelInfo;
            System.Windows.Forms.Label ServerPanelInfoCPU;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ui));
            System.Windows.Forms.Label ServerPanelInfoTime;
            System.Windows.Forms.Label ServerPanelInfoPlayerCount;
            System.Windows.Forms.Label ServerPanelInfoDifficulty;
            System.Windows.Forms.Label ServerPanelInfoVersion;
            System.Windows.Forms.Label ServerPanelInfoStatus;
            System.Windows.Forms.GroupBox ServerPanelControls;
            System.Windows.Forms.GroupBox ServerPanelConsole;
            System.Windows.Forms.GroupBox BotInfo;
            System.Windows.Forms.Label BotInfoStatus;
            System.Windows.Forms.Label BotInfoTime;
            System.Windows.Forms.Label BotInfoMessageSent;
            System.Windows.Forms.Label BotInfoMessageReceived;
            System.Windows.Forms.Label BotInfoQQ;
            System.Windows.Forms.GroupBox BotWebsocket;
            System.Windows.Forms.SplitContainer JSPluginSplitContainer;
            this.ServerPanelInfoCPU2 = new System.Windows.Forms.Label();
            this.ServerPanelInfoPlayerCount2 = new System.Windows.Forms.Label();
            this.ServerPanelInfoTime2 = new System.Windows.Forms.Label();
            this.ServerPanelInfoDifficulty2 = new System.Windows.Forms.Label();
            this.ServerPanelInfoVersion2 = new System.Windows.Forms.Label();
            this.ServerPanelInfoStatus2 = new System.Windows.Forms.Label();
            this.ServerPanelControlKill = new System.Windows.Forms.Button();
            this.ServerPanelControlRestart = new System.Windows.Forms.Button();
            this.ServerPanelControlStop = new System.Windows.Forms.Button();
            this.ServerPanelControlStart = new System.Windows.Forms.Button();
            this.ServerPanelConsoleServerPanel = new System.Windows.Forms.Panel();
            this.ServerPanelConsoleEnter = new System.Windows.Forms.Button();
            this.ServerPanelConsoleWebBrowser = new System.Windows.Forms.WebBrowser();
            this.ServerPanelConsoleInput = new System.Windows.Forms.TextBox();
            this.BotInfoStatus2 = new System.Windows.Forms.Label();
            this.BotInfoMessageReceived2 = new System.Windows.Forms.Label();
            this.BotInfoMessageSent2 = new System.Windows.Forms.Label();
            this.BotInfoTime2 = new System.Windows.Forms.Label();
            this.BotInfoQQ2 = new System.Windows.Forms.Label();
            this.BotClose = new System.Windows.Forms.Button();
            this.BotConnect = new System.Windows.Forms.Button();
            this.JSPluginWebBrowser = new System.Windows.Forms.WebBrowser();
            this.JSPluginList = new System.Windows.Forms.ListView();
            this.JSPluginListName = new System.Windows.Forms.ColumnHeader();
            this.JSPluginListVersion = new System.Windows.Forms.ColumnHeader();
            this.JSPluginListAuthor = new System.Windows.Forms.ColumnHeader();
            this.JSPluginListDescription = new System.Windows.Forms.ColumnHeader();
            this.JSPluginListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.JSPluginListContextMenuStrip_Disable = new System.Windows.Forms.ToolStripMenuItem();
            this.JSPluginListContextMenuStrip_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.JSPluginListContextMenuStrip_ClearConsole = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.JSPluginListContextMenuStrip_Docs = new System.Windows.Forms.ToolStripMenuItem();
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
            this.ServerPanel = new System.Windows.Forms.TabPage();
            this.ServerPanelTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.ServerPluginManager = new System.Windows.Forms.TabPage();
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
            this.JSPlugin = new System.Windows.Forms.TabPage();
            this.Setting = new System.Windows.Forms.TabPage();
            this.SettingServerPanel = new System.Windows.Forms.Panel();
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
            this.SettingSereinExtension = new System.Windows.Forms.Label();
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
            ServerPanelInfo = new System.Windows.Forms.GroupBox();
            ServerPanelInfoCPU = new System.Windows.Forms.Label();
            ServerPanelInfoTime = new System.Windows.Forms.Label();
            ServerPanelInfoPlayerCount = new System.Windows.Forms.Label();
            ServerPanelInfoDifficulty = new System.Windows.Forms.Label();
            ServerPanelInfoVersion = new System.Windows.Forms.Label();
            ServerPanelInfoStatus = new System.Windows.Forms.Label();
            ServerPanelControls = new System.Windows.Forms.GroupBox();
            ServerPanelConsole = new System.Windows.Forms.GroupBox();
            BotInfo = new System.Windows.Forms.GroupBox();
            BotInfoStatus = new System.Windows.Forms.Label();
            BotInfoTime = new System.Windows.Forms.Label();
            BotInfoMessageSent = new System.Windows.Forms.Label();
            BotInfoMessageReceived = new System.Windows.Forms.Label();
            BotInfoQQ = new System.Windows.Forms.Label();
            BotWebsocket = new System.Windows.Forms.GroupBox();
            JSPluginSplitContainer = new System.Windows.Forms.SplitContainer();
            ServerPanelInfo.SuspendLayout();
            ServerPanelControls.SuspendLayout();
            ServerPanelConsole.SuspendLayout();
            this.ServerPanelConsoleServerPanel.SuspendLayout();
            BotInfo.SuspendLayout();
            BotWebsocket.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(JSPluginSplitContainer)).BeginInit();
            JSPluginSplitContainer.Panel1.SuspendLayout();
            JSPluginSplitContainer.Panel2.SuspendLayout();
            JSPluginSplitContainer.SuspendLayout();
            this.JSPluginListContextMenuStrip.SuspendLayout();
            this.PluginContextMenuStrip.SuspendLayout();
            this.MainTableLayout.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.ServerPanel.SuspendLayout();
            this.ServerPanelTableLayout.SuspendLayout();
            this.ServerPluginManager.SuspendLayout();
            this.Regular.SuspendLayout();
            this.RegexContextMenuStrip.SuspendLayout();
            this.Task.SuspendLayout();
            this.TaskContextMenuStrip.SuspendLayout();
            this.Bot.SuspendLayout();
            this.BotTableLayoutPanel.SuspendLayout();
            this.Member.SuspendLayout();
            this.MemberContextMenuStrip.SuspendLayout();
            this.JSPlugin.SuspendLayout();
            this.Setting.SuspendLayout();
            this.SettingServerPanel.SuspendLayout();
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
            // ServerPanelInfo
            // 
            ServerPanelInfo.Controls.Add(ServerPanelInfoCPU);
            ServerPanelInfo.Controls.Add(ServerPanelInfoTime);
            ServerPanelInfo.Controls.Add(this.ServerPanelInfoCPU2);
            ServerPanelInfo.Controls.Add(this.ServerPanelInfoPlayerCount2);
            ServerPanelInfo.Controls.Add(this.ServerPanelInfoTime2);
            ServerPanelInfo.Controls.Add(this.ServerPanelInfoDifficulty2);
            ServerPanelInfo.Controls.Add(this.ServerPanelInfoVersion2);
            ServerPanelInfo.Controls.Add(ServerPanelInfoPlayerCount);
            ServerPanelInfo.Controls.Add(ServerPanelInfoDifficulty);
            ServerPanelInfo.Controls.Add(ServerPanelInfoVersion);
            ServerPanelInfo.Controls.Add(this.ServerPanelInfoStatus2);
            ServerPanelInfo.Controls.Add(ServerPanelInfoStatus);
            resources.ApplyResources(ServerPanelInfo, "ServerPanelInfo");
            ServerPanelInfo.Name = "ServerPanelInfo";
            ServerPanelInfo.TabStop = false;
            // 
            // ServerPanelInfoCPU
            // 
            resources.ApplyResources(ServerPanelInfoCPU, "ServerPanelInfoCPU");
            ServerPanelInfoCPU.Name = "ServerPanelInfoCPU";
            // 
            // ServerPanelInfoTime
            // 
            resources.ApplyResources(ServerPanelInfoTime, "ServerPanelInfoTime");
            ServerPanelInfoTime.Name = "ServerPanelInfoTime";
            // 
            // ServerPanelInfoCPU2
            // 
            resources.ApplyResources(this.ServerPanelInfoCPU2, "ServerPanelInfoCPU2");
            this.ServerPanelInfoCPU2.Name = "ServerPanelInfoCPU2";
            // 
            // ServerPanelInfoPlayerCount2
            // 
            resources.ApplyResources(this.ServerPanelInfoPlayerCount2, "ServerPanelInfoPlayerCount2");
            this.ServerPanelInfoPlayerCount2.Name = "ServerPanelInfoPlayerCount2";
            // 
            // ServerPanelInfoTime2
            // 
            resources.ApplyResources(this.ServerPanelInfoTime2, "ServerPanelInfoTime2");
            this.ServerPanelInfoTime2.Name = "ServerPanelInfoTime2";
            // 
            // ServerPanelInfoDifficulty2
            // 
            resources.ApplyResources(this.ServerPanelInfoDifficulty2, "ServerPanelInfoDifficulty2");
            this.ServerPanelInfoDifficulty2.Name = "ServerPanelInfoDifficulty2";
            // 
            // ServerPanelInfoVersion2
            // 
            resources.ApplyResources(this.ServerPanelInfoVersion2, "ServerPanelInfoVersion2");
            this.ServerPanelInfoVersion2.Name = "ServerPanelInfoVersion2";
            // 
            // ServerPanelInfoPlayerCount
            // 
            resources.ApplyResources(ServerPanelInfoPlayerCount, "ServerPanelInfoPlayerCount");
            ServerPanelInfoPlayerCount.Name = "ServerPanelInfoPlayerCount";
            // 
            // ServerPanelInfoDifficulty
            // 
            resources.ApplyResources(ServerPanelInfoDifficulty, "ServerPanelInfoDifficulty");
            ServerPanelInfoDifficulty.Name = "ServerPanelInfoDifficulty";
            // 
            // ServerPanelInfoVersion
            // 
            resources.ApplyResources(ServerPanelInfoVersion, "ServerPanelInfoVersion");
            ServerPanelInfoVersion.Name = "ServerPanelInfoVersion";
            // 
            // ServerPanelInfoStatus2
            // 
            resources.ApplyResources(this.ServerPanelInfoStatus2, "ServerPanelInfoStatus2");
            this.ServerPanelInfoStatus2.Name = "ServerPanelInfoStatus2";
            // 
            // ServerPanelInfoStatus
            // 
            resources.ApplyResources(ServerPanelInfoStatus, "ServerPanelInfoStatus");
            ServerPanelInfoStatus.Name = "ServerPanelInfoStatus";
            // 
            // ServerPanelControls
            // 
            ServerPanelControls.Controls.Add(this.ServerPanelControlKill);
            ServerPanelControls.Controls.Add(this.ServerPanelControlRestart);
            ServerPanelControls.Controls.Add(this.ServerPanelControlStop);
            ServerPanelControls.Controls.Add(this.ServerPanelControlStart);
            resources.ApplyResources(ServerPanelControls, "ServerPanelControls");
            ServerPanelControls.Name = "ServerPanelControls";
            ServerPanelControls.TabStop = false;
            // 
            // ServerPanelControlKill
            // 
            resources.ApplyResources(this.ServerPanelControlKill, "ServerPanelControlKill");
            this.ServerPanelControlKill.Name = "ServerPanelControlKill";
            this.ServerPanelControlKill.UseVisualStyleBackColor = true;
            this.ServerPanelControlKill.Click += new System.EventHandler(this.ServerPanelControlKill_Click);
            // 
            // ServerPanelControlRestart
            // 
            resources.ApplyResources(this.ServerPanelControlRestart, "ServerPanelControlRestart");
            this.ServerPanelControlRestart.Name = "ServerPanelControlRestart";
            this.ServerPanelControlRestart.UseVisualStyleBackColor = true;
            this.ServerPanelControlRestart.Click += new System.EventHandler(this.ServerPanelControlRestart_Click);
            // 
            // ServerPanelControlStop
            // 
            resources.ApplyResources(this.ServerPanelControlStop, "ServerPanelControlStop");
            this.ServerPanelControlStop.Name = "ServerPanelControlStop";
            this.ServerPanelControlStop.UseVisualStyleBackColor = true;
            this.ServerPanelControlStop.Click += new System.EventHandler(this.ServerPanelControlStop_Click);
            // 
            // ServerPanelControlStart
            // 
            resources.ApplyResources(this.ServerPanelControlStart, "ServerPanelControlStart");
            this.ServerPanelControlStart.Name = "ServerPanelControlStart";
            this.ServerPanelControlStart.UseVisualStyleBackColor = true;
            this.ServerPanelControlStart.Click += new System.EventHandler(this.ServerPanelControlStart_Click);
            // 
            // ServerPanelConsole
            // 
            resources.ApplyResources(ServerPanelConsole, "ServerPanelConsole");
            ServerPanelConsole.Controls.Add(this.ServerPanelConsoleServerPanel);
            ServerPanelConsole.Name = "ServerPanelConsole";
            this.ServerPanelTableLayout.SetRowSpan(ServerPanelConsole, 2);
            ServerPanelConsole.TabStop = false;
            // 
            // ServerPanelConsoleServerPanel
            // 
            this.ServerPanelConsoleServerPanel.Controls.Add(this.ServerPanelConsoleEnter);
            this.ServerPanelConsoleServerPanel.Controls.Add(this.ServerPanelConsoleWebBrowser);
            this.ServerPanelConsoleServerPanel.Controls.Add(this.ServerPanelConsoleInput);
            resources.ApplyResources(this.ServerPanelConsoleServerPanel, "ServerPanelConsoleServerPanel");
            this.ServerPanelConsoleServerPanel.Name = "ServerPanelConsoleServerPanel";
            // 
            // ServerPanelConsoleEnter
            // 
            resources.ApplyResources(this.ServerPanelConsoleEnter, "ServerPanelConsoleEnter");
            this.ServerPanelConsoleEnter.Name = "ServerPanelConsoleEnter";
            this.ServerPanelConsoleEnter.UseVisualStyleBackColor = true;
            this.ServerPanelConsoleEnter.Click += new System.EventHandler(this.ServerPanelConsoleEnter_Click);
            // 
            // ServerPanelConsoleWebBrowser
            // 
            this.ServerPanelConsoleWebBrowser.AllowWebBrowserDrop = false;
            resources.ApplyResources(this.ServerPanelConsoleWebBrowser, "ServerPanelConsoleWebBrowser");
            this.ServerPanelConsoleWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.ServerPanelConsoleWebBrowser.Name = "ServerPanelConsoleWebBrowser";
            this.ServerPanelConsoleWebBrowser.ScriptErrorsSuppressed = true;
            this.ServerPanelConsoleWebBrowser.ScrollBarsEnabled = false;
            this.ServerPanelConsoleWebBrowser.TabStop = false;
            // 
            // ServerPanelConsoleInput
            // 
            resources.ApplyResources(this.ServerPanelConsoleInput, "ServerPanelConsoleInput");
            this.ServerPanelConsoleInput.Name = "ServerPanelConsoleInput";
            this.ServerPanelConsoleInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ServerPanelConsoleInput_KeyDown);
            this.ServerPanelConsoleInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ServerPanelConsoleInput_KeyPress);
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
            // JSPluginSplitContainer
            // 
            resources.ApplyResources(JSPluginSplitContainer, "JSPluginSplitContainer");
            JSPluginSplitContainer.Name = "JSPluginSplitContainer";
            // 
            // JSPluginSplitContainer.ServerPanel1
            // 
            JSPluginSplitContainer.Panel1.Controls.Add(this.JSPluginWebBrowser);
            // 
            // JSPluginSplitContainer.ServerPanel2
            // 
            JSPluginSplitContainer.Panel2.Controls.Add(this.JSPluginList);
            // 
            // JSPluginWebBrowser
            // 
            this.JSPluginWebBrowser.AllowWebBrowserDrop = false;
            resources.ApplyResources(this.JSPluginWebBrowser, "JSPluginWebBrowser");
            this.JSPluginWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.JSPluginWebBrowser.Name = "JSPluginWebBrowser";
            this.JSPluginWebBrowser.ScriptErrorsSuppressed = true;
            this.JSPluginWebBrowser.ScrollBarsEnabled = false;
            this.JSPluginWebBrowser.TabStop = false;
            // 
            // JSPluginList
            // 
            this.JSPluginList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.JSPluginListName,
            this.JSPluginListVersion,
            this.JSPluginListAuthor,
            this.JSPluginListDescription});
            this.JSPluginList.ContextMenuStrip = this.JSPluginListContextMenuStrip;
            resources.ApplyResources(this.JSPluginList, "JSPluginList");
            this.JSPluginList.FullRowSelect = true;
            this.JSPluginList.GridLines = true;
            this.JSPluginList.MultiSelect = false;
            this.JSPluginList.Name = "JSPluginList";
            this.JSPluginList.UseCompatibleStateImageBehavior = false;
            this.JSPluginList.View = System.Windows.Forms.View.Details;
            // 
            // JSPluginListName
            // 
            resources.ApplyResources(this.JSPluginListName, "JSPluginListName");
            // 
            // JSPluginListVersion
            // 
            resources.ApplyResources(this.JSPluginListVersion, "JSPluginListVersion");
            // 
            // JSPluginListAuthor
            // 
            resources.ApplyResources(this.JSPluginListAuthor, "JSPluginListAuthor");
            // 
            // JSPluginListDescription
            // 
            resources.ApplyResources(this.JSPluginListDescription, "JSPluginListDescription");
            // 
            // JSPluginListContextMenuStrip
            // 
            this.JSPluginListContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.JSPluginListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.JSPluginListContextMenuStrip_Disable,
            this.JSPluginListContextMenuStrip_Reload,
            this.JSPluginListContextMenuStrip_ClearConsole,
            this.toolStripSeparator9,
            this.JSPluginListContextMenuStrip_Docs});
            this.JSPluginListContextMenuStrip.Name = "JSPluginListContextMenuStrip";
            resources.ApplyResources(this.JSPluginListContextMenuStrip, "JSPluginListContextMenuStrip");
            this.JSPluginListContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.JSPluginListContextMenuStrip_Opening);
            // 
            // JSPluginListContextMenuStrip_Disable
            // 
            this.JSPluginListContextMenuStrip_Disable.Name = "JSPluginListContextMenuStrip_Disable";
            resources.ApplyResources(this.JSPluginListContextMenuStrip_Disable, "JSPluginListContextMenuStrip_Disable");
            this.JSPluginListContextMenuStrip_Disable.Click += new System.EventHandler(this.JSPluginListContextMenuStrip_Disable_Click);
            // 
            // JSPluginListContextMenuStrip_Reload
            // 
            this.JSPluginListContextMenuStrip_Reload.Name = "JSPluginListContextMenuStrip_Reload";
            resources.ApplyResources(this.JSPluginListContextMenuStrip_Reload, "JSPluginListContextMenuStrip_Reload");
            this.JSPluginListContextMenuStrip_Reload.Click += new System.EventHandler(this.JSPluginListContextMenuStrip_Reload_Click);
            // 
            // JSPluginListContextMenuStrip_ClearConsole
            // 
            this.JSPluginListContextMenuStrip_ClearConsole.Name = "JSPluginListContextMenuStrip_ClearConsole";
            resources.ApplyResources(this.JSPluginListContextMenuStrip_ClearConsole, "JSPluginListContextMenuStrip_ClearConsole");
            this.JSPluginListContextMenuStrip_ClearConsole.Click += new System.EventHandler(this.JSPluginListContextMenuStrip_ClearConsole_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // JSPluginListContextMenuStrip_Docs
            // 
            this.JSPluginListContextMenuStrip_Docs.Name = "JSPluginListContextMenuStrip_Docs";
            resources.ApplyResources(this.JSPluginListContextMenuStrip_Docs, "JSPluginListContextMenuStrip_Docs");
            this.JSPluginListContextMenuStrip_Docs.Click += new System.EventHandler(this.JSPluginListContextMenuStrip_Docs_Click);
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
            this.tabControl.Controls.Add(this.ServerPanel);
            this.tabControl.Controls.Add(this.ServerPluginManager);
            this.tabControl.Controls.Add(this.Regular);
            this.tabControl.Controls.Add(this.Task);
            this.tabControl.Controls.Add(this.Bot);
            this.tabControl.Controls.Add(this.Member);
            this.tabControl.Controls.Add(this.JSPlugin);
            this.tabControl.Controls.Add(this.Setting);
            this.tabControl.Controls.Add(this.Debug);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // ServerPanel
            // 
            this.ServerPanel.Controls.Add(this.ServerPanelTableLayout);
            resources.ApplyResources(this.ServerPanel, "ServerPanel");
            this.ServerPanel.Name = "ServerPanel";
            this.ServerPanel.UseVisualStyleBackColor = true;
            // 
            // ServerPanelTableLayout
            // 
            resources.ApplyResources(this.ServerPanelTableLayout, "ServerPanelTableLayout");
            this.ServerPanelTableLayout.Controls.Add(ServerPanelInfo, 0, 0);
            this.ServerPanelTableLayout.Controls.Add(ServerPanelControls, 0, 1);
            this.ServerPanelTableLayout.Controls.Add(ServerPanelConsole, 1, 0);
            this.ServerPanelTableLayout.Name = "ServerPanelTableLayout";
            // 
            // ServerPluginManager
            // 
            this.ServerPluginManager.Controls.Add(this.PluginList);
            resources.ApplyResources(this.ServerPluginManager, "ServerPluginManager");
            this.ServerPluginManager.Name = "ServerPluginManager";
            this.ServerPluginManager.UseVisualStyleBackColor = true;
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
            // JSPlugin
            // 
            this.JSPlugin.Controls.Add(JSPluginSplitContainer);
            resources.ApplyResources(this.JSPlugin, "JSPlugin");
            this.JSPlugin.Name = "JSPlugin";
            this.JSPlugin.UseVisualStyleBackColor = true;
            // 
            // Setting
            // 
            this.Setting.Controls.Add(this.SettingServerPanel);
            resources.ApplyResources(this.Setting, "Setting");
            this.Setting.Name = "Setting";
            this.Setting.UseVisualStyleBackColor = true;
            // 
            // SettingServerPanel
            // 
            resources.ApplyResources(this.SettingServerPanel, "SettingServerPanel");
            this.SettingServerPanel.Controls.Add(this.SettingSerein);
            this.SettingServerPanel.Controls.Add(this.SettingEvent);
            this.SettingServerPanel.Controls.Add(this.SettingBot);
            this.SettingServerPanel.Controls.Add(this.SettingServer);
            this.SettingServerPanel.Name = "SettingServerPanel";
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
            this.SettingSerein.Controls.Add(this.SettingSereinExtension);
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
            // SettingSereinExtension
            // 
            resources.ApplyResources(this.SettingSereinExtension, "SettingSereinExtension");
            this.SettingSereinExtension.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SettingSereinExtension.ForeColor = System.Drawing.Color.RoyalBlue;
            this.SettingSereinExtension.Name = "SettingSereinExtension";
            this.SettingSereinExtension.Click += new System.EventHandler(this.SettingSereinExtension_Click);
            this.SettingSereinExtension.MouseHover += new System.EventHandler(this.SettingSereinExtension_MouseHover);
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
            // SettingEventSplitContainer.ServerPanel1
            // 
            this.SettingEventSplitContainer.Panel1.Controls.Add(this.SettingEventTreeView);
            // 
            // SettingEventSplitContainer.ServerPanel2
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
            // SettingServerOutputEncodingLabel
            // 
            resources.ApplyResources(this.SettingServerOutputEncodingLabel, "SettingServerOutputEncodingLabel");
            this.SettingServerOutputEncodingLabel.Name = "SettingServerOutputEncodingLabel";
            this.SettingServerOutputEncodingLabel.MouseHover += new System.EventHandler(this.SettingServerOutputEncodingLabel_MouseHover);
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
            ServerPanelInfo.ResumeLayout(false);
            ServerPanelInfo.PerformLayout();
            ServerPanelControls.ResumeLayout(false);
            ServerPanelConsole.ResumeLayout(false);
            this.ServerPanelConsoleServerPanel.ResumeLayout(false);
            this.ServerPanelConsoleServerPanel.PerformLayout();
            BotInfo.ResumeLayout(false);
            BotInfo.PerformLayout();
            BotWebsocket.ResumeLayout(false);
            JSPluginSplitContainer.Panel1.ResumeLayout(false);
            JSPluginSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(JSPluginSplitContainer)).EndInit();
            JSPluginSplitContainer.ResumeLayout(false);
            this.JSPluginListContextMenuStrip.ResumeLayout(false);
            this.PluginContextMenuStrip.ResumeLayout(false);
            this.MainTableLayout.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.ServerPanel.ResumeLayout(false);
            this.ServerPanelTableLayout.ResumeLayout(false);
            this.ServerPanelTableLayout.PerformLayout();
            this.ServerPluginManager.ResumeLayout(false);
            this.Regular.ResumeLayout(false);
            this.RegexContextMenuStrip.ResumeLayout(false);
            this.Task.ResumeLayout(false);
            this.TaskContextMenuStrip.ResumeLayout(false);
            this.Bot.ResumeLayout(false);
            this.BotTableLayoutPanel.ResumeLayout(false);
            this.Member.ResumeLayout(false);
            this.MemberContextMenuStrip.ResumeLayout(false);
            this.JSPlugin.ResumeLayout(false);
            this.Setting.ResumeLayout(false);
            this.SettingServerPanel.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage ServerPanel;
        private System.Windows.Forms.TableLayoutPanel ServerPanelTableLayout;
        private System.Windows.Forms.Label ServerPanelInfoCPU2;
        private System.Windows.Forms.Label ServerPanelInfoPlayerCount2;
        private System.Windows.Forms.Label ServerPanelInfoTime2;
        private System.Windows.Forms.Label ServerPanelInfoDifficulty2;
        private System.Windows.Forms.Label ServerPanelInfoVersion2;
        private System.Windows.Forms.Label ServerPanelInfoStatus2;
        private System.Windows.Forms.Button ServerPanelControlKill;
        private System.Windows.Forms.Button ServerPanelControlRestart;
        private System.Windows.Forms.Button ServerPanelControlStop;
        private System.Windows.Forms.Button ServerPanelControlStart;
        private System.Windows.Forms.Panel ServerPanelConsoleServerPanel;
        private System.Windows.Forms.WebBrowser ServerPanelConsoleWebBrowser;
        private System.Windows.Forms.TabPage ServerPluginManager;
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
        private System.Windows.Forms.Panel SettingServerPanel;
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
        private System.Windows.Forms.Label SettingSereinExtension;
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
        private System.Windows.Forms.Button ServerPanelConsoleEnter;
        private System.Windows.Forms.TextBox ServerPanelConsoleInput;
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
        private System.Windows.Forms.TabPage JSPlugin;
        private System.Windows.Forms.WebBrowser JSPluginWebBrowser;
        private System.Windows.Forms.ListView JSPluginList;
        private System.Windows.Forms.ColumnHeader JSPluginListName;
        private System.Windows.Forms.ColumnHeader JSPluginListVersion;
        private System.Windows.Forms.ColumnHeader JSPluginListAuthor;
        private System.Windows.Forms.ColumnHeader JSPluginListDescription;
        private System.Windows.Forms.ContextMenuStrip JSPluginListContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem JSPluginListContextMenuStrip_Reload;
        private System.Windows.Forms.CheckBox SettingBotAutoEscape;
        private System.Windows.Forms.ToolStripMenuItem JSPluginListContextMenuStrip_ClearConsole;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem JSPluginListContextMenuStrip_Docs;
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
        private System.Windows.Forms.ToolStripMenuItem JSPluginListContextMenuStrip_Disable;
    }
}

