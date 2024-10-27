namespace Serein.Lite.Ui.Members
{
    partial class PermissionGroupEditor
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
            System.Windows.Forms.TabControl TabControl;
            System.Windows.Forms.TabPage RegularTabPage;
            System.Windows.Forms.Label ParentsLabel;
            System.Windows.Forms.Label PriorityLabel;
            System.Windows.Forms.Label DescriptionLabel;
            System.Windows.Forms.Label NameLabel;
            System.Windows.Forms.Label IdLabel;
            System.Windows.Forms.TabPage MemberTabPage;
            System.Windows.Forms.ColumnHeader columnHeader4;
            System.Windows.Forms.ContextMenuStrip MemberContextMenuStrip;
            System.Windows.Forms.TabPage PermissionTabPage;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ColumnHeader columnHeader3;
            System.Windows.Forms.ContextMenuStrip PermissionContextMenuStrip;
            System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
            System.Windows.Forms.Label PermissionLabel;
            System.Windows.Forms.Label ValueLabel;
            System.Windows.Forms.ToolTip ToolTip;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PermissionGroupEditor));
            ParentsTextBox = new System.Windows.Forms.TextBox();
            PriorityNumericUpDown = new System.Windows.Forms.NumericUpDown();
            DescriptionTextBox = new System.Windows.Forms.TextBox();
            NameTextBox = new System.Windows.Forms.TextBox();
            IdTextBox = new System.Windows.Forms.TextBox();
            MemberListView = new System.Windows.Forms.ListView();
            AddMemberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DeleteMemberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            PermissionListView = new System.Windows.Forms.ListView();
            AddPermissionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DeletePermissionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            PermissionComboBox = new System.Windows.Forms.ComboBox();
            ValueComboBox = new System.Windows.Forms.ComboBox();
            ConfirmButton = new System.Windows.Forms.Button();
            ErrorProvider = new System.Windows.Forms.ErrorProvider(components);
            TabControl = new System.Windows.Forms.TabControl();
            RegularTabPage = new System.Windows.Forms.TabPage();
            ParentsLabel = new System.Windows.Forms.Label();
            PriorityLabel = new System.Windows.Forms.Label();
            DescriptionLabel = new System.Windows.Forms.Label();
            NameLabel = new System.Windows.Forms.Label();
            IdLabel = new System.Windows.Forms.Label();
            MemberTabPage = new System.Windows.Forms.TabPage();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            MemberContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            PermissionTabPage = new System.Windows.Forms.TabPage();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            PermissionContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            PermissionLabel = new System.Windows.Forms.Label();
            ValueLabel = new System.Windows.Forms.Label();
            ToolTip = new System.Windows.Forms.ToolTip(components);
            TabControl.SuspendLayout();
            RegularTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PriorityNumericUpDown).BeginInit();
            MemberTabPage.SuspendLayout();
            MemberContextMenuStrip.SuspendLayout();
            PermissionTabPage.SuspendLayout();
            PermissionContextMenuStrip.SuspendLayout();
            TableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ErrorProvider).BeginInit();
            SuspendLayout();
            // 
            // TabControl
            // 
            TabControl.Controls.Add(RegularTabPage);
            TabControl.Controls.Add(MemberTabPage);
            TabControl.Controls.Add(PermissionTabPage);
            TabControl.Dock = System.Windows.Forms.DockStyle.Top;
            TabControl.Location = new System.Drawing.Point(0, 0);
            TabControl.Name = "TabControl";
            TabControl.SelectedIndex = 0;
            TabControl.Size = new System.Drawing.Size(974, 615);
            TabControl.TabIndex = 0;
            // 
            // RegularTabPage
            // 
            RegularTabPage.Controls.Add(ParentsLabel);
            RegularTabPage.Controls.Add(ParentsTextBox);
            RegularTabPage.Controls.Add(PriorityNumericUpDown);
            RegularTabPage.Controls.Add(PriorityLabel);
            RegularTabPage.Controls.Add(DescriptionLabel);
            RegularTabPage.Controls.Add(DescriptionTextBox);
            RegularTabPage.Controls.Add(NameLabel);
            RegularTabPage.Controls.Add(NameTextBox);
            RegularTabPage.Controls.Add(IdLabel);
            RegularTabPage.Controls.Add(IdTextBox);
            RegularTabPage.Location = new System.Drawing.Point(8, 45);
            RegularTabPage.Name = "RegularTabPage";
            RegularTabPage.Padding = new System.Windows.Forms.Padding(3);
            RegularTabPage.Size = new System.Drawing.Size(958, 562);
            RegularTabPage.TabIndex = 0;
            RegularTabPage.Text = "常规";
            RegularTabPage.UseVisualStyleBackColor = true;
            // 
            // ParentsLabel
            // 
            ParentsLabel.AutoSize = true;
            ParentsLabel.Location = new System.Drawing.Point(42, 347);
            ParentsLabel.Name = "ParentsLabel";
            ParentsLabel.Size = new System.Drawing.Size(132, 31);
            ParentsLabel.TabIndex = 9;
            ParentsLabel.Text = "父权限组Id";
            ToolTip.SetToolTip(ParentsLabel, "要继承的父权限组的Id\r\n");
            // 
            // ParentsTextBox
            // 
            ParentsTextBox.AcceptsReturn = true;
            ParentsTextBox.Location = new System.Drawing.Point(42, 381);
            ParentsTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            ParentsTextBox.Multiline = true;
            ParentsTextBox.Name = "ParentsTextBox";
            ParentsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            ParentsTextBox.Size = new System.Drawing.Size(862, 168);
            ParentsTextBox.TabIndex = 10;
            ToolTip.SetToolTip(ParentsTextBox, "要继承的父权限组的Id\r\n");
            ParentsTextBox.Enter += ParentsTextBox_Enter;
            ParentsTextBox.Validating += ParentsTextBox_Validating;
            // 
            // PriorityNumericUpDown
            // 
            PriorityNumericUpDown.Location = new System.Drawing.Point(42, 294);
            PriorityNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            PriorityNumericUpDown.Maximum = new decimal(new int[] { int.MinValue, 0, 0, 0 });
            PriorityNumericUpDown.Minimum = new decimal(new int[] { int.MinValue, 0, 0, int.MinValue });
            PriorityNumericUpDown.Name = "PriorityNumericUpDown";
            PriorityNumericUpDown.Size = new System.Drawing.Size(304, 38);
            PriorityNumericUpDown.TabIndex = 8;
            ToolTip.SetToolTip(PriorityNumericUpDown, "决定权限的继承顺序");
            // 
            // PriorityLabel
            // 
            PriorityLabel.AutoSize = true;
            PriorityLabel.Location = new System.Drawing.Point(42, 260);
            PriorityLabel.Name = "PriorityLabel";
            PriorityLabel.Size = new System.Drawing.Size(86, 31);
            PriorityLabel.TabIndex = 7;
            PriorityLabel.Text = "优先级";
            ToolTip.SetToolTip(PriorityLabel, "决定权限的继承顺序");
            // 
            // DescriptionLabel
            // 
            DescriptionLabel.AutoSize = true;
            DescriptionLabel.Location = new System.Drawing.Point(42, 178);
            DescriptionLabel.Name = "DescriptionLabel";
            DescriptionLabel.Size = new System.Drawing.Size(62, 31);
            DescriptionLabel.TabIndex = 4;
            DescriptionLabel.Text = "描述";
            ToolTip.SetToolTip(DescriptionLabel, "为权限组提供描述文本");
            // 
            // DescriptionTextBox
            // 
            DescriptionTextBox.Location = new System.Drawing.Point(42, 212);
            DescriptionTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            DescriptionTextBox.Name = "DescriptionTextBox";
            DescriptionTextBox.Size = new System.Drawing.Size(862, 38);
            DescriptionTextBox.TabIndex = 5;
            ToolTip.SetToolTip(DescriptionTextBox, "为权限组提供描述文本");
            // 
            // NameLabel
            // 
            NameLabel.AutoSize = true;
            NameLabel.Location = new System.Drawing.Point(42, 96);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new System.Drawing.Size(62, 31);
            NameLabel.TabIndex = 2;
            NameLabel.Text = "名称";
            ToolTip.SetToolTip(NameLabel, "用于区分权限组，便于管理");
            // 
            // NameTextBox
            // 
            NameTextBox.Location = new System.Drawing.Point(42, 130);
            NameTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            NameTextBox.Name = "NameTextBox";
            NameTextBox.Size = new System.Drawing.Size(862, 38);
            NameTextBox.TabIndex = 3;
            ToolTip.SetToolTip(NameTextBox, "用于区分权限组，便于管理");
            // 
            // IdLabel
            // 
            IdLabel.AutoSize = true;
            IdLabel.Location = new System.Drawing.Point(42, 14);
            IdLabel.Name = "IdLabel";
            IdLabel.Size = new System.Drawing.Size(36, 31);
            IdLabel.TabIndex = 0;
            IdLabel.Text = "Id";
            ToolTip.SetToolTip(IdLabel, "用于区分权限组（一经填写无法修改）\r\n· 长度大于或等于3\r\n· 只由数字、字母和下划线组成\r\n");
            // 
            // IdTextBox
            // 
            IdTextBox.Location = new System.Drawing.Point(42, 48);
            IdTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            IdTextBox.Name = "IdTextBox";
            IdTextBox.Size = new System.Drawing.Size(862, 38);
            IdTextBox.TabIndex = 1;
            ToolTip.SetToolTip(IdTextBox, "用于区分权限组（一经填写无法修改）\r\n· 长度大于或等于3\r\n· 只由数字、字母和下划线组成");
            IdTextBox.Enter += IdTextBox_Enter;
            IdTextBox.Validating += IdTextBox_Validating;
            // 
            // MemberTabPage
            // 
            MemberTabPage.Controls.Add(MemberListView);
            MemberTabPage.Location = new System.Drawing.Point(8, 45);
            MemberTabPage.Name = "MemberTabPage";
            MemberTabPage.Padding = new System.Windows.Forms.Padding(3);
            MemberTabPage.Size = new System.Drawing.Size(958, 562);
            MemberTabPage.TabIndex = 2;
            MemberTabPage.Text = "成员";
            MemberTabPage.UseVisualStyleBackColor = true;
            // 
            // MemberListView
            // 
            MemberListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader4 });
            MemberListView.ContextMenuStrip = MemberContextMenuStrip;
            MemberListView.Dock = System.Windows.Forms.DockStyle.Fill;
            MemberListView.FullRowSelect = true;
            MemberListView.GridLines = true;
            MemberListView.LabelEdit = true;
            MemberListView.Location = new System.Drawing.Point(3, 3);
            MemberListView.Name = "MemberListView";
            MemberListView.Size = new System.Drawing.Size(952, 556);
            MemberListView.TabIndex = 0;
            MemberListView.UseCompatibleStateImageBehavior = false;
            MemberListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "用户Id";
            columnHeader4.Width = 700;
            // 
            // MemberContextMenuStrip
            // 
            MemberContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            MemberContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { AddMemberToolStripMenuItem, DeleteMemberToolStripMenuItem });
            MemberContextMenuStrip.Name = "PermissionContextMenuStrip";
            MemberContextMenuStrip.Size = new System.Drawing.Size(137, 80);
            MemberContextMenuStrip.Opening += MemberContextMenuStrip_Opening;
            // 
            // AddMemberToolStripMenuItem
            // 
            AddMemberToolStripMenuItem.Name = "AddMemberToolStripMenuItem";
            AddMemberToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            AddMemberToolStripMenuItem.Text = "添加";
            AddMemberToolStripMenuItem.Click += AddMemberToolStripMenuItem_Click;
            // 
            // DeleteMemberToolStripMenuItem
            // 
            DeleteMemberToolStripMenuItem.Name = "DeleteMemberToolStripMenuItem";
            DeleteMemberToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            DeleteMemberToolStripMenuItem.Text = "删除";
            DeleteMemberToolStripMenuItem.Click += DeleteMemberToolStripMenuItem_Click;
            // 
            // PermissionTabPage
            // 
            PermissionTabPage.Controls.Add(PermissionListView);
            PermissionTabPage.Controls.Add(TableLayoutPanel1);
            PermissionTabPage.Location = new System.Drawing.Point(8, 45);
            PermissionTabPage.Name = "PermissionTabPage";
            PermissionTabPage.Padding = new System.Windows.Forms.Padding(3);
            PermissionTabPage.Size = new System.Drawing.Size(958, 562);
            PermissionTabPage.TabIndex = 1;
            PermissionTabPage.Text = "权限";
            PermissionTabPage.UseVisualStyleBackColor = true;
            // 
            // PermissionListView
            // 
            PermissionListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            PermissionListView.ContextMenuStrip = PermissionContextMenuStrip;
            PermissionListView.Dock = System.Windows.Forms.DockStyle.Fill;
            PermissionListView.FullRowSelect = true;
            PermissionListView.GridLines = true;
            PermissionListView.Location = new System.Drawing.Point(3, 3);
            PermissionListView.MultiSelect = false;
            PermissionListView.Name = "PermissionListView";
            PermissionListView.Size = new System.Drawing.Size(952, 463);
            PermissionListView.TabIndex = 0;
            PermissionListView.UseCompatibleStateImageBehavior = false;
            PermissionListView.View = System.Windows.Forms.View.Details;
            PermissionListView.SelectedIndexChanged += PermissionListView_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "权限节点";
            columnHeader1.Width = 350;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "描述";
            columnHeader2.Width = 450;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "值";
            columnHeader3.Width = 100;
            // 
            // PermissionContextMenuStrip
            // 
            PermissionContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            PermissionContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { AddPermissionToolStripMenuItem, DeletePermissionToolStripMenuItem });
            PermissionContextMenuStrip.Name = "PermissionContextMenuStrip";
            PermissionContextMenuStrip.Size = new System.Drawing.Size(137, 80);
            PermissionContextMenuStrip.Opening += PermissionContextMenuStrip_Opening;
            // 
            // AddPermissionToolStripMenuItem
            // 
            AddPermissionToolStripMenuItem.Name = "AddPermissionToolStripMenuItem";
            AddPermissionToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            AddPermissionToolStripMenuItem.Text = "添加";
            AddPermissionToolStripMenuItem.Click += AddPermissionToolStripMenuItem_Click;
            // 
            // DeletePermissionToolStripMenuItem
            // 
            DeletePermissionToolStripMenuItem.Name = "DeletePermissionToolStripMenuItem";
            DeletePermissionToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            DeletePermissionToolStripMenuItem.Text = "删除";
            DeletePermissionToolStripMenuItem.Click += DeletePermissionToolStripMenuItem_Click;
            // 
            // TableLayoutPanel1
            // 
            TableLayoutPanel1.ColumnCount = 3;
            TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            TableLayoutPanel1.Controls.Add(PermissionComboBox, 0, 1);
            TableLayoutPanel1.Controls.Add(ValueComboBox, 2, 1);
            TableLayoutPanel1.Controls.Add(PermissionLabel, 0, 0);
            TableLayoutPanel1.Controls.Add(ValueLabel, 2, 0);
            TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            TableLayoutPanel1.Location = new System.Drawing.Point(3, 466);
            TableLayoutPanel1.Name = "TableLayoutPanel1";
            TableLayoutPanel1.RowCount = 2;
            TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanel1.Size = new System.Drawing.Size(952, 93);
            TableLayoutPanel1.TabIndex = 6;
            // 
            // PermissionComboBox
            // 
            PermissionComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            PermissionComboBox.Enabled = false;
            PermissionComboBox.FormattingEnabled = true;
            PermissionComboBox.Location = new System.Drawing.Point(3, 43);
            PermissionComboBox.Name = "PermissionComboBox";
            PermissionComboBox.Size = new System.Drawing.Size(786, 39);
            PermissionComboBox.TabIndex = 1;
            PermissionComboBox.Text = "选择一项进行修改";
            ToolTip.SetToolTip(PermissionComboBox, "支持通配符（*）");
            PermissionComboBox.DropDown += PermissionComboBox_DropDown;
            PermissionComboBox.SelectedIndexChanged += PermissionComboBox_TextUpdate;
            PermissionComboBox.TextUpdate += PermissionComboBox_TextUpdate;
            // 
            // ValueComboBox
            // 
            ValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ValueComboBox.FormattingEnabled = true;
            ValueComboBox.Items.AddRange(new object[] { "Null", "True", "False" });
            ValueComboBox.Location = new System.Drawing.Point(805, 43);
            ValueComboBox.Name = "ValueComboBox";
            ValueComboBox.Size = new System.Drawing.Size(144, 39);
            ValueComboBox.TabIndex = 5;
            ValueComboBox.SelectedIndexChanged += ValueComboBox_SelectedIndexChanged;
            // 
            // PermissionLabel
            // 
            PermissionLabel.AutoSize = true;
            PermissionLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            PermissionLabel.Location = new System.Drawing.Point(3, 9);
            PermissionLabel.Name = "PermissionLabel";
            PermissionLabel.Size = new System.Drawing.Size(786, 31);
            PermissionLabel.TabIndex = 6;
            PermissionLabel.Text = "权限节点";
            ToolTip.SetToolTip(PermissionLabel, "支持通配符（*）");
            // 
            // ValueLabel
            // 
            ValueLabel.AutoSize = true;
            ValueLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            ValueLabel.Location = new System.Drawing.Point(805, 9);
            ValueLabel.Name = "ValueLabel";
            ValueLabel.Size = new System.Drawing.Size(144, 31);
            ValueLabel.TabIndex = 7;
            ValueLabel.Text = "值";
            // 
            // ConfirmButton
            // 
            ConfirmButton.Location = new System.Drawing.Point(409, 621);
            ConfirmButton.Name = "ConfirmButton";
            ConfirmButton.Size = new System.Drawing.Size(150, 46);
            ConfirmButton.TabIndex = 4;
            ConfirmButton.Text = "确认";
            ConfirmButton.UseVisualStyleBackColor = true;
            ConfirmButton.Click += ConfirmButton_Click;
            // 
            // ErrorProvider
            // 
            ErrorProvider.ContainerControl = this;
            // 
            // PermissionGroupEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(974, 679);
            Controls.Add(ConfirmButton);
            Controls.Add(TabControl);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PermissionGroupEditor";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "权限组编辑器";
            TabControl.ResumeLayout(false);
            RegularTabPage.ResumeLayout(false);
            RegularTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PriorityNumericUpDown).EndInit();
            MemberTabPage.ResumeLayout(false);
            MemberContextMenuStrip.ResumeLayout(false);
            PermissionTabPage.ResumeLayout(false);
            PermissionContextMenuStrip.ResumeLayout(false);
            TableLayoutPanel1.ResumeLayout(false);
            TableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ErrorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.ComboBox PermissionComboBox;
        private System.Windows.Forms.ListView PermissionListView;
        private System.Windows.Forms.ComboBox ValueComboBox;
        private System.Windows.Forms.TextBox DescriptionTextBox;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.TextBox IdTextBox;
        private System.Windows.Forms.TextBox ParentsTextBox;
        private System.Windows.Forms.NumericUpDown PriorityNumericUpDown;
        private System.Windows.Forms.ToolStripMenuItem AddPermissionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeletePermissionToolStripMenuItem;
        private System.Windows.Forms.ListView MemberListView;
        private System.Windows.Forms.ToolStripMenuItem AddMemberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteMemberToolStripMenuItem;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
    }
}