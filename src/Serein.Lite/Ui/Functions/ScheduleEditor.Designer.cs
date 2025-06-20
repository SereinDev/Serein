namespace Serein.Lite.Ui.Functions
{
    partial class ScheduleEditor
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
            System.Windows.Forms.Label cronLabel;
            System.Windows.Forms.Label commandLabel;
            System.Windows.Forms.Label descriptionLabel;
            System.Windows.Forms.StatusStrip statusStrip;
            System.Windows.Forms.Button confirmButton;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScheduleEditor));
            _toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            _cronTextBox = new System.Windows.Forms.TextBox();
            _commandTextBox = new System.Windows.Forms.TextBox();
            __descriptionTextBox = new System.Windows.Forms.TextBox();
            _enableCheckBox = new System.Windows.Forms.CheckBox();
            _cronErrorProvider = new System.Windows.Forms.ErrorProvider(components);
            _commandErrorProvider = new System.Windows.Forms.ErrorProvider(components);
            cronLabel = new System.Windows.Forms.Label();
            commandLabel = new System.Windows.Forms.Label();
            descriptionLabel = new System.Windows.Forms.Label();
            statusStrip = new System.Windows.Forms.StatusStrip();
            confirmButton = new System.Windows.Forms.Button();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_cronErrorProvider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_commandErrorProvider).BeginInit();
            SuspendLayout();
            // 
            // cronLabel
            // 
            cronLabel.AutoSize = true;
            cronLabel.Location = new System.Drawing.Point(38, 91);
            cronLabel.Name = "cronLabel";
            cronLabel.Size = new System.Drawing.Size(141, 31);
            cronLabel.TabIndex = 0;
            cronLabel.Text = "Cron表达式";
            // 
            // commandLabel
            // 
            commandLabel.AutoSize = true;
            commandLabel.Location = new System.Drawing.Point(38, 178);
            commandLabel.Name = "commandLabel";
            commandLabel.Size = new System.Drawing.Size(62, 31);
            commandLabel.TabIndex = 2;
            commandLabel.Text = "命令";
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new System.Drawing.Point(38, 265);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new System.Drawing.Size(62, 31);
            descriptionLabel.TabIndex = 4;
            descriptionLabel.Text = "描述";
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _toolStripStatusLabel });
            statusStrip.Location = new System.Drawing.Point(0, 467);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(774, 22);
            statusStrip.TabIndex = 13;
            statusStrip.Text = "statusStrip1";
            // 
            // _toolStripStatusLabel
            // 
            _toolStripStatusLabel.Name = "_toolStripStatusLabel";
            _toolStripStatusLabel.Size = new System.Drawing.Size(0, 12);
            // 
            // _cronTextBox
            // 
            _cronTextBox.Location = new System.Drawing.Point(38, 125);
            _cronTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            _cronTextBox.Name = "_cronTextBox";
            _cronTextBox.Size = new System.Drawing.Size(693, 38);
            _cronTextBox.TabIndex = 1;
            _cronTextBox.TextChanged += CronTextBox_TextChanged;
            _cronTextBox.Leave += CronTextBox_Leave;
            _cronTextBox.Validating += CronTextBox_Validating;
            // 
            // _commandTextBox
            // 
            _commandTextBox.Location = new System.Drawing.Point(38, 212);
            _commandTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            _commandTextBox.Name = "_commandTextBox";
            _commandTextBox.Size = new System.Drawing.Size(693, 38);
            _commandTextBox.TabIndex = 3;
            _commandTextBox.Enter += CommandTextBox_Enter;
            _commandTextBox.Validating += CommandTextBox_Validating;
            // 
            // __descriptionTextBox
            // 
            __descriptionTextBox.Location = new System.Drawing.Point(38, 299);
            __descriptionTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            __descriptionTextBox.Name = "__descriptionTextBox";
            __descriptionTextBox.Size = new System.Drawing.Size(693, 38);
            __descriptionTextBox.TabIndex = 5;
            // 
            // _enableCheckBox
            // 
            _enableCheckBox.AutoSize = true;
            _enableCheckBox.Location = new System.Drawing.Point(38, 32);
            _enableCheckBox.Name = "_enableCheckBox";
            _enableCheckBox.Size = new System.Drawing.Size(94, 35);
            _enableCheckBox.TabIndex = 6;
            _enableCheckBox.Text = "启用";
            _enableCheckBox.UseVisualStyleBackColor = true;
            // 
            // confirmButton
            // 
            confirmButton.Location = new System.Drawing.Point(292, 367);
            confirmButton.Name = "confirmButton";
            confirmButton.Size = new System.Drawing.Size(150, 51);
            confirmButton.TabIndex = 12;
            confirmButton.Text = "确认";
            confirmButton.UseVisualStyleBackColor = true;
            confirmButton.Click += ConfirmButton_Click;
            // 
            // _cronErrorProvider
            // 
            _cronErrorProvider.ContainerControl = this;
            // 
            // _commandErrorProvider
            // 
            _commandErrorProvider.ContainerControl = this;
            // 
            // ScheduleEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(774, 489);
            Controls.Add(statusStrip);
            Controls.Add(confirmButton);
            Controls.Add(_enableCheckBox);
            Controls.Add(__descriptionTextBox);
            Controls.Add(descriptionLabel);
            Controls.Add(_commandTextBox);
            Controls.Add(commandLabel);
            Controls.Add(_cronTextBox);
            Controls.Add(cronLabel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(800, 560);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(800, 560);
            Name = "ScheduleEditor";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "定时任务编辑器";
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_cronErrorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)_commandErrorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox _cronTextBox;
        private System.Windows.Forms.TextBox _commandTextBox;
        private System.Windows.Forms.TextBox __descriptionTextBox;
        private System.Windows.Forms.CheckBox _enableCheckBox;
        private System.Windows.Forms.ToolStripStatusLabel _toolStripStatusLabel;
        private System.Windows.Forms.ErrorProvider _cronErrorProvider;
        private System.Windows.Forms.ErrorProvider _commandErrorProvider;
    }
}