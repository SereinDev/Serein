﻿namespace Serein.Lite.Ui.Servers
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
            System.Windows.Forms.Label IdLabel;
            System.Windows.Forms.Label NameLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationEditor));
            FileNameLabel = new System.Windows.Forms.Label();
            FileNameTextBox = new System.Windows.Forms.TextBox();
            IdTextBox = new System.Windows.Forms.TextBox();
            MainTabControl = new System.Windows.Forms.TabControl();
            CommonTabPage = new System.Windows.Forms.TabPage();
            NameTextBox = new System.Windows.Forms.TextBox();
            ArgumentTextBox = new System.Windows.Forms.TextBox();
            ArgumentLabel = new System.Windows.Forms.Label();
            InputAndOutputTabPage = new System.Windows.Forms.TabPage();
            LineTerminatorTextBox = new System.Windows.Forms.TextBox();
            LineTerminatorLabel = new System.Windows.Forms.Label();
            UseUnicodeCharsCheckBox = new System.Windows.Forms.CheckBox();
            OutputCommandUserInputCheckBox = new System.Windows.Forms.CheckBox();
            SaveLogCheckBox = new System.Windows.Forms.CheckBox();
            OutputStyleLabel = new System.Windows.Forms.Label();
            OutputStyleComboBox = new System.Windows.Forms.ComboBox();
            OutputEncondingLabel = new System.Windows.Forms.Label();
            InputEncondingLabel = new System.Windows.Forms.Label();
            OutputEncondingComboBox = new System.Windows.Forms.ComboBox();
            InputEncondingComboBox = new System.Windows.Forms.ComboBox();
            MoreTabPage = new System.Windows.Forms.TabPage();
            StopCommandsTextBox = new System.Windows.Forms.TextBox();
            StopLabel = new System.Windows.Forms.Label();
            PortLabel = new System.Windows.Forms.Label();
            PortNumericUpDown = new System.Windows.Forms.NumericUpDown();
            StartWhenSettingUpCheckBox = new System.Windows.Forms.CheckBox();
            AutoStopWhenCrashingCheckBox = new System.Windows.Forms.CheckBox();
            AutoRestartCheckBox = new System.Windows.Forms.CheckBox();
            ConfirmButton = new System.Windows.Forms.Button();
            IdLabel = new System.Windows.Forms.Label();
            NameLabel = new System.Windows.Forms.Label();
            MainTabControl.SuspendLayout();
            CommonTabPage.SuspendLayout();
            InputAndOutputTabPage.SuspendLayout();
            MoreTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PortNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // IdLabel
            // 
            IdLabel.AutoSize = true;
            IdLabel.Location = new System.Drawing.Point(17, 15);
            IdLabel.Name = "IdLabel";
            IdLabel.Size = new System.Drawing.Size(36, 31);
            IdLabel.TabIndex = 0;
            IdLabel.Text = "Id";
            // 
            // NameLabel
            // 
            NameLabel.AutoSize = true;
            NameLabel.Location = new System.Drawing.Point(17, 111);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new System.Drawing.Size(62, 31);
            NameLabel.TabIndex = 2;
            NameLabel.Text = "名称";
            // 
            // FileNameLabel
            // 
            FileNameLabel.AutoSize = true;
            FileNameLabel.Location = new System.Drawing.Point(17, 207);
            FileNameLabel.Name = "FileNameLabel";
            FileNameLabel.Size = new System.Drawing.Size(110, 31);
            FileNameLabel.TabIndex = 4;
            FileNameLabel.Text = "启动文件";
            // 
            // FileNameTextBox
            // 
            FileNameTextBox.Location = new System.Drawing.Point(17, 245);
            FileNameTextBox.Margin = new System.Windows.Forms.Padding(3, 7, 3, 20);
            FileNameTextBox.Name = "FileNameTextBox";
            FileNameTextBox.PlaceholderText = "启动时运行的文件（双击选择文件）";
            FileNameTextBox.Size = new System.Drawing.Size(722, 38);
            FileNameTextBox.TabIndex = 5;
            FileNameTextBox.DoubleClick += FileName_DoubleClick;
            // 
            // IdTextBox
            // 
            IdTextBox.Location = new System.Drawing.Point(17, 53);
            IdTextBox.Margin = new System.Windows.Forms.Padding(3, 7, 3, 20);
            IdTextBox.Name = "IdTextBox";
            IdTextBox.PlaceholderText = "用于区分服务器；只能由字母、数字和下划线组成";
            IdTextBox.Size = new System.Drawing.Size(722, 38);
            IdTextBox.TabIndex = 1;
            // 
            // MainTabControl
            // 
            MainTabControl.Controls.Add(CommonTabPage);
            MainTabControl.Controls.Add(InputAndOutputTabPage);
            MainTabControl.Controls.Add(MoreTabPage);
            MainTabControl.Dock = System.Windows.Forms.DockStyle.Top;
            MainTabControl.Location = new System.Drawing.Point(0, 0);
            MainTabControl.Name = "MainTabControl";
            MainTabControl.SelectedIndex = 0;
            MainTabControl.Size = new System.Drawing.Size(774, 465);
            MainTabControl.TabIndex = 2;
            // 
            // CommonTabPage
            // 
            CommonTabPage.AutoScroll = true;
            CommonTabPage.Controls.Add(NameLabel);
            CommonTabPage.Controls.Add(NameTextBox);
            CommonTabPage.Controls.Add(ArgumentTextBox);
            CommonTabPage.Controls.Add(ArgumentLabel);
            CommonTabPage.Controls.Add(FileNameLabel);
            CommonTabPage.Controls.Add(FileNameTextBox);
            CommonTabPage.Controls.Add(IdLabel);
            CommonTabPage.Controls.Add(IdTextBox);
            CommonTabPage.Location = new System.Drawing.Point(8, 45);
            CommonTabPage.Name = "CommonTabPage";
            CommonTabPage.Padding = new System.Windows.Forms.Padding(3);
            CommonTabPage.Size = new System.Drawing.Size(758, 412);
            CommonTabPage.TabIndex = 0;
            CommonTabPage.Text = "常规";
            CommonTabPage.UseVisualStyleBackColor = true;
            // 
            // NameTextBox
            // 
            NameTextBox.Location = new System.Drawing.Point(17, 149);
            NameTextBox.Margin = new System.Windows.Forms.Padding(3, 7, 3, 20);
            NameTextBox.Name = "NameTextBox";
            NameTextBox.Size = new System.Drawing.Size(722, 38);
            NameTextBox.TabIndex = 3;
            // 
            // ArgumentTextBox
            // 
            ArgumentTextBox.Location = new System.Drawing.Point(17, 341);
            ArgumentTextBox.Margin = new System.Windows.Forms.Padding(3, 7, 3, 20);
            ArgumentTextBox.Name = "ArgumentTextBox";
            ArgumentTextBox.PlaceholderText = "启动时附加的参数";
            ArgumentTextBox.Size = new System.Drawing.Size(722, 38);
            ArgumentTextBox.TabIndex = 7;
            // 
            // ArgumentLabel
            // 
            ArgumentLabel.AutoSize = true;
            ArgumentLabel.Location = new System.Drawing.Point(17, 303);
            ArgumentLabel.Name = "ArgumentLabel";
            ArgumentLabel.Size = new System.Drawing.Size(110, 31);
            ArgumentLabel.TabIndex = 6;
            ArgumentLabel.Text = "启动参数";
            // 
            // InputAndOutputTabPage
            // 
            InputAndOutputTabPage.Controls.Add(LineTerminatorTextBox);
            InputAndOutputTabPage.Controls.Add(LineTerminatorLabel);
            InputAndOutputTabPage.Controls.Add(UseUnicodeCharsCheckBox);
            InputAndOutputTabPage.Controls.Add(OutputCommandUserInputCheckBox);
            InputAndOutputTabPage.Controls.Add(SaveLogCheckBox);
            InputAndOutputTabPage.Controls.Add(OutputStyleLabel);
            InputAndOutputTabPage.Controls.Add(OutputStyleComboBox);
            InputAndOutputTabPage.Controls.Add(OutputEncondingLabel);
            InputAndOutputTabPage.Controls.Add(InputEncondingLabel);
            InputAndOutputTabPage.Controls.Add(OutputEncondingComboBox);
            InputAndOutputTabPage.Controls.Add(InputEncondingComboBox);
            InputAndOutputTabPage.Location = new System.Drawing.Point(8, 45);
            InputAndOutputTabPage.Name = "InputAndOutputTabPage";
            InputAndOutputTabPage.Padding = new System.Windows.Forms.Padding(3);
            InputAndOutputTabPage.Size = new System.Drawing.Size(758, 412);
            InputAndOutputTabPage.TabIndex = 1;
            InputAndOutputTabPage.Text = "输出/输入";
            InputAndOutputTabPage.UseVisualStyleBackColor = true;
            // 
            // LineTerminatorTextBox
            // 
            LineTerminatorTextBox.Location = new System.Drawing.Point(23, 329);
            LineTerminatorTextBox.Margin = new System.Windows.Forms.Padding(3, 7, 3, 20);
            LineTerminatorTextBox.Name = "LineTerminatorTextBox";
            LineTerminatorTextBox.Size = new System.Drawing.Size(722, 38);
            LineTerminatorTextBox.TabIndex = 7;
            // 
            // LineTerminatorLabel
            // 
            LineTerminatorLabel.AutoSize = true;
            LineTerminatorLabel.Location = new System.Drawing.Point(23, 291);
            LineTerminatorLabel.Name = "LineTerminatorLabel";
            LineTerminatorLabel.Size = new System.Drawing.Size(110, 31);
            LineTerminatorLabel.TabIndex = 6;
            LineTerminatorLabel.Text = "行终止符";
            // 
            // UseUnicodeCharsCheckBox
            // 
            UseUnicodeCharsCheckBox.AutoSize = true;
            UseUnicodeCharsCheckBox.Location = new System.Drawing.Point(365, 128);
            UseUnicodeCharsCheckBox.Name = "UseUnicodeCharsCheckBox";
            UseUnicodeCharsCheckBox.Size = new System.Drawing.Size(237, 35);
            UseUnicodeCharsCheckBox.TabIndex = 10;
            UseUnicodeCharsCheckBox.Text = "使用Unicode字符";
            UseUnicodeCharsCheckBox.UseVisualStyleBackColor = true;
            // 
            // OutputCommandUserInputCheckBox
            // 
            OutputCommandUserInputCheckBox.AutoSize = true;
            OutputCommandUserInputCheckBox.Location = new System.Drawing.Point(365, 87);
            OutputCommandUserInputCheckBox.Name = "OutputCommandUserInputCheckBox";
            OutputCommandUserInputCheckBox.Size = new System.Drawing.Size(214, 35);
            OutputCommandUserInputCheckBox.TabIndex = 9;
            OutputCommandUserInputCheckBox.Text = "显示输出的命令";
            OutputCommandUserInputCheckBox.UseVisualStyleBackColor = true;
            // 
            // SaveLogCheckBox
            // 
            SaveLogCheckBox.AutoSize = true;
            SaveLogCheckBox.Location = new System.Drawing.Point(365, 46);
            SaveLogCheckBox.Name = "SaveLogCheckBox";
            SaveLogCheckBox.Size = new System.Drawing.Size(142, 35);
            SaveLogCheckBox.TabIndex = 8;
            SaveLogCheckBox.Text = "保存日志";
            SaveLogCheckBox.UseVisualStyleBackColor = true;
            // 
            // OutputStyleLabel
            // 
            OutputStyleLabel.AutoSize = true;
            OutputStyleLabel.Location = new System.Drawing.Point(23, 198);
            OutputStyleLabel.Name = "OutputStyleLabel";
            OutputStyleLabel.Size = new System.Drawing.Size(110, 31);
            OutputStyleLabel.TabIndex = 4;
            OutputStyleLabel.Text = "输出样式";
            // 
            // OutputStyleComboBox
            // 
            OutputStyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            OutputStyleComboBox.FormattingEnabled = true;
            OutputStyleComboBox.Items.AddRange(new object[] { "无颜色", "原始颜色", "高亮", "混合" });
            OutputStyleComboBox.Location = new System.Drawing.Point(23, 232);
            OutputStyleComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            OutputStyleComboBox.Name = "OutputStyleComboBox";
            OutputStyleComboBox.Size = new System.Drawing.Size(197, 39);
            OutputStyleComboBox.TabIndex = 5;
            // 
            // OutputEncondingLabel
            // 
            OutputEncondingLabel.AutoSize = true;
            OutputEncondingLabel.Location = new System.Drawing.Point(23, 105);
            OutputEncondingLabel.Name = "OutputEncondingLabel";
            OutputEncondingLabel.Size = new System.Drawing.Size(110, 31);
            OutputEncondingLabel.TabIndex = 2;
            OutputEncondingLabel.Text = "输出编码";
            // 
            // InputEncondingLabel
            // 
            InputEncondingLabel.AutoSize = true;
            InputEncondingLabel.Location = new System.Drawing.Point(23, 12);
            InputEncondingLabel.Name = "InputEncondingLabel";
            InputEncondingLabel.Size = new System.Drawing.Size(110, 31);
            InputEncondingLabel.TabIndex = 0;
            InputEncondingLabel.Text = "输入编码";
            // 
            // OutputEncondingComboBox
            // 
            OutputEncondingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            OutputEncondingComboBox.FormattingEnabled = true;
            OutputEncondingComboBox.Items.AddRange(new object[] { "UTF8", "UTF16-LE", "UTF16-BE", "GBK" });
            OutputEncondingComboBox.Location = new System.Drawing.Point(23, 139);
            OutputEncondingComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            OutputEncondingComboBox.Name = "OutputEncondingComboBox";
            OutputEncondingComboBox.Size = new System.Drawing.Size(197, 39);
            OutputEncondingComboBox.TabIndex = 3;
            // 
            // InputEncondingComboBox
            // 
            InputEncondingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            InputEncondingComboBox.FormattingEnabled = true;
            InputEncondingComboBox.Items.AddRange(new object[] { "UTF8", "UTF16-LE", "UTF16-BE", "GBK" });
            InputEncondingComboBox.Location = new System.Drawing.Point(23, 46);
            InputEncondingComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            InputEncondingComboBox.Name = "InputEncondingComboBox";
            InputEncondingComboBox.Size = new System.Drawing.Size(197, 39);
            InputEncondingComboBox.TabIndex = 1;
            // 
            // MoreTabPage
            // 
            MoreTabPage.Controls.Add(StopCommandsTextBox);
            MoreTabPage.Controls.Add(StopLabel);
            MoreTabPage.Controls.Add(PortLabel);
            MoreTabPage.Controls.Add(PortNumericUpDown);
            MoreTabPage.Controls.Add(StartWhenSettingUpCheckBox);
            MoreTabPage.Controls.Add(AutoStopWhenCrashingCheckBox);
            MoreTabPage.Controls.Add(AutoRestartCheckBox);
            MoreTabPage.Location = new System.Drawing.Point(8, 45);
            MoreTabPage.Name = "MoreTabPage";
            MoreTabPage.Padding = new System.Windows.Forms.Padding(3);
            MoreTabPage.Size = new System.Drawing.Size(758, 412);
            MoreTabPage.TabIndex = 2;
            MoreTabPage.Text = "更多";
            MoreTabPage.UseVisualStyleBackColor = true;
            // 
            // StopCommandsTextBox
            // 
            StopCommandsTextBox.AcceptsReturn = true;
            StopCommandsTextBox.Location = new System.Drawing.Point(26, 251);
            StopCommandsTextBox.Multiline = true;
            StopCommandsTextBox.Name = "StopCommandsTextBox";
            StopCommandsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            StopCommandsTextBox.Size = new System.Drawing.Size(682, 119);
            StopCommandsTextBox.TabIndex = 6;
            StopCommandsTextBox.Text = "stop";
            // 
            // StopLabel
            // 
            StopLabel.AutoSize = true;
            StopLabel.Location = new System.Drawing.Point(26, 217);
            StopLabel.Name = "StopLabel";
            StopLabel.Size = new System.Drawing.Size(110, 31);
            StopLabel.TabIndex = 5;
            StopLabel.Text = "关服命令";
            // 
            // PortLabel
            // 
            PortLabel.AutoSize = true;
            PortLabel.Location = new System.Drawing.Point(25, 136);
            PortLabel.Name = "PortLabel";
            PortLabel.Size = new System.Drawing.Size(111, 31);
            PortLabel.TabIndex = 3;
            PortLabel.Text = "IPv4端口";
            // 
            // PortNumericUpDown
            // 
            PortNumericUpDown.Location = new System.Drawing.Point(26, 170);
            PortNumericUpDown.Maximum = new decimal(new int[] { 65536, 0, 0, 0 });
            PortNumericUpDown.Name = "PortNumericUpDown";
            PortNumericUpDown.Size = new System.Drawing.Size(240, 38);
            PortNumericUpDown.TabIndex = 4;
            // 
            // StartWhenSettingUpCheckBox
            // 
            StartWhenSettingUpCheckBox.AutoSize = true;
            StartWhenSettingUpCheckBox.Location = new System.Drawing.Point(26, 88);
            StartWhenSettingUpCheckBox.Name = "StartWhenSettingUpCheckBox";
            StartWhenSettingUpCheckBox.Size = new System.Drawing.Size(382, 35);
            StartWhenSettingUpCheckBox.TabIndex = 2;
            StartWhenSettingUpCheckBox.Text = "应用程序启动后自动运行服务器";
            StartWhenSettingUpCheckBox.UseVisualStyleBackColor = true;
            // 
            // AutoStopWhenCrashingCheckBox
            // 
            AutoStopWhenCrashingCheckBox.AutoSize = true;
            AutoStopWhenCrashingCheckBox.Location = new System.Drawing.Point(26, 47);
            AutoStopWhenCrashingCheckBox.Name = "AutoStopWhenCrashingCheckBox";
            AutoStopWhenCrashingCheckBox.Size = new System.Drawing.Size(382, 35);
            AutoStopWhenCrashingCheckBox.TabIndex = 1;
            AutoStopWhenCrashingCheckBox.Text = "应用程序崩溃时自动停止服务器";
            AutoStopWhenCrashingCheckBox.UseVisualStyleBackColor = true;
            // 
            // AutoRestartCheckBox
            // 
            AutoRestartCheckBox.AutoSize = true;
            AutoRestartCheckBox.Location = new System.Drawing.Point(26, 6);
            AutoRestartCheckBox.Name = "AutoRestartCheckBox";
            AutoRestartCheckBox.Size = new System.Drawing.Size(142, 35);
            AutoRestartCheckBox.TabIndex = 0;
            AutoRestartCheckBox.Text = "自动重启";
            AutoRestartCheckBox.UseVisualStyleBackColor = true;
            // 
            // ConfirmButton
            // 
            ConfirmButton.Location = new System.Drawing.Point(310, 471);
            ConfirmButton.Name = "ConfirmButton";
            ConfirmButton.Size = new System.Drawing.Size(150, 46);
            ConfirmButton.TabIndex = 3;
            ConfirmButton.Text = "确认";
            ConfirmButton.UseVisualStyleBackColor = true;
            ConfirmButton.Click += ConfirmButton_Click;
            // 
            // ConfigurationEditor
            // 
            AcceptButton = ConfirmButton;
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(774, 529);
            Controls.Add(ConfirmButton);
            Controls.Add(MainTabControl);
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
            CommonTabPage.ResumeLayout(false);
            CommonTabPage.PerformLayout();
            InputAndOutputTabPage.ResumeLayout(false);
            InputAndOutputTabPage.PerformLayout();
            MoreTabPage.ResumeLayout(false);
            MoreTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PortNumericUpDown).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label FileNameLabel;
        private System.Windows.Forms.TextBox IdTextBox;
        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage CommonTabPage;
        private System.Windows.Forms.TabPage InputAndOutputTabPage;
        private System.Windows.Forms.TextBox ArgumentTextBox;
        private System.Windows.Forms.Label ArgumentLabel;
        private System.Windows.Forms.TabPage MoreTabPage;
        private System.Windows.Forms.CheckBox AutoStopWhenCrashingCheckBox;
        private System.Windows.Forms.CheckBox AutoRestartCheckBox;
        private System.Windows.Forms.CheckBox StartWhenSettingUpCheckBox;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.NumericUpDown PortNumericUpDown;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.TextBox StopCommandsTextBox;
        private System.Windows.Forms.Label StopLabel;
        private System.Windows.Forms.ComboBox InputEncondingComboBox;
        private System.Windows.Forms.Label OutputEncondingLabel;
        private System.Windows.Forms.Label InputEncondingLabel;
        private System.Windows.Forms.ComboBox OutputEncondingComboBox;
        private System.Windows.Forms.ComboBox OutputStyleComboBox;
        private System.Windows.Forms.Label OutputStyleLabel;
        private System.Windows.Forms.CheckBox SaveLogCheckBox;
        private System.Windows.Forms.CheckBox OutputCommandUserInputCheckBox;
        private System.Windows.Forms.CheckBox UseUnicodeCharsCheckBox;
        private System.Windows.Forms.TextBox LineTerminatorTextBox;
        private System.Windows.Forms.Label LineTerminatorLabel;
        private System.Windows.Forms.TextBox NameTextBox;
    }
}