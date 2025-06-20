namespace Serein.Lite.Ui.Functions
{
    partial class MatchEditor
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
            System.Windows.Forms.Label regexLabel;
            System.Windows.Forms.Label commandLabel;
            System.Windows.Forms.Label descriptionLabel;
            System.Windows.Forms.Label exclusionsLabel;
            System.Windows.Forms.Label fieldTypeLabel;
            System.Windows.Forms.Button confirmButton;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatchEditor));
            _regexTextBox = new System.Windows.Forms.TextBox();
            _commandTextBox = new System.Windows.Forms.TextBox();
            _descriptionTextBox = new System.Windows.Forms.TextBox();
            _exclusionsTextBox = new System.Windows.Forms.TextBox();
            _fieldTypeComboBox = new System.Windows.Forms.ComboBox();
            _requireAdminCheckBox = new System.Windows.Forms.CheckBox();
            _regexErrorProvider = new System.Windows.Forms.ErrorProvider(components);
            _commandErrorProvider = new System.Windows.Forms.ErrorProvider(components);
            regexLabel = new System.Windows.Forms.Label();
            commandLabel = new System.Windows.Forms.Label();
            descriptionLabel = new System.Windows.Forms.Label();
            exclusionsLabel = new System.Windows.Forms.Label();
            fieldTypeLabel = new System.Windows.Forms.Label();
            confirmButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)_regexErrorProvider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_commandErrorProvider).BeginInit();
            SuspendLayout();
            // 
            // regexLabel
            // 
            regexLabel.AutoSize = true;
            regexLabel.Location = new System.Drawing.Point(35, 25);
            regexLabel.Name = "regexLabel";
            regexLabel.Size = new System.Drawing.Size(134, 31);
            regexLabel.TabIndex = 0;
            regexLabel.Text = "正则表达式";
            // 
            // commandLabel
            // 
            commandLabel.AutoSize = true;
            commandLabel.Location = new System.Drawing.Point(35, 174);
            commandLabel.Name = "commandLabel";
            commandLabel.Size = new System.Drawing.Size(62, 31);
            commandLabel.TabIndex = 5;
            commandLabel.Text = "命令";
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new System.Drawing.Point(35, 256);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new System.Drawing.Size(62, 31);
            descriptionLabel.TabIndex = 7;
            descriptionLabel.Text = "描述";
            // 
            // exclusionsLabel
            // 
            exclusionsLabel.AutoSize = true;
            exclusionsLabel.Location = new System.Drawing.Point(35, 338);
            exclusionsLabel.Name = "exclusionsLabel";
            exclusionsLabel.Size = new System.Drawing.Size(110, 31);
            exclusionsLabel.TabIndex = 9;
            exclusionsLabel.Text = "限制参数";
            // 
            // fieldTypeLabel
            // 
            fieldTypeLabel.AutoSize = true;
            fieldTypeLabel.Location = new System.Drawing.Point(35, 122);
            fieldTypeLabel.Name = "fieldTypeLabel";
            fieldTypeLabel.Size = new System.Drawing.Size(86, 31);
            fieldTypeLabel.TabIndex = 2;
            fieldTypeLabel.Text = "匹配域";
            // 
            // confirmButton
            // 
            confirmButton.Location = new System.Drawing.Point(297, 445);
            confirmButton.Name = "confirmButton";
            confirmButton.Size = new System.Drawing.Size(150, 51);
            confirmButton.TabIndex = 11;
            confirmButton.Text = "确认";
            confirmButton.UseVisualStyleBackColor = true;
            confirmButton.Click += ConfirmButton_Click;
            // 
            // _regexTextBox
            // 
            _regexTextBox.Location = new System.Drawing.Point(35, 59);
            _regexTextBox.Name = "_regexTextBox";
            _regexTextBox.Size = new System.Drawing.Size(693, 38);
            _regexTextBox.TabIndex = 1;
            _regexTextBox.Enter += RegexTextBox_Enter;
            _regexTextBox.Validating += Regex_Validating;
            // 
            // _commandTextBox
            // 
            _commandTextBox.Location = new System.Drawing.Point(35, 208);
            _commandTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _commandTextBox.Name = "_commandTextBox";
            _commandTextBox.Size = new System.Drawing.Size(693, 38);
            _commandTextBox.TabIndex = 6;
            _commandTextBox.Enter += CommandTextBox_Enter;
            _commandTextBox.Validating += CommandTextBox_Validating;
            // 
            // _descriptionTextBox
            // 
            _descriptionTextBox.Location = new System.Drawing.Point(35, 290);
            _descriptionTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _descriptionTextBox.Name = "_descriptionTextBox";
            _descriptionTextBox.Size = new System.Drawing.Size(693, 38);
            _descriptionTextBox.TabIndex = 8;
            // 
            // _exclusionsTextBox
            // 
            _exclusionsTextBox.Location = new System.Drawing.Point(35, 372);
            _exclusionsTextBox.Name = "_exclusionsTextBox";
            _exclusionsTextBox.Size = new System.Drawing.Size(693, 38);
            _exclusionsTextBox.TabIndex = 10;
            // 
            // _fieldTypeComboBox
            // 
            _fieldTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _fieldTypeComboBox.FormattingEnabled = true;
            _fieldTypeComboBox.Items.AddRange(new object[] { "禁用", "服务器输出", "服务器输入", "群聊消息（Group）", "私聊消息（Private/Direct）", "自身消息（Self）", "频道消息（Channel）", "群组消息（Guild）" });
            _fieldTypeComboBox.Location = new System.Drawing.Point(127, 119);
            _fieldTypeComboBox.Name = "_fieldTypeComboBox";
            _fieldTypeComboBox.Size = new System.Drawing.Size(242, 39);
            _fieldTypeComboBox.TabIndex = 3;
            _fieldTypeComboBox.SelectedIndexChanged += FieldType_SelectedIndexChanged;
            // 
            // _requireAdminCheckBox
            // 
            _requireAdminCheckBox.AutoSize = true;
            _requireAdminCheckBox.Location = new System.Drawing.Point(474, 118);
            _requireAdminCheckBox.Name = "_requireAdminCheckBox";
            _requireAdminCheckBox.Size = new System.Drawing.Size(214, 35);
            _requireAdminCheckBox.TabIndex = 4;
            _requireAdminCheckBox.Text = "需要管理员权限";
            _requireAdminCheckBox.UseVisualStyleBackColor = true;
            // 
            // _regexErrorProvider
            // 
            _regexErrorProvider.ContainerControl = this;
            // 
            // _commandErrorProvider
            // 
            _commandErrorProvider.ContainerControl = this;
            // 
            // MatchEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(774, 529);
            Controls.Add(confirmButton);
            Controls.Add(fieldTypeLabel);
            Controls.Add(_requireAdminCheckBox);
            Controls.Add(_fieldTypeComboBox);
            Controls.Add(_exclusionsTextBox);
            Controls.Add(exclusionsLabel);
            Controls.Add(_descriptionTextBox);
            Controls.Add(descriptionLabel);
            Controls.Add(_commandTextBox);
            Controls.Add(commandLabel);
            Controls.Add(_regexTextBox);
            Controls.Add(regexLabel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(800, 600);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(800, 600);
            Name = "MatchEditor";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "匹配编辑器";
            ((System.ComponentModel.ISupportInitialize)_regexErrorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)_commandErrorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox _regexTextBox;
        private System.Windows.Forms.TextBox _commandTextBox;
        private System.Windows.Forms.TextBox _descriptionTextBox;
        private System.Windows.Forms.TextBox _exclusionsTextBox;
        private System.Windows.Forms.ComboBox _fieldTypeComboBox;
        private System.Windows.Forms.CheckBox _requireAdminCheckBox;
        private System.Windows.Forms.ErrorProvider _regexErrorProvider;
        private System.Windows.Forms.ErrorProvider _commandErrorProvider;
    }
}