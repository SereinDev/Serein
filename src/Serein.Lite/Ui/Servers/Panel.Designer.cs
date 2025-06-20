namespace Serein.Lite.Ui.Servers
{
    partial class Panel
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
            System.Windows.Forms.GroupBox infoGroupBox;
            System.Windows.Forms.TableLayoutPanel informationTableLayoutPanel;
            System.Windows.Forms.Label statusLabel;
            System.Windows.Forms.Label versionLabel;
            System.Windows.Forms.Label playerCountLabel;
            System.Windows.Forms.Label runTimeLabel;
            System.Windows.Forms.Label cpuPercentLabel;
            System.Windows.Forms.GroupBox controlGroupBox;
            System.Windows.Forms.TableLayoutPanel controlTableLayoutPanel;
            System.Windows.Forms.Button startButton;
            System.Windows.Forms.Button stopButton;
            System.Windows.Forms.Button restartButton;
            System.Windows.Forms.Button terminateButton;
            System.Windows.Forms.GroupBox consoleGroupBox;
            System.Windows.Forms.TableLayoutPanel consoleTableLayoutPanel;
            System.Windows.Forms.Button enterButton;
            System.Windows.Forms.GroupBox shortcutGroupBox;
            System.Windows.Forms.TableLayoutPanel shortcutTableLayoutPanel;
            System.Windows.Forms.Button openDirectoryButton;
            System.Windows.Forms.Button startPluginManagerButton;
            _cpuPercentDynamicLabel = new System.Windows.Forms.Label();
            _runTimeDynamicLabel = new System.Windows.Forms.Label();
            _statusDynamicLabel = new System.Windows.Forms.Label();
            _versionDynamicLabel = new System.Windows.Forms.Label();
            _playerCountDynamicLabel = new System.Windows.Forms.Label();
            _consoleBrowser = new Serein.Lite.Ui.Controls.ConsoleWebBrowser();
            _inputTextBox = new System.Windows.Forms.TextBox();
            _toolTip = new System.Windows.Forms.ToolTip(components);
            mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            infoGroupBox = new System.Windows.Forms.GroupBox();
            informationTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            statusLabel = new System.Windows.Forms.Label();
            versionLabel = new System.Windows.Forms.Label();
            playerCountLabel = new System.Windows.Forms.Label();
            runTimeLabel = new System.Windows.Forms.Label();
            cpuPercentLabel = new System.Windows.Forms.Label();
            controlGroupBox = new System.Windows.Forms.GroupBox();
            controlTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            startButton = new System.Windows.Forms.Button();
            stopButton = new System.Windows.Forms.Button();
            restartButton = new System.Windows.Forms.Button();
            terminateButton = new System.Windows.Forms.Button();
            consoleGroupBox = new System.Windows.Forms.GroupBox();
            consoleTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            enterButton = new System.Windows.Forms.Button();
            shortcutGroupBox = new System.Windows.Forms.GroupBox();
            shortcutTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            openDirectoryButton = new System.Windows.Forms.Button();
            startPluginManagerButton = new System.Windows.Forms.Button();
            mainTableLayoutPanel.SuspendLayout();
            infoGroupBox.SuspendLayout();
            informationTableLayoutPanel.SuspendLayout();
            controlGroupBox.SuspendLayout();
            controlTableLayoutPanel.SuspendLayout();
            consoleGroupBox.SuspendLayout();
            consoleTableLayoutPanel.SuspendLayout();
            shortcutGroupBox.SuspendLayout();
            shortcutTableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            mainTableLayoutPanel.ColumnCount = 2;
            mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            mainTableLayoutPanel.Controls.Add(infoGroupBox, 0, 0);
            mainTableLayoutPanel.Controls.Add(controlGroupBox, 0, 1);
            mainTableLayoutPanel.Controls.Add(consoleGroupBox, 1, 0);
            mainTableLayoutPanel.Controls.Add(shortcutGroupBox, 0, 2);
            mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            mainTableLayoutPanel.RowCount = 3;
            mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            mainTableLayoutPanel.Size = new System.Drawing.Size(1280, 720);
            mainTableLayoutPanel.TabIndex = 0;
            // 
            // infoGroupBox
            // 
            infoGroupBox.Controls.Add(informationTableLayoutPanel);
            infoGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            infoGroupBox.Location = new System.Drawing.Point(3, 3);
            infoGroupBox.Name = "infoGroupBox";
            infoGroupBox.Size = new System.Drawing.Size(294, 244);
            infoGroupBox.TabIndex = 0;
            infoGroupBox.TabStop = false;
            infoGroupBox.Text = "信息";
            // 
            // informationTableLayoutPanel
            // 
            informationTableLayoutPanel.ColumnCount = 2;
            informationTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            informationTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            informationTableLayoutPanel.Controls.Add(_cpuPercentDynamicLabel, 1, 4);
            informationTableLayoutPanel.Controls.Add(_runTimeDynamicLabel, 1, 3);
            informationTableLayoutPanel.Controls.Add(statusLabel, 0, 0);
            informationTableLayoutPanel.Controls.Add(_statusDynamicLabel, 1, 0);
            informationTableLayoutPanel.Controls.Add(versionLabel, 0, 1);
            informationTableLayoutPanel.Controls.Add(_versionDynamicLabel, 1, 1);
            informationTableLayoutPanel.Controls.Add(playerCountLabel, 0, 2);
            informationTableLayoutPanel.Controls.Add(_playerCountDynamicLabel, 1, 2);
            informationTableLayoutPanel.Controls.Add(runTimeLabel, 0, 3);
            informationTableLayoutPanel.Controls.Add(cpuPercentLabel, 0, 4);
            informationTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            informationTableLayoutPanel.Location = new System.Drawing.Point(3, 34);
            informationTableLayoutPanel.Name = "informationTableLayoutPanel";
            informationTableLayoutPanel.RowCount = 5;
            informationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            informationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            informationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            informationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            informationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            informationTableLayoutPanel.Size = new System.Drawing.Size(288, 207);
            informationTableLayoutPanel.TabIndex = 0;
            // 
            // _cpuPercentDynamicLabel
            // 
            _cpuPercentDynamicLabel.AutoEllipsis = true;
            _cpuPercentDynamicLabel.AutoSize = true;
            _cpuPercentDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _cpuPercentDynamicLabel.Location = new System.Drawing.Point(123, 164);
            _cpuPercentDynamicLabel.Name = "_cpuPercentDynamicLabel";
            _cpuPercentDynamicLabel.Size = new System.Drawing.Size(162, 43);
            _cpuPercentDynamicLabel.TabIndex = 9;
            _cpuPercentDynamicLabel.Text = "-";
            _cpuPercentDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _runTimeDynamicLabel
            // 
            _runTimeDynamicLabel.AutoEllipsis = true;
            _runTimeDynamicLabel.AutoSize = true;
            _runTimeDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _runTimeDynamicLabel.Location = new System.Drawing.Point(123, 123);
            _runTimeDynamicLabel.Name = "_runTimeDynamicLabel";
            _runTimeDynamicLabel.Size = new System.Drawing.Size(162, 41);
            _runTimeDynamicLabel.TabIndex = 7;
            _runTimeDynamicLabel.Text = "-";
            _runTimeDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            statusLabel.Location = new System.Drawing.Point(3, 0);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new System.Drawing.Size(114, 41);
            statusLabel.TabIndex = 0;
            statusLabel.Text = "状态";
            statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _statusDynamicLabel
            // 
            _statusDynamicLabel.AutoSize = true;
            _statusDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _statusDynamicLabel.Location = new System.Drawing.Point(123, 0);
            _statusDynamicLabel.Name = "_statusDynamicLabel";
            _statusDynamicLabel.Size = new System.Drawing.Size(162, 41);
            _statusDynamicLabel.TabIndex = 1;
            _statusDynamicLabel.Text = "未启动";
            _statusDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // versionLabel
            // 
            versionLabel.AutoSize = true;
            versionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            versionLabel.Location = new System.Drawing.Point(3, 41);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new System.Drawing.Size(114, 41);
            versionLabel.TabIndex = 2;
            versionLabel.Text = "版本";
            versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _versionDynamicLabel
            // 
            _versionDynamicLabel.AutoEllipsis = true;
            _versionDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _versionDynamicLabel.Location = new System.Drawing.Point(123, 41);
            _versionDynamicLabel.Name = "_versionDynamicLabel";
            _versionDynamicLabel.Size = new System.Drawing.Size(162, 41);
            _versionDynamicLabel.TabIndex = 3;
            _versionDynamicLabel.Text = "-";
            _versionDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // playerCountLabel
            // 
            playerCountLabel.AutoSize = true;
            playerCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            playerCountLabel.Location = new System.Drawing.Point(3, 82);
            playerCountLabel.Name = "playerCountLabel";
            playerCountLabel.Size = new System.Drawing.Size(114, 41);
            playerCountLabel.TabIndex = 4;
            playerCountLabel.Text = "玩家数";
            playerCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _playerCountDynamicLabel
            // 
            _playerCountDynamicLabel.AutoEllipsis = true;
            _playerCountDynamicLabel.AutoSize = true;
            _playerCountDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _playerCountDynamicLabel.Location = new System.Drawing.Point(123, 82);
            _playerCountDynamicLabel.Name = "_playerCountDynamicLabel";
            _playerCountDynamicLabel.Size = new System.Drawing.Size(162, 41);
            _playerCountDynamicLabel.TabIndex = 5;
            _playerCountDynamicLabel.Text = "-";
            _playerCountDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // runTimeLabel
            // 
            runTimeLabel.AutoSize = true;
            runTimeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            runTimeLabel.Location = new System.Drawing.Point(3, 123);
            runTimeLabel.Name = "runTimeLabel";
            runTimeLabel.Size = new System.Drawing.Size(114, 41);
            runTimeLabel.TabIndex = 6;
            runTimeLabel.Text = "运行时长";
            runTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cpuPercentLabel
            // 
            cpuPercentLabel.AutoSize = true;
            cpuPercentLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            cpuPercentLabel.Location = new System.Drawing.Point(3, 164);
            cpuPercentLabel.Name = "cpuPercentLabel";
            cpuPercentLabel.Size = new System.Drawing.Size(114, 43);
            cpuPercentLabel.TabIndex = 8;
            cpuPercentLabel.Text = "进程占用";
            cpuPercentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // controlGroupBox
            // 
            controlGroupBox.Controls.Add(controlTableLayoutPanel);
            controlGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            controlGroupBox.Location = new System.Drawing.Point(3, 253);
            controlGroupBox.Name = "controlGroupBox";
            controlGroupBox.Size = new System.Drawing.Size(294, 154);
            controlGroupBox.TabIndex = 1;
            controlGroupBox.TabStop = false;
            controlGroupBox.Text = "控制";
            // 
            // controlTableLayoutPanel
            // 
            controlTableLayoutPanel.ColumnCount = 2;
            controlTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            controlTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            controlTableLayoutPanel.Controls.Add(startButton, 0, 0);
            controlTableLayoutPanel.Controls.Add(stopButton, 1, 0);
            controlTableLayoutPanel.Controls.Add(restartButton, 0, 1);
            controlTableLayoutPanel.Controls.Add(terminateButton, 1, 1);
            controlTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            controlTableLayoutPanel.Location = new System.Drawing.Point(3, 34);
            controlTableLayoutPanel.Name = "controlTableLayoutPanel";
            controlTableLayoutPanel.RowCount = 2;
            controlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            controlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            controlTableLayoutPanel.Size = new System.Drawing.Size(288, 117);
            controlTableLayoutPanel.TabIndex = 0;
            // 
            // startButton
            // 
            startButton.Dock = System.Windows.Forms.DockStyle.Fill;
            startButton.Location = new System.Drawing.Point(3, 3);
            startButton.Name = "startButton";
            startButton.Size = new System.Drawing.Size(138, 52);
            startButton.TabIndex = 0;
            startButton.Text = "启动";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += StartButton_Click;
            // 
            // stopButton
            // 
            stopButton.Dock = System.Windows.Forms.DockStyle.Fill;
            stopButton.Location = new System.Drawing.Point(147, 3);
            stopButton.Name = "stopButton";
            stopButton.Size = new System.Drawing.Size(138, 52);
            stopButton.TabIndex = 1;
            stopButton.Text = "停止";
            stopButton.UseVisualStyleBackColor = true;
            stopButton.Click += StopButton_Click;
            // 
            // restartButton
            // 
            restartButton.Dock = System.Windows.Forms.DockStyle.Fill;
            restartButton.Location = new System.Drawing.Point(3, 61);
            restartButton.Name = "restartButton";
            restartButton.Size = new System.Drawing.Size(138, 53);
            restartButton.TabIndex = 2;
            restartButton.Text = "重启";
            restartButton.UseVisualStyleBackColor = true;
            restartButton.Click += RestartButton_Click;
            // 
            // terminateButton
            // 
            terminateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            terminateButton.Location = new System.Drawing.Point(147, 61);
            terminateButton.Name = "terminateButton";
            terminateButton.Size = new System.Drawing.Size(138, 53);
            terminateButton.TabIndex = 3;
            terminateButton.Text = "强制结束";
            terminateButton.UseVisualStyleBackColor = true;
            terminateButton.Click += TerminateButton_Click;
            // 
            // consoleGroupBox
            // 
            consoleGroupBox.Controls.Add(consoleTableLayoutPanel);
            consoleGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            consoleGroupBox.Location = new System.Drawing.Point(303, 3);
            consoleGroupBox.Name = "consoleGroupBox";
            mainTableLayoutPanel.SetRowSpan(consoleGroupBox, 3);
            consoleGroupBox.Size = new System.Drawing.Size(974, 714);
            consoleGroupBox.TabIndex = 2;
            consoleGroupBox.TabStop = false;
            consoleGroupBox.Text = "控制台";
            // 
            // consoleTableLayoutPanel
            // 
            consoleTableLayoutPanel.ColumnCount = 2;
            consoleTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            consoleTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            consoleTableLayoutPanel.Controls.Add(_consoleBrowser, 0, 0);
            consoleTableLayoutPanel.Controls.Add(_inputTextBox, 0, 1);
            consoleTableLayoutPanel.Controls.Add(enterButton, 1, 1);
            consoleTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            consoleTableLayoutPanel.Location = new System.Drawing.Point(3, 34);
            consoleTableLayoutPanel.Name = "consoleTableLayoutPanel";
            consoleTableLayoutPanel.RowCount = 2;
            consoleTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            consoleTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            consoleTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            consoleTableLayoutPanel.Size = new System.Drawing.Size(968, 677);
            consoleTableLayoutPanel.TabIndex = 0;
            // 
            // _consoleBrowser
            // 
            _consoleBrowser.AllowNavigation = false;
            consoleTableLayoutPanel.SetColumnSpan(_consoleBrowser, 2);
            _consoleBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            _consoleBrowser.IsWebBrowserContextMenuEnabled = false;
            _consoleBrowser.Location = new System.Drawing.Point(5, 5);
            _consoleBrowser.Margin = new System.Windows.Forms.Padding(5);
            _consoleBrowser.Name = "_consoleBrowser";
            _consoleBrowser.Size = new System.Drawing.Size(958, 619);
            _consoleBrowser.TabIndex = 0;
            _consoleBrowser.TabStop = false;
            // 
            // _inputTextBox
            // 
            _inputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _inputTextBox.Location = new System.Drawing.Point(3, 632);
            _inputTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
            _inputTextBox.Name = "_inputTextBox";
            _inputTextBox.Size = new System.Drawing.Size(910, 38);
            _inputTextBox.TabIndex = 0;
            _inputTextBox.KeyDown += InputTextBox_KeyDown;
            // 
            // enterButton
            // 
            enterButton.Dock = System.Windows.Forms.DockStyle.Fill;
            enterButton.Location = new System.Drawing.Point(921, 632);
            enterButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            enterButton.Name = "enterButton";
            enterButton.Size = new System.Drawing.Size(44, 40);
            enterButton.TabIndex = 1;
            enterButton.Text = "▲";
            enterButton.UseVisualStyleBackColor = true;
            enterButton.Click += EnterButton_Click;
            // 
            // shortcutGroupBox
            // 
            shortcutGroupBox.Controls.Add(shortcutTableLayoutPanel);
            shortcutGroupBox.Location = new System.Drawing.Point(3, 413);
            shortcutGroupBox.Name = "shortcutGroupBox";
            shortcutGroupBox.Size = new System.Drawing.Size(294, 161);
            shortcutGroupBox.TabIndex = 3;
            shortcutGroupBox.TabStop = false;
            shortcutGroupBox.Text = "快捷操作";
            // 
            // shortcutTableLayoutPanel
            // 
            shortcutTableLayoutPanel.ColumnCount = 1;
            shortcutTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            shortcutTableLayoutPanel.Controls.Add(openDirectoryButton, 0, 0);
            shortcutTableLayoutPanel.Controls.Add(startPluginManagerButton, 0, 1);
            shortcutTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            shortcutTableLayoutPanel.Location = new System.Drawing.Point(3, 34);
            shortcutTableLayoutPanel.Name = "shortcutTableLayoutPanel";
            shortcutTableLayoutPanel.RowCount = 2;
            shortcutTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            shortcutTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            shortcutTableLayoutPanel.Size = new System.Drawing.Size(288, 124);
            shortcutTableLayoutPanel.TabIndex = 0;
            // 
            // openDirectoryButton
            // 
            openDirectoryButton.Dock = System.Windows.Forms.DockStyle.Top;
            openDirectoryButton.Location = new System.Drawing.Point(3, 3);
            openDirectoryButton.Name = "openDirectoryButton";
            openDirectoryButton.Size = new System.Drawing.Size(282, 46);
            openDirectoryButton.TabIndex = 0;
            openDirectoryButton.Text = "打开启动文件所在位置";
            openDirectoryButton.UseVisualStyleBackColor = true;
            openDirectoryButton.Click += OpenDirectoryButton_Click;
            // 
            // startPluginManagerButton
            // 
            startPluginManagerButton.Dock = System.Windows.Forms.DockStyle.Top;
            startPluginManagerButton.Location = new System.Drawing.Point(3, 65);
            startPluginManagerButton.Name = "startPluginManagerButton";
            startPluginManagerButton.Size = new System.Drawing.Size(282, 46);
            startPluginManagerButton.TabIndex = 1;
            startPluginManagerButton.Text = "插件管理";
            startPluginManagerButton.UseVisualStyleBackColor = true;
            startPluginManagerButton.Click += StartPluginManagerButton_Click;
            // 
            // Panel
            // 
            AllowDrop = true;
            BackColor = System.Drawing.Color.White;
            Controls.Add(mainTableLayoutPanel);
            Name = "Panel";
            Size = new System.Drawing.Size(1280, 720);
            DragDrop += Panel_DragDrop;
            DragEnter += Panel_DragEnter;
            mainTableLayoutPanel.ResumeLayout(false);
            infoGroupBox.ResumeLayout(false);
            informationTableLayoutPanel.ResumeLayout(false);
            informationTableLayoutPanel.PerformLayout();
            controlGroupBox.ResumeLayout(false);
            controlTableLayoutPanel.ResumeLayout(false);
            consoleGroupBox.ResumeLayout(false);
            consoleTableLayoutPanel.ResumeLayout(false);
            consoleTableLayoutPanel.PerformLayout();
            shortcutGroupBox.ResumeLayout(false);
            shortcutTableLayoutPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox _inputTextBox;
        private System.Windows.Forms.Label _statusDynamicLabel;
        private System.Windows.Forms.Label _runTimeDynamicLabel;
        private System.Windows.Forms.Label _versionDynamicLabel;
        private System.Windows.Forms.Label _playerCountDynamicLabel;
        private System.Windows.Forms.Label _cpuPercentDynamicLabel;
        private Serein.Lite.Ui.Controls.ConsoleWebBrowser _consoleBrowser;
        private System.Windows.Forms.ToolTip _toolTip;
    }
}
