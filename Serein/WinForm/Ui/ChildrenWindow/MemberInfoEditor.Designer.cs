namespace Serein.Ui.ChildrenWindow

{
    partial class MemberInfoEditor
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
            System.Windows.Forms.Label GameIDLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MemberInfoEditor));
            this.GameIDBox = new System.Windows.Forms.TextBox();
            this.ID = new System.Windows.Forms.Label();
            this.NickName = new System.Windows.Forms.Label();
            this.Confirm = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            GameIDLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // GameIDBox
            // 
            this.GameIDBox.Location = new System.Drawing.Point(74, 169);
            this.GameIDBox.Name = "GameIDBox";
            this.GameIDBox.Size = new System.Drawing.Size(367, 38);
            this.GameIDBox.TabIndex = 0;
            // 
            // ID
            // 
            this.ID.AutoSize = true;
            this.ID.Location = new System.Drawing.Point(74, 30);
            this.ID.Margin = new System.Windows.Forms.Padding(10);
            this.ID.Name = "ID";
            this.ID.Size = new System.Drawing.Size(78, 31);
            this.ID.TabIndex = 1;
            this.ID.Text = "QQ：";
            // 
            // NickName
            // 
            this.NickName.AutoSize = true;
            this.NickName.Location = new System.Drawing.Point(74, 81);
            this.NickName.Margin = new System.Windows.Forms.Padding(10);
            this.NickName.Name = "NickName";
            this.NickName.Size = new System.Drawing.Size(86, 31);
            this.NickName.TabIndex = 2;
            this.NickName.Text = "昵称：";
            // 
            // GameIDLabel
            // 
            GameIDLabel.AutoSize = true;
            GameIDLabel.Location = new System.Drawing.Point(74, 132);
            GameIDLabel.Margin = new System.Windows.Forms.Padding(10, 10, 10, 3);
            GameIDLabel.Name = "GameIDLabel";
            GameIDLabel.Size = new System.Drawing.Size(87, 31);
            GameIDLabel.TabIndex = 3;
            GameIDLabel.Text = "游戏ID";
            // 
            // Confirm
            // 
            this.Confirm.Location = new System.Drawing.Point(74, 254);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(150, 58);
            this.Confirm.TabIndex = 4;
            this.Confirm.Text = "确认";
            this.Confirm.UseVisualStyleBackColor = true;
            this.Confirm.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(291, 254);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(150, 58);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "取消";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // MemberInfoEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 353);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(GameIDLabel);
            this.Controls.Add(this.NickName);
            this.Controls.Add(this.ID);
            this.Controls.Add(this.GameIDBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MemberInfoEditor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "成员信息编辑器";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.MemberInfoEditer_HelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label ID;
        private System.Windows.Forms.Label NickName;
        private System.Windows.Forms.Label GameIDLabel;
        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.Button Cancel;
        public System.Windows.Forms.TextBox GameIDBox;
    }
}