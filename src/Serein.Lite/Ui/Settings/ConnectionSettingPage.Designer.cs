namespace Serein.Lite.Ui.Settings
{
    partial class ConnectionSettingPage
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
            System.Windows.Forms.Label AdministratorsLabel;
            System.Windows.Forms.Label GroupsLabel;
            System.Windows.Forms.Label SubProtocolsLabel;
            System.Windows.Forms.Label AccessTokenLabel;
            System.Windows.Forms.Label UriLabel;
            AdministratorsTextBox = new System.Windows.Forms.TextBox();
            GroupsTextBox = new System.Windows.Forms.TextBox();
            GivePermissionToAllAdminsCheckBox = new System.Windows.Forms.CheckBox();
            SaveLogCheckBox = new System.Windows.Forms.CheckBox();
            AutoEscapeCheckBox = new System.Windows.Forms.CheckBox();
            OutputDataCheckBox = new System.Windows.Forms.CheckBox();
            AutoReconnectCheckBox = new System.Windows.Forms.CheckBox();
            UseReverseWebSocketCheckBox = new System.Windows.Forms.CheckBox();
            AccessTokenMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            SubProtocolsTextBox = new System.Windows.Forms.TextBox();
            UriTextBox = new System.Windows.Forms.TextBox();
            AdministratorsLabel = new System.Windows.Forms.Label();
            GroupsLabel = new System.Windows.Forms.Label();
            SubProtocolsLabel = new System.Windows.Forms.Label();
            AccessTokenLabel = new System.Windows.Forms.Label();
            UriLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // AdministratorsLabel
            // 
            AdministratorsLabel.AutoSize = true;
            AdministratorsLabel.Location = new System.Drawing.Point(21, 715);
            AdministratorsLabel.Name = "AdministratorsLabel";
            AdministratorsLabel.Size = new System.Drawing.Size(158, 31);
            AdministratorsLabel.TabIndex = 30;
            AdministratorsLabel.Text = "管理权限列表";
            // 
            // GroupsLabel
            // 
            GroupsLabel.AutoSize = true;
            GroupsLabel.Location = new System.Drawing.Point(21, 633);
            GroupsLabel.Name = "GroupsLabel";
            GroupsLabel.Size = new System.Drawing.Size(134, 31);
            GroupsLabel.TabIndex = 28;
            GroupsLabel.Text = "监听群列表";
            // 
            // SubProtocolsLabel
            // 
            SubProtocolsLabel.AutoSize = true;
            SubProtocolsLabel.Location = new System.Drawing.Point(21, 181);
            SubProtocolsLabel.Name = "SubProtocolsLabel";
            SubProtocolsLabel.Size = new System.Drawing.Size(216, 31);
            SubProtocolsLabel.TabIndex = 20;
            SubProtocolsLabel.Text = "WebSocket子协议";
            // 
            // AccessTokenLabel
            // 
            AccessTokenLabel.AutoSize = true;
            AccessTokenLabel.Location = new System.Drawing.Point(21, 99);
            AccessTokenLabel.Name = "AccessTokenLabel";
            AccessTokenLabel.Size = new System.Drawing.Size(229, 31);
            AccessTokenLabel.TabIndex = 18;
            AccessTokenLabel.Text = "鉴权凭证（Token）";
            // 
            // UriLabel
            // 
            UriLabel.AutoSize = true;
            UriLabel.Location = new System.Drawing.Point(21, 17);
            UriLabel.Name = "UriLabel";
            UriLabel.Size = new System.Drawing.Size(62, 31);
            UriLabel.TabIndex = 16;
            UriLabel.Text = "地址";
            // 
            // AdministratorsTextBox
            // 
            AdministratorsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            AdministratorsTextBox.Location = new System.Drawing.Point(21, 749);
            AdministratorsTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            AdministratorsTextBox.Name = "AdministratorsTextBox";
            AdministratorsTextBox.PlaceholderText = "使用;分隔";
            AdministratorsTextBox.Size = new System.Drawing.Size(1211, 38);
            AdministratorsTextBox.TabIndex = 31;
            AdministratorsTextBox.TextChanged += AdministratorsTextBox_TextChanged;
            // 
            // GroupsTextBox
            // 
            GroupsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GroupsTextBox.Location = new System.Drawing.Point(21, 667);
            GroupsTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            GroupsTextBox.Name = "GroupsTextBox";
            GroupsTextBox.PlaceholderText = "使用;分隔";
            GroupsTextBox.Size = new System.Drawing.Size(1211, 38);
            GroupsTextBox.TabIndex = 29;
            GroupsTextBox.TextChanged += GroupsTextBox_TextChanged;
            // 
            // GivePermissionToAllAdminsCheckBox
            // 
            GivePermissionToAllAdminsCheckBox.AutoSize = true;
            GivePermissionToAllAdminsCheckBox.Location = new System.Drawing.Point(21, 595);
            GivePermissionToAllAdminsCheckBox.Name = "GivePermissionToAllAdminsCheckBox";
            GivePermissionToAllAdminsCheckBox.Size = new System.Drawing.Size(382, 35);
            GivePermissionToAllAdminsCheckBox.TabIndex = 27;
            GivePermissionToAllAdminsCheckBox.Text = "赋予所有群主和管理员管理权限";
            GivePermissionToAllAdminsCheckBox.UseVisualStyleBackColor = true;
            GivePermissionToAllAdminsCheckBox.Click += OnPropertyChanged;
            // 
            // SaveLogCheckBox
            // 
            SaveLogCheckBox.AutoSize = true;
            SaveLogCheckBox.Location = new System.Drawing.Point(21, 525);
            SaveLogCheckBox.Name = "SaveLogCheckBox";
            SaveLogCheckBox.Size = new System.Drawing.Size(238, 35);
            SaveLogCheckBox.TabIndex = 26;
            SaveLogCheckBox.Text = "保存数据包到日志";
            SaveLogCheckBox.UseVisualStyleBackColor = true;
            SaveLogCheckBox.Click += OnPropertyChanged;
            // 
            // AutoEscapeCheckBox
            // 
            AutoEscapeCheckBox.AutoSize = true;
            AutoEscapeCheckBox.Location = new System.Drawing.Point(21, 484);
            AutoEscapeCheckBox.Name = "AutoEscapeCheckBox";
            AutoEscapeCheckBox.Size = new System.Drawing.Size(166, 35);
            AutoEscapeCheckBox.TabIndex = 25;
            AutoEscapeCheckBox.Text = "纯文本发送";
            AutoEscapeCheckBox.UseVisualStyleBackColor = true;
            AutoEscapeCheckBox.Click += OnPropertyChanged;
            // 
            // OutputDataCheckBox
            // 
            OutputDataCheckBox.AutoSize = true;
            OutputDataCheckBox.Location = new System.Drawing.Point(21, 443);
            OutputDataCheckBox.Name = "OutputDataCheckBox";
            OutputDataCheckBox.Size = new System.Drawing.Size(214, 35);
            OutputDataCheckBox.TabIndex = 24;
            OutputDataCheckBox.Text = "输出收发的数据";
            OutputDataCheckBox.UseVisualStyleBackColor = true;
            OutputDataCheckBox.Click += OnPropertyChanged;
            // 
            // AutoReconnectCheckBox
            // 
            AutoReconnectCheckBox.AutoSize = true;
            AutoReconnectCheckBox.Location = new System.Drawing.Point(21, 373);
            AutoReconnectCheckBox.Name = "AutoReconnectCheckBox";
            AutoReconnectCheckBox.Size = new System.Drawing.Size(190, 35);
            AutoReconnectCheckBox.TabIndex = 23;
            AutoReconnectCheckBox.Text = "断线自动重连";
            AutoReconnectCheckBox.UseVisualStyleBackColor = true;
            AutoReconnectCheckBox.Click += OnPropertyChanged;
            // 
            // UseReverseWebSocketCheckBox
            // 
            UseReverseWebSocketCheckBox.AutoSize = true;
            UseReverseWebSocketCheckBox.Location = new System.Drawing.Point(21, 332);
            UseReverseWebSocketCheckBox.Name = "UseReverseWebSocketCheckBox";
            UseReverseWebSocketCheckBox.Size = new System.Drawing.Size(272, 35);
            UseReverseWebSocketCheckBox.TabIndex = 22;
            UseReverseWebSocketCheckBox.Text = "使用反向WebSocket";
            UseReverseWebSocketCheckBox.UseVisualStyleBackColor = true;
            UseReverseWebSocketCheckBox.Click += OnPropertyChanged;
            // 
            // AccessTokenMaskedTextBox
            // 
            AccessTokenMaskedTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            AccessTokenMaskedTextBox.Location = new System.Drawing.Point(21, 133);
            AccessTokenMaskedTextBox.Name = "AccessTokenMaskedTextBox";
            AccessTokenMaskedTextBox.PasswordChar = '*';
            AccessTokenMaskedTextBox.Size = new System.Drawing.Size(1211, 38);
            AccessTokenMaskedTextBox.TabIndex = 19;
            AccessTokenMaskedTextBox.TextChanged += OnPropertyChanged;
            // 
            // SubProtocolsTextBox
            // 
            SubProtocolsTextBox.AcceptsReturn = true;
            SubProtocolsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            SubProtocolsTextBox.Location = new System.Drawing.Point(21, 215);
            SubProtocolsTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            SubProtocolsTextBox.Multiline = true;
            SubProtocolsTextBox.Name = "SubProtocolsTextBox";
            SubProtocolsTextBox.Size = new System.Drawing.Size(1211, 104);
            SubProtocolsTextBox.TabIndex = 21;
            SubProtocolsTextBox.TextChanged += SubProtocolsTextBox_TextChanged;
            // 
            // UriTextBox
            // 
            UriTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            UriTextBox.Location = new System.Drawing.Point(21, 51);
            UriTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            UriTextBox.Name = "UriTextBox";
            UriTextBox.Size = new System.Drawing.Size(1211, 38);
            UriTextBox.TabIndex = 17;
            UriTextBox.TextChanged += OnPropertyChanged;
            // 
            // ConnectionSettingPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(AdministratorsTextBox);
            Controls.Add(AdministratorsLabel);
            Controls.Add(GroupsTextBox);
            Controls.Add(GroupsLabel);
            Controls.Add(GivePermissionToAllAdminsCheckBox);
            Controls.Add(SaveLogCheckBox);
            Controls.Add(AutoEscapeCheckBox);
            Controls.Add(OutputDataCheckBox);
            Controls.Add(AutoReconnectCheckBox);
            Controls.Add(UseReverseWebSocketCheckBox);
            Controls.Add(AccessTokenMaskedTextBox);
            Controls.Add(SubProtocolsTextBox);
            Controls.Add(SubProtocolsLabel);
            Controls.Add(AccessTokenLabel);
            Controls.Add(UriTextBox);
            Controls.Add(UriLabel);
            Name = "ConnectionSettingPage";
            Size = new System.Drawing.Size(1280, 808);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox AdministratorsTextBox;
        private System.Windows.Forms.TextBox GroupsTextBox;
        private System.Windows.Forms.CheckBox GivePermissionToAllAdminsCheckBox;
        private System.Windows.Forms.CheckBox SaveLogCheckBox;
        private System.Windows.Forms.CheckBox AutoEscapeCheckBox;
        private System.Windows.Forms.CheckBox OutputDataCheckBox;
        private System.Windows.Forms.CheckBox AutoReconnectCheckBox;
        private System.Windows.Forms.CheckBox UseReverseWebSocketCheckBox;
        private System.Windows.Forms.MaskedTextBox AccessTokenMaskedTextBox;
        private System.Windows.Forms.TextBox SubProtocolsTextBox;
        private System.Windows.Forms.TextBox UriTextBox;
    }
}
