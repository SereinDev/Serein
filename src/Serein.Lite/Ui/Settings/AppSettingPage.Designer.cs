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
            System.Windows.Forms.GroupBox PluginGroupBox;
            System.Windows.Forms.Label JSPatternToSkipLoadingSpecifiedFileLabel;
            System.Windows.Forms.Label JSGlobalAssembliesLabel;
            System.Windows.Forms.Label PluginEventMaxWaitingTimeLabel;
            System.Windows.Forms.GroupBox BindingGroupBox;
            System.Windows.Forms.Label RegexForCheckingGameIDLabel;
            System.Windows.Forms.GroupBox OtherGroupBox;
            System.Windows.Forms.Label CustomTitleLabel;
            System.Windows.Forms.Label PattenForEnableMatchMuiltLinesLabel;
            System.Windows.Forms.GroupBox UpdateGroupBox;
            JSPatternToSkipLoadingSpecifiedFileTextBox = new System.Windows.Forms.TextBox();
            JSGlobalAssembliesTextBox = new System.Windows.Forms.TextBox();
            PluginEventMaxWaitingTimeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            DisableBinderWhenServerClosedCheckBox = new System.Windows.Forms.CheckBox();
            RegexForCheckingGameIDTextBox = new System.Windows.Forms.TextBox();
            CustomTitleTextBox = new System.Windows.Forms.TextBox();
            PattenForEnableMatchMuiltLinesTextBox = new System.Windows.Forms.TextBox();
            CheckUpdateButton = new System.Windows.Forms.Button();
            LatestVersionLabel = new System.Windows.Forms.Label();
            VersionLabel = new System.Windows.Forms.Label();
            AutoUpdateCheckBox = new System.Windows.Forms.CheckBox();
            CheckUpdateCheckBox = new System.Windows.Forms.CheckBox();
            PluginGroupBox = new System.Windows.Forms.GroupBox();
            JSPatternToSkipLoadingSpecifiedFileLabel = new System.Windows.Forms.Label();
            JSGlobalAssembliesLabel = new System.Windows.Forms.Label();
            PluginEventMaxWaitingTimeLabel = new System.Windows.Forms.Label();
            BindingGroupBox = new System.Windows.Forms.GroupBox();
            RegexForCheckingGameIDLabel = new System.Windows.Forms.Label();
            OtherGroupBox = new System.Windows.Forms.GroupBox();
            CustomTitleLabel = new System.Windows.Forms.Label();
            PattenForEnableMatchMuiltLinesLabel = new System.Windows.Forms.Label();
            UpdateGroupBox = new System.Windows.Forms.GroupBox();
            PluginGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PluginEventMaxWaitingTimeNumericUpDown).BeginInit();
            BindingGroupBox.SuspendLayout();
            OtherGroupBox.SuspendLayout();
            UpdateGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // PluginGroupBox
            // 
            PluginGroupBox.Controls.Add(JSPatternToSkipLoadingSpecifiedFileTextBox);
            PluginGroupBox.Controls.Add(JSPatternToSkipLoadingSpecifiedFileLabel);
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
            // JSPatternToSkipLoadingSpecifiedFileTextBox
            // 
            JSPatternToSkipLoadingSpecifiedFileTextBox.AcceptsReturn = true;
            JSPatternToSkipLoadingSpecifiedFileTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            JSPatternToSkipLoadingSpecifiedFileTextBox.Location = new System.Drawing.Point(27, 339);
            JSPatternToSkipLoadingSpecifiedFileTextBox.Multiline = true;
            JSPatternToSkipLoadingSpecifiedFileTextBox.Name = "JSPatternToSkipLoadingSpecifiedFileTextBox";
            JSPatternToSkipLoadingSpecifiedFileTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            JSPatternToSkipLoadingSpecifiedFileTextBox.Size = new System.Drawing.Size(1206, 132);
            JSPatternToSkipLoadingSpecifiedFileTextBox.TabIndex = 3;
            JSPatternToSkipLoadingSpecifiedFileTextBox.TextChanged += JSPatternToSkipLoadingSpecifiedFileTextBox_TextChanged;
            // 
            // JSPatternToSkipLoadingSpecifiedFileLabel
            // 
            JSPatternToSkipLoadingSpecifiedFileLabel.AutoSize = true;
            JSPatternToSkipLoadingSpecifiedFileLabel.Location = new System.Drawing.Point(27, 305);
            JSPatternToSkipLoadingSpecifiedFileLabel.Name = "JSPatternToSkipLoadingSpecifiedFileLabel";
            JSPatternToSkipLoadingSpecifiedFileLabel.Size = new System.Drawing.Size(326, 31);
            JSPatternToSkipLoadingSpecifiedFileLabel.TabIndex = 4;
            JSPatternToSkipLoadingSpecifiedFileLabel.Text = "JS插件加载时忽略的文件后缀";
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
            // 
            // PluginEventMaxWaitingTimeNumericUpDown
            // 
            PluginEventMaxWaitingTimeNumericUpDown.Location = new System.Drawing.Point(27, 81);
            PluginEventMaxWaitingTimeNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            PluginEventMaxWaitingTimeNumericUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            PluginEventMaxWaitingTimeNumericUpDown.Name = "PluginEventMaxWaitingTimeNumericUpDown";
            PluginEventMaxWaitingTimeNumericUpDown.Size = new System.Drawing.Size(223, 38);
            PluginEventMaxWaitingTimeNumericUpDown.TabIndex = 1;
            // 
            // PluginEventMaxWaitingTimeLabel
            // 
            PluginEventMaxWaitingTimeLabel.AutoSize = true;
            PluginEventMaxWaitingTimeLabel.Location = new System.Drawing.Point(27, 47);
            PluginEventMaxWaitingTimeLabel.Name = "PluginEventMaxWaitingTimeLabel";
            PluginEventMaxWaitingTimeLabel.Size = new System.Drawing.Size(262, 31);
            PluginEventMaxWaitingTimeLabel.TabIndex = 0;
            PluginEventMaxWaitingTimeLabel.Text = "事件最大等待时间 (ms)";
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
            // 
            // RegexForCheckingGameIDLabel
            // 
            RegexForCheckingGameIDLabel.AutoSize = true;
            RegexForCheckingGameIDLabel.Location = new System.Drawing.Point(27, 45);
            RegexForCheckingGameIDLabel.Name = "RegexForCheckingGameIDLabel";
            RegexForCheckingGameIDLabel.Size = new System.Drawing.Size(206, 31);
            RegexForCheckingGameIDLabel.TabIndex = 5;
            RegexForCheckingGameIDLabel.Text = "游戏名称检验正则";
            // 
            // OtherGroupBox
            // 
            OtherGroupBox.Controls.Add(CustomTitleTextBox);
            OtherGroupBox.Controls.Add(CustomTitleLabel);
            OtherGroupBox.Controls.Add(PattenForEnableMatchMuiltLinesTextBox);
            OtherGroupBox.Controls.Add(PattenForEnableMatchMuiltLinesLabel);
            OtherGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            OtherGroupBox.Location = new System.Drawing.Point(10, 695);
            OtherGroupBox.Name = "OtherGroupBox";
            OtherGroupBox.Size = new System.Drawing.Size(1260, 309);
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
            // 
            // CustomTitleLabel
            // 
            CustomTitleLabel.AutoSize = true;
            CustomTitleLabel.Location = new System.Drawing.Point(27, 220);
            CustomTitleLabel.Name = "CustomTitleLabel";
            CustomTitleLabel.Size = new System.Drawing.Size(110, 31);
            CustomTitleLabel.TabIndex = 10;
            CustomTitleLabel.Text = "标题后缀";
            // 
            // PattenForEnableMatchMuiltLinesTextBox
            // 
            PattenForEnableMatchMuiltLinesTextBox.AcceptsReturn = true;
            PattenForEnableMatchMuiltLinesTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PattenForEnableMatchMuiltLinesTextBox.Location = new System.Drawing.Point(27, 78);
            PattenForEnableMatchMuiltLinesTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            PattenForEnableMatchMuiltLinesTextBox.Multiline = true;
            PattenForEnableMatchMuiltLinesTextBox.Name = "PattenForEnableMatchMuiltLinesTextBox";
            PattenForEnableMatchMuiltLinesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            PattenForEnableMatchMuiltLinesTextBox.Size = new System.Drawing.Size(1206, 132);
            PattenForEnableMatchMuiltLinesTextBox.TabIndex = 9;
            PattenForEnableMatchMuiltLinesTextBox.TextChanged += PattenForEnableMatchMuiltLinesTextBox_TextChanged;
            // 
            // PattenForEnableMatchMuiltLinesLabel
            // 
            PattenForEnableMatchMuiltLinesLabel.AutoSize = true;
            PattenForEnableMatchMuiltLinesLabel.Location = new System.Drawing.Point(27, 44);
            PattenForEnableMatchMuiltLinesLabel.Name = "PattenForEnableMatchMuiltLinesLabel";
            PattenForEnableMatchMuiltLinesLabel.Size = new System.Drawing.Size(278, 31);
            PattenForEnableMatchMuiltLinesLabel.TabIndex = 8;
            PattenForEnableMatchMuiltLinesLabel.Text = "用于触发多行匹配的文本";
            // 
            // UpdateGroupBox
            // 
            UpdateGroupBox.Controls.Add(CheckUpdateButton);
            UpdateGroupBox.Controls.Add(LatestVersionLabel);
            UpdateGroupBox.Controls.Add(VersionLabel);
            UpdateGroupBox.Controls.Add(AutoUpdateCheckBox);
            UpdateGroupBox.Controls.Add(CheckUpdateCheckBox);
            UpdateGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            UpdateGroupBox.Location = new System.Drawing.Point(10, 1004);
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
            CheckUpdateCheckBox.UseVisualStyleBackColor = true;
            CheckUpdateCheckBox.Click += OnPropertyChanged;
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
            Size = new System.Drawing.Size(1280, 1211);
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
        private System.Windows.Forms.TextBox JSPatternToSkipLoadingSpecifiedFileTextBox;
        private System.Windows.Forms.CheckBox DisableBinderWhenServerClosedCheckBox;
        private System.Windows.Forms.TextBox RegexForCheckingGameIDTextBox;
        private System.Windows.Forms.TextBox PattenForEnableMatchMuiltLinesTextBox;
        private System.Windows.Forms.TextBox CustomTitleTextBox;
        private System.Windows.Forms.Button CheckUpdateButton;
        private System.Windows.Forms.Label LatestVersionLabel;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.CheckBox AutoUpdateCheckBox;
        private System.Windows.Forms.CheckBox CheckUpdateCheckBox;
    }
}
