namespace Serein.Lite.Ui.Function
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
            System.Windows.Forms.Label RegexLabel;
            System.Windows.Forms.Label CommandLabel;
            System.Windows.Forms.Label DescriptionLabel;
            System.Windows.Forms.Label RestrictionsLabel;
            System.Windows.Forms.Label FieldTypeLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatchEditor));
            RegexTextBox = new System.Windows.Forms.TextBox();
            CommandTextBox = new System.Windows.Forms.TextBox();
            DescriptionTextBox = new System.Windows.Forms.TextBox();
            RestrictionsTextBox = new System.Windows.Forms.TextBox();
            FieldTypeComboBox = new System.Windows.Forms.ComboBox();
            RequireAdminCheckBox = new System.Windows.Forms.CheckBox();
            ConfirmButton = new System.Windows.Forms.Button();
            RegexErrorProvider = new System.Windows.Forms.ErrorProvider(components);
            CommandErrorProvider = new System.Windows.Forms.ErrorProvider(components);
            RegexLabel = new System.Windows.Forms.Label();
            CommandLabel = new System.Windows.Forms.Label();
            DescriptionLabel = new System.Windows.Forms.Label();
            RestrictionsLabel = new System.Windows.Forms.Label();
            FieldTypeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)RegexErrorProvider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CommandErrorProvider).BeginInit();
            SuspendLayout();
            // 
            // RegexLabel
            // 
            RegexLabel.AutoSize = true;
            RegexLabel.Location = new System.Drawing.Point(35, 25);
            RegexLabel.Name = "RegexLabel";
            RegexLabel.Size = new System.Drawing.Size(134, 31);
            RegexLabel.TabIndex = 0;
            RegexLabel.Text = "正则表达式";
            // 
            // CommandLabel
            // 
            CommandLabel.AutoSize = true;
            CommandLabel.Location = new System.Drawing.Point(35, 174);
            CommandLabel.Name = "CommandLabel";
            CommandLabel.Size = new System.Drawing.Size(62, 31);
            CommandLabel.TabIndex = 5;
            CommandLabel.Text = "命令";
            // 
            // DescriptionLabel
            // 
            DescriptionLabel.AutoSize = true;
            DescriptionLabel.Location = new System.Drawing.Point(35, 256);
            DescriptionLabel.Name = "DescriptionLabel";
            DescriptionLabel.Size = new System.Drawing.Size(62, 31);
            DescriptionLabel.TabIndex = 7;
            DescriptionLabel.Text = "描述";
            // 
            // RestrictionsLabel
            // 
            RestrictionsLabel.AutoSize = true;
            RestrictionsLabel.Location = new System.Drawing.Point(35, 338);
            RestrictionsLabel.Name = "RestrictionsLabel";
            RestrictionsLabel.Size = new System.Drawing.Size(110, 31);
            RestrictionsLabel.TabIndex = 9;
            RestrictionsLabel.Text = "限制参数";
            // 
            // FieldTypeLabel
            // 
            FieldTypeLabel.AutoSize = true;
            FieldTypeLabel.Location = new System.Drawing.Point(35, 122);
            FieldTypeLabel.Name = "FieldTypeLabel";
            FieldTypeLabel.Size = new System.Drawing.Size(86, 31);
            FieldTypeLabel.TabIndex = 2;
            FieldTypeLabel.Text = "匹配域";
            // 
            // RegexTextBox
            // 
            RegexTextBox.Location = new System.Drawing.Point(35, 59);
            RegexTextBox.Name = "RegexTextBox";
            RegexTextBox.Size = new System.Drawing.Size(693, 38);
            RegexTextBox.TabIndex = 1;
            RegexTextBox.Enter += RegexTextBox_Enter;
            RegexTextBox.Validating += Regex_Validating;
            // 
            // CommandTextBox
            // 
            CommandTextBox.Location = new System.Drawing.Point(35, 208);
            CommandTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            CommandTextBox.Name = "CommandTextBox";
            CommandTextBox.Size = new System.Drawing.Size(693, 38);
            CommandTextBox.TabIndex = 6;
            CommandTextBox.Enter += CommandTextBox_Enter;
            CommandTextBox.Validating += CommandTextBox_Validating;
            // 
            // DescriptionTextBox
            // 
            DescriptionTextBox.Location = new System.Drawing.Point(35, 290);
            DescriptionTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            DescriptionTextBox.Name = "DescriptionTextBox";
            DescriptionTextBox.Size = new System.Drawing.Size(693, 38);
            DescriptionTextBox.TabIndex = 8;
            // 
            // RestrictionsTextBox
            // 
            RestrictionsTextBox.Location = new System.Drawing.Point(35, 372);
            RestrictionsTextBox.Name = "RestrictionsTextBox";
            RestrictionsTextBox.Size = new System.Drawing.Size(693, 38);
            RestrictionsTextBox.TabIndex = 10;
            // 
            // FieldTypeComboBox
            // 
            FieldTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            FieldTypeComboBox.FormattingEnabled = true;
            FieldTypeComboBox.Items.AddRange(new object[] { "禁用", "服务器输出", "服务器输入", "群聊消息", "私聊消息", "自身消息" });
            FieldTypeComboBox.Location = new System.Drawing.Point(127, 119);
            FieldTypeComboBox.Name = "FieldTypeComboBox";
            FieldTypeComboBox.Size = new System.Drawing.Size(242, 39);
            FieldTypeComboBox.TabIndex = 3;
            FieldTypeComboBox.SelectedIndexChanged += FieldType_SelectedIndexChanged;
            // 
            // RequireAdminCheckBox
            // 
            RequireAdminCheckBox.AutoSize = true;
            RequireAdminCheckBox.Location = new System.Drawing.Point(474, 118);
            RequireAdminCheckBox.Name = "RequireAdminCheckBox";
            RequireAdminCheckBox.Size = new System.Drawing.Size(214, 35);
            RequireAdminCheckBox.TabIndex = 4;
            RequireAdminCheckBox.Text = "需要管理员权限";
            RequireAdminCheckBox.UseVisualStyleBackColor = true;
            // 
            // ConfirmButton
            // 
            ConfirmButton.Location = new System.Drawing.Point(297, 445);
            ConfirmButton.Name = "ConfirmButton";
            ConfirmButton.Size = new System.Drawing.Size(150, 51);
            ConfirmButton.TabIndex = 11;
            ConfirmButton.Text = "确认";
            ConfirmButton.UseVisualStyleBackColor = true;
            ConfirmButton.Click += ConfirmButton_Click;
            // 
            // RegexErrorProvider
            // 
            RegexErrorProvider.ContainerControl = this;
            // 
            // CommandErrorProvider
            // 
            CommandErrorProvider.ContainerControl = this;
            // 
            // MatchEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(774, 529);
            Controls.Add(ConfirmButton);
            Controls.Add(FieldTypeLabel);
            Controls.Add(RequireAdminCheckBox);
            Controls.Add(FieldTypeComboBox);
            Controls.Add(RestrictionsTextBox);
            Controls.Add(RestrictionsLabel);
            Controls.Add(DescriptionTextBox);
            Controls.Add(DescriptionLabel);
            Controls.Add(CommandTextBox);
            Controls.Add(CommandLabel);
            Controls.Add(RegexTextBox);
            Controls.Add(RegexLabel);
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
            ((System.ComponentModel.ISupportInitialize)RegexErrorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)CommandErrorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox RegexTextBox;
        private System.Windows.Forms.TextBox CommandTextBox;
        private System.Windows.Forms.TextBox DescriptionTextBox;
        private System.Windows.Forms.TextBox RestrictionsTextBox;
        private System.Windows.Forms.ComboBox FieldTypeComboBox;
        private System.Windows.Forms.CheckBox RequireAdminCheckBox;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.ErrorProvider RegexErrorProvider;
        private System.Windows.Forms.ErrorProvider CommandErrorProvider;
    }
}