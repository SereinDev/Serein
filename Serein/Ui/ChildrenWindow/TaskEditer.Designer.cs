namespace Serein.Ui.ChildrenWindow
{
    partial class TaskEditer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskEditer));
            this.Cron = new System.Windows.Forms.TextBox();
            this.CronLabel = new System.Windows.Forms.Label();
            this.CommandLabel = new System.Windows.Forms.Label();
            this.Command = new System.Windows.Forms.TextBox();
            this.RemarkLabel = new System.Windows.Forms.Label();
            this.Remark = new System.Windows.Forms.TextBox();
            this.Confirm = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.CronNextTime = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Cron
            // 
            this.Cron.Location = new System.Drawing.Point(198, 56);
            this.Cron.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Cron.Name = "Cron";
            this.Cron.Size = new System.Drawing.Size(642, 38);
            this.Cron.TabIndex = 0;
            this.Cron.TextChanged += new System.EventHandler(this.Cron_TextChanged);
            this.Cron.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Cron_KeyDown);
            this.Cron.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Cron_KeyPress);
            // 
            // CronLabel
            // 
            this.CronLabel.AutoSize = true;
            this.CronLabel.Location = new System.Drawing.Point(40, 59);
            this.CronLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CronLabel.Name = "CronLabel";
            this.CronLabel.Size = new System.Drawing.Size(141, 31);
            this.CronLabel.TabIndex = 1;
            this.CronLabel.Text = "Cron表达式";
            // 
            // CommandLabel
            // 
            this.CommandLabel.AutoSize = true;
            this.CommandLabel.Location = new System.Drawing.Point(68, 145);
            this.CommandLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CommandLabel.Name = "CommandLabel";
            this.CommandLabel.Size = new System.Drawing.Size(110, 31);
            this.CommandLabel.TabIndex = 3;
            this.CommandLabel.Text = "执行命令";
            // 
            // Command
            // 
            this.Command.Location = new System.Drawing.Point(198, 141);
            this.Command.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Command.Name = "Command";
            this.Command.Size = new System.Drawing.Size(642, 38);
            this.Command.TabIndex = 2;
            this.Command.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Command_KeyDown);
            this.Command.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Command_KeyPress);
            // 
            // RemarkLabel
            // 
            this.RemarkLabel.AutoSize = true;
            this.RemarkLabel.Location = new System.Drawing.Point(124, 230);
            this.RemarkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RemarkLabel.Name = "RemarkLabel";
            this.RemarkLabel.Size = new System.Drawing.Size(62, 31);
            this.RemarkLabel.TabIndex = 5;
            this.RemarkLabel.Text = "备注";
            // 
            // Remark
            // 
            this.Remark.Location = new System.Drawing.Point(198, 226);
            this.Remark.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Remark.Name = "Remark";
            this.Remark.Size = new System.Drawing.Size(642, 38);
            this.Remark.TabIndex = 4;
            this.Remark.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Remark_KeyDown);
            this.Remark.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Remark_KeyPress);
            // 
            // Confirm
            // 
            this.Confirm.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Confirm.Location = new System.Drawing.Point(198, 389);
            this.Confirm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(177, 71);
            this.Confirm.TabIndex = 6;
            this.Confirm.Text = "确定";
            this.Confirm.UseVisualStyleBackColor = true;
            this.Confirm.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(531, 389);
            this.Cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(177, 71);
            this.Cancel.TabIndex = 7;
            this.Cancel.Text = "取消";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // CronNextTime
            // 
            this.CronNextTime.ForeColor = System.Drawing.SystemColors.WindowText;
            this.CronNextTime.Location = new System.Drawing.Point(44, 311);
            this.CronNextTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CronNextTime.Name = "CronNextTime";
            this.CronNextTime.ReadOnly = true;
            this.CronNextTime.Size = new System.Drawing.Size(796, 38);
            this.CronNextTime.TabIndex = 8;
            // 
            // TaskEditer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(903, 490);
            this.Controls.Add(this.CronNextTime);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(this.RemarkLabel);
            this.Controls.Add(this.Remark);
            this.Controls.Add(this.CommandLabel);
            this.Controls.Add(this.Command);
            this.Controls.Add(this.CronLabel);
            this.Controls.Add(this.Cron);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaskEditer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "任务编辑器";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.TaskEditer_HelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label CronLabel;
        private System.Windows.Forms.Label CommandLabel;
        private System.Windows.Forms.Label RemarkLabel;
        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.TextBox CronNextTime;
        public System.Windows.Forms.TextBox Cron;
        public System.Windows.Forms.TextBox Command;
        public System.Windows.Forms.TextBox Remark;
    }
}