namespace Serein.Lite.Ui.Settings
{
    partial class AppSettingPage
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
            System.Windows.Forms.GroupBox PluginGroupBox;
            System.Windows.Forms.Label JSPatternToSkipLoadingSingleFileLabel;
            System.Windows.Forms.Label JSGlobalAssembliesLabel;
            System.Windows.Forms.Label PluginEventMaxWaitingTimeLabel;
            System.Windows.Forms.GroupBox BindingGroupBox;
            System.Windows.Forms.Label RegexForCheckingGameIDLabel;
            System.Windows.Forms.GroupBox OtherGroupBox;
            System.Windows.Forms.Label CustomTitleLabel;
            System.Windows.Forms.Label PattenForEnableMatchingMuiltLinesLabel;
            System.Windows.Forms.GroupBox UpdateGroupBox;
            System.Windows.Forms.ToolTip ToolTip;
            JSPatternToSkipLoadingSingleFileTextBox = new System.Windows.Forms.TextBox();
            JSGlobalAssembliesTextBox = new System.Windows.Forms.TextBox();
            PluginEventMaxWaitingTimeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            DisableBinderWhenServerClosedCheckBox = new System.Windows.Forms.CheckBox();
            RegexForCheckingGameIDTextBox = new System.Windows.Forms.TextBox();
            CustomTitleTextBox = new System.Windows.Forms.TextBox();
            PattenForEnableMatchingMuiltLinesTextBox = new System.Windows.Forms.TextBox();
            CheckUpdateButton = new System.Windows.Forms.Button();
            LatestVersionLabel = new System.Windows.Forms.Label();
            VersionLabel = new System.Windows.Forms.Label();
            AutoUpdateCheckBox = new System.Windows.Forms.CheckBox();
            CheckUpdateCheckBox = new System.Windows.Forms.CheckBox();
            EnableSentryCheckBox = new System.Windows.Forms.CheckBox();
            PluginGroupBox = new System.Windows.Forms.GroupBox();
            JSPatternToSkipLoadingSingleFileLabel = new System.Windows.Forms.Label();
            JSGlobalAssembliesLabel = new System.Windows.Forms.Label();
            PluginEventMaxWaitingTimeLabel = new System.Windows.Forms.Label();
            BindingGroupBox = new System.Windows.Forms.GroupBox();
            RegexForCheckingGameIDLabel = new System.Windows.Forms.Label();
            OtherGroupBox = new System.Windows.Forms.GroupBox();
            CustomTitleLabel = new System.Windows.Forms.Label();
            PattenForEnableMatchingMuiltLinesLabel = new System.Windows.Forms.Label();
            UpdateGroupBox = new System.Windows.Forms.GroupBox();
            ToolTip = new System.Windows.Forms.ToolTip(components);
            PluginGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PluginEventMaxWaitingTimeNumericUpDown).BeginInit();
            BindingGroupBox.SuspendLayout();
            OtherGroupBox.SuspendLayout();
            UpdateGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // PluginGroupBox
            // 
            PluginGroupBox.Controls.Add(JSPatternToSkipLoadingSingleFileTextBox);
            PluginGroupBox.Controls.Add(JSPatternToSkipLoadingSingleFileLabel);
            PluginGroupBox.Controls.Add(JSGlobalAssembliesTextBox);
            PluginGroupBox.Controls.Add(JSGlobalAssembliesLabel);
            PluginGroupBox.Controls.Add(PluginEventMaxWaitingTimeNumericUpDown);
            PluginGroupBox.Controls.Add(PluginEventMaxWaitingTimeLabel);
            PluginGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            PluginGroupBox.Location = new System.Drawing.Point(10, 10);
            PluginGroupBox.Name = "PluginGroupBox";
            PluginGroupBox.Size = new System.Drawing.Size(1260, 498);
            PluginGroupBox.TabIndex = 0;
            PluginGroupBox.TabStop = false;
            PluginGroupBox.Text = "插件";
            // 
            // JSPatternToSkipLoadingSingleFileTextBox
            // 
            JSPatternToSkipLoadingSingleFileTextBox.AcceptsReturn = true;
            JSPatternToSkipLoadingSingleFileTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            JSPatternToSkipLoadingSingleFileTextBox.Location = new System.Drawing.Point(27, 339);
            JSPatternToSkipLoadingSingleFileTextBox.Multiline = true;
            JSPatternToSkipLoadingSingleFileTextBox.Name = "JSPatternToSkipLoadingSingleFileTextBox";
            JSPatternToSkipLoadingSingleFileTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            JSPatternToSkipLoadingSingleFileTextBox.Size = new System.Drawing.Size(1206, 132);
            JSPatternToSkipLoadingSingleFileTextBox.TabIndex = 3;
            ToolTip.SetToolTip(JSPatternToSkipLoadingSingleFileTextBox, "以此处设定的文件后缀结尾的文件不会被当作JavaScript插件加载（一行一个）");
            JSPatternToSkipLoadingSingleFileTextBox.TextChanged += JSPatternToSkipLoadingSingleFileTextBox_TextChanged;
            // 
            // JSPatternToSkipLoadingSingleFileLabel
            // 
            JSPatternToSkipLoadingSingleFileLabel.AutoSize = true;
            JSPatternToSkipLoadingSingleFileLabel.Location = new System.Drawing.Point(27, 305);
            JSPatternToSkipLoadingSingleFileLabel.Name = "JSPatternToSkipLoadingSingleFileLabel";
            JSPatternToSkipLoadingSingleFileLabel.Size = new System.Drawing.Size(326, 31);
            JSPatternToSkipLoadingSingleFileLabel.TabIndex = 4;
            JSPatternToSkipLoadingSingleFileLabel.Text = "JS插件加载时忽略的文件后缀";
            ToolTip.SetToolTip(JSPatternToSkipLoadingSingleFileLabel, "以此处设定的文件后缀结尾的文件不会被当作JavaScript插件加载（一行一个）");
            // 
            // JSGlobalAssembliesTextBox
            // 
            JSGlobalAssembliesTextBox.AcceptsReturn = true;
            JSGlobalAssembliesTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            JSGlobalAssembliesTextBox.Location = new System.Drawing.Point(27, 163);
            JSGlobalAssembliesTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            JSGlobalAssembliesTextBox.Multiline = true;
            JSGlobalAssembliesTextBox.Name = "JSGlobalAssembliesTextBox";
            JSGlobalAssembliesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            JSGlobalAssembliesTextBox.Size = new System.Drawing.Size(1206, 132);
            JSGlobalAssembliesTextBox.TabIndex = 1;
            ToolTip.SetToolTip(JSGlobalAssembliesTextBox, "此处填写的程序集将被所有JavaScript插件加载（一行一个）");
            JSGlobalAssembliesTextBox.TextChanged += JSGlobalAssembliesTextBox_TextChanged;
            // 
            // JSGlobalAssembliesLabel
            // 
            JSGlobalAssembliesLabel.AutoSize = true;
            JSGlobalAssembliesLabel.Location = new System.Drawing.Point(27, 129);
            JSGlobalAssembliesLabel.Name = "JSGlobalAssembliesLabel";
            JSGlobalAssembliesLabel.Size = new System.Drawing.Size(278, 31);
            JSGlobalAssembliesLabel.TabIndex = 2;
            JSGlobalAssembliesLabel.Text = "JS插件全局加载的程序集";
            ToolTip.SetToolTip(JSGlobalAssembliesLabel, "此处填写的程序集将被所有JavaScript插件加载（一行一个）");
            // 
            // PluginEventMaxWaitingTimeNumericUpDown
            // 
            PluginEventMaxWaitingTimeNumericUpDown.Location = new System.Drawing.Point(27, 81);
            PluginEventMaxWaitingTimeNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            PluginEventMaxWaitingTimeNumericUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            PluginEventMaxWaitingTimeNumericUpDown.Name = "PluginEventMaxWaitingTimeNumericUpDown";
            PluginEventMaxWaitingTimeNumericUpDown.Size = new System.Drawing.Size(223, 38);
            PluginEventMaxWaitingTimeNumericUpDown.TabIndex = 1;
            ToolTip.SetToolTip(PluginEventMaxWaitingTimeNumericUpDown, "超过此时间返回的监听结果将被忽略");
            PluginEventMaxWaitingTimeNumericUpDown.ValueChanged += OnPropertyChanged;
            // 
            // PluginEventMaxWaitingTimeLabel
            // 
            PluginEventMaxWaitingTimeLabel.AutoSize = true;
            PluginEventMaxWaitingTimeLabel.Location = new System.Drawing.Point(27, 47);
            PluginEventMaxWaitingTimeLabel.Name = "PluginEventMaxWaitingTimeLabel";
            PluginEventMaxWaitingTimeLabel.Size = new System.Drawing.Size(262, 31);
            PluginEventMaxWaitingTimeLabel.TabIndex = 0;
            PluginEventMaxWaitingTimeLabel.Text = "事件最大等待时间 (ms)";
            ToolTip.SetToolTip(PluginEventMaxWaitingTimeLabel, "超过此时间返回的监听结果将被忽略");
            // 
            // BindingGroupBox
            // 
            BindingGroupBox.Controls.Add(DisableBinderWhenServerClosedCheckBox);
            BindingGroupBox.Controls.Add(RegexForCheckingGameIDTextBox);
            BindingGroupBox.Controls.Add(RegexForCheckingGameIDLabel);
            BindingGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            BindingGroupBox.Location = new System.Drawing.Point(10, 508);
            BindingGroupBox.Name = "BindingGroupBox";
            BindingGroupBox.Size = new System.Drawing.Size(1260, 187);
            BindingGroupBox.TabIndex = 1;
            BindingGroupBox.TabStop = false;
            BindingGroupBox.Text = "绑定";
            // 
            // DisableBinderWhenServerClosedCheckBox
            // 
            DisableBinderWhenServerClosedCheckBox.AutoSize = true;
            DisableBinderWhenServerClosedCheckBox.Location = new System.Drawing.Point(27, 130);
            DisableBinderWhenServerClosedCheckBox.Name = "DisableBinderWhenServerClosedCheckBox";
            DisableBinderWhenServerClosedCheckBox.Size = new System.Drawing.Size(358, 35);
            DisableBinderWhenServerClosedCheckBox.TabIndex = 7;
            DisableBinderWhenServerClosedCheckBox.Text = "当服务器关闭时禁用绑定功能";
            DisableBinderWhenServerClosedCheckBox.UseVisualStyleBackColor = true;
            DisableBinderWhenServerClosedCheckBox.Click += OnPropertyChanged;
            // 
            // RegexForCheckingGameIDTextBox
            // 
            RegexForCheckingGameIDTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            RegexForCheckingGameIDTextBox.Location = new System.Drawing.Point(27, 79);
            RegexForCheckingGameIDTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            RegexForCheckingGameIDTextBox.Name = "RegexForCheckingGameIDTextBox";
            RegexForCheckingGameIDTextBox.Size = new System.Drawing.Size(1206, 38);
            RegexForCheckingGameIDTextBox.TabIndex = 6;
            ToolTip.SetToolTip(RegexForCheckingGameIDTextBox, "用于绑定功能中校验游戏名称的正确与否");
            RegexForCheckingGameIDTextBox.TextChanged += OnPropertyChanged;
            // 
            // RegexForCheckingGameIDLabel
            // 
            RegexForCheckingGameIDLabel.AutoSize = true;
            RegexForCheckingGameIDLabel.Location = new System.Drawing.Point(27, 45);
            RegexForCheckingGameIDLabel.Name = "RegexForCheckingGameIDLabel";
            RegexForCheckingGameIDLabel.Size = new System.Drawing.Size(206, 31);
            RegexForCheckingGameIDLabel.TabIndex = 5;
            RegexForCheckingGameIDLabel.Text = "游戏名称检验正则";
            ToolTip.SetToolTip(RegexForCheckingGameIDLabel, "用于绑定功能中校验游戏名称的正确与否");
            // 
            // OtherGroupBox
            // 
            OtherGroupBox.Controls.Add(EnableSentryCheckBox);
            OtherGroupBox.Controls.Add(CustomTitleTextBox);
            OtherGroupBox.Controls.Add(CustomTitleLabel);
            OtherGroupBox.Controls.Add(PattenForEnableMatchingMuiltLinesTextBox);
            OtherGroupBox.Controls.Add(PattenForEnableMatchingMuiltLinesLabel);
            OtherGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            OtherGroupBox.Location = new System.Drawing.Point(10, 695);
            OtherGroupBox.Name = "OtherGroupBox";
            OtherGroupBox.Size = new System.Drawing.Size(1260, 359);
            OtherGroupBox.TabIndex = 2;
            OtherGroupBox.TabStop = false;
            OtherGroupBox.Text = "其他";
            // 
            // CustomTitleTextBox
            // 
            CustomTitleTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CustomTitleTextBox.Location = new System.Drawing.Point(27, 254);
            CustomTitleTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            CustomTitleTextBox.Name = "CustomTitleTextBox";
            CustomTitleTextBox.Size = new System.Drawing.Size(1206, 38);
            CustomTitleTextBox.TabIndex = 11;
            ToolTip.SetToolTip(CustomTitleTextBox, "标题栏后缀，可以使用命令变量");
            CustomTitleTextBox.TextChanged += OnPropertyChanged;
            // 
            // CustomTitleLabel
            // 
            CustomTitleLabel.AutoSize = true;
            CustomTitleLabel.Location = new System.Drawing.Point(27, 220);
            CustomTitleLabel.Name = "CustomTitleLabel";
            CustomTitleLabel.Size = new System.Drawing.Size(110, 31);
            CustomTitleLabel.TabIndex = 10;
            CustomTitleLabel.Text = "标题后缀";
            ToolTip.SetToolTip(CustomTitleLabel, "标题栏后缀，可以使用命令变量");
            // 
            // PattenForEnableMatchingMuiltLinesTextBox
            // 
            PattenForEnableMatchingMuiltLinesTextBox.AcceptsReturn = true;
            PattenForEnableMatchingMuiltLinesTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PattenForEnableMatchingMuiltLinesTextBox.Location = new System.Drawing.Point(27, 78);
            PattenForEnableMatchingMuiltLinesTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            PattenForEnableMatchingMuiltLinesTextBox.Multiline = true;
            PattenForEnableMatchingMuiltLinesTextBox.Name = "PattenForEnableMatchingMuiltLinesTextBox";
            PattenForEnableMatchingMuiltLinesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            PattenForEnableMatchingMuiltLinesTextBox.Size = new System.Drawing.Size(1206, 132);
            PattenForEnableMatchingMuiltLinesTextBox.TabIndex = 9;
            ToolTip.SetToolTip(PattenForEnableMatchingMuiltLinesTextBox, "一行一个；不支持正则表达式");
            PattenForEnableMatchingMuiltLinesTextBox.TextChanged += PattenForEnableMatchingMuiltLinesTextBox_TextChanged;
            // 
            // PattenForEnableMatchingMuiltLinesLabel
            // 
            PattenForEnableMatchingMuiltLinesLabel.AutoSize = true;
            PattenForEnableMatchingMuiltLinesLabel.Location = new System.Drawing.Point(27, 44);
            PattenForEnableMatchingMuiltLinesLabel.Name = "PattenForEnableMatchingMuiltLinesLabel";
            PattenForEnableMatchingMuiltLinesLabel.Size = new System.Drawing.Size(278, 31);
            PattenForEnableMatchingMuiltLinesLabel.TabIndex = 8;
            PattenForEnableMatchingMuiltLinesLabel.Text = "用于触发多行匹配的文本";
            ToolTip.SetToolTip(PattenForEnableMatchingMuiltLinesLabel, "一行一个；不支持正则表达式");
            // 
            // UpdateGroupBox
            // 
            UpdateGroupBox.Controls.Add(CheckUpdateButton);
            UpdateGroupBox.Controls.Add(LatestVersionLabel);
            UpdateGroupBox.Controls.Add(VersionLabel);
            UpdateGroupBox.Controls.Add(AutoUpdateCheckBox);
            UpdateGroupBox.Controls.Add(CheckUpdateCheckBox);
            UpdateGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            UpdateGroupBox.Location = new System.Drawing.Point(10, 1054);
            UpdateGroupBox.Name = "UpdateGroupBox";
            UpdateGroupBox.Size = new System.Drawing.Size(1260, 196);
            UpdateGroupBox.TabIndex = 3;
            UpdateGroupBox.TabStop = false;
            UpdateGroupBox.Text = "更新";
            // 
            // CheckUpdateButton
            // 
            CheckUpdateButton.Location = new System.Drawing.Point(27, 128);
            CheckUpdateButton.Name = "CheckUpdateButton";
            CheckUpdateButton.Size = new System.Drawing.Size(150, 42);
            CheckUpdateButton.TabIndex = 4;
            CheckUpdateButton.Text = "检查更新";
            CheckUpdateButton.UseVisualStyleBackColor = true;
            CheckUpdateButton.Click += CheckUpdateButton_Click;
            // 
            // LatestVersionLabel
            // 
            LatestVersionLabel.AutoSize = true;
            LatestVersionLabel.Location = new System.Drawing.Point(340, 81);
            LatestVersionLabel.Name = "LatestVersionLabel";
            LatestVersionLabel.Size = new System.Drawing.Size(110, 31);
            LatestVersionLabel.TabIndex = 3;
            LatestVersionLabel.Text = "最新版本";
            // 
            // VersionLabel
            // 
            VersionLabel.AutoSize = true;
            VersionLabel.Location = new System.Drawing.Point(340, 41);
            VersionLabel.Name = "VersionLabel";
            VersionLabel.Size = new System.Drawing.Size(110, 31);
            VersionLabel.TabIndex = 2;
            VersionLabel.Text = "当前版本";
            // 
            // AutoUpdateCheckBox
            // 
            AutoUpdateCheckBox.AutoSize = true;
            AutoUpdateCheckBox.Location = new System.Drawing.Point(27, 81);
            AutoUpdateCheckBox.Name = "AutoUpdateCheckBox";
            AutoUpdateCheckBox.Size = new System.Drawing.Size(142, 35);
            AutoUpdateCheckBox.TabIndex = 1;
            AutoUpdateCheckBox.Text = "自动更新";
            ToolTip.SetToolTip(AutoUpdateCheckBox, "当获取到新版本时在自动后台下载，并于程序退出后自动替换");
            AutoUpdateCheckBox.UseVisualStyleBackColor = true;
            AutoUpdateCheckBox.Click += OnPropertyChanged;
            // 
            // CheckUpdateCheckBox
            // 
            CheckUpdateCheckBox.AutoSize = true;
            CheckUpdateCheckBox.Location = new System.Drawing.Point(27, 40);
            CheckUpdateCheckBox.Name = "CheckUpdateCheckBox";
            CheckUpdateCheckBox.Size = new System.Drawing.Size(190, 35);
            CheckUpdateCheckBox.TabIndex = 0;
            CheckUpdateCheckBox.Text = "获取更新提示";
            ToolTip.SetToolTip(CheckUpdateCheckBox, "启动后每隔一段时间获取一次更新");
            CheckUpdateCheckBox.UseVisualStyleBackColor = true;
            CheckUpdateCheckBox.Click += OnPropertyChanged;
            // 
            // EnableSentryCheckBox
            // 
            EnableSentryCheckBox.AutoSize = true;
            EnableSentryCheckBox.Location = new System.Drawing.Point(27, 305);
            EnableSentryCheckBox.Name = "EnableSentryCheckBox";
            EnableSentryCheckBox.Size = new System.Drawing.Size(216, 35);
            EnableSentryCheckBox.TabIndex = 8;
            EnableSentryCheckBox.Text = "使用Sentry上报";
            ToolTip.SetToolTip(EnableSentryCheckBox, "当出现异常或崩溃时自动匿名上报（建议开启；重启生效；上报时可能会收集部分系统和环境信息并产生一定网络流量）");
            EnableSentryCheckBox.UseVisualStyleBackColor = true;
            // 
            // AppSettingPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(UpdateGroupBox);
            Controls.Add(OtherGroupBox);
            Controls.Add(BindingGroupBox);
            Controls.Add(PluginGroupBox);
            Name = "AppSettingPage";
            Padding = new System.Windows.Forms.Padding(10);
            Size = new System.Drawing.Size(1280, 1265);
            PluginGroupBox.ResumeLayout(false);
            PluginGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PluginEventMaxWaitingTimeNumericUpDown).EndInit();
            BindingGroupBox.ResumeLayout(false);
            BindingGroupBox.PerformLayout();
            OtherGroupBox.ResumeLayout(false);
            OtherGroupBox.PerformLayout();
            UpdateGroupBox.ResumeLayout(false);
            UpdateGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox JSGlobalAssembliesTextBox;
        private System.Windows.Forms.NumericUpDown PluginEventMaxWaitingTimeNumericUpDown;
        private System.Windows.Forms.TextBox JSPatternToSkipLoadingSingleFileTextBox;
        private System.Windows.Forms.CheckBox DisableBinderWhenServerClosedCheckBox;
        private System.Windows.Forms.TextBox RegexForCheckingGameIDTextBox;
        private System.Windows.Forms.TextBox PattenForEnableMatchingMuiltLinesTextBox;
        private System.Windows.Forms.TextBox CustomTitleTextBox;
        private System.Windows.Forms.Button CheckUpdateButton;
        private System.Windows.Forms.Label LatestVersionLabel;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.CheckBox AutoUpdateCheckBox;
        private System.Windows.Forms.CheckBox CheckUpdateCheckBox;
        private System.Windows.Forms.CheckBox EnableSentryCheckBox;
    }
}
