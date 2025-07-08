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
            System.Windows.Forms.GroupBox pluginGroupBox;
            System.Windows.Forms.Label jsPatternToSkipLoadingSingleFileLabel;
            System.Windows.Forms.Label jsGlobalAssembliesLabel;
            System.Windows.Forms.Label pluginEventMaxWaitingTimeLabel;
            System.Windows.Forms.GroupBox bindingGroupBox;
            System.Windows.Forms.Label regexForCheckingGameIDLabel;
            System.Windows.Forms.GroupBox moreGroupBox;
            System.Windows.Forms.Label customTitleLabel;
            System.Windows.Forms.Label pattenForEnableMatchingMuiltLinesLabel;
            System.Windows.Forms.GroupBox updateGroupBox;
            System.Windows.Forms.ToolTip toolTip;
            _jsPatternToSkipLoadingSingleFileTextBox = new System.Windows.Forms.TextBox();
            _jsGlobalAssembliesTextBox = new System.Windows.Forms.TextBox();
            _pluginEventMaxWaitingTimeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            _disableBindingManagerWhenServerClosedCheckBox = new System.Windows.Forms.CheckBox();
            _regexForCheckingGameIDTextBox = new System.Windows.Forms.TextBox();
            _enableSentryCheckBox = new System.Windows.Forms.CheckBox();
            _customTitleTextBox = new System.Windows.Forms.TextBox();
            _pattenForEnableMatchingMuiltLinesTextBox = new System.Windows.Forms.TextBox();
            _checkUpdateButton = new System.Windows.Forms.Button();
            _latestVersionLabel = new System.Windows.Forms.Label();
            _versionLabel = new System.Windows.Forms.Label();
            _autoUpdateCheckBox = new System.Windows.Forms.CheckBox();
            _checkUpdateCheckBox = new System.Windows.Forms.CheckBox();
            pluginGroupBox = new System.Windows.Forms.GroupBox();
            jsPatternToSkipLoadingSingleFileLabel = new System.Windows.Forms.Label();
            jsGlobalAssembliesLabel = new System.Windows.Forms.Label();
            pluginEventMaxWaitingTimeLabel = new System.Windows.Forms.Label();
            bindingGroupBox = new System.Windows.Forms.GroupBox();
            regexForCheckingGameIDLabel = new System.Windows.Forms.Label();
            moreGroupBox = new System.Windows.Forms.GroupBox();
            customTitleLabel = new System.Windows.Forms.Label();
            pattenForEnableMatchingMuiltLinesLabel = new System.Windows.Forms.Label();
            updateGroupBox = new System.Windows.Forms.GroupBox();
            toolTip = new System.Windows.Forms.ToolTip(components);
            pluginGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_pluginEventMaxWaitingTimeNumericUpDown).BeginInit();
            bindingGroupBox.SuspendLayout();
            moreGroupBox.SuspendLayout();
            updateGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // pluginGroupBox
            // 
            pluginGroupBox.Controls.Add(_jsPatternToSkipLoadingSingleFileTextBox);
            pluginGroupBox.Controls.Add(jsPatternToSkipLoadingSingleFileLabel);
            pluginGroupBox.Controls.Add(_jsGlobalAssembliesTextBox);
            pluginGroupBox.Controls.Add(jsGlobalAssembliesLabel);
            pluginGroupBox.Controls.Add(_pluginEventMaxWaitingTimeNumericUpDown);
            pluginGroupBox.Controls.Add(pluginEventMaxWaitingTimeLabel);
            pluginGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            pluginGroupBox.Location = new System.Drawing.Point(10, 10);
            pluginGroupBox.Name = "pluginGroupBox";
            pluginGroupBox.Size = new System.Drawing.Size(1260, 498);
            pluginGroupBox.TabIndex = 0;
            pluginGroupBox.TabStop = false;
            pluginGroupBox.Text = "插件";
            // 
            // _jsPatternToSkipLoadingSingleFileTextBox
            // 
            _jsPatternToSkipLoadingSingleFileTextBox.AcceptsReturn = true;
            _jsPatternToSkipLoadingSingleFileTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _jsPatternToSkipLoadingSingleFileTextBox.Location = new System.Drawing.Point(27, 339);
            _jsPatternToSkipLoadingSingleFileTextBox.Multiline = true;
            _jsPatternToSkipLoadingSingleFileTextBox.Name = "_jsPatternToSkipLoadingSingleFileTextBox";
            _jsPatternToSkipLoadingSingleFileTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _jsPatternToSkipLoadingSingleFileTextBox.Size = new System.Drawing.Size(1206, 132);
            _jsPatternToSkipLoadingSingleFileTextBox.TabIndex = 3;
            toolTip.SetToolTip(_jsPatternToSkipLoadingSingleFileTextBox, "凡是以所选内容结尾的文件都不会被加载（一行一个）");
            _jsPatternToSkipLoadingSingleFileTextBox.TextChanged += JSPatternToSkipLoadingSingleFileTextBox_TextChanged;
            // 
            // jsPatternToSkipLoadingSingleFileLabel
            // 
            jsPatternToSkipLoadingSingleFileLabel.AutoSize = true;
            jsPatternToSkipLoadingSingleFileLabel.Location = new System.Drawing.Point(27, 305);
            jsPatternToSkipLoadingSingleFileLabel.Name = "jsPatternToSkipLoadingSingleFileLabel";
            jsPatternToSkipLoadingSingleFileLabel.Size = new System.Drawing.Size(326, 31);
            jsPatternToSkipLoadingSingleFileLabel.TabIndex = 4;
            jsPatternToSkipLoadingSingleFileLabel.Text = "JS插件加载时忽略的文件后缀";
            toolTip.SetToolTip(jsPatternToSkipLoadingSingleFileLabel, "凡是以所选内容结尾的文件都不会被加载（一行一个）");
            // 
            // _jsGlobalAssembliesTextBox
            // 
            _jsGlobalAssembliesTextBox.AcceptsReturn = true;
            _jsGlobalAssembliesTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _jsGlobalAssembliesTextBox.Location = new System.Drawing.Point(27, 163);
            _jsGlobalAssembliesTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _jsGlobalAssembliesTextBox.Multiline = true;
            _jsGlobalAssembliesTextBox.Name = "_jsGlobalAssembliesTextBox";
            _jsGlobalAssembliesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _jsGlobalAssembliesTextBox.Size = new System.Drawing.Size(1206, 132);
            _jsGlobalAssembliesTextBox.TabIndex = 1;
            toolTip.SetToolTip(_jsGlobalAssembliesTextBox, "此处的程序集将会被所有JS插件加载（一行一个）\r\n");
            _jsGlobalAssembliesTextBox.TextChanged += JSGlobalAssembliesTextBox_TextChanged;
            // 
            // jsGlobalAssembliesLabel
            // 
            jsGlobalAssembliesLabel.AutoSize = true;
            jsGlobalAssembliesLabel.Location = new System.Drawing.Point(27, 129);
            jsGlobalAssembliesLabel.Name = "jsGlobalAssembliesLabel";
            jsGlobalAssembliesLabel.Size = new System.Drawing.Size(278, 31);
            jsGlobalAssembliesLabel.TabIndex = 2;
            jsGlobalAssembliesLabel.Text = "JS插件全局加载的程序集";
            toolTip.SetToolTip(jsGlobalAssembliesLabel, "此处的程序集将会被所有JS插件加载（一行一个）\r\n");
            // 
            // _pluginEventMaxWaitingTimeNumericUpDown
            // 
            _pluginEventMaxWaitingTimeNumericUpDown.Location = new System.Drawing.Point(27, 81);
            _pluginEventMaxWaitingTimeNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _pluginEventMaxWaitingTimeNumericUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            _pluginEventMaxWaitingTimeNumericUpDown.Name = "_pluginEventMaxWaitingTimeNumericUpDown";
            _pluginEventMaxWaitingTimeNumericUpDown.Size = new System.Drawing.Size(223, 38);
            _pluginEventMaxWaitingTimeNumericUpDown.TabIndex = 1;
            toolTip.SetToolTip(_pluginEventMaxWaitingTimeNumericUpDown, "超出此时间返回的结果将被忽略；设置成0可禁用等待");
            _pluginEventMaxWaitingTimeNumericUpDown.ValueChanged += OnPropertyChanged;
            // 
            // pluginEventMaxWaitingTimeLabel
            // 
            pluginEventMaxWaitingTimeLabel.AutoSize = true;
            pluginEventMaxWaitingTimeLabel.Location = new System.Drawing.Point(27, 47);
            pluginEventMaxWaitingTimeLabel.Name = "pluginEventMaxWaitingTimeLabel";
            pluginEventMaxWaitingTimeLabel.Size = new System.Drawing.Size(262, 31);
            pluginEventMaxWaitingTimeLabel.TabIndex = 0;
            pluginEventMaxWaitingTimeLabel.Text = "事件最大等待时间 (ms)";
            toolTip.SetToolTip(pluginEventMaxWaitingTimeLabel, "超出此时间返回的结果将被忽略；设置成0可禁用等待");
            // 
            // bindingGroupBox
            // 
            bindingGroupBox.Controls.Add(_disableBindingManagerWhenServerClosedCheckBox);
            bindingGroupBox.Controls.Add(_regexForCheckingGameIDTextBox);
            bindingGroupBox.Controls.Add(regexForCheckingGameIDLabel);
            bindingGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            bindingGroupBox.Location = new System.Drawing.Point(10, 508);
            bindingGroupBox.Name = "bindingGroupBox";
            bindingGroupBox.Size = new System.Drawing.Size(1260, 187);
            bindingGroupBox.TabIndex = 1;
            bindingGroupBox.TabStop = false;
            bindingGroupBox.Text = "绑定";
            // 
            // _disableBindingManagerWhenServerClosedCheckBox
            // 
            _disableBindingManagerWhenServerClosedCheckBox.AutoSize = true;
            _disableBindingManagerWhenServerClosedCheckBox.Location = new System.Drawing.Point(27, 130);
            _disableBindingManagerWhenServerClosedCheckBox.Name = "_disableBindingManagerWhenServerClosedCheckBox";
            _disableBindingManagerWhenServerClosedCheckBox.Size = new System.Drawing.Size(358, 35);
            _disableBindingManagerWhenServerClosedCheckBox.TabIndex = 7;
            _disableBindingManagerWhenServerClosedCheckBox.Text = "当服务器关闭时禁用绑定功能";
            toolTip.SetToolTip(_disableBindingManagerWhenServerClosedCheckBox, "只影响通过Serein命令执行的绑定");
            _disableBindingManagerWhenServerClosedCheckBox.UseVisualStyleBackColor = true;
            _disableBindingManagerWhenServerClosedCheckBox.Click += OnPropertyChanged;
            // 
            // _regexForCheckingGameIDTextBox
            // 
            _regexForCheckingGameIDTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _regexForCheckingGameIDTextBox.Location = new System.Drawing.Point(27, 79);
            _regexForCheckingGameIDTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _regexForCheckingGameIDTextBox.Name = "_regexForCheckingGameIDTextBox";
            _regexForCheckingGameIDTextBox.Size = new System.Drawing.Size(1206, 38);
            _regexForCheckingGameIDTextBox.TabIndex = 6;
            toolTip.SetToolTip(_regexForCheckingGameIDTextBox, "绑定时游戏名称需要符合此正则");
            _regexForCheckingGameIDTextBox.TextChanged += OnPropertyChanged;
            // 
            // regexForCheckingGameIDLabel
            // 
            regexForCheckingGameIDLabel.AutoSize = true;
            regexForCheckingGameIDLabel.Location = new System.Drawing.Point(27, 45);
            regexForCheckingGameIDLabel.Name = "regexForCheckingGameIDLabel";
            regexForCheckingGameIDLabel.Size = new System.Drawing.Size(206, 31);
            regexForCheckingGameIDLabel.TabIndex = 5;
            regexForCheckingGameIDLabel.Text = "游戏名称检验正则";
            toolTip.SetToolTip(regexForCheckingGameIDLabel, "绑定时游戏名称需要符合此正则");
            // 
            // moreGroupBox
            // 
            moreGroupBox.Controls.Add(_enableSentryCheckBox);
            moreGroupBox.Controls.Add(_customTitleTextBox);
            moreGroupBox.Controls.Add(customTitleLabel);
            moreGroupBox.Controls.Add(_pattenForEnableMatchingMuiltLinesTextBox);
            moreGroupBox.Controls.Add(pattenForEnableMatchingMuiltLinesLabel);
            moreGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            moreGroupBox.Location = new System.Drawing.Point(10, 695);
            moreGroupBox.Name = "moreGroupBox";
            moreGroupBox.Size = new System.Drawing.Size(1260, 359);
            moreGroupBox.TabIndex = 2;
            moreGroupBox.TabStop = false;
            moreGroupBox.Text = "其他";
            // 
            // _enableSentryCheckBox
            // 
            _enableSentryCheckBox.AutoSize = true;
            _enableSentryCheckBox.Location = new System.Drawing.Point(27, 305);
            _enableSentryCheckBox.Name = "_enableSentryCheckBox";
            _enableSentryCheckBox.Size = new System.Drawing.Size(216, 35);
            _enableSentryCheckBox.TabIndex = 8;
            _enableSentryCheckBox.Text = "使用Sentry上报";
            toolTip.SetToolTip(_enableSentryCheckBox, "当出现异常或崩溃时自动匿名上报，便于开发者及时定位问题（建议开启）\r\n· 更改后重启生效\r\n· 上报时可能会收集部分系统和环境信息并产生一定网络流量");
            _enableSentryCheckBox.UseVisualStyleBackColor = true;
            // 
            // _customTitleTextBox
            // 
            _customTitleTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _customTitleTextBox.Location = new System.Drawing.Point(27, 254);
            _customTitleTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _customTitleTextBox.Name = "_customTitleTextBox";
            _customTitleTextBox.Size = new System.Drawing.Size(1206, 38);
            _customTitleTextBox.TabIndex = 11;
            toolTip.SetToolTip(_customTitleTextBox, "显示在标题栏的内容\r\n· 可使用命令的变量");
            _customTitleTextBox.TextChanged += OnPropertyChanged;
            // 
            // customTitleLabel
            // 
            customTitleLabel.AutoSize = true;
            customTitleLabel.Location = new System.Drawing.Point(27, 220);
            customTitleLabel.Name = "customTitleLabel";
            customTitleLabel.Size = new System.Drawing.Size(110, 31);
            customTitleLabel.TabIndex = 10;
            customTitleLabel.Text = "标题后缀";
            toolTip.SetToolTip(customTitleLabel, "显示在标题栏的内容\r\n· 可使用命令的变量");
            // 
            // _pattenForEnableMatchingMuiltLinesTextBox
            // 
            _pattenForEnableMatchingMuiltLinesTextBox.AcceptsReturn = true;
            _pattenForEnableMatchingMuiltLinesTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _pattenForEnableMatchingMuiltLinesTextBox.Location = new System.Drawing.Point(27, 78);
            _pattenForEnableMatchingMuiltLinesTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _pattenForEnableMatchingMuiltLinesTextBox.Multiline = true;
            _pattenForEnableMatchingMuiltLinesTextBox.Name = "_pattenForEnableMatchingMuiltLinesTextBox";
            _pattenForEnableMatchingMuiltLinesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _pattenForEnableMatchingMuiltLinesTextBox.Size = new System.Drawing.Size(1206, 132);
            _pattenForEnableMatchingMuiltLinesTextBox.TabIndex = 9;
            toolTip.SetToolTip(_pattenForEnableMatchingMuiltLinesTextBox, "当输入的内容若含有以下内容将触发多行匹配（一行一个）");
            _pattenForEnableMatchingMuiltLinesTextBox.TextChanged += PattenForEnableMatchingMuiltLinesTextBox_TextChanged;
            // 
            // pattenForEnableMatchingMuiltLinesLabel
            // 
            pattenForEnableMatchingMuiltLinesLabel.AutoSize = true;
            pattenForEnableMatchingMuiltLinesLabel.Location = new System.Drawing.Point(27, 44);
            pattenForEnableMatchingMuiltLinesLabel.Name = "pattenForEnableMatchingMuiltLinesLabel";
            pattenForEnableMatchingMuiltLinesLabel.Size = new System.Drawing.Size(278, 31);
            pattenForEnableMatchingMuiltLinesLabel.TabIndex = 8;
            pattenForEnableMatchingMuiltLinesLabel.Text = "用于触发多行匹配的文本";
            toolTip.SetToolTip(pattenForEnableMatchingMuiltLinesLabel, "当输入的内容若含有以下内容将触发多行匹配（一行一个）");
            // 
            // updateGroupBox
            // 
            updateGroupBox.Controls.Add(_checkUpdateButton);
            updateGroupBox.Controls.Add(_latestVersionLabel);
            updateGroupBox.Controls.Add(_versionLabel);
            updateGroupBox.Controls.Add(_autoUpdateCheckBox);
            updateGroupBox.Controls.Add(_checkUpdateCheckBox);
            updateGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            updateGroupBox.Location = new System.Drawing.Point(10, 1054);
            updateGroupBox.Name = "updateGroupBox";
            updateGroupBox.Size = new System.Drawing.Size(1260, 196);
            updateGroupBox.TabIndex = 3;
            updateGroupBox.TabStop = false;
            updateGroupBox.Text = "更新";
            // 
            // _checkUpdateButton
            // 
            _checkUpdateButton.Location = new System.Drawing.Point(27, 128);
            _checkUpdateButton.Name = "_checkUpdateButton";
            _checkUpdateButton.Size = new System.Drawing.Size(150, 42);
            _checkUpdateButton.TabIndex = 4;
            _checkUpdateButton.Text = "检查更新";
            _checkUpdateButton.UseVisualStyleBackColor = true;
            _checkUpdateButton.Click += CheckUpdateButton_Click;
            // 
            // _latestVersionLabel
            // 
            _latestVersionLabel.AutoSize = true;
            _latestVersionLabel.Location = new System.Drawing.Point(340, 81);
            _latestVersionLabel.Name = "_latestVersionLabel";
            _latestVersionLabel.Size = new System.Drawing.Size(110, 31);
            _latestVersionLabel.TabIndex = 3;
            _latestVersionLabel.Text = "最新版本";
            // 
            // _versionLabel
            // 
            _versionLabel.AutoSize = true;
            _versionLabel.Location = new System.Drawing.Point(340, 41);
            _versionLabel.Name = "_versionLabel";
            _versionLabel.Size = new System.Drawing.Size(110, 31);
            _versionLabel.TabIndex = 2;
            _versionLabel.Text = "当前版本";
            // 
            // _autoUpdateCheckBox
            // 
            _autoUpdateCheckBox.AutoSize = true;
            _autoUpdateCheckBox.Location = new System.Drawing.Point(27, 81);
            _autoUpdateCheckBox.Name = "_autoUpdateCheckBox";
            _autoUpdateCheckBox.Size = new System.Drawing.Size(142, 35);
            _autoUpdateCheckBox.TabIndex = 1;
            _autoUpdateCheckBox.Text = "自动更新";
            toolTip.SetToolTip(_autoUpdateCheckBox, "应用关闭后自动替换新版本");
            _autoUpdateCheckBox.UseVisualStyleBackColor = true;
            _autoUpdateCheckBox.Click += OnPropertyChanged;
            // 
            // _checkUpdateCheckBox
            // 
            _checkUpdateCheckBox.AutoSize = true;
            _checkUpdateCheckBox.Location = new System.Drawing.Point(27, 40);
            _checkUpdateCheckBox.Name = "_checkUpdateCheckBox";
            _checkUpdateCheckBox.Size = new System.Drawing.Size(190, 35);
            _checkUpdateCheckBox.TabIndex = 0;
            _checkUpdateCheckBox.Text = "获取更新提示";
            toolTip.SetToolTip(_checkUpdateCheckBox, "启动后自动获取更新提示（建议开启）");
            _checkUpdateCheckBox.UseVisualStyleBackColor = true;
            _checkUpdateCheckBox.Click += OnPropertyChanged;
            // 
            // AppSettingPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(updateGroupBox);
            Controls.Add(moreGroupBox);
            Controls.Add(bindingGroupBox);
            Controls.Add(pluginGroupBox);
            Name = "AppSettingPage";
            Padding = new System.Windows.Forms.Padding(10);
            Size = new System.Drawing.Size(1280, 1265);
            pluginGroupBox.ResumeLayout(false);
            pluginGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_pluginEventMaxWaitingTimeNumericUpDown).EndInit();
            bindingGroupBox.ResumeLayout(false);
            bindingGroupBox.PerformLayout();
            moreGroupBox.ResumeLayout(false);
            moreGroupBox.PerformLayout();
            updateGroupBox.ResumeLayout(false);
            updateGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox _jsGlobalAssembliesTextBox;
        private System.Windows.Forms.NumericUpDown _pluginEventMaxWaitingTimeNumericUpDown;
        private System.Windows.Forms.TextBox _jsPatternToSkipLoadingSingleFileTextBox;
        private System.Windows.Forms.CheckBox _disableBindingManagerWhenServerClosedCheckBox;
        private System.Windows.Forms.TextBox _regexForCheckingGameIDTextBox;
        private System.Windows.Forms.TextBox _pattenForEnableMatchingMuiltLinesTextBox;
        private System.Windows.Forms.TextBox _customTitleTextBox;
        private System.Windows.Forms.Button _checkUpdateButton;
        private System.Windows.Forms.Label _latestVersionLabel;
        private System.Windows.Forms.Label _versionLabel;
        private System.Windows.Forms.CheckBox _autoUpdateCheckBox;
        private System.Windows.Forms.CheckBox _checkUpdateCheckBox;
        private System.Windows.Forms.CheckBox _enableSentryCheckBox;
    }
}
