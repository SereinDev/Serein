namespace Serein.Lite.Ui.Servers
{
    partial class ConfigurationEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.Label idLabel;
            System.Windows.Forms.Label nameLabel;
            System.Windows.Forms.Label fileNameLabel;
            System.Windows.Forms.Label argumentLabel;
            System.Windows.Forms.Label lineTerminatorLabel;
            System.Windows.Forms.Label outputStyleLabel;
            System.Windows.Forms.Label outputEncondingLabel;
            System.Windows.Forms.Label inputEncondingLabel;
            System.Windows.Forms.Label stopLabel;
            System.Windows.Forms.Label portLabel;
            System.Windows.Forms.Button openFileButton;
            System.Windows.Forms.TabPage commonTabPage;
            System.Windows.Forms.TabPage inputAndOutputTabPage;
            System.Windows.Forms.TabPage moreTabPage;
            System.Windows.Forms.Button confirmButton;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationEditor));
            _fileNameTextBox = new System.Windows.Forms.TextBox();
            _idTextBox = new System.Windows.Forms.TextBox();
            MainTabControl = new System.Windows.Forms.TabControl();
            _nameTextBox = new System.Windows.Forms.TextBox();
            _argumentTextBox = new System.Windows.Forms.TextBox();
            _forceWinPtyCheckBox = new System.Windows.Forms.CheckBox();
            _usePtyCheckBox = new System.Windows.Forms.CheckBox();
            _lineTerminatorTextBox = new System.Windows.Forms.TextBox();
            _useUnicodeCharsCheckBox = new System.Windows.Forms.CheckBox();
            _outputCommandUserInputCheckBox = new System.Windows.Forms.CheckBox();
            _saveLogCheckBox = new System.Windows.Forms.CheckBox();
            _outputStyleComboBox = new System.Windows.Forms.ComboBox();
            _outputEncondingComboBox = new System.Windows.Forms.ComboBox();
            _inputEncondingComboBox = new System.Windows.Forms.ComboBox();
            _stopCommandsTextBox = new System.Windows.Forms.TextBox();
            _portNumericUpDown = new System.Windows.Forms.NumericUpDown();
            _startWhenSettingUpCheckBox = new System.Windows.Forms.CheckBox();
            _autoStopWhenCrashingCheckBox = new System.Windows.Forms.CheckBox();
            _autoRestartCheckBox = new System.Windows.Forms.CheckBox();
            _errorProvider = new System.Windows.Forms.ErrorProvider(components);
            toolTip = new System.Windows.Forms.ToolTip(components);
            idLabel = new System.Windows.Forms.Label();
            nameLabel = new System.Windows.Forms.Label();
            fileNameLabel = new System.Windows.Forms.Label();
            argumentLabel = new System.Windows.Forms.Label();
            lineTerminatorLabel = new System.Windows.Forms.Label();
            outputStyleLabel = new System.Windows.Forms.Label();
            outputEncondingLabel = new System.Windows.Forms.Label();
            inputEncondingLabel = new System.Windows.Forms.Label();
            stopLabel = new System.Windows.Forms.Label();
            portLabel = new System.Windows.Forms.Label();
            openFileButton = new System.Windows.Forms.Button();
            commonTabPage = new System.Windows.Forms.TabPage();
            inputAndOutputTabPage = new System.Windows.Forms.TabPage();
            moreTabPage = new System.Windows.Forms.TabPage();
            confirmButton = new System.Windows.Forms.Button();
            MainTabControl.SuspendLayout();
            commonTabPage.SuspendLayout();
            inputAndOutputTabPage.SuspendLayout();
            moreTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_portNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).BeginInit();
            SuspendLayout();
            // 
            // idLabel
            // 
            idLabel.AutoSize = true;
            idLabel.Location = new System.Drawing.Point(23, 12);
            idLabel.Name = "idLabel";
            idLabel.Size = new System.Drawing.Size(36, 31);
            idLabel.TabIndex = 0;
            idLabel.Text = "Id";
            toolTip.SetToolTip(idLabel, "用于区分服务器（一经填写无法修改）\r\n· 长度大于或等于3\r\n· 只由数字、字母和下划线组成");
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(23, 108);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(62, 31);
            nameLabel.TabIndex = 2;
            nameLabel.Text = "名称";
            toolTip.SetToolTip(nameLabel, "用于标识服务器，便于管理");
            // 
            // fileNameLabel
            // 
            fileNameLabel.AutoSize = true;
            fileNameLabel.Location = new System.Drawing.Point(23, 204);
            fileNameLabel.Name = "fileNameLabel";
            fileNameLabel.Size = new System.Drawing.Size(110, 31);
            fileNameLabel.TabIndex = 4;
            fileNameLabel.Text = "启动文件";
            toolTip.SetToolTip(fileNameLabel, "启动进程的文件，通常为可执行文件或批处理文件\r\n【提示】你可以双击文本框打开选择文件对话框");
            // 
            // argumentLabel
            // 
            argumentLabel.AutoSize = true;
            argumentLabel.Location = new System.Drawing.Point(23, 300);
            argumentLabel.Name = "argumentLabel";
            argumentLabel.Size = new System.Drawing.Size(110, 31);
            argumentLabel.TabIndex = 6;
            argumentLabel.Text = "启动参数";
            toolTip.SetToolTip(argumentLabel, "附加在启动文件后的参数");
            // 
            // lineTerminatorLabel
            // 
            lineTerminatorLabel.AutoSize = true;
            lineTerminatorLabel.Location = new System.Drawing.Point(23, 291);
            lineTerminatorLabel.Name = "lineTerminatorLabel";
            lineTerminatorLabel.Size = new System.Drawing.Size(110, 31);
            lineTerminatorLabel.TabIndex = 6;
            lineTerminatorLabel.Text = "行终止符";
            toolTip.SetToolTip(lineTerminatorLabel, "用于标记每行的结尾\r\n· 在Windows平台下默认为CRLF（\\r\\n）\r\n· 在其他平台下默认为LF（\\n）\r\n· 随意更改可能导致服务器无法输入命令");
            // 
            // outputStyleLabel
            // 
            outputStyleLabel.AutoSize = true;
            outputStyleLabel.Location = new System.Drawing.Point(23, 198);
            outputStyleLabel.Name = "outputStyleLabel";
            outputStyleLabel.Size = new System.Drawing.Size(110, 31);
            outputStyleLabel.TabIndex = 4;
            outputStyleLabel.Text = "输出样式";
            toolTip.SetToolTip(outputStyleLabel, "控制台中渲染输出内容的样式");
            // 
            // outputEncondingLabel
            // 
            outputEncondingLabel.AutoSize = true;
            outputEncondingLabel.Location = new System.Drawing.Point(23, 105);
            outputEncondingLabel.Name = "outputEncondingLabel";
            outputEncondingLabel.Size = new System.Drawing.Size(110, 31);
            outputEncondingLabel.TabIndex = 2;
            outputEncondingLabel.Text = "输出编码";
            toolTip.SetToolTip(outputEncondingLabel, "读取服务器输出的编码（修改后需要重新启动服务器方可生效）");
            // 
            // inputEncondingLabel
            // 
            inputEncondingLabel.AutoSize = true;
            inputEncondingLabel.Location = new System.Drawing.Point(23, 12);
            inputEncondingLabel.Name = "inputEncondingLabel";
            inputEncondingLabel.Size = new System.Drawing.Size(110, 31);
            inputEncondingLabel.TabIndex = 0;
            inputEncondingLabel.Text = "输入编码";
            toolTip.SetToolTip(inputEncondingLabel, "输入到服务器的编码");
            // 
            // stopLabel
            // 
            stopLabel.AutoSize = true;
            stopLabel.Location = new System.Drawing.Point(26, 217);
            stopLabel.Name = "stopLabel";
            stopLabel.Size = new System.Drawing.Size(110, 31);
            stopLabel.TabIndex = 5;
            stopLabel.Text = "关服命令";
            toolTip.SetToolTip(stopLabel, "关闭服务器时输入的命令（一行一个）");
            // 
            // portLabel
            // 
            portLabel.AutoSize = true;
            portLabel.Location = new System.Drawing.Point(25, 136);
            portLabel.Name = "portLabel";
            portLabel.Size = new System.Drawing.Size(111, 31);
            portLabel.TabIndex = 3;
            portLabel.Text = "IPv4端口";
            toolTip.SetToolTip(portLabel, "服务器的IPv4端口，用于获取服务器相关信息（版本、在线玩家数）");
            // 
            // openFileButton
            // 
            openFileButton.Location = new System.Drawing.Point(605, 241);
            openFileButton.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            openFileButton.Name = "openFileButton";
            openFileButton.Size = new System.Drawing.Size(118, 40);
            openFileButton.TabIndex = 8;
            openFileButton.Text = "打开...";
            openFileButton.UseVisualStyleBackColor = true;
            openFileButton.Click += OpenFileButton_Click;
            // 
            // _fileNameTextBox
            // 
            _fileNameTextBox.Location = new System.Drawing.Point(23, 242);
            _fileNameTextBox.Margin = new System.Windows.Forms.Padding(3, 7, 3, 20);
            _fileNameTextBox.Name = "_fileNameTextBox";
            _fileNameTextBox.Size = new System.Drawing.Size(576, 38);
            _fileNameTextBox.TabIndex = 5;
            toolTip.SetToolTip(_fileNameTextBox, "启动进程的文件，通常为可执行文件或批处理文件\r\n【提示】你可以双击文本框打开选择文件对话框");
            // 
            // _idTextBox
            // 
            _idTextBox.Location = new System.Drawing.Point(23, 50);
            _idTextBox.Margin = new System.Windows.Forms.Padding(3, 7, 3, 20);
            _idTextBox.Name = "_idTextBox";
            _idTextBox.Size = new System.Drawing.Size(700, 38);
            _idTextBox.TabIndex = 1;
            toolTip.SetToolTip(_idTextBox, "用于区分服务器（一经填写无法修改）\r\n· 长度大于或等于3\r\n· 只由数字、字母和下划线组成");
            _idTextBox.Enter += IdTextBox_Enter;
            _idTextBox.Validating += IdTextBox_Validating;
            // 
            // MainTabControl
            // 
            MainTabControl.Controls.Add(commonTabPage);
            MainTabControl.Controls.Add(inputAndOutputTabPage);
            MainTabControl.Controls.Add(moreTabPage);
            MainTabControl.Dock = System.Windows.Forms.DockStyle.Top;
            MainTabControl.Location = new System.Drawing.Point(0, 0);
            MainTabControl.Name = "MainTabControl";
            MainTabControl.SelectedIndex = 0;
            MainTabControl.Size = new System.Drawing.Size(774, 465);
            MainTabControl.TabIndex = 2;
            // 
            // commonTabPage
            // 
            commonTabPage.AutoScroll = true;
            commonTabPage.Controls.Add(openFileButton);
            commonTabPage.Controls.Add(nameLabel);
            commonTabPage.Controls.Add(_nameTextBox);
            commonTabPage.Controls.Add(_argumentTextBox);
            commonTabPage.Controls.Add(argumentLabel);
            commonTabPage.Controls.Add(fileNameLabel);
            commonTabPage.Controls.Add(_fileNameTextBox);
            commonTabPage.Controls.Add(idLabel);
            commonTabPage.Controls.Add(_idTextBox);
            commonTabPage.Location = new System.Drawing.Point(8, 45);
            commonTabPage.Name = "commonTabPage";
            commonTabPage.Padding = new System.Windows.Forms.Padding(3);
            commonTabPage.Size = new System.Drawing.Size(758, 412);
            commonTabPage.TabIndex = 0;
            commonTabPage.Text = "常规";
            commonTabPage.UseVisualStyleBackColor = true;
            // 
            // _nameTextBox
            // 
            _nameTextBox.Location = new System.Drawing.Point(23, 146);
            _nameTextBox.Margin = new System.Windows.Forms.Padding(3, 7, 3, 20);
            _nameTextBox.Name = "_nameTextBox";
            _nameTextBox.Size = new System.Drawing.Size(700, 38);
            _nameTextBox.TabIndex = 3;
            toolTip.SetToolTip(_nameTextBox, "用于标识服务器，便于管理");
            // 
            // _argumentTextBox
            // 
            _argumentTextBox.Location = new System.Drawing.Point(23, 338);
            _argumentTextBox.Margin = new System.Windows.Forms.Padding(3, 7, 3, 20);
            _argumentTextBox.Name = "_argumentTextBox";
            _argumentTextBox.Size = new System.Drawing.Size(700, 38);
            _argumentTextBox.TabIndex = 7;
            toolTip.SetToolTip(_argumentTextBox, "附加在启动文件后的参数");
            // 
            // inputAndOutputTabPage
            // 
            inputAndOutputTabPage.Controls.Add(_forceWinPtyCheckBox);
            inputAndOutputTabPage.Controls.Add(_usePtyCheckBox);
            inputAndOutputTabPage.Controls.Add(_lineTerminatorTextBox);
            inputAndOutputTabPage.Controls.Add(lineTerminatorLabel);
            inputAndOutputTabPage.Controls.Add(_useUnicodeCharsCheckBox);
            inputAndOutputTabPage.Controls.Add(_outputCommandUserInputCheckBox);
            inputAndOutputTabPage.Controls.Add(_saveLogCheckBox);
            inputAndOutputTabPage.Controls.Add(outputStyleLabel);
            inputAndOutputTabPage.Controls.Add(_outputStyleComboBox);
            inputAndOutputTabPage.Controls.Add(outputEncondingLabel);
            inputAndOutputTabPage.Controls.Add(inputEncondingLabel);
            inputAndOutputTabPage.Controls.Add(_outputEncondingComboBox);
            inputAndOutputTabPage.Controls.Add(_inputEncondingComboBox);
            inputAndOutputTabPage.Location = new System.Drawing.Point(8, 45);
            inputAndOutputTabPage.Name = "inputAndOutputTabPage";
            inputAndOutputTabPage.Padding = new System.Windows.Forms.Padding(3);
            inputAndOutputTabPage.Size = new System.Drawing.Size(758, 412);
            inputAndOutputTabPage.TabIndex = 1;
            inputAndOutputTabPage.Text = "输出/输入";
            inputAndOutputTabPage.UseVisualStyleBackColor = true;
            // 
            // _forceWinPtyCheckBox
            // 
            _forceWinPtyCheckBox.AutoSize = true;
            _forceWinPtyCheckBox.Location = new System.Drawing.Point(365, 236);
            _forceWinPtyCheckBox.Name = "_forceWinPtyCheckBox";
            _forceWinPtyCheckBox.Size = new System.Drawing.Size(224, 35);
            _forceWinPtyCheckBox.TabIndex = 12;
            _forceWinPtyCheckBox.Text = "强制使用WinPty";
            toolTip.SetToolTip(_forceWinPtyCheckBox, "· 仅在Windows平台下生效\r\n· 若不勾选此项，你需要手动补全相应的动态链接库\r\n· 不推荐修改此项，除非你知道你在做什么！");
            _forceWinPtyCheckBox.UseVisualStyleBackColor = true;
            // 
            // _usePtyCheckBox
            // 
            _usePtyCheckBox.AutoSize = true;
            _usePtyCheckBox.Location = new System.Drawing.Point(365, 194);
            _usePtyCheckBox.Name = "_usePtyCheckBox";
            _usePtyCheckBox.Size = new System.Drawing.Size(190, 35);
            _usePtyCheckBox.TabIndex = 11;
            _usePtyCheckBox.Text = "使用虚拟终端";
            toolTip.SetToolTip(_usePtyCheckBox, "使用虚拟终端输入和输出\r\n· 用于解决一些控制台无输入或输出的问题\r\n· 可能因系统版本不同而有不同的效果\r\n· 这是一个实验性选项，后续版本中可能会发生变化");
            _usePtyCheckBox.UseVisualStyleBackColor = true;
            _usePtyCheckBox.CheckedChanged += UsePtyCheckBox_CheckedChanged;
            // 
            // _lineTerminatorTextBox
            // 
            _lineTerminatorTextBox.Location = new System.Drawing.Point(23, 329);
            _lineTerminatorTextBox.Margin = new System.Windows.Forms.Padding(3, 7, 3, 20);
            _lineTerminatorTextBox.Name = "_lineTerminatorTextBox";
            _lineTerminatorTextBox.Size = new System.Drawing.Size(722, 38);
            _lineTerminatorTextBox.TabIndex = 7;
            toolTip.SetToolTip(_lineTerminatorTextBox, "用于标记每行的结尾\r\n· 在Windows平台下默认为CRLF（\\r\\n）\r\n· 在其他平台下默认为LF（\\n）\r\n· 随意更改可能导致服务器无法输入命令");
            // 
            // _useUnicodeCharsCheckBox
            // 
            _useUnicodeCharsCheckBox.AutoSize = true;
            _useUnicodeCharsCheckBox.Location = new System.Drawing.Point(365, 128);
            _useUnicodeCharsCheckBox.Name = "_useUnicodeCharsCheckBox";
            _useUnicodeCharsCheckBox.Size = new System.Drawing.Size(237, 35);
            _useUnicodeCharsCheckBox.TabIndex = 10;
            _useUnicodeCharsCheckBox.Text = "使用Unicode字符";
            toolTip.SetToolTip(_useUnicodeCharsCheckBox, "使用Unicode字符输入（如\"§\"→\"\\u00a7\"），通常用于解决基岩版服务器输入Tellraw的编码问题");
            _useUnicodeCharsCheckBox.UseVisualStyleBackColor = true;
            // 
            // _outputCommandUserInputCheckBox
            // 
            _outputCommandUserInputCheckBox.AutoSize = true;
            _outputCommandUserInputCheckBox.Location = new System.Drawing.Point(365, 87);
            _outputCommandUserInputCheckBox.Name = "_outputCommandUserInputCheckBox";
            _outputCommandUserInputCheckBox.Size = new System.Drawing.Size(214, 35);
            _outputCommandUserInputCheckBox.TabIndex = 9;
            _outputCommandUserInputCheckBox.Text = "显示输出的命令";
            toolTip.SetToolTip(_outputCommandUserInputCheckBox, "在控制台显示由用户输入的命令");
            _outputCommandUserInputCheckBox.UseVisualStyleBackColor = true;
            // 
            // _saveLogCheckBox
            // 
            _saveLogCheckBox.AutoSize = true;
            _saveLogCheckBox.Location = new System.Drawing.Point(365, 46);
            _saveLogCheckBox.Name = "_saveLogCheckBox";
            _saveLogCheckBox.Size = new System.Drawing.Size(142, 35);
            _saveLogCheckBox.TabIndex = 8;
            _saveLogCheckBox.Text = "保存日志";
            toolTip.SetToolTip(_saveLogCheckBox, "将控制台内容保存到文件“Serein/logs/servers/{id}-{datetime}.log”");
            _saveLogCheckBox.UseVisualStyleBackColor = true;
            // 
            // _outputStyleComboBox
            // 
            _outputStyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _outputStyleComboBox.FormattingEnabled = true;
            _outputStyleComboBox.Items.AddRange(new object[] { "无颜色", "原始颜色" });
            _outputStyleComboBox.Location = new System.Drawing.Point(23, 232);
            _outputStyleComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            _outputStyleComboBox.Name = "_outputStyleComboBox";
            _outputStyleComboBox.Size = new System.Drawing.Size(197, 39);
            _outputStyleComboBox.TabIndex = 5;
            toolTip.SetToolTip(_outputStyleComboBox, "控制台中渲染输出内容的样式");
            // 
            // _outputEncondingComboBox
            // 
            _outputEncondingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _outputEncondingComboBox.FormattingEnabled = true;
            _outputEncondingComboBox.Items.AddRange(new object[] { "UTF8", "UTF16-LE", "UTF16-BE", "GBK" });
            _outputEncondingComboBox.Location = new System.Drawing.Point(23, 139);
            _outputEncondingComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            _outputEncondingComboBox.Name = "_outputEncondingComboBox";
            _outputEncondingComboBox.Size = new System.Drawing.Size(197, 39);
            _outputEncondingComboBox.TabIndex = 3;
            toolTip.SetToolTip(_outputEncondingComboBox, "读取服务器输出的编码（修改后需要重新启动服务器方可生效）");
            // 
            // _inputEncondingComboBox
            // 
            _inputEncondingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _inputEncondingComboBox.FormattingEnabled = true;
            _inputEncondingComboBox.Items.AddRange(new object[] { "UTF8", "UTF16-LE", "UTF16-BE", "GBK" });
            _inputEncondingComboBox.Location = new System.Drawing.Point(23, 46);
            _inputEncondingComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            _inputEncondingComboBox.Name = "_inputEncondingComboBox";
            _inputEncondingComboBox.Size = new System.Drawing.Size(197, 39);
            _inputEncondingComboBox.TabIndex = 1;
            toolTip.SetToolTip(_inputEncondingComboBox, "输入到服务器的编码");
            // 
            // moreTabPage
            // 
            moreTabPage.Controls.Add(_stopCommandsTextBox);
            moreTabPage.Controls.Add(stopLabel);
            moreTabPage.Controls.Add(portLabel);
            moreTabPage.Controls.Add(_portNumericUpDown);
            moreTabPage.Controls.Add(_startWhenSettingUpCheckBox);
            moreTabPage.Controls.Add(_autoStopWhenCrashingCheckBox);
            moreTabPage.Controls.Add(_autoRestartCheckBox);
            moreTabPage.Location = new System.Drawing.Point(8, 45);
            moreTabPage.Name = "moreTabPage";
            moreTabPage.Padding = new System.Windows.Forms.Padding(3);
            moreTabPage.Size = new System.Drawing.Size(758, 412);
            moreTabPage.TabIndex = 2;
            moreTabPage.Text = "更多";
            moreTabPage.UseVisualStyleBackColor = true;
            // 
            // _stopCommandsTextBox
            // 
            _stopCommandsTextBox.AcceptsReturn = true;
            _stopCommandsTextBox.Location = new System.Drawing.Point(26, 251);
            _stopCommandsTextBox.Multiline = true;
            _stopCommandsTextBox.Name = "_stopCommandsTextBox";
            _stopCommandsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _stopCommandsTextBox.Size = new System.Drawing.Size(682, 119);
            _stopCommandsTextBox.TabIndex = 6;
            _stopCommandsTextBox.Text = "stop";
            toolTip.SetToolTip(_stopCommandsTextBox, "关闭服务器时输入的命令（一行一个）");
            // 
            // _portNumericUpDown
            // 
            _portNumericUpDown.Location = new System.Drawing.Point(26, 170);
            _portNumericUpDown.Maximum = new decimal(new int[] { 65536, 0, 0, 0 });
            _portNumericUpDown.Name = "_portNumericUpDown";
            _portNumericUpDown.Size = new System.Drawing.Size(240, 38);
            _portNumericUpDown.TabIndex = 4;
            toolTip.SetToolTip(_portNumericUpDown, "服务器的IPv4端口，用于获取服务器相关信息（版本、在线玩家数）");
            // 
            // _startWhenSettingUpCheckBox
            // 
            _startWhenSettingUpCheckBox.AutoSize = true;
            _startWhenSettingUpCheckBox.Location = new System.Drawing.Point(26, 88);
            _startWhenSettingUpCheckBox.Name = "_startWhenSettingUpCheckBox";
            _startWhenSettingUpCheckBox.Size = new System.Drawing.Size(310, 35);
            _startWhenSettingUpCheckBox.TabIndex = 2;
            _startWhenSettingUpCheckBox.Text = "应用程序启动后自动运行";
            _startWhenSettingUpCheckBox.UseVisualStyleBackColor = true;
            // 
            // _autoStopWhenCrashingCheckBox
            // 
            _autoStopWhenCrashingCheckBox.AutoSize = true;
            _autoStopWhenCrashingCheckBox.Location = new System.Drawing.Point(26, 47);
            _autoStopWhenCrashingCheckBox.Name = "_autoStopWhenCrashingCheckBox";
            _autoStopWhenCrashingCheckBox.Size = new System.Drawing.Size(382, 35);
            _autoStopWhenCrashingCheckBox.TabIndex = 1;
            _autoStopWhenCrashingCheckBox.Text = "应用程序崩溃时自动停止服务器";
            _autoStopWhenCrashingCheckBox.UseVisualStyleBackColor = true;
            // 
            // _autoRestartCheckBox
            // 
            _autoRestartCheckBox.AutoSize = true;
            _autoRestartCheckBox.Location = new System.Drawing.Point(26, 6);
            _autoRestartCheckBox.Name = "_autoRestartCheckBox";
            _autoRestartCheckBox.Size = new System.Drawing.Size(358, 35);
            _autoRestartCheckBox.TabIndex = 0;
            _autoRestartCheckBox.Text = "当退出代码不为零时自动重启";
            _autoRestartCheckBox.UseVisualStyleBackColor = true;
            // 
            // confirmButton
            // 
            confirmButton.Location = new System.Drawing.Point(310, 471);
            confirmButton.Name = "confirmButton";
            confirmButton.Size = new System.Drawing.Size(150, 46);
            confirmButton.TabIndex = 3;
            confirmButton.Text = "确认";
            confirmButton.UseVisualStyleBackColor = true;
            confirmButton.Click += ConfirmButton_Click;
            // 
            // _errorProvider
            // 
            _errorProvider.ContainerControl = this;
            // 
            // ConfigurationEditor
            // 
            AcceptButton = confirmButton;
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(774, 529);
            Controls.Add(confirmButton);
            Controls.Add(MainTabControl);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(800, 600);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(800, 600);
            Name = "ConfigurationEditor";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "服务器配置编辑器";
            MainTabControl.ResumeLayout(false);
            commonTabPage.ResumeLayout(false);
            commonTabPage.PerformLayout();
            inputAndOutputTabPage.ResumeLayout(false);
            inputAndOutputTabPage.PerformLayout();
            moreTabPage.ResumeLayout(false);
            moreTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_portNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox _idTextBox;
        private System.Windows.Forms.TextBox _fileNameTextBox;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TextBox _argumentTextBox;
        private System.Windows.Forms.CheckBox _autoStopWhenCrashingCheckBox;
        private System.Windows.Forms.CheckBox _autoRestartCheckBox;
        private System.Windows.Forms.CheckBox _startWhenSettingUpCheckBox;
        private System.Windows.Forms.NumericUpDown _portNumericUpDown;
        private System.Windows.Forms.TextBox _stopCommandsTextBox;
        private System.Windows.Forms.ComboBox _inputEncondingComboBox;
        private System.Windows.Forms.ComboBox _outputEncondingComboBox;
        private System.Windows.Forms.ComboBox _outputStyleComboBox;
        private System.Windows.Forms.CheckBox _saveLogCheckBox;
        private System.Windows.Forms.CheckBox _outputCommandUserInputCheckBox;
        private System.Windows.Forms.CheckBox _useUnicodeCharsCheckBox;
        private System.Windows.Forms.TextBox _lineTerminatorTextBox;
        private System.Windows.Forms.TextBox _nameTextBox;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox _usePtyCheckBox;
        private System.Windows.Forms.CheckBox _forceWinPtyCheckBox;
    }
}
