namespace Serein.Lite.Ui.Function
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
            System.Windows.Forms.Label CronLabel;
            System.Windows.Forms.Label CommandLabel;
            System.Windows.Forms.Label DescriptionLabel;
            System.Windows.Forms.StatusStrip StatusStrip;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScheduleEditor));
            ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            CronTextBox = new System.Windows.Forms.TextBox();
            CommandTextBox = new System.Windows.Forms.TextBox();
            DescriptionTextBox = new System.Windows.Forms.TextBox();
            EnableCheckBox = new System.Windows.Forms.CheckBox();
            ConfirmButton = new System.Windows.Forms.Button();
            CronErrorProvider = new System.Windows.Forms.ErrorProvider(components);
            CommandErrorProvider = new System.Windows.Forms.ErrorProvider(components);
            CronLabel = new System.Windows.Forms.Label();
            CommandLabel = new System.Windows.Forms.Label();
            DescriptionLabel = new System.Windows.Forms.Label();
            StatusStrip = new System.Windows.Forms.StatusStrip();
            StatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)CronErrorProvider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CommandErrorProvider).BeginInit();
            SuspendLayout();
            // 
            // CronLabel
            // 
            CronLabel.AutoSize = true;
            CronLabel.Location = new System.Drawing.Point(38, 91);
            CronLabel.Name = "CronLabel";
            CronLabel.Size = new System.Drawing.Size(141, 31);
            CronLabel.TabIndex = 0;
            CronLabel.Text = "Cron表达式";
            // 
            // CommandLabel
            // 
            CommandLabel.AutoSize = true;
            CommandLabel.Location = new System.Drawing.Point(38, 178);
            CommandLabel.Name = "CommandLabel";
            CommandLabel.Size = new System.Drawing.Size(62, 31);
            CommandLabel.TabIndex = 2;
            CommandLabel.Text = "命令";
            // 
            // DescriptionLabel
            // 
            DescriptionLabel.AutoSize = true;
            DescriptionLabel.Location = new System.Drawing.Point(38, 265);
            DescriptionLabel.Name = "DescriptionLabel";
            DescriptionLabel.Size = new System.Drawing.Size(62, 31);
            DescriptionLabel.TabIndex = 4;
            DescriptionLabel.Text = "描述";
            // 
            // StatusStrip
            // 
            StatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripStatusLabel });
            StatusStrip.Location = new System.Drawing.Point(0, 467);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Size = new System.Drawing.Size(774, 22);
            StatusStrip.TabIndex = 13;
            StatusStrip.Text = "statusStrip1";
            // 
            // ToolStripStatusLabel
            // 
            ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            ToolStripStatusLabel.Size = new System.Drawing.Size(0, 12);
            // 
            // CronTextBox
            // 
            CronTextBox.Location = new System.Drawing.Point(38, 125);
            CronTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            CronTextBox.Name = "CronTextBox";
            CronTextBox.Size = new System.Drawing.Size(693, 38);
            CronTextBox.TabIndex = 1;
            CronTextBox.TextChanged += CronTextBox_TextChanged;
            CronTextBox.Leave += CronTextBox_Leave;
            CronTextBox.Validating += CronTextBox_Validating;
            // 
            // CommandTextBox
            // 
            CommandTextBox.Location = new System.Drawing.Point(38, 212);
            CommandTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            CommandTextBox.Name = "CommandTextBox";
            CommandTextBox.Size = new System.Drawing.Size(693, 38);
            CommandTextBox.TabIndex = 3;
            CommandTextBox.Enter += CommandTextBox_Enter;
            CommandTextBox.Validating += CommandTextBox_Validating;
            // 
            // DescriptionTextBox
            // 
            DescriptionTextBox.Location = new System.Drawing.Point(38, 299);
            DescriptionTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            DescriptionTextBox.Name = "DescriptionTextBox";
            DescriptionTextBox.Size = new System.Drawing.Size(693, 38);
            DescriptionTextBox.TabIndex = 5;
            // 
            // EnableCheckBox
            // 
            EnableCheckBox.AutoSize = true;
            EnableCheckBox.Location = new System.Drawing.Point(38, 32);
            EnableCheckBox.Name = "EnableCheckBox";
            EnableCheckBox.Size = new System.Drawing.Size(94, 35);
            EnableCheckBox.TabIndex = 6;
            EnableCheckBox.Text = "启用";
            EnableCheckBox.UseVisualStyleBackColor = true;
            // 
            // ConfirmButton
            // 
            ConfirmButton.Location = new System.Drawing.Point(292, 367);
            ConfirmButton.Name = "ConfirmButton";
            ConfirmButton.Size = new System.Drawing.Size(150, 51);
            ConfirmButton.TabIndex = 12;
            ConfirmButton.Text = "确认";
            ConfirmButton.UseVisualStyleBackColor = true;
            ConfirmButton.Click += ConfirmButton_Click;
            // 
            // CronErrorProvider
            // 
            CronErrorProvider.ContainerControl = this;
            // 
            // CommandErrorProvider
            // 
            CommandErrorProvider.ContainerControl = this;
            // 
            // ScheduleEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(774, 489);
            Controls.Add(StatusStrip);
            Controls.Add(ConfirmButton);
            Controls.Add(EnableCheckBox);
            Controls.Add(DescriptionTextBox);
            Controls.Add(DescriptionLabel);
            Controls.Add(CommandTextBox);
            Controls.Add(CommandLabel);
            Controls.Add(CronTextBox);
            Controls.Add(CronLabel);
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
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)CronErrorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)CommandErrorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox CronTextBox;
        private System.Windows.Forms.TextBox CommandTextBox;
        private System.Windows.Forms.TextBox DescriptionTextBox;
        private System.Windows.Forms.CheckBox EnableCheckBox;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
        private System.Windows.Forms.ErrorProvider CronErrorProvider;
        private System.Windows.Forms.ErrorProvider CommandErrorProvider;
    }
}