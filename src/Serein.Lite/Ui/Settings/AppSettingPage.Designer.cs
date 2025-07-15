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
            System.Windows.Forms.Label jsFilesToExcludeFromLoadingLabel;
            System.Windows.Forms.Label jsDefaultAssembliesLabel;
            System.Windows.Forms.Label maximumWaitTimeForPluginEventsLabel;
            System.Windows.Forms.GroupBox bindingGroupBox;
            System.Windows.Forms.Label gameIdValidationPatternLabel;
            System.Windows.Forms.GroupBox moreGroupBox;
            System.Windows.Forms.Label customTitleLabel;
            System.Windows.Forms.Label multiLineMatchingPatternsLabel;
            System.Windows.Forms.GroupBox updateGroupBox;
            System.Windows.Forms.ToolTip toolTip;
            _jsFilesToExcludeFromLoadingTextBox = new System.Windows.Forms.TextBox();
            _jsDefaultAssembliesTextBox = new System.Windows.Forms.TextBox();
            _maximumWaitTimeForPluginEventsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            _disableBindingManagerWhenAllServersStoppedCheckBox = new System.Windows.Forms.CheckBox();
            _gameIdValidationPatternTextBox = new System.Windows.Forms.TextBox();
            _enableSentryCheckBox = new System.Windows.Forms.CheckBox();
            _customTitleTextBox = new System.Windows.Forms.TextBox();
            _multiLineMatchingPatternsTextBox = new System.Windows.Forms.TextBox();
            _checkUpdateButton = new System.Windows.Forms.Button();
            _latestVersionLabel = new System.Windows.Forms.Label();
            _versionLabel = new System.Windows.Forms.Label();
            _autoUpdateCheckBox = new System.Windows.Forms.CheckBox();
            _checkUpdateCheckBox = new System.Windows.Forms.CheckBox();
            pluginGroupBox = new System.Windows.Forms.GroupBox();
            jsFilesToExcludeFromLoadingLabel = new System.Windows.Forms.Label();
            jsDefaultAssembliesLabel = new System.Windows.Forms.Label();
            maximumWaitTimeForPluginEventsLabel = new System.Windows.Forms.Label();
            bindingGroupBox = new System.Windows.Forms.GroupBox();
            gameIdValidationPatternLabel = new System.Windows.Forms.Label();
            moreGroupBox = new System.Windows.Forms.GroupBox();
            customTitleLabel = new System.Windows.Forms.Label();
            multiLineMatchingPatternsLabel = new System.Windows.Forms.Label();
            updateGroupBox = new System.Windows.Forms.GroupBox();
            toolTip = new System.Windows.Forms.ToolTip(components);
            pluginGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_maximumWaitTimeForPluginEventsNumericUpDown).BeginInit();
            bindingGroupBox.SuspendLayout();
            moreGroupBox.SuspendLayout();
            updateGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // pluginGroupBox
            // 
            pluginGroupBox.Controls.Add(_jsFilesToExcludeFromLoadingTextBox);
            pluginGroupBox.Controls.Add(jsFilesToExcludeFromLoadingLabel);
            pluginGroupBox.Controls.Add(_jsDefaultAssembliesTextBox);
            pluginGroupBox.Controls.Add(jsDefaultAssembliesLabel);
            pluginGroupBox.Controls.Add(_maximumWaitTimeForPluginEventsNumericUpDown);
            pluginGroupBox.Controls.Add(maximumWaitTimeForPluginEventsLabel);
            pluginGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            pluginGroupBox.Location = new System.Drawing.Point(10, 10);
            pluginGroupBox.Name = "pluginGroupBox";
            pluginGroupBox.Size = new System.Drawing.Size(1260, 498);
            pluginGroupBox.TabIndex = 0;
            pluginGroupBox.TabStop = false;
            pluginGroupBox.Text = "插件";
            // 
            // _jsFilesToExcludeFromLoadingTextBox
            // 
            _jsFilesToExcludeFromLoadingTextBox.AcceptsReturn = true;
            _jsFilesToExcludeFromLoadingTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _jsFilesToExcludeFromLoadingTextBox.Location = new System.Drawing.Point(27, 339);
            _jsFilesToExcludeFromLoadingTextBox.Multiline = true;
            _jsFilesToExcludeFromLoadingTextBox.Name = "_jsFilesToExcludeFromLoadingTextBox";
            _jsFilesToExcludeFromLoadingTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _jsFilesToExcludeFromLoadingTextBox.Size = new System.Drawing.Size(1206, 132);
            _jsFilesToExcludeFromLoadingTextBox.TabIndex = 3;
            toolTip.SetToolTip(_jsFilesToExcludeFromLoadingTextBox, "凡是以所选内容结尾的文件都不会被加载（一行一个）");
            _jsFilesToExcludeFromLoadingTextBox.TextChanged += JsFilesToExcludeFromLoadingTextBox_TextChanged;
            // 
            // jsFilesToExcludeFromLoadingLabel
            // 
            jsFilesToExcludeFromLoadingLabel.AutoSize = true;
            jsFilesToExcludeFromLoadingLabel.Location = new System.Drawing.Point(27, 305);
            jsFilesToExcludeFromLoadingLabel.Name = "jsFilesToExcludeFromLoadingLabel";
            jsFilesToExcludeFromLoadingLabel.Size = new System.Drawing.Size(326, 31);
            jsFilesToExcludeFromLoadingLabel.TabIndex = 4;
            jsFilesToExcludeFromLoadingLabel.Text = "Js插件加载时忽略的文件后缀";
            toolTip.SetToolTip(jsFilesToExcludeFromLoadingLabel, "凡是以所选内容结尾的文件都不会被加载（一行一个）");
            // 
            // _jsDefaultAssembliesTextBox
            // 
            _jsDefaultAssembliesTextBox.AcceptsReturn = true;
            _jsDefaultAssembliesTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _jsDefaultAssembliesTextBox.Location = new System.Drawing.Point(27, 163);
            _jsDefaultAssembliesTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _jsDefaultAssembliesTextBox.Multiline = true;
            _jsDefaultAssembliesTextBox.Name = "_jsDefaultAssembliesTextBox";
            _jsDefaultAssembliesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _jsDefaultAssembliesTextBox.Size = new System.Drawing.Size(1206, 132);
            _jsDefaultAssembliesTextBox.TabIndex = 1;
            toolTip.SetToolTip(_jsDefaultAssembliesTextBox, "此处的程序集将会被所有Js插件加载（一行一个）\r\n");
            _jsDefaultAssembliesTextBox.TextChanged += JsDefaultAssembliesTextBox_TextChanged;
            // 
            // jsDefaultAssembliesLabel
            // 
            jsDefaultAssembliesLabel.AutoSize = true;
            jsDefaultAssembliesLabel.Location = new System.Drawing.Point(27, 129);
            jsDefaultAssembliesLabel.Name = "jsDefaultAssembliesLabel";
            jsDefaultAssembliesLabel.Size = new System.Drawing.Size(278, 31);
            jsDefaultAssembliesLabel.TabIndex = 2;
            jsDefaultAssembliesLabel.Text = "Js插件默认加载的程序集";
            toolTip.SetToolTip(jsDefaultAssembliesLabel, "此处的程序集将会被所有Js插件加载（一行一个）\r\n");
            // 
            // _maximumWaitTimeForPluginEventsNumericUpDown
            // 
            _maximumWaitTimeForPluginEventsNumericUpDown.Location = new System.Drawing.Point(27, 81);
            _maximumWaitTimeForPluginEventsNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _maximumWaitTimeForPluginEventsNumericUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            _maximumWaitTimeForPluginEventsNumericUpDown.Name = "_maximumWaitTimeForPluginEventsNumericUpDown";
            _maximumWaitTimeForPluginEventsNumericUpDown.Size = new System.Drawing.Size(223, 38);
            _maximumWaitTimeForPluginEventsNumericUpDown.TabIndex = 1;
            toolTip.SetToolTip(_maximumWaitTimeForPluginEventsNumericUpDown, "超出此时间返回的结果将被忽略；设置成0可禁用等待");
            _maximumWaitTimeForPluginEventsNumericUpDown.ValueChanged += OnPropertyChanged;
            // 
            // maximumWaitTimeForPluginEventsLabel
            // 
            maximumWaitTimeForPluginEventsLabel.AutoSize = true;
            maximumWaitTimeForPluginEventsLabel.Location = new System.Drawing.Point(27, 47);
            maximumWaitTimeForPluginEventsLabel.Name = "maximumWaitTimeForPluginEventsLabel";
            maximumWaitTimeForPluginEventsLabel.Size = new System.Drawing.Size(262, 31);
            maximumWaitTimeForPluginEventsLabel.TabIndex = 0;
            maximumWaitTimeForPluginEventsLabel.Text = "事件最大等待时间 (ms)";
            toolTip.SetToolTip(maximumWaitTimeForPluginEventsLabel, "超出此时间返回的结果将被忽略；设置成0可禁用等待");
            // 
            // bindingGroupBox
            // 
            bindingGroupBox.Controls.Add(_disableBindingManagerWhenAllServersStoppedCheckBox);
            bindingGroupBox.Controls.Add(_gameIdValidationPatternTextBox);
            bindingGroupBox.Controls.Add(gameIdValidationPatternLabel);
            bindingGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            bindingGroupBox.Location = new System.Drawing.Point(10, 508);
            bindingGroupBox.Name = "bindingGroupBox";
            bindingGroupBox.Size = new System.Drawing.Size(1260, 187);
            bindingGroupBox.TabIndex = 1;
            bindingGroupBox.TabStop = false;
            bindingGroupBox.Text = "绑定";
            // 
            // _disableBindingManagerWhenAllServersStoppedCheckBox
            // 
            _disableBindingManagerWhenAllServersStoppedCheckBox.AutoSize = true;
            _disableBindingManagerWhenAllServersStoppedCheckBox.Location = new System.Drawing.Point(27, 130);
            _disableBindingManagerWhenAllServersStoppedCheckBox.Name = "_disableBindingManagerWhenAllServersStoppedCheckBox";
            _disableBindingManagerWhenAllServersStoppedCheckBox.Size = new System.Drawing.Size(358, 35);
            _disableBindingManagerWhenAllServersStoppedCheckBox.TabIndex = 7;
            _disableBindingManagerWhenAllServersStoppedCheckBox.Text = "当服务器关闭时禁用绑定功能";
            toolTip.SetToolTip(_disableBindingManagerWhenAllServersStoppedCheckBox, "只影响通过Serein命令执行的绑定");
            _disableBindingManagerWhenAllServersStoppedCheckBox.UseVisualStyleBackColor = true;
            _disableBindingManagerWhenAllServersStoppedCheckBox.Click += OnPropertyChanged;
            // 
            // _gameIdValidationPatternTextBox
            // 
            _gameIdValidationPatternTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _gameIdValidationPatternTextBox.Location = new System.Drawing.Point(27, 79);
            _gameIdValidationPatternTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _gameIdValidationPatternTextBox.Name = "_gameIdValidationPatternTextBox";
            _gameIdValidationPatternTextBox.Size = new System.Drawing.Size(1206, 38);
            _gameIdValidationPatternTextBox.TabIndex = 6;
            toolTip.SetToolTip(_gameIdValidationPatternTextBox, "绑定时游戏名称需要符合此正则");
            _gameIdValidationPatternTextBox.TextChanged += OnPropertyChanged;
            // 
            // gameIdValidationPatternLabel
            // 
            gameIdValidationPatternLabel.AutoSize = true;
            gameIdValidationPatternLabel.Location = new System.Drawing.Point(27, 45);
            gameIdValidationPatternLabel.Name = "gameIdValidationPatternLabel";
            gameIdValidationPatternLabel.Size = new System.Drawing.Size(206, 31);
            gameIdValidationPatternLabel.TabIndex = 5;
            gameIdValidationPatternLabel.Text = "游戏名称检验正则";
            toolTip.SetToolTip(gameIdValidationPatternLabel, "绑定时游戏名称需要符合此正则");
            // 
            // moreGroupBox
            // 
            moreGroupBox.Controls.Add(_enableSentryCheckBox);
            moreGroupBox.Controls.Add(_customTitleTextBox);
            moreGroupBox.Controls.Add(customTitleLabel);
            moreGroupBox.Controls.Add(_multiLineMatchingPatternsTextBox);
            moreGroupBox.Controls.Add(multiLineMatchingPatternsLabel);
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
            // _multiLineMatchingPatternsTextBox
            // 
            _multiLineMatchingPatternsTextBox.AcceptsReturn = true;
            _multiLineMatchingPatternsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _multiLineMatchingPatternsTextBox.Location = new System.Drawing.Point(27, 78);
            _multiLineMatchingPatternsTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _multiLineMatchingPatternsTextBox.Multiline = true;
            _multiLineMatchingPatternsTextBox.Name = "_multiLineMatchingPatternsTextBox";
            _multiLineMatchingPatternsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _multiLineMatchingPatternsTextBox.Size = new System.Drawing.Size(1206, 132);
            _multiLineMatchingPatternsTextBox.TabIndex = 9;
            toolTip.SetToolTip(_multiLineMatchingPatternsTextBox, "当输入的内容若含有以下内容将触发多行匹配（一行一个）");
            _multiLineMatchingPatternsTextBox.TextChanged += multiLineMatchingPatternsTextBox_TextChanged;
            // 
            // multiLineMatchingPatternsLabel
            // 
            multiLineMatchingPatternsLabel.AutoSize = true;
            multiLineMatchingPatternsLabel.Location = new System.Drawing.Point(27, 44);
            multiLineMatchingPatternsLabel.Name = "multiLineMatchingPatternsLabel";
            multiLineMatchingPatternsLabel.Size = new System.Drawing.Size(278, 31);
            multiLineMatchingPatternsLabel.TabIndex = 8;
            multiLineMatchingPatternsLabel.Text = "用于触发多行匹配的文本";
            toolTip.SetToolTip(multiLineMatchingPatternsLabel, "当输入的内容若含有以下内容将触发多行匹配（一行一个）");
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
            ((System.ComponentModel.ISupportInitialize)_maximumWaitTimeForPluginEventsNumericUpDown).EndInit();
            bindingGroupBox.ResumeLayout(false);
            bindingGroupBox.PerformLayout();
            moreGroupBox.ResumeLayout(false);
            moreGroupBox.PerformLayout();
            updateGroupBox.ResumeLayout(false);
            updateGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox _jsDefaultAssembliesTextBox;
        private System.Windows.Forms.NumericUpDown _maximumWaitTimeForPluginEventsNumericUpDown;
        private System.Windows.Forms.TextBox _jsFilesToExcludeFromLoadingTextBox;
        private System.Windows.Forms.CheckBox _disableBindingManagerWhenAllServersStoppedCheckBox;
        private System.Windows.Forms.TextBox _gameIdValidationPatternTextBox;
        private System.Windows.Forms.TextBox _multiLineMatchingPatternsTextBox;
        private System.Windows.Forms.TextBox _customTitleTextBox;
        private System.Windows.Forms.Button _checkUpdateButton;
        private System.Windows.Forms.Label _latestVersionLabel;
        private System.Windows.Forms.Label _versionLabel;
        private System.Windows.Forms.CheckBox _autoUpdateCheckBox;
        private System.Windows.Forms.CheckBox _checkUpdateCheckBox;
        private System.Windows.Forms.CheckBox _enableSentryCheckBox;
    }
}
