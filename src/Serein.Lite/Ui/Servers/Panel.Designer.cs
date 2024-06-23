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
            System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
            System.Windows.Forms.GroupBox InfoGroupBox;
            System.Windows.Forms.TableLayoutPanel InformationTableLayoutPanel;
            System.Windows.Forms.Label StatusLabel;
            System.Windows.Forms.Label VersionLabel;
            System.Windows.Forms.Label PlayerCountLabel;
            System.Windows.Forms.Label RunTimeLabel;
            System.Windows.Forms.Label CPUPercentLabel;
            System.Windows.Forms.GroupBox ControlGroupBox;
            System.Windows.Forms.TableLayoutPanel ControlTableLayoutPanel;
            System.Windows.Forms.GroupBox ConsoleGroupBox;
            System.Windows.Forms.TableLayoutPanel ConsoleTableLayoutPanel;
            CPUPercentDynamicLabel = new System.Windows.Forms.Label();
            RunTimeDynamicLabel = new System.Windows.Forms.Label();
            StatusDynamicLabel = new System.Windows.Forms.Label();
            VersionDynamicLabel = new System.Windows.Forms.Label();
            PlayerCountDynamicLabel = new System.Windows.Forms.Label();
            StartButton = new System.Windows.Forms.Button();
            StopButton = new System.Windows.Forms.Button();
            RestartButton = new System.Windows.Forms.Button();
            TerminateButton = new System.Windows.Forms.Button();
            ConsoleBrowser = new Serein.Lite.Ui.Controls.ConsoleWebBrowser();
            InputTextBox = new System.Windows.Forms.TextBox();
            EnterButton = new System.Windows.Forms.Button();
            MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            InfoGroupBox = new System.Windows.Forms.GroupBox();
            InformationTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            StatusLabel = new System.Windows.Forms.Label();
            VersionLabel = new System.Windows.Forms.Label();
            PlayerCountLabel = new System.Windows.Forms.Label();
            RunTimeLabel = new System.Windows.Forms.Label();
            CPUPercentLabel = new System.Windows.Forms.Label();
            ControlGroupBox = new System.Windows.Forms.GroupBox();
            ControlTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ConsoleGroupBox = new System.Windows.Forms.GroupBox();
            ConsoleTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            MainTableLayoutPanel.SuspendLayout();
            InfoGroupBox.SuspendLayout();
            InformationTableLayoutPanel.SuspendLayout();
            ControlGroupBox.SuspendLayout();
            ControlTableLayoutPanel.SuspendLayout();
            ConsoleGroupBox.SuspendLayout();
            ConsoleTableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            MainTableLayoutPanel.ColumnCount = 2;
            MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            MainTableLayoutPanel.Controls.Add(InfoGroupBox, 0, 0);
            MainTableLayoutPanel.Controls.Add(ControlGroupBox, 0, 1);
            MainTableLayoutPanel.Controls.Add(ConsoleGroupBox, 1, 0);
            MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            MainTableLayoutPanel.RowCount = 3;
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            MainTableLayoutPanel.Size = new System.Drawing.Size(1280, 720);
            MainTableLayoutPanel.TabIndex = 0;
            // 
            // InfoGroupBox
            // 
            InfoGroupBox.Controls.Add(InformationTableLayoutPanel);
            InfoGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            InfoGroupBox.Location = new System.Drawing.Point(3, 3);
            InfoGroupBox.Name = "InfoGroupBox";
            InfoGroupBox.Size = new System.Drawing.Size(294, 244);
            InfoGroupBox.TabIndex = 0;
            InfoGroupBox.TabStop = false;
            InfoGroupBox.Text = "信息";
            // 
            // InformationTableLayoutPanel
            // 
            InformationTableLayoutPanel.ColumnCount = 2;
            InformationTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            InformationTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            InformationTableLayoutPanel.Controls.Add(CPUPercentDynamicLabel, 1, 4);
            InformationTableLayoutPanel.Controls.Add(RunTimeDynamicLabel, 1, 3);
            InformationTableLayoutPanel.Controls.Add(StatusLabel, 0, 0);
            InformationTableLayoutPanel.Controls.Add(StatusDynamicLabel, 1, 0);
            InformationTableLayoutPanel.Controls.Add(VersionLabel, 0, 1);
            InformationTableLayoutPanel.Controls.Add(VersionDynamicLabel, 1, 1);
            InformationTableLayoutPanel.Controls.Add(PlayerCountLabel, 0, 2);
            InformationTableLayoutPanel.Controls.Add(PlayerCountDynamicLabel, 1, 2);
            InformationTableLayoutPanel.Controls.Add(RunTimeLabel, 0, 3);
            InformationTableLayoutPanel.Controls.Add(CPUPercentLabel, 0, 4);
            InformationTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            InformationTableLayoutPanel.Location = new System.Drawing.Point(3, 34);
            InformationTableLayoutPanel.Name = "InformationTableLayoutPanel";
            InformationTableLayoutPanel.RowCount = 5;
            InformationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            InformationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            InformationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            InformationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            InformationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            InformationTableLayoutPanel.Size = new System.Drawing.Size(288, 207);
            InformationTableLayoutPanel.TabIndex = 0;
            // 
            // CPUPercentDynamicLabel
            // 
            CPUPercentDynamicLabel.AutoEllipsis = true;
            CPUPercentDynamicLabel.AutoSize = true;
            CPUPercentDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            CPUPercentDynamicLabel.Location = new System.Drawing.Point(123, 164);
            CPUPercentDynamicLabel.Name = "CPUPercentDynamicLabel";
            CPUPercentDynamicLabel.Size = new System.Drawing.Size(162, 43);
            CPUPercentDynamicLabel.TabIndex = 9;
            CPUPercentDynamicLabel.Text = "-";
            CPUPercentDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RunTimeDynamicLabel
            // 
            RunTimeDynamicLabel.AutoEllipsis = true;
            RunTimeDynamicLabel.AutoSize = true;
            RunTimeDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            RunTimeDynamicLabel.Location = new System.Drawing.Point(123, 123);
            RunTimeDynamicLabel.Name = "RunTimeDynamicLabel";
            RunTimeDynamicLabel.Size = new System.Drawing.Size(162, 41);
            RunTimeDynamicLabel.TabIndex = 7;
            RunTimeDynamicLabel.Text = "-";
            RunTimeDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StatusLabel
            // 
            StatusLabel.AutoSize = true;
            StatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            StatusLabel.Location = new System.Drawing.Point(3, 0);
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new System.Drawing.Size(114, 41);
            StatusLabel.TabIndex = 0;
            StatusLabel.Text = "状态";
            StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusDynamicLabel
            // 
            StatusDynamicLabel.AutoSize = true;
            StatusDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            StatusDynamicLabel.Location = new System.Drawing.Point(123, 0);
            StatusDynamicLabel.Name = "StatusDynamicLabel";
            StatusDynamicLabel.Size = new System.Drawing.Size(162, 41);
            StatusDynamicLabel.TabIndex = 1;
            StatusDynamicLabel.Text = "未启动";
            StatusDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // VersionLabel
            // 
            VersionLabel.AutoSize = true;
            VersionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            VersionLabel.Location = new System.Drawing.Point(3, 41);
            VersionLabel.Name = "VersionLabel";
            VersionLabel.Size = new System.Drawing.Size(114, 41);
            VersionLabel.TabIndex = 2;
            VersionLabel.Text = "版本";
            VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VersionDynamicLabel
            // 
            VersionDynamicLabel.AutoEllipsis = true;
            VersionDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            VersionDynamicLabel.Location = new System.Drawing.Point(123, 41);
            VersionDynamicLabel.Name = "VersionDynamicLabel";
            VersionDynamicLabel.Size = new System.Drawing.Size(162, 41);
            VersionDynamicLabel.TabIndex = 3;
            VersionDynamicLabel.Text = "-";
            VersionDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PlayerCountLabel
            // 
            PlayerCountLabel.AutoSize = true;
            PlayerCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            PlayerCountLabel.Location = new System.Drawing.Point(3, 82);
            PlayerCountLabel.Name = "PlayerCountLabel";
            PlayerCountLabel.Size = new System.Drawing.Size(114, 41);
            PlayerCountLabel.TabIndex = 4;
            PlayerCountLabel.Text = "玩家数";
            PlayerCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PlayerCountDynamicLabel
            // 
            PlayerCountDynamicLabel.AutoEllipsis = true;
            PlayerCountDynamicLabel.AutoSize = true;
            PlayerCountDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            PlayerCountDynamicLabel.Location = new System.Drawing.Point(123, 82);
            PlayerCountDynamicLabel.Name = "PlayerCountDynamicLabel";
            PlayerCountDynamicLabel.Size = new System.Drawing.Size(162, 41);
            PlayerCountDynamicLabel.TabIndex = 5;
            PlayerCountDynamicLabel.Text = "-";
            PlayerCountDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RunTimeLabel
            // 
            RunTimeLabel.AutoSize = true;
            RunTimeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            RunTimeLabel.Location = new System.Drawing.Point(3, 123);
            RunTimeLabel.Name = "RunTimeLabel";
            RunTimeLabel.Size = new System.Drawing.Size(114, 41);
            RunTimeLabel.TabIndex = 6;
            RunTimeLabel.Text = "运行时长";
            RunTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CPUPercentLabel
            // 
            CPUPercentLabel.AutoSize = true;
            CPUPercentLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            CPUPercentLabel.Location = new System.Drawing.Point(3, 164);
            CPUPercentLabel.Name = "CPUPercentLabel";
            CPUPercentLabel.Size = new System.Drawing.Size(114, 43);
            CPUPercentLabel.TabIndex = 8;
            CPUPercentLabel.Text = "进程占用";
            CPUPercentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ControlGroupBox
            // 
            ControlGroupBox.Controls.Add(ControlTableLayoutPanel);
            ControlGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            ControlGroupBox.Location = new System.Drawing.Point(3, 253);
            ControlGroupBox.Name = "ControlGroupBox";
            ControlGroupBox.Size = new System.Drawing.Size(294, 154);
            ControlGroupBox.TabIndex = 1;
            ControlGroupBox.TabStop = false;
            ControlGroupBox.Text = "控制";
            // 
            // ControlTableLayoutPanel
            // 
            ControlTableLayoutPanel.ColumnCount = 2;
            ControlTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            ControlTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            ControlTableLayoutPanel.Controls.Add(StartButton, 0, 0);
            ControlTableLayoutPanel.Controls.Add(StopButton, 1, 0);
            ControlTableLayoutPanel.Controls.Add(RestartButton, 0, 1);
            ControlTableLayoutPanel.Controls.Add(TerminateButton, 1, 1);
            ControlTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            ControlTableLayoutPanel.Location = new System.Drawing.Point(3, 34);
            ControlTableLayoutPanel.Name = "ControlTableLayoutPanel";
            ControlTableLayoutPanel.RowCount = 2;
            ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            ControlTableLayoutPanel.Size = new System.Drawing.Size(288, 117);
            ControlTableLayoutPanel.TabIndex = 0;
            // 
            // StartButton
            // 
            StartButton.Dock = System.Windows.Forms.DockStyle.Fill;
            StartButton.Location = new System.Drawing.Point(3, 3);
            StartButton.Name = "StartButton";
            StartButton.Size = new System.Drawing.Size(138, 52);
            StartButton.TabIndex = 0;
            StartButton.Text = "启动";
            StartButton.UseVisualStyleBackColor = true;
            StartButton.Click += StartButton_Click;
            // 
            // StopButton
            // 
            StopButton.Dock = System.Windows.Forms.DockStyle.Fill;
            StopButton.Location = new System.Drawing.Point(147, 3);
            StopButton.Name = "StopButton";
            StopButton.Size = new System.Drawing.Size(138, 52);
            StopButton.TabIndex = 1;
            StopButton.Text = "停止";
            StopButton.UseVisualStyleBackColor = true;
            StopButton.Click += StopButton_Click;
            // 
            // RestartButton
            // 
            RestartButton.Dock = System.Windows.Forms.DockStyle.Fill;
            RestartButton.Location = new System.Drawing.Point(3, 61);
            RestartButton.Name = "RestartButton";
            RestartButton.Size = new System.Drawing.Size(138, 53);
            RestartButton.TabIndex = 2;
            RestartButton.Text = "重启";
            RestartButton.UseVisualStyleBackColor = true;
            RestartButton.Click += RestartButton_Click;
            // 
            // TerminateButton
            // 
            TerminateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            TerminateButton.Location = new System.Drawing.Point(147, 61);
            TerminateButton.Name = "TerminateButton";
            TerminateButton.Size = new System.Drawing.Size(138, 53);
            TerminateButton.TabIndex = 3;
            TerminateButton.Text = "强制结束";
            TerminateButton.UseVisualStyleBackColor = true;
            TerminateButton.Click += TerminateButton_Click;
            // 
            // ConsoleGroupBox
            // 
            ConsoleGroupBox.Controls.Add(ConsoleTableLayoutPanel);
            ConsoleGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            ConsoleGroupBox.Location = new System.Drawing.Point(303, 3);
            ConsoleGroupBox.Name = "ConsoleGroupBox";
            MainTableLayoutPanel.SetRowSpan(ConsoleGroupBox, 3);
            ConsoleGroupBox.Size = new System.Drawing.Size(974, 714);
            ConsoleGroupBox.TabIndex = 2;
            ConsoleGroupBox.TabStop = false;
            ConsoleGroupBox.Text = "控制台";
            // 
            // ConsoleTableLayoutPanel
            // 
            ConsoleTableLayoutPanel.ColumnCount = 2;
            ConsoleTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ConsoleTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            ConsoleTableLayoutPanel.Controls.Add(ConsoleBrowser, 0, 0);
            ConsoleTableLayoutPanel.Controls.Add(InputTextBox, 0, 1);
            ConsoleTableLayoutPanel.Controls.Add(EnterButton, 1, 1);
            ConsoleTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            ConsoleTableLayoutPanel.Location = new System.Drawing.Point(3, 34);
            ConsoleTableLayoutPanel.Name = "ConsoleTableLayoutPanel";
            ConsoleTableLayoutPanel.RowCount = 2;
            ConsoleTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ConsoleTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            ConsoleTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            ConsoleTableLayoutPanel.Size = new System.Drawing.Size(968, 677);
            ConsoleTableLayoutPanel.TabIndex = 0;
            // 
            // ConsoleBrowser
            // 
            ConsoleBrowser.AllowNavigation = false;
            ConsoleTableLayoutPanel.SetColumnSpan(ConsoleBrowser, 2);
            ConsoleBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            ConsoleBrowser.IsWebBrowserContextMenuEnabled = false;
            ConsoleBrowser.Location = new System.Drawing.Point(5, 5);
            ConsoleBrowser.Margin = new System.Windows.Forms.Padding(5);
            ConsoleBrowser.Name = "ConsoleBrowser";
            ConsoleBrowser.Size = new System.Drawing.Size(958, 619);
            ConsoleBrowser.TabIndex = 0;
            ConsoleBrowser.TabStop = false;
            // 
            // InputTextBox
            // 
            InputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            InputTextBox.Location = new System.Drawing.Point(3, 632);
            InputTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
            InputTextBox.Name = "InputTextBox";
            InputTextBox.Size = new System.Drawing.Size(910, 38);
            InputTextBox.TabIndex = 0;
            InputTextBox.KeyDown += InputTextBox_KeyDown;
            // 
            // EnterButton
            // 
            EnterButton.Dock = System.Windows.Forms.DockStyle.Fill;
            EnterButton.Location = new System.Drawing.Point(921, 632);
            EnterButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            EnterButton.Name = "EnterButton";
            EnterButton.Size = new System.Drawing.Size(44, 40);
            EnterButton.TabIndex = 1;
            EnterButton.Text = "▲";
            EnterButton.UseVisualStyleBackColor = true;
            EnterButton.Click += EnterButton_Click;
            // 
            // Panel
            // 
            BackColor = System.Drawing.Color.White;
            Controls.Add(MainTableLayoutPanel);
            Name = "Panel";
            Size = new System.Drawing.Size(1280, 720);
            MainTableLayoutPanel.ResumeLayout(false);
            InfoGroupBox.ResumeLayout(false);
            InformationTableLayoutPanel.ResumeLayout(false);
            InformationTableLayoutPanel.PerformLayout();
            ControlGroupBox.ResumeLayout(false);
            ControlTableLayoutPanel.ResumeLayout(false);
            ConsoleGroupBox.ResumeLayout(false);
            ConsoleTableLayoutPanel.ResumeLayout(false);
            ConsoleTableLayoutPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.Button EnterButton;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button RestartButton;
        private System.Windows.Forms.Button TerminateButton;
        private System.Windows.Forms.Label StatusDynamicLabel;
        private System.Windows.Forms.Label RunTimeDynamicLabel;
        private System.Windows.Forms.Label VersionDynamicLabel;
        private System.Windows.Forms.Label PlayerCountDynamicLabel;
        private System.Windows.Forms.Label CPUPercentDynamicLabel;
        private Serein.Lite.Ui.Controls.ConsoleWebBrowser ConsoleBrowser;
    }
}
