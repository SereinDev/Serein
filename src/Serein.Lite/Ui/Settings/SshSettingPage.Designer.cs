namespace Serein.Lite.Ui.Settings
{
    partial class SshSettingPage
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
            System.Windows.Forms.Label PortLabel;
            System.Windows.Forms.Label IpAddressLabel;
            System.Windows.Forms.Label UsersLabel;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ContextMenuStrip ContextMenuStrip;
            System.Windows.Forms.ToolStripMenuItem AddToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator ToolStripSeparator;
            System.Windows.Forms.ToolStripMenuItem RefreshToolStripMenuItem;
            UpdatePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            EnableCheckBox = new System.Windows.Forms.CheckBox();
            PortNumericUpDown = new System.Windows.Forms.NumericUpDown();
            IpAddressTextBox = new System.Windows.Forms.TextBox();
            UsersListView = new System.Windows.Forms.ListView();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            PortLabel = new System.Windows.Forms.Label();
            IpAddressLabel = new System.Windows.Forms.Label();
            UsersLabel = new System.Windows.Forms.Label();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            AddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PortNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // PortLabel
            // 
            PortLabel.AutoSize = true;
            PortLabel.Location = new System.Drawing.Point(27, 169);
            PortLabel.Name = "PortLabel";
            PortLabel.Size = new System.Drawing.Size(62, 31);
            PortLabel.TabIndex = 6;
            PortLabel.Text = "端口";
            // 
            // IpAddressLabel
            // 
            IpAddressLabel.AutoSize = true;
            IpAddressLabel.Location = new System.Drawing.Point(27, 87);
            IpAddressLabel.Name = "IpAddressLabel";
            IpAddressLabel.Size = new System.Drawing.Size(84, 31);
            IpAddressLabel.TabIndex = 15;
            IpAddressLabel.Text = "IP地址";
            // 
            // UsersLabel
            // 
            UsersLabel.AutoSize = true;
            UsersLabel.Location = new System.Drawing.Point(27, 251);
            UsersLabel.Name = "UsersLabel";
            UsersLabel.Size = new System.Drawing.Size(62, 31);
            UsersLabel.TabIndex = 17;
            UsersLabel.Text = "用户";
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "用户名";
            columnHeader1.Width = 200;
            // 
            // ContextMenuStrip
            // 
            ContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { AddToolStripMenuItem, UpdatePasswordToolStripMenuItem, DeleteToolStripMenuItem, ToolStripSeparator, RefreshToolStripMenuItem });
            ContextMenuStrip.Name = "ContextMenuStrip";
            ContextMenuStrip.Size = new System.Drawing.Size(185, 162);
            ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            // 
            // AddToolStripMenuItem
            // 
            AddToolStripMenuItem.Name = "AddToolStripMenuItem";
            AddToolStripMenuItem.Size = new System.Drawing.Size(184, 38);
            AddToolStripMenuItem.Text = "添加";
            AddToolStripMenuItem.Click += AddToolStripMenuItem_Click;
            // 
            // UpdatePasswordToolStripMenuItem
            // 
            UpdatePasswordToolStripMenuItem.Name = "UpdatePasswordToolStripMenuItem";
            UpdatePasswordToolStripMenuItem.Size = new System.Drawing.Size(184, 38);
            UpdatePasswordToolStripMenuItem.Text = "修改密码";
            UpdatePasswordToolStripMenuItem.Click += UpdatePasswordToolStripMenuItem_Click;
            // 
            // DeleteToolStripMenuItem
            // 
            DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
            DeleteToolStripMenuItem.Size = new System.Drawing.Size(184, 38);
            DeleteToolStripMenuItem.Text = "删除";
            DeleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
            // 
            // ToolStripSeparator
            // 
            ToolStripSeparator.Name = "ToolStripSeparator";
            ToolStripSeparator.Size = new System.Drawing.Size(181, 6);
            // 
            // RefreshToolStripMenuItem
            // 
            RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
            RefreshToolStripMenuItem.Size = new System.Drawing.Size(184, 38);
            RefreshToolStripMenuItem.Text = "刷新";
            RefreshToolStripMenuItem.Click += RefreshToolStripMenuItem_Click;
            // 
            // EnableCheckBox
            // 
            EnableCheckBox.AutoSize = true;
            EnableCheckBox.Location = new System.Drawing.Point(27, 25);
            EnableCheckBox.Name = "EnableCheckBox";
            EnableCheckBox.Size = new System.Drawing.Size(94, 35);
            EnableCheckBox.TabIndex = 1;
            EnableCheckBox.Text = "启用";
            EnableCheckBox.UseVisualStyleBackColor = true;
            // 
            // PortNumericUpDown
            // 
            PortNumericUpDown.Location = new System.Drawing.Point(27, 203);
            PortNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            PortNumericUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            PortNumericUpDown.Name = "PortNumericUpDown";
            PortNumericUpDown.Size = new System.Drawing.Size(268, 38);
            PortNumericUpDown.TabIndex = 7;
            PortNumericUpDown.ValueChanged += OnPropertyChanged;
            // 
            // IpAddressTextBox
            // 
            IpAddressTextBox.Location = new System.Drawing.Point(27, 121);
            IpAddressTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            IpAddressTextBox.Name = "IpAddressTextBox";
            IpAddressTextBox.Size = new System.Drawing.Size(268, 38);
            IpAddressTextBox.TabIndex = 16;
            IpAddressTextBox.TextChanged += OnPropertyChanged;
            // 
            // UsersListView
            // 
            UsersListView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            UsersListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2 });
            UsersListView.ContextMenuStrip = ContextMenuStrip;
            UsersListView.FullRowSelect = true;
            UsersListView.GridLines = true;
            UsersListView.Location = new System.Drawing.Point(27, 285);
            UsersListView.Name = "UsersListView";
            UsersListView.Size = new System.Drawing.Size(1211, 388);
            UsersListView.TabIndex = 18;
            UsersListView.UseCompatibleStateImageBehavior = false;
            UsersListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "密码";
            columnHeader2.Width = 300;
            // 
            // SshSettingPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = System.Drawing.Color.White;
            Controls.Add(UsersListView);
            Controls.Add(UsersLabel);
            Controls.Add(IpAddressTextBox);
            Controls.Add(IpAddressLabel);
            Controls.Add(PortNumericUpDown);
            Controls.Add(PortLabel);
            Controls.Add(EnableCheckBox);
            Margin = new System.Windows.Forms.Padding(0);
            Name = "SshSettingPage";
            Size = new System.Drawing.Size(1280, 720);
            ContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PortNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox EnableCheckBox;
        private System.Windows.Forms.NumericUpDown PortNumericUpDown;
        private System.Windows.Forms.TextBox IpAddressTextBox;
        private System.Windows.Forms.ListView UsersListView;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripMenuItem UpdatePasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteToolStripMenuItem;
    }
}
