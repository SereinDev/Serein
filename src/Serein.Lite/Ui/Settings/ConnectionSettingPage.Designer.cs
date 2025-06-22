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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolTip toolTip;
            System.Windows.Forms.Label selfPlatformLabel;
            System.Windows.Forms.Label selfUserIdLabel;
            System.Windows.Forms.Label selfLabel;
            System.Windows.Forms.Label administratorUserIdsLabel;
            System.Windows.Forms.Label listenedIdsLabel;
            System.Windows.Forms.Label adapterLabel;
            System.Windows.Forms.Label oneBotVersionLabel;
            System.Windows.Forms.Label webSocketUriLabel;
            System.Windows.Forms.Label webSocketSubProtocolsLabel;
            System.Windows.Forms.Label oneBotAccessTokenLabel;
            System.Windows.Forms.Label satoriAccessTokenLabel;
            System.Windows.Forms.Label satoriUriLabel;
            System.Windows.Forms.GroupBox commonGroupBox;
            System.Windows.Forms.GroupBox satoriGroupBox;
            System.Windows.Forms.GroupBox oneBotGroupBox;
            _selfPlatformTextBox = new System.Windows.Forms.TextBox();
            _selfUserIdTextBox = new System.Windows.Forms.TextBox();
            _administratorUserIdsTextBox = new System.Windows.Forms.TextBox();
            _listenedIdsTextBox = new System.Windows.Forms.TextBox();
            _connectWhenSettingUpcheckBox = new System.Windows.Forms.CheckBox();
            _saveLogCheckBox = new System.Windows.Forms.CheckBox();
            _outputDataCheckBox = new System.Windows.Forms.CheckBox();
            _adapterComboBox = new System.Windows.Forms.ComboBox();
            _satoriAccessTokenMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            _satoriUriTextBox = new System.Windows.Forms.TextBox();
            _autoEscapeCheckBox = new System.Windows.Forms.CheckBox();
            _grantPermissionToGroupOwnerAndAdminsCheckBox = new System.Windows.Forms.CheckBox();
            _autoReconnectCheckBox = new System.Windows.Forms.CheckBox();
            _oneBotAccessTokenMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            _webSocketSubProtocolsTextBox = new System.Windows.Forms.TextBox();
            _webSocketUriTextBox = new System.Windows.Forms.TextBox();
            _oneBotVersionComboBox = new System.Windows.Forms.ComboBox();
            toolTip = new System.Windows.Forms.ToolTip(components);
            selfPlatformLabel = new System.Windows.Forms.Label();
            selfUserIdLabel = new System.Windows.Forms.Label();
            selfLabel = new System.Windows.Forms.Label();
            administratorUserIdsLabel = new System.Windows.Forms.Label();
            listenedIdsLabel = new System.Windows.Forms.Label();
            adapterLabel = new System.Windows.Forms.Label();
            oneBotVersionLabel = new System.Windows.Forms.Label();
            webSocketUriLabel = new System.Windows.Forms.Label();
            webSocketSubProtocolsLabel = new System.Windows.Forms.Label();
            oneBotAccessTokenLabel = new System.Windows.Forms.Label();
            satoriAccessTokenLabel = new System.Windows.Forms.Label();
            satoriUriLabel = new System.Windows.Forms.Label();
            commonGroupBox = new System.Windows.Forms.GroupBox();
            satoriGroupBox = new System.Windows.Forms.GroupBox();
            oneBotGroupBox = new System.Windows.Forms.GroupBox();
            commonGroupBox.SuspendLayout();
            satoriGroupBox.SuspendLayout();
            oneBotGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // _selfPlatformTextBox
            // 
            _selfPlatformTextBox.Location = new System.Drawing.Point(158, 544);
            _selfPlatformTextBox.Name = "_selfPlatformTextBox";
            _selfPlatformTextBox.Size = new System.Drawing.Size(402, 38);
            _selfPlatformTextBox.TabIndex = 13;
            toolTip.SetToolTip(_selfPlatformTextBox, "当同时登录了多个账号时且在命令中未指定发送的账号时，采用此账号作为默认的发送者\r\n· 若选择OneBot适配器（V12），只登录了一个账号时可不设置此项\r\n· 若选择Satori适配器，无论是否登录多个账号都需要设置此项");
            _selfPlatformTextBox.TextChanged += OnPropertyChanged;
            // 
            // selfPlatformLabel
            // 
            selfPlatformLabel.AutoSize = true;
            selfPlatformLabel.Location = new System.Drawing.Point(41, 547);
            selfPlatformLabel.Name = "selfPlatformLabel";
            selfPlatformLabel.Size = new System.Drawing.Size(110, 31);
            selfPlatformLabel.TabIndex = 12;
            selfPlatformLabel.Text = "平台名称";
            toolTip.SetToolTip(selfPlatformLabel, "当同时登录了多个账号时且在命令中未指定发送的账号时，采用此账号作为默认的发送者\r\n· 若选择OneBot适配器（V12），只登录了一个账号时可不设置此项\r\n· 若选择Satori适配器，无论是否登录多个账号都需要设置此项");
            // 
            // _selfUserIdTextBox
            // 
            _selfUserIdTextBox.Location = new System.Drawing.Point(158, 493);
            _selfUserIdTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _selfUserIdTextBox.Name = "_selfUserIdTextBox";
            _selfUserIdTextBox.Size = new System.Drawing.Size(402, 38);
            _selfUserIdTextBox.TabIndex = 11;
            toolTip.SetToolTip(_selfUserIdTextBox, "当同时登录了多个账号时且在命令中未指定发送的账号时，采用此账号作为默认的发送者\r\n· 若选择OneBot适配器（V12），只登录了一个账号时可不设置此项\r\n· 若选择Satori适配器，无论是否登录多个账号都需要设置此项");
            _selfUserIdTextBox.TextChanged += OnPropertyChanged;
            // 
            // selfUserIdLabel
            // 
            selfUserIdLabel.AutoSize = true;
            selfUserIdLabel.Location = new System.Drawing.Point(41, 496);
            selfUserIdLabel.Name = "selfUserIdLabel";
            selfUserIdLabel.Size = new System.Drawing.Size(84, 31);
            selfUserIdLabel.TabIndex = 10;
            selfUserIdLabel.Text = "用户Id";
            toolTip.SetToolTip(selfUserIdLabel, "当同时登录了多个账号时且在命令中未指定发送的账号时，采用此账号作为默认的发送者\r\n· 若选择OneBot适配器（V12），只登录了一个账号时可不设置此项\r\n· 若选择Satori适配器，无论是否登录多个账号都需要设置此项");
            // 
            // selfLabel
            // 
            selfLabel.AutoSize = true;
            selfLabel.Location = new System.Drawing.Point(27, 449);
            selfLabel.Name = "selfLabel";
            selfLabel.Size = new System.Drawing.Size(134, 31);
            selfLabel.TabIndex = 9;
            selfLabel.Text = "默认发送者";
            toolTip.SetToolTip(selfLabel, "当同时登录了多个账号时且在命令中未指定发送的账号时，采用此账号作为默认的发送者\r\n· 若选择OneBot适配器（V12），只登录了一个账号时可不设置此项\r\n· 若选择Satori适配器，无论是否登录多个账号都需要设置此项");
            // 
            // _administratorUserIdsTextBox
            // 
            _administratorUserIdsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _administratorUserIdsTextBox.Location = new System.Drawing.Point(27, 401);
            _administratorUserIdsTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _administratorUserIdsTextBox.Name = "_administratorUserIdsTextBox";
            _administratorUserIdsTextBox.Size = new System.Drawing.Size(1211, 38);
            _administratorUserIdsTextBox.TabIndex = 8;
            toolTip.SetToolTip(_administratorUserIdsTextBox, "有管理权限的用户\r\n· 使用分号\";\"分隔每一个值\r\n");
            _administratorUserIdsTextBox.TextChanged += AdministratorUserIdsTextBox_TextChanged;
            // 
            // administratorUserIdsLabel
            // 
            administratorUserIdsLabel.AutoSize = true;
            administratorUserIdsLabel.Location = new System.Drawing.Point(27, 367);
            administratorUserIdsLabel.Name = "administratorUserIdsLabel";
            administratorUserIdsLabel.Size = new System.Drawing.Size(158, 31);
            administratorUserIdsLabel.TabIndex = 7;
            administratorUserIdsLabel.Text = "管理权限列表";
            toolTip.SetToolTip(administratorUserIdsLabel, "有管理权限的用户\r\n· 使用分号\";\"分隔每一个值");
            // 
            // _listenedIdsTextBox
            // 
            _listenedIdsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _listenedIdsTextBox.Location = new System.Drawing.Point(27, 319);
            _listenedIdsTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _listenedIdsTextBox.Name = "_listenedIdsTextBox";
            _listenedIdsTextBox.Size = new System.Drawing.Size(1211, 38);
            _listenedIdsTextBox.TabIndex = 6;
            toolTip.SetToolTip(_listenedIdsTextBox, "要监听消息的群聊、频道或群组\r\n· 直接填写Id 或 [来源]:[Id] 的格式字符串\r\n    · 其中来源可为g/group（群聊）、c/channel（频道）、guild（群组）\r\n    · 例：“g:12345”或“group:12345”\r\n· 使用分号\";\"分隔每一个值");
            _listenedIdsTextBox.TextChanged += ListenedIdsTextBox_TextChanged;
            // 
            // listenedIdsLabel
            // 
            listenedIdsLabel.AutoSize = true;
            listenedIdsLabel.Location = new System.Drawing.Point(27, 285);
            listenedIdsLabel.Name = "listenedIdsLabel";
            listenedIdsLabel.Size = new System.Drawing.Size(110, 31);
            listenedIdsLabel.TabIndex = 5;
            listenedIdsLabel.Text = "监听列表";
            toolTip.SetToolTip(listenedIdsLabel, "要监听消息的群聊、频道或群组\r\n· 直接填写Id 或 [来源]:[Id] 的格式字符串\r\n    · 其中来源可为g/group（群聊）、c/channel（频道）、guild（群组）\r\n    · 例：“g:12345”或“group:12345”\r\n· 使用分号\";\"分隔每一个值\r\n");
            // 
            // _connectWhenSettingUpcheckBox
            // 
            _connectWhenSettingUpcheckBox.AutoSize = true;
            _connectWhenSettingUpcheckBox.Location = new System.Drawing.Point(27, 231);
            _connectWhenSettingUpcheckBox.Name = "_connectWhenSettingUpcheckBox";
            _connectWhenSettingUpcheckBox.Size = new System.Drawing.Size(142, 35);
            _connectWhenSettingUpcheckBox.TabIndex = 4;
            _connectWhenSettingUpcheckBox.Text = "自动连接";
            toolTip.SetToolTip(_connectWhenSettingUpcheckBox, "Serein启动后自动开启连接");
            _connectWhenSettingUpcheckBox.UseVisualStyleBackColor = true;
            _connectWhenSettingUpcheckBox.Click += OnPropertyChanged;
            // 
            // _saveLogCheckBox
            // 
            _saveLogCheckBox.AutoSize = true;
            _saveLogCheckBox.Location = new System.Drawing.Point(27, 190);
            _saveLogCheckBox.Name = "_saveLogCheckBox";
            _saveLogCheckBox.Size = new System.Drawing.Size(166, 35);
            _saveLogCheckBox.TabIndex = 3;
            _saveLogCheckBox.Text = "保存到日志";
            toolTip.SetToolTip(_saveLogCheckBox, "将收到的数据包以文本格式保存到日志文件（./log/connection/）");
            _saveLogCheckBox.UseVisualStyleBackColor = true;
            _saveLogCheckBox.Click += OnPropertyChanged;
            // 
            // _outputDataCheckBox
            // 
            _outputDataCheckBox.AutoSize = true;
            _outputDataCheckBox.Location = new System.Drawing.Point(27, 149);
            _outputDataCheckBox.Name = "_outputDataCheckBox";
            _outputDataCheckBox.Size = new System.Drawing.Size(214, 35);
            _outputDataCheckBox.TabIndex = 2;
            _outputDataCheckBox.Text = "输出收发的数据";
            toolTip.SetToolTip(_outputDataCheckBox, "在连接控制台中输出接收和发送的数据，但可能导致控制台可读性下降");
            _outputDataCheckBox.UseVisualStyleBackColor = true;
            _outputDataCheckBox.Click += OnPropertyChanged;
            // 
            // _adapterComboBox
            // 
            _adapterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _adapterComboBox.FormattingEnabled = true;
            _adapterComboBox.Items.AddRange(new object[] { "OneBot（WebSocket正向连接）", "OneBot（WebSocket反向连接）", "Satori", "插件注册（实验性）" });
            _adapterComboBox.Location = new System.Drawing.Point(27, 83);
            _adapterComboBox.Name = "_adapterComboBox";
            _adapterComboBox.Size = new System.Drawing.Size(420, 39);
            _adapterComboBox.TabIndex = 1;
            toolTip.SetToolTip(_adapterComboBox, "决定Serein将以何种方式连接");
            _adapterComboBox.SelectedIndexChanged += AdapterComboBox_SelectedIndexChanged;
            // 
            // adapterLabel
            // 
            adapterLabel.AutoSize = true;
            adapterLabel.Location = new System.Drawing.Point(27, 49);
            adapterLabel.Name = "adapterLabel";
            adapterLabel.Size = new System.Drawing.Size(86, 31);
            adapterLabel.TabIndex = 0;
            adapterLabel.Text = "适配器";
            toolTip.SetToolTip(adapterLabel, "决定Serein将以何种方式连接");
            // 
            // oneBotVersionLabel
            // 
            oneBotVersionLabel.AutoSize = true;
            oneBotVersionLabel.Location = new System.Drawing.Point(27, 51);
            oneBotVersionLabel.Name = "oneBotVersionLabel";
            oneBotVersionLabel.Size = new System.Drawing.Size(150, 31);
            oneBotVersionLabel.TabIndex = 2;
            oneBotVersionLabel.Text = "OneBot版本";
            toolTip.SetToolTip(oneBotVersionLabel, "将影响Serein中对数据包的处理方式");
            // 
            // webSocketUriLabel
            // 
            webSocketUriLabel.AutoSize = true;
            webSocketUriLabel.Location = new System.Drawing.Point(27, 134);
            webSocketUriLabel.Name = "webSocketUriLabel";
            webSocketUriLabel.Size = new System.Drawing.Size(192, 31);
            webSocketUriLabel.TabIndex = 14;
            webSocketUriLabel.Text = "WebSocket地址";
            toolTip.SetToolTip(webSocketUriLabel, "Websocket服务器的地址\r\n· 当选择了正向连接适配器时，这个地址应以\"ws://\"或\"wss://\"开头，Serein将会连接到这个地址\r\n· 当选择了反向连接适配器时，这个地址应以\"http://\"或\"https://\"开头，Serein将会开启一个WebSocket服务器供OneBot实现连接");
            // 
            // webSocketSubProtocolsLabel
            // 
            webSocketSubProtocolsLabel.AutoSize = true;
            webSocketSubProtocolsLabel.Location = new System.Drawing.Point(27, 298);
            webSocketSubProtocolsLabel.Name = "webSocketSubProtocolsLabel";
            webSocketSubProtocolsLabel.Size = new System.Drawing.Size(216, 31);
            webSocketSubProtocolsLabel.TabIndex = 16;
            webSocketSubProtocolsLabel.Text = "WebSocket子协议";
            toolTip.SetToolTip(webSocketSubProtocolsLabel, "连接WebSocket时的子协议（一行一个）\r\n· 仅适用于WebSocket正向连接");
            // 
            // oneBotAccessTokenLabel
            // 
            oneBotAccessTokenLabel.AutoSize = true;
            oneBotAccessTokenLabel.Location = new System.Drawing.Point(27, 216);
            oneBotAccessTokenLabel.Name = "oneBotAccessTokenLabel";
            oneBotAccessTokenLabel.Size = new System.Drawing.Size(229, 31);
            oneBotAccessTokenLabel.TabIndex = 18;
            oneBotAccessTokenLabel.Text = "鉴权凭证（Token）";
            toolTip.SetToolTip(oneBotAccessTokenLabel, "用于鉴权的Access-Token");
            // 
            // _satoriAccessTokenMaskedTextBox
            // 
            _satoriAccessTokenMaskedTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _satoriAccessTokenMaskedTextBox.Location = new System.Drawing.Point(27, 169);
            _satoriAccessTokenMaskedTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _satoriAccessTokenMaskedTextBox.Name = "_satoriAccessTokenMaskedTextBox";
            _satoriAccessTokenMaskedTextBox.Size = new System.Drawing.Size(1211, 38);
            _satoriAccessTokenMaskedTextBox.TabIndex = 23;
            toolTip.SetToolTip(_satoriAccessTokenMaskedTextBox, "用于鉴权的Token");
            _satoriAccessTokenMaskedTextBox.TextChanged += OnPropertyChanged;
            // 
            // satoriAccessTokenLabel
            // 
            satoriAccessTokenLabel.AutoSize = true;
            satoriAccessTokenLabel.Location = new System.Drawing.Point(27, 135);
            satoriAccessTokenLabel.Name = "satoriAccessTokenLabel";
            satoriAccessTokenLabel.Size = new System.Drawing.Size(110, 31);
            satoriAccessTokenLabel.TabIndex = 22;
            satoriAccessTokenLabel.Text = "鉴权凭证";
            toolTip.SetToolTip(satoriAccessTokenLabel, "用于鉴权的Token");
            // 
            // _satoriUriTextBox
            // 
            _satoriUriTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _satoriUriTextBox.Location = new System.Drawing.Point(27, 87);
            _satoriUriTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _satoriUriTextBox.Name = "_satoriUriTextBox";
            _satoriUriTextBox.Size = new System.Drawing.Size(1211, 38);
            _satoriUriTextBox.TabIndex = 21;
            toolTip.SetToolTip(_satoriUriTextBox, "用于Api请求的地址");
            _satoriUriTextBox.TextChanged += OnPropertyChanged;
            // 
            // satoriUriLabel
            // 
            satoriUriLabel.AutoSize = true;
            satoriUriLabel.Location = new System.Drawing.Point(27, 53);
            satoriUriLabel.Name = "satoriUriLabel";
            satoriUriLabel.Size = new System.Drawing.Size(62, 31);
            satoriUriLabel.TabIndex = 20;
            satoriUriLabel.Text = "地址";
            toolTip.SetToolTip(satoriUriLabel, "用于Api请求的地址");
            // 
            // _autoEscapeCheckBox
            // 
            _autoEscapeCheckBox.AutoSize = true;
            _autoEscapeCheckBox.Location = new System.Drawing.Point(27, 518);
            _autoEscapeCheckBox.Name = "_autoEscapeCheckBox";
            _autoEscapeCheckBox.Size = new System.Drawing.Size(166, 35);
            _autoEscapeCheckBox.TabIndex = 22;
            _autoEscapeCheckBox.Text = "纯文本发送";
            toolTip.SetToolTip(_autoEscapeCheckBox, "消息内容作为纯文本发送（即不解析CQ码）\r\n· 仅当OneBot版本为V11时生效");
            _autoEscapeCheckBox.UseVisualStyleBackColor = true;
            _autoEscapeCheckBox.Click += OnPropertyChanged;
            // 
            // _grantPermissionToGroupOwnerAndAdminsCheckBox
            // 
            _grantPermissionToGroupOwnerAndAdminsCheckBox.AutoSize = true;
            _grantPermissionToGroupOwnerAndAdminsCheckBox.Location = new System.Drawing.Point(27, 559);
            _grantPermissionToGroupOwnerAndAdminsCheckBox.Name = "_grantPermissionToGroupOwnerAndAdminsCheckBox";
            _grantPermissionToGroupOwnerAndAdminsCheckBox.Size = new System.Drawing.Size(382, 35);
            _grantPermissionToGroupOwnerAndAdminsCheckBox.TabIndex = 21;
            _grantPermissionToGroupOwnerAndAdminsCheckBox.Text = "赋予所有群主和管理员管理权限";
            toolTip.SetToolTip(_grantPermissionToGroupOwnerAndAdminsCheckBox, "使监听群的群主和管理员与上方的管理权限列表中的用户拥有相同权限\r\n· 仅当OneBot版本为V11时生效");
            _grantPermissionToGroupOwnerAndAdminsCheckBox.UseVisualStyleBackColor = true;
            _grantPermissionToGroupOwnerAndAdminsCheckBox.Click += OnPropertyChanged;
            // 
            // _autoReconnectCheckBox
            // 
            _autoReconnectCheckBox.AutoSize = true;
            _autoReconnectCheckBox.Location = new System.Drawing.Point(27, 477);
            _autoReconnectCheckBox.Name = "_autoReconnectCheckBox";
            _autoReconnectCheckBox.Size = new System.Drawing.Size(190, 35);
            _autoReconnectCheckBox.TabIndex = 20;
            _autoReconnectCheckBox.Text = "断线自动重连";
            toolTip.SetToolTip(_autoReconnectCheckBox, "WebSocket连接异常断开时自动重连\r\n· 仅适用于WebSocket正向连接");
            _autoReconnectCheckBox.UseVisualStyleBackColor = true;
            _autoReconnectCheckBox.Click += OnPropertyChanged;
            // 
            // _oneBotAccessTokenMaskedTextBox
            // 
            _oneBotAccessTokenMaskedTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _oneBotAccessTokenMaskedTextBox.Location = new System.Drawing.Point(27, 250);
            _oneBotAccessTokenMaskedTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _oneBotAccessTokenMaskedTextBox.Name = "_oneBotAccessTokenMaskedTextBox";
            _oneBotAccessTokenMaskedTextBox.Size = new System.Drawing.Size(1211, 38);
            _oneBotAccessTokenMaskedTextBox.TabIndex = 19;
            toolTip.SetToolTip(_oneBotAccessTokenMaskedTextBox, "用于鉴权的Access-Token");
            _oneBotAccessTokenMaskedTextBox.TextChanged += OnPropertyChanged;
            // 
            // _webSocketSubProtocolsTextBox
            // 
            _webSocketSubProtocolsTextBox.AcceptsReturn = true;
            _webSocketSubProtocolsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _webSocketSubProtocolsTextBox.Location = new System.Drawing.Point(27, 332);
            _webSocketSubProtocolsTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _webSocketSubProtocolsTextBox.Multiline = true;
            _webSocketSubProtocolsTextBox.Name = "_webSocketSubProtocolsTextBox";
            _webSocketSubProtocolsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _webSocketSubProtocolsTextBox.Size = new System.Drawing.Size(1211, 132);
            _webSocketSubProtocolsTextBox.TabIndex = 17;
            toolTip.SetToolTip(_webSocketSubProtocolsTextBox, "连接WebSocket时的子协议（一行一个）\r\n· 仅适用于WebSocket正向连接");
            _webSocketSubProtocolsTextBox.TextChanged += WebSocketSubProtocolsTextBox_TextChanged;
            // 
            // _webSocketUriTextBox
            // 
            _webSocketUriTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _webSocketUriTextBox.Location = new System.Drawing.Point(27, 168);
            _webSocketUriTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _webSocketUriTextBox.Name = "_webSocketUriTextBox";
            _webSocketUriTextBox.Size = new System.Drawing.Size(1211, 38);
            _webSocketUriTextBox.TabIndex = 15;
            toolTip.SetToolTip(_webSocketUriTextBox, "Websocket服务器的地址\r\n· 当选择了正向连接适配器时，这个地址应以\"ws://\"或\"wss://\"开头，Serein将会连接到这个地址\r\n· 当选择了反向连接适配器时，这个地址应以\"http://\"或\"https://\"开头，Serein将会开启一个WebSocket服务器供OneBot实现连接");
            _webSocketUriTextBox.TextChanged += OnPropertyChanged;
            // 
            // _oneBotVersionComboBox
            // 
            _oneBotVersionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _oneBotVersionComboBox.FormattingEnabled = true;
            _oneBotVersionComboBox.Items.AddRange(new object[] { "V11", "V12" });
            _oneBotVersionComboBox.Location = new System.Drawing.Point(27, 85);
            _oneBotVersionComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _oneBotVersionComboBox.Name = "_oneBotVersionComboBox";
            _oneBotVersionComboBox.Size = new System.Drawing.Size(248, 39);
            _oneBotVersionComboBox.TabIndex = 3;
            toolTip.SetToolTip(_oneBotVersionComboBox, "将影响Serein中对数据包的处理方式");
            _oneBotVersionComboBox.SelectedIndexChanged += OneBotVersionComboBox_SelectedIndexChanged;
            // 
            // commonGroupBox
            // 
            commonGroupBox.Controls.Add(_selfPlatformTextBox);
            commonGroupBox.Controls.Add(selfPlatformLabel);
            commonGroupBox.Controls.Add(_selfUserIdTextBox);
            commonGroupBox.Controls.Add(selfUserIdLabel);
            commonGroupBox.Controls.Add(selfLabel);
            commonGroupBox.Controls.Add(_administratorUserIdsTextBox);
            commonGroupBox.Controls.Add(administratorUserIdsLabel);
            commonGroupBox.Controls.Add(_listenedIdsTextBox);
            commonGroupBox.Controls.Add(listenedIdsLabel);
            commonGroupBox.Controls.Add(_connectWhenSettingUpcheckBox);
            commonGroupBox.Controls.Add(_saveLogCheckBox);
            commonGroupBox.Controls.Add(_outputDataCheckBox);
            commonGroupBox.Controls.Add(_adapterComboBox);
            commonGroupBox.Controls.Add(adapterLabel);
            commonGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            commonGroupBox.Location = new System.Drawing.Point(10, 10);
            commonGroupBox.Name = "commonGroupBox";
            commonGroupBox.Size = new System.Drawing.Size(1260, 612);
            commonGroupBox.TabIndex = 0;
            commonGroupBox.TabStop = false;
            commonGroupBox.Text = "通用";
            // 
            // satoriGroupBox
            // 
            satoriGroupBox.Controls.Add(_satoriAccessTokenMaskedTextBox);
            satoriGroupBox.Controls.Add(satoriAccessTokenLabel);
            satoriGroupBox.Controls.Add(_satoriUriTextBox);
            satoriGroupBox.Controls.Add(satoriUriLabel);
            satoriGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            satoriGroupBox.Location = new System.Drawing.Point(10, 1237);
            satoriGroupBox.Name = "satoriGroupBox";
            satoriGroupBox.Size = new System.Drawing.Size(1260, 238);
            satoriGroupBox.TabIndex = 2;
            satoriGroupBox.TabStop = false;
            satoriGroupBox.Text = "Satori";
            // 
            // oneBotGroupBox
            // 
            oneBotGroupBox.Controls.Add(_autoEscapeCheckBox);
            oneBotGroupBox.Controls.Add(_grantPermissionToGroupOwnerAndAdminsCheckBox);
            oneBotGroupBox.Controls.Add(_autoReconnectCheckBox);
            oneBotGroupBox.Controls.Add(_oneBotAccessTokenMaskedTextBox);
            oneBotGroupBox.Controls.Add(oneBotAccessTokenLabel);
            oneBotGroupBox.Controls.Add(_webSocketSubProtocolsTextBox);
            oneBotGroupBox.Controls.Add(webSocketSubProtocolsLabel);
            oneBotGroupBox.Controls.Add(_webSocketUriTextBox);
            oneBotGroupBox.Controls.Add(webSocketUriLabel);
            oneBotGroupBox.Controls.Add(_oneBotVersionComboBox);
            oneBotGroupBox.Controls.Add(oneBotVersionLabel);
            oneBotGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            oneBotGroupBox.Location = new System.Drawing.Point(10, 622);
            oneBotGroupBox.Name = "oneBotGroupBox";
            oneBotGroupBox.Size = new System.Drawing.Size(1260, 615);
            oneBotGroupBox.TabIndex = 1;
            oneBotGroupBox.TabStop = false;
            oneBotGroupBox.Text = "OneBot";
            // 
            // ConnectionSettingPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(satoriGroupBox);
            Controls.Add(oneBotGroupBox);
            Controls.Add(commonGroupBox);
            Name = "ConnectionSettingPage";
            Padding = new System.Windows.Forms.Padding(10);
            Size = new System.Drawing.Size(1280, 1488);
            commonGroupBox.ResumeLayout(false);
            commonGroupBox.PerformLayout();
            satoriGroupBox.ResumeLayout(false);
            satoriGroupBox.PerformLayout();
            oneBotGroupBox.ResumeLayout(false);
            oneBotGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ComboBox _adapterComboBox;
        private System.Windows.Forms.CheckBox _outputDataCheckBox;
        private System.Windows.Forms.TextBox _listenedIdsTextBox;
        private System.Windows.Forms.CheckBox _connectWhenSettingUpcheckBox;
        private System.Windows.Forms.CheckBox _saveLogCheckBox;
        private System.Windows.Forms.TextBox _administratorUserIdsTextBox;
        private System.Windows.Forms.TextBox _selfPlatformTextBox;
        private System.Windows.Forms.TextBox _selfUserIdTextBox;
        private System.Windows.Forms.ComboBox _oneBotVersionComboBox;
        private System.Windows.Forms.TextBox _webSocketUriTextBox;
        private System.Windows.Forms.MaskedTextBox _oneBotAccessTokenMaskedTextBox;
        private System.Windows.Forms.TextBox _webSocketSubProtocolsTextBox;
        private System.Windows.Forms.CheckBox _autoEscapeCheckBox;
        private System.Windows.Forms.CheckBox _grantPermissionToGroupOwnerAndAdminsCheckBox;
        private System.Windows.Forms.CheckBox _autoReconnectCheckBox;
        private System.Windows.Forms.MaskedTextBox _satoriAccessTokenMaskedTextBox;
        private System.Windows.Forms.TextBox _satoriUriTextBox;
    }
}
