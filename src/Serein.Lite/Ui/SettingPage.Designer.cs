namespace Serein.Lite.Ui
{
    partial class SettingPage
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
            System.Windows.Forms.TabControl TabControl;
            System.Windows.Forms.TabPage ConnectionTabPage;
            System.Windows.Forms.Panel ConnectionPanel;
            System.Windows.Forms.Label AdministratorsLabel;
            System.Windows.Forms.Label GroupsLabel;
            System.Windows.Forms.Label SubProtocolsLabel;
            System.Windows.Forms.Label AccessTokenLabel;
            System.Windows.Forms.Label UriLabel;
            System.Windows.Forms.TabPage ReactionTabPage;
            System.Windows.Forms.TabPage ApplicationTabPage;
            AdministratorsTextBox = new System.Windows.Forms.TextBox();
            GroupsLabelTextBox = new System.Windows.Forms.TextBox();
            GivePermissionToAllAdminsCheckBox = new System.Windows.Forms.CheckBox();
            SaveLogCheckBox = new System.Windows.Forms.CheckBox();
            AutoEscapeCheckBox = new System.Windows.Forms.CheckBox();
            OutputDataCheckBox = new System.Windows.Forms.CheckBox();
            AutoReconnectCheckBox = new System.Windows.Forms.CheckBox();
            UseReverseWebSocketCheckBox = new System.Windows.Forms.CheckBox();
            AccessTokenMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            SubProtocolsTextBox = new System.Windows.Forms.TextBox();
            UriTextBox = new System.Windows.Forms.TextBox();
            AboutTabPage = new System.Windows.Forms.TabPage();
            TabControl = new System.Windows.Forms.TabControl();
            ConnectionTabPage = new System.Windows.Forms.TabPage();
            ConnectionPanel = new System.Windows.Forms.Panel();
            AdministratorsLabel = new System.Windows.Forms.Label();
            GroupsLabel = new System.Windows.Forms.Label();
            SubProtocolsLabel = new System.Windows.Forms.Label();
            AccessTokenLabel = new System.Windows.Forms.Label();
            UriLabel = new System.Windows.Forms.Label();
            ReactionTabPage = new System.Windows.Forms.TabPage();
            ApplicationTabPage = new System.Windows.Forms.TabPage();
            TabControl.SuspendLayout();
            ConnectionTabPage.SuspendLayout();
            ConnectionPanel.SuspendLayout();
            SuspendLayout();
            // 
            // TabControl
            // 
            TabControl.Controls.Add(ConnectionTabPage);
            TabControl.Controls.Add(ReactionTabPage);
            TabControl.Controls.Add(ApplicationTabPage);
            TabControl.Controls.Add(AboutTabPage);
            TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            TabControl.Location = new System.Drawing.Point(0, 0);
            TabControl.Margin = new System.Windows.Forms.Padding(0);
            TabControl.Name = "TabControl";
            TabControl.SelectedIndex = 0;
            TabControl.Size = new System.Drawing.Size(1280, 720);
            TabControl.TabIndex = 0;
            // 
            // ConnectionTabPage
            // 
            ConnectionTabPage.Controls.Add(ConnectionPanel);
            ConnectionTabPage.Location = new System.Drawing.Point(8, 45);
            ConnectionTabPage.Name = "ConnectionTabPage";
            ConnectionTabPage.Padding = new System.Windows.Forms.Padding(3);
            ConnectionTabPage.Size = new System.Drawing.Size(1264, 667);
            ConnectionTabPage.TabIndex = 0;
            ConnectionTabPage.Text = "连接";
            ConnectionTabPage.UseVisualStyleBackColor = true;
            // 
            // ConnectionPanel
            // 
            ConnectionPanel.AutoScroll = true;
            ConnectionPanel.Controls.Add(AdministratorsTextBox);
            ConnectionPanel.Controls.Add(AdministratorsLabel);
            ConnectionPanel.Controls.Add(GroupsLabelTextBox);
            ConnectionPanel.Controls.Add(GroupsLabel);
            ConnectionPanel.Controls.Add(GivePermissionToAllAdminsCheckBox);
            ConnectionPanel.Controls.Add(SaveLogCheckBox);
            ConnectionPanel.Controls.Add(AutoEscapeCheckBox);
            ConnectionPanel.Controls.Add(OutputDataCheckBox);
            ConnectionPanel.Controls.Add(AutoReconnectCheckBox);
            ConnectionPanel.Controls.Add(UseReverseWebSocketCheckBox);
            ConnectionPanel.Controls.Add(AccessTokenMaskedTextBox);
            ConnectionPanel.Controls.Add(SubProtocolsTextBox);
            ConnectionPanel.Controls.Add(SubProtocolsLabel);
            ConnectionPanel.Controls.Add(AccessTokenLabel);
            ConnectionPanel.Controls.Add(UriTextBox);
            ConnectionPanel.Controls.Add(UriLabel);
            ConnectionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            ConnectionPanel.Location = new System.Drawing.Point(3, 3);
            ConnectionPanel.Margin = new System.Windows.Forms.Padding(0);
            ConnectionPanel.Name = "ConnectionPanel";
            ConnectionPanel.Size = new System.Drawing.Size(1258, 661);
            ConnectionPanel.TabIndex = 0;
            // 
            // AdministratorsTextBox
            // 
            AdministratorsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            AdministratorsTextBox.Location = new System.Drawing.Point(26, 683);
            AdministratorsTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            AdministratorsTextBox.Name = "AdministratorsTextBox";
            AdministratorsTextBox.PlaceholderText = "使用;分隔";
            AdministratorsTextBox.Size = new System.Drawing.Size(1134, 38);
            AdministratorsTextBox.TabIndex = 15;
            // 
            // AdministratorsLabel
            // 
            AdministratorsLabel.AutoSize = true;
            AdministratorsLabel.Location = new System.Drawing.Point(26, 649);
            AdministratorsLabel.Name = "AdministratorsLabel";
            AdministratorsLabel.Size = new System.Drawing.Size(158, 31);
            AdministratorsLabel.TabIndex = 14;
            AdministratorsLabel.Text = "管理权限列表";
            // 
            // GroupsLabelTextBox
            // 
            GroupsLabelTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GroupsLabelTextBox.Location = new System.Drawing.Point(26, 601);
            GroupsLabelTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            GroupsLabelTextBox.Name = "GroupsLabelTextBox";
            GroupsLabelTextBox.PlaceholderText = "使用;分隔";
            GroupsLabelTextBox.Size = new System.Drawing.Size(1134, 38);
            GroupsLabelTextBox.TabIndex = 13;
            // 
            // GroupsLabel
            // 
            GroupsLabel.AutoSize = true;
            GroupsLabel.Location = new System.Drawing.Point(26, 567);
            GroupsLabel.Name = "GroupsLabel";
            GroupsLabel.Size = new System.Drawing.Size(134, 31);
            GroupsLabel.TabIndex = 12;
            GroupsLabel.Text = "监听群列表";
            // 
            // GivePermissionToAllAdminsCheckBox
            // 
            GivePermissionToAllAdminsCheckBox.AutoSize = true;
            GivePermissionToAllAdminsCheckBox.Location = new System.Drawing.Point(26, 529);
            GivePermissionToAllAdminsCheckBox.Name = "GivePermissionToAllAdminsCheckBox";
            GivePermissionToAllAdminsCheckBox.Size = new System.Drawing.Size(382, 35);
            GivePermissionToAllAdminsCheckBox.TabIndex = 11;
            GivePermissionToAllAdminsCheckBox.Text = "赋予所有群主和管理员管理权限";
            GivePermissionToAllAdminsCheckBox.UseVisualStyleBackColor = true;
            // 
            // SaveLogCheckBox
            // 
            SaveLogCheckBox.AutoSize = true;
            SaveLogCheckBox.Location = new System.Drawing.Point(26, 459);
            SaveLogCheckBox.Name = "SaveLogCheckBox";
            SaveLogCheckBox.Size = new System.Drawing.Size(238, 35);
            SaveLogCheckBox.TabIndex = 10;
            SaveLogCheckBox.Text = "保存数据包到日志";
            SaveLogCheckBox.UseVisualStyleBackColor = true;
            // 
            // AutoEscapeCheckBox
            // 
            AutoEscapeCheckBox.AutoSize = true;
            AutoEscapeCheckBox.Location = new System.Drawing.Point(26, 418);
            AutoEscapeCheckBox.Name = "AutoEscapeCheckBox";
            AutoEscapeCheckBox.Size = new System.Drawing.Size(166, 35);
            AutoEscapeCheckBox.TabIndex = 9;
            AutoEscapeCheckBox.Text = "纯文本发送";
            AutoEscapeCheckBox.UseVisualStyleBackColor = true;
            // 
            // OutputDataCheckBox
            // 
            OutputDataCheckBox.AutoSize = true;
            OutputDataCheckBox.Location = new System.Drawing.Point(26, 377);
            OutputDataCheckBox.Name = "OutputDataCheckBox";
            OutputDataCheckBox.Size = new System.Drawing.Size(214, 35);
            OutputDataCheckBox.TabIndex = 8;
            OutputDataCheckBox.Text = "输出收发的数据";
            OutputDataCheckBox.UseVisualStyleBackColor = true;
            // 
            // AutoReconnectCheckBox
            // 
            AutoReconnectCheckBox.AutoSize = true;
            AutoReconnectCheckBox.Location = new System.Drawing.Point(26, 307);
            AutoReconnectCheckBox.Name = "AutoReconnectCheckBox";
            AutoReconnectCheckBox.Size = new System.Drawing.Size(190, 35);
            AutoReconnectCheckBox.TabIndex = 7;
            AutoReconnectCheckBox.Text = "断线自动重连";
            AutoReconnectCheckBox.UseVisualStyleBackColor = true;
            // 
            // UseReverseWebSocketCheckBox
            // 
            UseReverseWebSocketCheckBox.AutoSize = true;
            UseReverseWebSocketCheckBox.Location = new System.Drawing.Point(26, 266);
            UseReverseWebSocketCheckBox.Name = "UseReverseWebSocketCheckBox";
            UseReverseWebSocketCheckBox.Size = new System.Drawing.Size(272, 35);
            UseReverseWebSocketCheckBox.TabIndex = 6;
            UseReverseWebSocketCheckBox.Text = "使用反向WebSocket";
            UseReverseWebSocketCheckBox.UseVisualStyleBackColor = true;
            // 
            // AccessTokenMaskedTextBox
            // 
            AccessTokenMaskedTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            AccessTokenMaskedTextBox.Location = new System.Drawing.Point(26, 133);
            AccessTokenMaskedTextBox.Name = "AccessTokenMaskedTextBox";
            AccessTokenMaskedTextBox.Size = new System.Drawing.Size(1134, 38);
            AccessTokenMaskedTextBox.TabIndex = 3;
            // 
            // SubProtocolsTextBox
            // 
            SubProtocolsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            SubProtocolsTextBox.Location = new System.Drawing.Point(26, 215);
            SubProtocolsTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            SubProtocolsTextBox.Name = "SubProtocolsTextBox";
            SubProtocolsTextBox.Size = new System.Drawing.Size(1134, 38);
            SubProtocolsTextBox.TabIndex = 5;
            // 
            // SubProtocolsLabel
            // 
            SubProtocolsLabel.AutoSize = true;
            SubProtocolsLabel.Location = new System.Drawing.Point(26, 181);
            SubProtocolsLabel.Name = "SubProtocolsLabel";
            SubProtocolsLabel.Size = new System.Drawing.Size(216, 31);
            SubProtocolsLabel.TabIndex = 4;
            SubProtocolsLabel.Text = "WebSocket子协议";
            // 
            // AccessTokenLabel
            // 
            AccessTokenLabel.AutoSize = true;
            AccessTokenLabel.Location = new System.Drawing.Point(26, 99);
            AccessTokenLabel.Name = "AccessTokenLabel";
            AccessTokenLabel.Size = new System.Drawing.Size(229, 31);
            AccessTokenLabel.TabIndex = 2;
            AccessTokenLabel.Text = "鉴权凭证（Token）";
            // 
            // UriTextBox
            // 
            UriTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            UriTextBox.Location = new System.Drawing.Point(26, 51);
            UriTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            UriTextBox.Name = "UriTextBox";
            UriTextBox.Size = new System.Drawing.Size(1134, 38);
            UriTextBox.TabIndex = 1;
            // 
            // UriLabel
            // 
            UriLabel.AutoSize = true;
            UriLabel.Location = new System.Drawing.Point(26, 17);
            UriLabel.Name = "UriLabel";
            UriLabel.Size = new System.Drawing.Size(62, 31);
            UriLabel.TabIndex = 0;
            UriLabel.Text = "地址";
            // 
            // ReactionTabPage
            // 
            ReactionTabPage.Location = new System.Drawing.Point(8, 45);
            ReactionTabPage.Name = "ReactionTabPage";
            ReactionTabPage.Padding = new System.Windows.Forms.Padding(3);
            ReactionTabPage.Size = new System.Drawing.Size(1264, 667);
            ReactionTabPage.TabIndex = 1;
            ReactionTabPage.Text = "反应";
            ReactionTabPage.UseVisualStyleBackColor = true;
            // 
            // ApplicationTabPage
            // 
            ApplicationTabPage.Location = new System.Drawing.Point(8, 45);
            ApplicationTabPage.Name = "ApplicationTabPage";
            ApplicationTabPage.Padding = new System.Windows.Forms.Padding(3);
            ApplicationTabPage.Size = new System.Drawing.Size(1264, 667);
            ApplicationTabPage.TabIndex = 2;
            ApplicationTabPage.Text = "应用";
            ApplicationTabPage.UseVisualStyleBackColor = true;
            // 
            // AboutTabPage
            // 
            AboutTabPage.Location = new System.Drawing.Point(8, 45);
            AboutTabPage.Name = "AboutTabPage";
            AboutTabPage.Size = new System.Drawing.Size(1264, 667);
            AboutTabPage.TabIndex = 3;
            AboutTabPage.Text = "关于";
            AboutTabPage.UseVisualStyleBackColor = true;
            // 
            // SettingPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(TabControl);
            Name = "SettingPage";
            Size = new System.Drawing.Size(1280, 720);
            TabControl.ResumeLayout(false);
            ConnectionTabPage.ResumeLayout(false);
            ConnectionPanel.ResumeLayout(false);
            ConnectionPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabPage AboutTabPage;
        private System.Windows.Forms.MaskedTextBox AccessTokenMaskedTextBox;
        private System.Windows.Forms.TextBox SubProtocolsTextBox;
        private System.Windows.Forms.TextBox UriTextBox;
        private System.Windows.Forms.CheckBox SaveLogCheckBox;
        private System.Windows.Forms.CheckBox AutoEscapeCheckBox;
        private System.Windows.Forms.CheckBox OutputDataCheckBox;
        private System.Windows.Forms.CheckBox AutoReconnectCheckBox;
        private System.Windows.Forms.CheckBox UseReverseWebSocketCheckBox;
        private System.Windows.Forms.CheckBox GivePermissionToAllAdminsCheckBox;
        private System.Windows.Forms.TextBox AdministratorsTextBox;
        private System.Windows.Forms.TextBox GroupsLabelTextBox;
    }
}
