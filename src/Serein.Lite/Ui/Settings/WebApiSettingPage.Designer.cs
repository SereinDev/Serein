namespace Serein.Lite.Ui.Settings
{
    partial class WebApiSettingPage
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
            System.Windows.Forms.Label UrlPrefixesLabel;
            System.Windows.Forms.Label MaxRequestsPerSecondLabel;
            System.Windows.Forms.Label WhiteListLabel;
            System.Windows.Forms.Label AccessTokensLabel;
            System.Windows.Forms.GroupBox CertificateGroupBox;
            System.Windows.Forms.Label PasswordLabel;
            System.Windows.Forms.Label PathLabel;
            System.Windows.Forms.ToolTip ToolTip;
            PasswordMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            PathTextBox = new System.Windows.Forms.TextBox();
            AutoLoadCertificateCheckBox = new System.Windows.Forms.CheckBox();
            AutoRegisterCertificateCheckBox = new System.Windows.Forms.CheckBox();
            CertificateEnableCheckBox = new System.Windows.Forms.CheckBox();
            UrlPrefixesTextBox = new System.Windows.Forms.TextBox();
            AllowCrossOriginCheckBox = new System.Windows.Forms.CheckBox();
            MaxRequestsPerSecondNumericUpDown = new System.Windows.Forms.NumericUpDown();
            WhiteListTextBox = new System.Windows.Forms.TextBox();
            AccessTokensTextBox = new System.Windows.Forms.TextBox();
            EnableCheckBox = new System.Windows.Forms.CheckBox();
            UrlPrefixesLabel = new System.Windows.Forms.Label();
            MaxRequestsPerSecondLabel = new System.Windows.Forms.Label();
            WhiteListLabel = new System.Windows.Forms.Label();
            AccessTokensLabel = new System.Windows.Forms.Label();
            CertificateGroupBox = new System.Windows.Forms.GroupBox();
            PasswordLabel = new System.Windows.Forms.Label();
            PathLabel = new System.Windows.Forms.Label();
            ToolTip = new System.Windows.Forms.ToolTip(components);
            CertificateGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MaxRequestsPerSecondNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // UrlPrefixesLabel
            // 
            UrlPrefixesLabel.AutoSize = true;
            UrlPrefixesLabel.Location = new System.Drawing.Point(27, 79);
            UrlPrefixesLabel.Name = "UrlPrefixesLabel";
            UrlPrefixesLabel.Size = new System.Drawing.Size(95, 31);
            UrlPrefixesLabel.TabIndex = 1;
            UrlPrefixesLabel.Text = "Url前缀";
            ToolTip.SetToolTip(UrlPrefixesLabel, "Http服务器监听的Url（一行一个）\r\n示例：\r\n· http://127.0.0.1:{端口} 只能由本机访问\r\n· http://:*:{端口} 允许外网访问，但需要手动以管理员权限运行Serein（系操作系统限制）\r\n· https://{域名}/ 需要在下方配置证书");
            // 
            // MaxRequestsPerSecondLabel
            // 
            MaxRequestsPerSecondLabel.AutoSize = true;
            MaxRequestsPerSecondLabel.Location = new System.Drawing.Point(27, 303);
            MaxRequestsPerSecondLabel.Name = "MaxRequestsPerSecondLabel";
            MaxRequestsPerSecondLabel.Size = new System.Drawing.Size(182, 31);
            MaxRequestsPerSecondLabel.TabIndex = 4;
            MaxRequestsPerSecondLabel.Text = "每秒最大请求数";
            ToolTip.SetToolTip(MaxRequestsPerSecondLabel, "每秒请求数超过此值的IP（下方白名单内的除外）将被封禁");
            // 
            // WhiteListLabel
            // 
            WhiteListLabel.AutoSize = true;
            WhiteListLabel.Location = new System.Drawing.Point(27, 385);
            WhiteListLabel.Name = "WhiteListLabel";
            WhiteListLabel.Size = new System.Drawing.Size(156, 31);
            WhiteListLabel.TabIndex = 6;
            WhiteListLabel.Text = "IP请求白名单";
            ToolTip.SetToolTip(WhiteListLabel, "不会被封禁的IP列表（一行一个）");
            // 
            // AccessTokensLabel
            // 
            AccessTokensLabel.AutoSize = true;
            AccessTokensLabel.Location = new System.Drawing.Point(27, 561);
            AccessTokensLabel.Name = "AccessTokensLabel";
            AccessTokensLabel.Size = new System.Drawing.Size(110, 31);
            AccessTokensLabel.TabIndex = 8;
            AccessTokensLabel.Text = "访问凭证";
            ToolTip.SetToolTip(AccessTokensLabel, "Access Token（一行一个）\r\n· 若为空，则访问无需Token（不安全）\r\n· 若不为空，则访问时需要在Header中添加Authentication项");
            // 
            // CertificateGroupBox
            // 
            CertificateGroupBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CertificateGroupBox.Controls.Add(PasswordMaskedTextBox);
            CertificateGroupBox.Controls.Add(PasswordLabel);
            CertificateGroupBox.Controls.Add(PathTextBox);
            CertificateGroupBox.Controls.Add(PathLabel);
            CertificateGroupBox.Controls.Add(AutoLoadCertificateCheckBox);
            CertificateGroupBox.Controls.Add(AutoRegisterCertificateCheckBox);
            CertificateGroupBox.Controls.Add(CertificateEnableCheckBox);
            CertificateGroupBox.Location = new System.Drawing.Point(28, 740);
            CertificateGroupBox.Name = "CertificateGroupBox";
            CertificateGroupBox.Size = new System.Drawing.Size(1210, 342);
            CertificateGroupBox.TabIndex = 10;
            CertificateGroupBox.TabStop = false;
            CertificateGroupBox.Text = "证书";
            // 
            // PasswordMaskedTextBox
            // 
            PasswordMaskedTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PasswordMaskedTextBox.Location = new System.Drawing.Point(31, 280);
            PasswordMaskedTextBox.Name = "PasswordMaskedTextBox";
            PasswordMaskedTextBox.Size = new System.Drawing.Size(1153, 38);
            PasswordMaskedTextBox.TabIndex = 16;
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Location = new System.Drawing.Point(31, 246);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new System.Drawing.Size(62, 31);
            PasswordLabel.TabIndex = 15;
            PasswordLabel.Text = "密码";
            // 
            // PathTextBox
            // 
            PathTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PathTextBox.Location = new System.Drawing.Point(31, 198);
            PathTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            PathTextBox.Name = "PathTextBox";
            PathTextBox.Size = new System.Drawing.Size(1153, 38);
            PathTextBox.TabIndex = 14;
            ToolTip.SetToolTip(PathTextBox, "【提示】你可以双击文本框打开选择文件对话框");
            PathTextBox.DoubleClick += PathTextBox_DoubleClick;
            // 
            // PathLabel
            // 
            PathLabel.AutoSize = true;
            PathLabel.Location = new System.Drawing.Point(31, 164);
            PathLabel.Name = "PathLabel";
            PathLabel.Size = new System.Drawing.Size(110, 31);
            PathLabel.TabIndex = 13;
            PathLabel.Text = "证书路径";
            ToolTip.SetToolTip(PathLabel, "【提示】你可以双击文本框打开选择文件对话框");
            // 
            // AutoLoadCertificateCheckBox
            // 
            AutoLoadCertificateCheckBox.AutoSize = true;
            AutoLoadCertificateCheckBox.Location = new System.Drawing.Point(31, 119);
            AutoLoadCertificateCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            AutoLoadCertificateCheckBox.Name = "AutoLoadCertificateCheckBox";
            AutoLoadCertificateCheckBox.Size = new System.Drawing.Size(190, 35);
            AutoLoadCertificateCheckBox.TabIndex = 12;
            AutoLoadCertificateCheckBox.Text = "自动加载证书";
            AutoLoadCertificateCheckBox.UseVisualStyleBackColor = true;
            AutoLoadCertificateCheckBox.Click += OnPropertyChanged;
            // 
            // AutoRegisterCertificateCheckBox
            // 
            AutoRegisterCertificateCheckBox.AutoSize = true;
            AutoRegisterCertificateCheckBox.Location = new System.Drawing.Point(31, 78);
            AutoRegisterCertificateCheckBox.Name = "AutoRegisterCertificateCheckBox";
            AutoRegisterCertificateCheckBox.Size = new System.Drawing.Size(190, 35);
            AutoRegisterCertificateCheckBox.TabIndex = 11;
            AutoRegisterCertificateCheckBox.Text = "自动注册证书";
            AutoRegisterCertificateCheckBox.UseVisualStyleBackColor = true;
            AutoRegisterCertificateCheckBox.Click += OnPropertyChanged;
            // 
            // CertificateEnableCheckBox
            // 
            CertificateEnableCheckBox.AutoSize = true;
            CertificateEnableCheckBox.Location = new System.Drawing.Point(31, 37);
            CertificateEnableCheckBox.Name = "CertificateEnableCheckBox";
            CertificateEnableCheckBox.Size = new System.Drawing.Size(94, 35);
            CertificateEnableCheckBox.TabIndex = 11;
            CertificateEnableCheckBox.Text = "启用";
            CertificateEnableCheckBox.UseVisualStyleBackColor = true;
            CertificateEnableCheckBox.Click += OnPropertyChanged;
            // 
            // UrlPrefixesTextBox
            // 
            UrlPrefixesTextBox.AcceptsReturn = true;
            UrlPrefixesTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            UrlPrefixesTextBox.Location = new System.Drawing.Point(27, 113);
            UrlPrefixesTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            UrlPrefixesTextBox.Multiline = true;
            UrlPrefixesTextBox.Name = "UrlPrefixesTextBox";
            UrlPrefixesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            UrlPrefixesTextBox.Size = new System.Drawing.Size(1211, 132);
            UrlPrefixesTextBox.TabIndex = 2;
            ToolTip.SetToolTip(UrlPrefixesTextBox, "Http服务器监听的Url\r\n示例：\r\n· http://127.0.0.1:{端口} 只能由本机访问\r\n· http://*:{端口} 允许外网访问，但需要手动以管理员权限运行Serein（系操作系统限制）\r\n· https://{域名}/ 需要在下方配置证书");
            UrlPrefixesTextBox.TextChanged += UrlPrefixesTextBox_TextChanged;
            // 
            // AllowCrossOriginCheckBox
            // 
            AllowCrossOriginCheckBox.AutoSize = true;
            AllowCrossOriginCheckBox.Location = new System.Drawing.Point(27, 258);
            AllowCrossOriginCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            AllowCrossOriginCheckBox.Name = "AllowCrossOriginCheckBox";
            AllowCrossOriginCheckBox.Size = new System.Drawing.Size(190, 35);
            AllowCrossOriginCheckBox.TabIndex = 3;
            AllowCrossOriginCheckBox.Text = "允许跨源请求";
            ToolTip.SetToolTip(AllowCrossOriginCheckBox, "开启后会在响应头添加 Access-Control-Allow-Origin: *");
            AllowCrossOriginCheckBox.UseVisualStyleBackColor = true;
            AllowCrossOriginCheckBox.Click += OnPropertyChanged;
            // 
            // MaxRequestsPerSecondNumericUpDown
            // 
            MaxRequestsPerSecondNumericUpDown.Location = new System.Drawing.Point(27, 337);
            MaxRequestsPerSecondNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            MaxRequestsPerSecondNumericUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            MaxRequestsPerSecondNumericUpDown.Name = "MaxRequestsPerSecondNumericUpDown";
            MaxRequestsPerSecondNumericUpDown.Size = new System.Drawing.Size(223, 38);
            MaxRequestsPerSecondNumericUpDown.TabIndex = 5;
            ToolTip.SetToolTip(MaxRequestsPerSecondNumericUpDown, "每秒请求数超过此值的IP（下方白名单内的除外）将被封禁");
            // 
            // WhiteListTextBox
            // 
            WhiteListTextBox.AcceptsReturn = true;
            WhiteListTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            WhiteListTextBox.Location = new System.Drawing.Point(27, 419);
            WhiteListTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            WhiteListTextBox.Multiline = true;
            WhiteListTextBox.Name = "WhiteListTextBox";
            WhiteListTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            WhiteListTextBox.Size = new System.Drawing.Size(1211, 132);
            WhiteListTextBox.TabIndex = 7;
            ToolTip.SetToolTip(WhiteListTextBox, "不会被封禁的IP列表（一行一个）");
            WhiteListTextBox.TextChanged += WhiteListTextBox_TextChanged;
            // 
            // AccessTokensTextBox
            // 
            AccessTokensTextBox.AcceptsReturn = true;
            AccessTokensTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            AccessTokensTextBox.Location = new System.Drawing.Point(27, 595);
            AccessTokensTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            AccessTokensTextBox.Multiline = true;
            AccessTokensTextBox.Name = "AccessTokensTextBox";
            AccessTokensTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            AccessTokensTextBox.Size = new System.Drawing.Size(1211, 132);
            AccessTokensTextBox.TabIndex = 9;
            ToolTip.SetToolTip(AccessTokensTextBox, "Access Token（一行一个）\r\n· 若为空，则访问无需Token（不安全）\r\n· 若不为空，则访问时需要在Header中添加Authentication项");
            AccessTokensTextBox.TextChanged += AccessTokensTextBox_TextChanged;
            // 
            // EnableCheckBox
            // 
            EnableCheckBox.AutoSize = true;
            EnableCheckBox.Location = new System.Drawing.Point(27, 25);
            EnableCheckBox.Name = "EnableCheckBox";
            EnableCheckBox.Size = new System.Drawing.Size(94, 35);
            EnableCheckBox.TabIndex = 0;
            EnableCheckBox.Text = "启用";
            EnableCheckBox.UseVisualStyleBackColor = true;
            EnableCheckBox.Click += EnableCheckBox_Click;
            // 
            // WebApiSettingPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = System.Drawing.Color.White;
            Controls.Add(CertificateGroupBox);
            Controls.Add(AccessTokensTextBox);
            Controls.Add(AccessTokensLabel);
            Controls.Add(WhiteListTextBox);
            Controls.Add(WhiteListLabel);
            Controls.Add(MaxRequestsPerSecondNumericUpDown);
            Controls.Add(MaxRequestsPerSecondLabel);
            Controls.Add(AllowCrossOriginCheckBox);
            Controls.Add(UrlPrefixesTextBox);
            Controls.Add(UrlPrefixesLabel);
            Controls.Add(EnableCheckBox);
            Margin = new System.Windows.Forms.Padding(0);
            Name = "WebApiSettingPage";
            Size = new System.Drawing.Size(1280, 1117);
            CertificateGroupBox.ResumeLayout(false);
            CertificateGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MaxRequestsPerSecondNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox EnableCheckBox;
        private System.Windows.Forms.TextBox UrlPrefixesTextBox;
        private System.Windows.Forms.CheckBox AllowCrossOriginCheckBox;
        private System.Windows.Forms.NumericUpDown MaxRequestsPerSecondNumericUpDown;
        private System.Windows.Forms.TextBox WhiteListTextBox;
        private System.Windows.Forms.TextBox AccessTokensTextBox;
        private System.Windows.Forms.CheckBox AutoLoadCertificateCheckBox;
        private System.Windows.Forms.CheckBox AutoRegisterCertificateCheckBox;
        private System.Windows.Forms.CheckBox CertificateEnableCheckBox;
        private System.Windows.Forms.MaskedTextBox PasswordMaskedTextBox;
        private System.Windows.Forms.TextBox PathTextBox;
    }
}
