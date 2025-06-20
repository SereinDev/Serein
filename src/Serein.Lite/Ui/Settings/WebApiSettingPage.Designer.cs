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
            System.Windows.Forms.Label urlPrefixesLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebApiSettingPage));
            System.Windows.Forms.Label maxRequestsPerSecondLabel;
            System.Windows.Forms.Label whiteListLabel;
            System.Windows.Forms.Label accessTokensLabel;
            System.Windows.Forms.GroupBox certificateGroupBox;
            System.Windows.Forms.Button _openFileButton;
            System.Windows.Forms.Label passwordLabel;
            System.Windows.Forms.Label pathLabel;
            System.Windows.Forms.ToolTip toolTip;
            _passwordMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            _pathTextBox = new System.Windows.Forms.TextBox();
            _autoLoadCertificateCheckBox = new System.Windows.Forms.CheckBox();
            _autoRegisterCertificateCheckBox = new System.Windows.Forms.CheckBox();
            _certificateEnableCheckBox = new System.Windows.Forms.CheckBox();
            _urlPrefixesTextBox = new System.Windows.Forms.TextBox();
            _allowCrossOriginCheckBox = new System.Windows.Forms.CheckBox();
            _maxRequestsPerSecondNumericUpDown = new System.Windows.Forms.NumericUpDown();
            _whiteListTextBox = new System.Windows.Forms.TextBox();
            _accessTokensTextBox = new System.Windows.Forms.TextBox();
            _isEnableCheckBox = new System.Windows.Forms.CheckBox();
            urlPrefixesLabel = new System.Windows.Forms.Label();
            maxRequestsPerSecondLabel = new System.Windows.Forms.Label();
            whiteListLabel = new System.Windows.Forms.Label();
            accessTokensLabel = new System.Windows.Forms.Label();
            certificateGroupBox = new System.Windows.Forms.GroupBox();
            _openFileButton = new System.Windows.Forms.Button();
            passwordLabel = new System.Windows.Forms.Label();
            pathLabel = new System.Windows.Forms.Label();
            toolTip = new System.Windows.Forms.ToolTip(components);
            certificateGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_maxRequestsPerSecondNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // urlPrefixesLabel
            // 
            urlPrefixesLabel.AutoSize = true;
            urlPrefixesLabel.Location = new System.Drawing.Point(27, 79);
            urlPrefixesLabel.Name = "urlPrefixesLabel";
            urlPrefixesLabel.Size = new System.Drawing.Size(95, 31);
            urlPrefixesLabel.TabIndex = 1;
            urlPrefixesLabel.Text = "Url前缀";
            toolTip.SetToolTip(urlPrefixesLabel, resources.GetString("urlPrefixesLabel.ToolTip"));
            // 
            // maxRequestsPerSecondLabel
            // 
            maxRequestsPerSecondLabel.AutoSize = true;
            maxRequestsPerSecondLabel.Location = new System.Drawing.Point(27, 303);
            maxRequestsPerSecondLabel.Name = "maxRequestsPerSecondLabel";
            maxRequestsPerSecondLabel.Size = new System.Drawing.Size(182, 31);
            maxRequestsPerSecondLabel.TabIndex = 4;
            maxRequestsPerSecondLabel.Text = "每秒最大请求数";
            toolTip.SetToolTip(maxRequestsPerSecondLabel, "每秒请求数超过此值的IP（下方白名单内的除外）将被封禁");
            // 
            // whiteListLabel
            // 
            whiteListLabel.AutoSize = true;
            whiteListLabel.Location = new System.Drawing.Point(27, 385);
            whiteListLabel.Name = "whiteListLabel";
            whiteListLabel.Size = new System.Drawing.Size(156, 31);
            whiteListLabel.TabIndex = 6;
            whiteListLabel.Text = "IP请求白名单";
            toolTip.SetToolTip(whiteListLabel, "无视请求速度限制的IP列表（一行一个）");
            // 
            // accessTokensLabel
            // 
            accessTokensLabel.AutoSize = true;
            accessTokensLabel.Location = new System.Drawing.Point(27, 561);
            accessTokensLabel.Name = "accessTokensLabel";
            accessTokensLabel.Size = new System.Drawing.Size(110, 31);
            accessTokensLabel.TabIndex = 8;
            accessTokensLabel.Text = "访问凭证";
            toolTip.SetToolTip(accessTokensLabel, "若值不为空，请求时“/api”下的任意资源均需要在请求头中添加Authentication项，即“Authentication: Bearer [Token]”");
            // 
            // certificateGroupBox
            // 
            certificateGroupBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            certificateGroupBox.Controls.Add(_openFileButton);
            certificateGroupBox.Controls.Add(_passwordMaskedTextBox);
            certificateGroupBox.Controls.Add(passwordLabel);
            certificateGroupBox.Controls.Add(_pathTextBox);
            certificateGroupBox.Controls.Add(pathLabel);
            certificateGroupBox.Controls.Add(_autoLoadCertificateCheckBox);
            certificateGroupBox.Controls.Add(_autoRegisterCertificateCheckBox);
            certificateGroupBox.Controls.Add(_certificateEnableCheckBox);
            certificateGroupBox.Location = new System.Drawing.Point(28, 740);
            certificateGroupBox.Name = "certificateGroupBox";
            certificateGroupBox.Size = new System.Drawing.Size(1210, 342);
            certificateGroupBox.TabIndex = 10;
            certificateGroupBox.TabStop = false;
            certificateGroupBox.Text = "证书";
            // 
            // _openFileButton
            // 
            _openFileButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _openFileButton.Location = new System.Drawing.Point(1053, 197);
            _openFileButton.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            _openFileButton.Name = "_openFileButton";
            _openFileButton.Size = new System.Drawing.Size(131, 40);
            _openFileButton.TabIndex = 17;
            _openFileButton.Text = "打开...";
            _openFileButton.UseVisualStyleBackColor = true;
            _openFileButton.Click += OpenFileButton_Click;
            // 
            // _passwordMaskedTextBox
            // 
            _passwordMaskedTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _passwordMaskedTextBox.Location = new System.Drawing.Point(31, 280);
            _passwordMaskedTextBox.Name = "_passwordMaskedTextBox";
            _passwordMaskedTextBox.Size = new System.Drawing.Size(1153, 38);
            _passwordMaskedTextBox.TabIndex = 16;
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new System.Drawing.Point(31, 246);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new System.Drawing.Size(62, 31);
            passwordLabel.TabIndex = 15;
            passwordLabel.Text = "密码";
            // 
            // _pathTextBox
            // 
            _pathTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _pathTextBox.Location = new System.Drawing.Point(31, 198);
            _pathTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _pathTextBox.Name = "_pathTextBox";
            _pathTextBox.Size = new System.Drawing.Size(1003, 38);
            _pathTextBox.TabIndex = 14;
            toolTip.SetToolTip(_pathTextBox, "【提示】你可以双击文本框打开选择文件对话框");
            // 
            // pathLabel
            // 
            pathLabel.AutoSize = true;
            pathLabel.Location = new System.Drawing.Point(31, 164);
            pathLabel.Name = "pathLabel";
            pathLabel.Size = new System.Drawing.Size(110, 31);
            pathLabel.TabIndex = 13;
            pathLabel.Text = "证书路径";
            toolTip.SetToolTip(pathLabel, "【提示】你可以双击文本框打开选择文件对话框");
            // 
            // _autoLoadCertificateCheckBox
            // 
            _autoLoadCertificateCheckBox.AutoSize = true;
            _autoLoadCertificateCheckBox.Location = new System.Drawing.Point(31, 119);
            _autoLoadCertificateCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _autoLoadCertificateCheckBox.Name = "_autoLoadCertificateCheckBox";
            _autoLoadCertificateCheckBox.Size = new System.Drawing.Size(190, 35);
            _autoLoadCertificateCheckBox.TabIndex = 12;
            _autoLoadCertificateCheckBox.Text = "自动加载证书";
            toolTip.SetToolTip(_autoLoadCertificateCheckBox, "自动从默认的证书存储区中读取证书");
            _autoLoadCertificateCheckBox.UseVisualStyleBackColor = true;
            _autoLoadCertificateCheckBox.Click += OnPropertyChanged;
            // 
            // _autoRegisterCertificateCheckBox
            // 
            _autoRegisterCertificateCheckBox.AutoSize = true;
            _autoRegisterCertificateCheckBox.Location = new System.Drawing.Point(31, 78);
            _autoRegisterCertificateCheckBox.Name = "_autoRegisterCertificateCheckBox";
            _autoRegisterCertificateCheckBox.Size = new System.Drawing.Size(190, 35);
            _autoRegisterCertificateCheckBox.TabIndex = 11;
            _autoRegisterCertificateCheckBox.Text = "自动注册证书";
            toolTip.SetToolTip(_autoRegisterCertificateCheckBox, "自动将使用的证书注册到默认的证书存储区");
            _autoRegisterCertificateCheckBox.UseVisualStyleBackColor = true;
            _autoRegisterCertificateCheckBox.Click += OnPropertyChanged;
            // 
            // _certificateEnableCheckBox
            // 
            _certificateEnableCheckBox.AutoSize = true;
            _certificateEnableCheckBox.Location = new System.Drawing.Point(31, 37);
            _certificateEnableCheckBox.Name = "_certificateEnableCheckBox";
            _certificateEnableCheckBox.Size = new System.Drawing.Size(94, 35);
            _certificateEnableCheckBox.TabIndex = 11;
            _certificateEnableCheckBox.Text = "启用";
            _certificateEnableCheckBox.UseVisualStyleBackColor = true;
            _certificateEnableCheckBox.Click += OnPropertyChanged;
            // 
            // _urlPrefixesTextBox
            // 
            _urlPrefixesTextBox.AcceptsReturn = true;
            _urlPrefixesTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _urlPrefixesTextBox.Location = new System.Drawing.Point(27, 113);
            _urlPrefixesTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _urlPrefixesTextBox.Multiline = true;
            _urlPrefixesTextBox.Name = "_urlPrefixesTextBox";
            _urlPrefixesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _urlPrefixesTextBox.Size = new System.Drawing.Size(1211, 132);
            _urlPrefixesTextBox.TabIndex = 2;
            toolTip.SetToolTip(_urlPrefixesTextBox, resources.GetString("_urlPrefixesTextBox.ToolTip"));
            _urlPrefixesTextBox.TextChanged += UrlPrefixesTextBox_TextChanged;
            // 
            // _allowCrossOriginCheckBox
            // 
            _allowCrossOriginCheckBox.AutoSize = true;
            _allowCrossOriginCheckBox.Location = new System.Drawing.Point(27, 258);
            _allowCrossOriginCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _allowCrossOriginCheckBox.Name = "_allowCrossOriginCheckBox";
            _allowCrossOriginCheckBox.Size = new System.Drawing.Size(190, 35);
            _allowCrossOriginCheckBox.TabIndex = 3;
            _allowCrossOriginCheckBox.Text = "允许跨源请求";
            toolTip.SetToolTip(_allowCrossOriginCheckBox, "开启后会在响应头添加“Access-Control-Allow-Origin: *”");
            _allowCrossOriginCheckBox.UseVisualStyleBackColor = true;
            _allowCrossOriginCheckBox.Click += OnPropertyChanged;
            // 
            // _maxRequestsPerSecondNumericUpDown
            // 
            _maxRequestsPerSecondNumericUpDown.Location = new System.Drawing.Point(27, 337);
            _maxRequestsPerSecondNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _maxRequestsPerSecondNumericUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            _maxRequestsPerSecondNumericUpDown.Name = "_maxRequestsPerSecondNumericUpDown";
            _maxRequestsPerSecondNumericUpDown.Size = new System.Drawing.Size(223, 38);
            _maxRequestsPerSecondNumericUpDown.TabIndex = 5;
            toolTip.SetToolTip(_maxRequestsPerSecondNumericUpDown, "每秒请求数超过此值的IP（下方白名单内的除外）将被封禁");
            // 
            // _whiteListTextBox
            // 
            _whiteListTextBox.AcceptsReturn = true;
            _whiteListTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _whiteListTextBox.Location = new System.Drawing.Point(27, 419);
            _whiteListTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _whiteListTextBox.Multiline = true;
            _whiteListTextBox.Name = "_whiteListTextBox";
            _whiteListTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _whiteListTextBox.Size = new System.Drawing.Size(1211, 132);
            _whiteListTextBox.TabIndex = 7;
            toolTip.SetToolTip(_whiteListTextBox, "无视请求速度限制的IP列表（一行一个）");
            _whiteListTextBox.TextChanged += WhiteListTextBox_TextChanged;
            // 
            // _accessTokensTextBox
            // 
            _accessTokensTextBox.AcceptsReturn = true;
            _accessTokensTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _accessTokensTextBox.Location = new System.Drawing.Point(27, 595);
            _accessTokensTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _accessTokensTextBox.Multiline = true;
            _accessTokensTextBox.Name = "_accessTokensTextBox";
            _accessTokensTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _accessTokensTextBox.Size = new System.Drawing.Size(1211, 132);
            _accessTokensTextBox.TabIndex = 9;
            toolTip.SetToolTip(_accessTokensTextBox, "若值不为空，请求时“/api”下的任意资源均需要在请求头中添加Authentication项，即“Authentication: Bearer [Token]”");
            _accessTokensTextBox.TextChanged += AccessTokensTextBox_TextChanged;
            // 
            // _isEnableCheckBox
            // 
            _isEnableCheckBox.AutoSize = true;
            _isEnableCheckBox.Location = new System.Drawing.Point(27, 25);
            _isEnableCheckBox.Name = "_isEnableCheckBox";
            _isEnableCheckBox.Size = new System.Drawing.Size(94, 35);
            _isEnableCheckBox.TabIndex = 0;
            _isEnableCheckBox.Text = "启用";
            _isEnableCheckBox.UseVisualStyleBackColor = true;
            _isEnableCheckBox.Click += EnableCheckBox_Click;
            // 
            // WebApiSettingPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = System.Drawing.Color.White;
            Controls.Add(certificateGroupBox);
            Controls.Add(_accessTokensTextBox);
            Controls.Add(accessTokensLabel);
            Controls.Add(_whiteListTextBox);
            Controls.Add(whiteListLabel);
            Controls.Add(_maxRequestsPerSecondNumericUpDown);
            Controls.Add(maxRequestsPerSecondLabel);
            Controls.Add(_allowCrossOriginCheckBox);
            Controls.Add(_urlPrefixesTextBox);
            Controls.Add(urlPrefixesLabel);
            Controls.Add(_isEnableCheckBox);
            Margin = new System.Windows.Forms.Padding(0);
            Name = "WebApiSettingPage";
            Size = new System.Drawing.Size(1280, 1117);
            certificateGroupBox.ResumeLayout(false);
            certificateGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_maxRequestsPerSecondNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox _isEnableCheckBox;
        private System.Windows.Forms.TextBox _urlPrefixesTextBox;
        private System.Windows.Forms.CheckBox _allowCrossOriginCheckBox;
        private System.Windows.Forms.NumericUpDown _maxRequestsPerSecondNumericUpDown;
        private System.Windows.Forms.TextBox _whiteListTextBox;
        private System.Windows.Forms.TextBox _accessTokensTextBox;
        private System.Windows.Forms.CheckBox _autoLoadCertificateCheckBox;
        private System.Windows.Forms.CheckBox _autoRegisterCertificateCheckBox;
        private System.Windows.Forms.CheckBox _certificateEnableCheckBox;
        private System.Windows.Forms.MaskedTextBox _passwordMaskedTextBox;
        private System.Windows.Forms.TextBox _pathTextBox;
    }
}
