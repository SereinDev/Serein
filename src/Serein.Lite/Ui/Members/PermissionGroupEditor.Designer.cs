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
            System.Windows.Forms.TabControl tabControl;
            System.Windows.Forms.TabPage regularTabPage;
            System.Windows.Forms.Label parentsLabel;
            System.Windows.Forms.Label priorityLabel;
            System.Windows.Forms.Label descriptionLabel;
            System.Windows.Forms.Label nameLabel;
            System.Windows.Forms.Label idLabel;
            System.Windows.Forms.TabPage memberTabPage;
            System.Windows.Forms.ColumnHeader columnHeader4;
            System.Windows.Forms.ContextMenuStrip memberContextMenuStrip;
            System.Windows.Forms.ToolStripMenuItem addMemberToolStripMenuItem;
            System.Windows.Forms.TabPage permissionTabPage;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ColumnHeader columnHeader3;
            System.Windows.Forms.ContextMenuStrip permissionContextMenuStrip;
            System.Windows.Forms.ToolStripMenuItem addPermissionToolStripMenuItem;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
            System.Windows.Forms.Label permissionLabel;
            System.Windows.Forms.Label valueLabel;
            System.Windows.Forms.ToolTip toolTip;
            System.Windows.Forms.Button confirmButton;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PermissionGroupEditor));
            _parentsTextBox = new System.Windows.Forms.TextBox();
            _priorityNumericUpDown = new System.Windows.Forms.NumericUpDown();
            _descriptionTextBox = new System.Windows.Forms.TextBox();
            _nameTextBox = new System.Windows.Forms.TextBox();
            _idTextBox = new System.Windows.Forms.TextBox();
            _memberListView = new System.Windows.Forms.ListView();
            _deleteMemberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _permissionListView = new System.Windows.Forms.ListView();
            _deletePermissionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _permissionComboBox = new System.Windows.Forms.ComboBox();
            _valueComboBox = new System.Windows.Forms.ComboBox();
            _errorProvider = new System.Windows.Forms.ErrorProvider(components);
            tabControl = new System.Windows.Forms.TabControl();
            regularTabPage = new System.Windows.Forms.TabPage();
            parentsLabel = new System.Windows.Forms.Label();
            priorityLabel = new System.Windows.Forms.Label();
            descriptionLabel = new System.Windows.Forms.Label();
            nameLabel = new System.Windows.Forms.Label();
            idLabel = new System.Windows.Forms.Label();
            memberTabPage = new System.Windows.Forms.TabPage();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            memberContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            addMemberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            permissionTabPage = new System.Windows.Forms.TabPage();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            permissionContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            addPermissionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            permissionLabel = new System.Windows.Forms.Label();
            valueLabel = new System.Windows.Forms.Label();
            toolTip = new System.Windows.Forms.ToolTip(components);
            confirmButton = new System.Windows.Forms.Button();
            tabControl.SuspendLayout();
            regularTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_priorityNumericUpDown).BeginInit();
            memberTabPage.SuspendLayout();
            memberContextMenuStrip.SuspendLayout();
            permissionTabPage.SuspendLayout();
            permissionContextMenuStrip.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).BeginInit();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(regularTabPage);
            tabControl.Controls.Add(memberTabPage);
            tabControl.Controls.Add(permissionTabPage);
            tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            tabControl.Location = new System.Drawing.Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(974, 615);
            tabControl.TabIndex = 0;
            // 
            // regularTabPage
            // 
            regularTabPage.Controls.Add(parentsLabel);
            regularTabPage.Controls.Add(_parentsTextBox);
            regularTabPage.Controls.Add(_priorityNumericUpDown);
            regularTabPage.Controls.Add(priorityLabel);
            regularTabPage.Controls.Add(descriptionLabel);
            regularTabPage.Controls.Add(_descriptionTextBox);
            regularTabPage.Controls.Add(nameLabel);
            regularTabPage.Controls.Add(_nameTextBox);
            regularTabPage.Controls.Add(idLabel);
            regularTabPage.Controls.Add(_idTextBox);
            regularTabPage.Location = new System.Drawing.Point(8, 45);
            regularTabPage.Name = "regularTabPage";
            regularTabPage.Padding = new System.Windows.Forms.Padding(3);
            regularTabPage.Size = new System.Drawing.Size(958, 562);
            regularTabPage.TabIndex = 0;
            regularTabPage.Text = "常规";
            regularTabPage.UseVisualStyleBackColor = true;
            // 
            // parentsLabel
            // 
            parentsLabel.AutoSize = true;
            parentsLabel.Location = new System.Drawing.Point(42, 347);
            parentsLabel.Name = "parentsLabel";
            parentsLabel.Size = new System.Drawing.Size(132, 31);
            parentsLabel.TabIndex = 9;
            parentsLabel.Text = "父权限组Id";
            toolTip.SetToolTip(parentsLabel, "要继承的父权限组的Id\r\n");
            // 
            // _parentsTextBox
            // 
            _parentsTextBox.AcceptsReturn = true;
            _parentsTextBox.Location = new System.Drawing.Point(42, 381);
            _parentsTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _parentsTextBox.Multiline = true;
            _parentsTextBox.Name = "_parentsTextBox";
            _parentsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _parentsTextBox.Size = new System.Drawing.Size(862, 168);
            _parentsTextBox.TabIndex = 10;
            toolTip.SetToolTip(_parentsTextBox, "要继承的父权限组的Id\r\n");
            _parentsTextBox.Enter += ParentsTextBox_Enter;
            _parentsTextBox.Validating += ParentsTextBox_Validating;
            // 
            // _priorityNumericUpDown
            // 
            _priorityNumericUpDown.Location = new System.Drawing.Point(42, 294);
            _priorityNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            _priorityNumericUpDown.Maximum = new decimal(new int[] { int.MinValue, 0, 0, 0 });
            _priorityNumericUpDown.Minimum = new decimal(new int[] { int.MinValue, 0, 0, int.MinValue });
            _priorityNumericUpDown.Name = "_priorityNumericUpDown";
            _priorityNumericUpDown.Size = new System.Drawing.Size(304, 38);
            _priorityNumericUpDown.TabIndex = 8;
            toolTip.SetToolTip(_priorityNumericUpDown, "决定权限的继承顺序");
            // 
            // priorityLabel
            // 
            priorityLabel.AutoSize = true;
            priorityLabel.Location = new System.Drawing.Point(42, 260);
            priorityLabel.Name = "priorityLabel";
            priorityLabel.Size = new System.Drawing.Size(86, 31);
            priorityLabel.TabIndex = 7;
            priorityLabel.Text = "优先级";
            toolTip.SetToolTip(priorityLabel, "决定权限的继承顺序");
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new System.Drawing.Point(42, 178);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new System.Drawing.Size(62, 31);
            descriptionLabel.TabIndex = 4;
            descriptionLabel.Text = "描述";
            toolTip.SetToolTip(descriptionLabel, "为权限组提供描述文本");
            // 
            // _descriptionTextBox
            // 
            _descriptionTextBox.Location = new System.Drawing.Point(42, 212);
            _descriptionTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _descriptionTextBox.Name = "_descriptionTextBox";
            _descriptionTextBox.Size = new System.Drawing.Size(862, 38);
            _descriptionTextBox.TabIndex = 5;
            toolTip.SetToolTip(_descriptionTextBox, "为权限组提供描述文本");
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(42, 96);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(62, 31);
            nameLabel.TabIndex = 2;
            nameLabel.Text = "名称";
            toolTip.SetToolTip(nameLabel, "用于区分权限组，便于管理");
            // 
            // _nameTextBox
            // 
            _nameTextBox.Location = new System.Drawing.Point(42, 130);
            _nameTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _nameTextBox.Name = "_nameTextBox";
            _nameTextBox.Size = new System.Drawing.Size(862, 38);
            _nameTextBox.TabIndex = 3;
            toolTip.SetToolTip(_nameTextBox, "用于区分权限组，便于管理");
            // 
            // idLabel
            // 
            idLabel.AutoSize = true;
            idLabel.Location = new System.Drawing.Point(42, 14);
            idLabel.Name = "idLabel";
            idLabel.Size = new System.Drawing.Size(36, 31);
            idLabel.TabIndex = 0;
            idLabel.Text = "Id";
            toolTip.SetToolTip(idLabel, "用于区分权限组（一经填写无法修改）\r\n· 长度大于或等于3\r\n· 只由数字、字母和下划线组成\r\n");
            // 
            // _idTextBox
            // 
            _idTextBox.Location = new System.Drawing.Point(42, 48);
            _idTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _idTextBox.Name = "_idTextBox";
            _idTextBox.Size = new System.Drawing.Size(862, 38);
            _idTextBox.TabIndex = 1;
            toolTip.SetToolTip(_idTextBox, "用于区分权限组（一经填写无法修改）\r\n· 长度大于或等于3\r\n· 只由数字、字母和下划线组成");
            _idTextBox.Enter += IdTextBox_Enter;
            _idTextBox.Validating += IdTextBox_Validating;
            // 
            // memberTabPage
            // 
            memberTabPage.Controls.Add(_memberListView);
            memberTabPage.Location = new System.Drawing.Point(8, 45);
            memberTabPage.Name = "memberTabPage";
            memberTabPage.Padding = new System.Windows.Forms.Padding(3);
            memberTabPage.Size = new System.Drawing.Size(958, 562);
            memberTabPage.TabIndex = 2;
            memberTabPage.Text = "成员";
            memberTabPage.UseVisualStyleBackColor = true;
            // 
            // _memberListView
            // 
            _memberListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader4 });
            _memberListView.ContextMenuStrip = memberContextMenuStrip;
            _memberListView.Dock = System.Windows.Forms.DockStyle.Fill;
            _memberListView.FullRowSelect = true;
            _memberListView.GridLines = true;
            _memberListView.LabelEdit = true;
            _memberListView.Location = new System.Drawing.Point(3, 3);
            _memberListView.Name = "_memberListView";
            _memberListView.Size = new System.Drawing.Size(952, 556);
            _memberListView.TabIndex = 0;
            _memberListView.UseCompatibleStateImageBehavior = false;
            _memberListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "用户Id";
            columnHeader4.Width = 700;
            // 
            // memberContextMenuStrip
            // 
            memberContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            memberContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addMemberToolStripMenuItem, _deleteMemberToolStripMenuItem });
            memberContextMenuStrip.Name = "PermissionContextMenuStrip";
            memberContextMenuStrip.Size = new System.Drawing.Size(137, 80);
            memberContextMenuStrip.Opening += MemberContextMenuStrip_Opening;
            // 
            // addMemberToolStripMenuItem
            // 
            addMemberToolStripMenuItem.Name = "addMemberToolStripMenuItem";
            addMemberToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            addMemberToolStripMenuItem.Text = "添加";
            addMemberToolStripMenuItem.Click += AddMemberToolStripMenuItem_Click;
            // 
            // _deleteMemberToolStripMenuItem
            // 
            _deleteMemberToolStripMenuItem.Name = "_deleteMemberToolStripMenuItem";
            _deleteMemberToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            _deleteMemberToolStripMenuItem.Text = "删除";
            _deleteMemberToolStripMenuItem.Click += DeleteMemberToolStripMenuItem_Click;
            // 
            // permissionTabPage
            // 
            permissionTabPage.Controls.Add(_permissionListView);
            permissionTabPage.Controls.Add(tableLayoutPanel);
            permissionTabPage.Location = new System.Drawing.Point(8, 45);
            permissionTabPage.Name = "permissionTabPage";
            permissionTabPage.Padding = new System.Windows.Forms.Padding(3);
            permissionTabPage.Size = new System.Drawing.Size(958, 562);
            permissionTabPage.TabIndex = 1;
            permissionTabPage.Text = "权限";
            permissionTabPage.UseVisualStyleBackColor = true;
            // 
            // _permissionListView
            // 
            _permissionListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            _permissionListView.ContextMenuStrip = permissionContextMenuStrip;
            _permissionListView.Dock = System.Windows.Forms.DockStyle.Fill;
            _permissionListView.FullRowSelect = true;
            _permissionListView.GridLines = true;
            _permissionListView.Location = new System.Drawing.Point(3, 3);
            _permissionListView.MultiSelect = false;
            _permissionListView.Name = "_permissionListView";
            _permissionListView.Size = new System.Drawing.Size(952, 463);
            _permissionListView.TabIndex = 0;
            _permissionListView.UseCompatibleStateImageBehavior = false;
            _permissionListView.View = System.Windows.Forms.View.Details;
            _permissionListView.SelectedIndexChanged += PermissionListView_SelectedIndexChanged;
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
            // permissionContextMenuStrip
            // 
            permissionContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            permissionContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addPermissionToolStripMenuItem, _deletePermissionToolStripMenuItem });
            permissionContextMenuStrip.Name = "PermissionContextMenuStrip";
            permissionContextMenuStrip.Size = new System.Drawing.Size(137, 80);
            permissionContextMenuStrip.Opening += PermissionContextMenuStrip_Opening;
            // 
            // addPermissionToolStripMenuItem
            // 
            addPermissionToolStripMenuItem.Name = "addPermissionToolStripMenuItem";
            addPermissionToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            addPermissionToolStripMenuItem.Text = "添加";
            addPermissionToolStripMenuItem.Click += AddPermissionToolStripMenuItem_Click;
            // 
            // _deletePermissionToolStripMenuItem
            // 
            _deletePermissionToolStripMenuItem.Name = "_deletePermissionToolStripMenuItem";
            _deletePermissionToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            _deletePermissionToolStripMenuItem.Text = "删除";
            _deletePermissionToolStripMenuItem.Click += DeletePermissionToolStripMenuItem_Click;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 3;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            tableLayoutPanel.Controls.Add(_permissionComboBox, 0, 1);
            tableLayoutPanel.Controls.Add(_valueComboBox, 2, 1);
            tableLayoutPanel.Controls.Add(permissionLabel, 0, 0);
            tableLayoutPanel.Controls.Add(valueLabel, 2, 0);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            tableLayoutPanel.Location = new System.Drawing.Point(3, 466);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.Size = new System.Drawing.Size(952, 93);
            tableLayoutPanel.TabIndex = 6;
            // 
            // _permissionComboBox
            // 
            _permissionComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _permissionComboBox.Enabled = false;
            _permissionComboBox.FormattingEnabled = true;
            _permissionComboBox.Location = new System.Drawing.Point(3, 43);
            _permissionComboBox.Name = "_permissionComboBox";
            _permissionComboBox.Size = new System.Drawing.Size(786, 39);
            _permissionComboBox.TabIndex = 1;
            _permissionComboBox.Text = "选择一项进行修改";
            toolTip.SetToolTip(_permissionComboBox, "支持通配符（*）");
            _permissionComboBox.DropDown += PermissionComboBox_DropDown;
            _permissionComboBox.SelectedIndexChanged += PermissionComboBox_TextUpdate;
            _permissionComboBox.TextUpdate += PermissionComboBox_TextUpdate;
            // 
            // _valueComboBox
            // 
            _valueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _valueComboBox.FormattingEnabled = true;
            _valueComboBox.Items.AddRange(new object[] { "Null", "True", "False" });
            _valueComboBox.Location = new System.Drawing.Point(805, 43);
            _valueComboBox.Name = "_valueComboBox";
            _valueComboBox.Size = new System.Drawing.Size(144, 39);
            _valueComboBox.TabIndex = 5;
            _valueComboBox.SelectedIndexChanged += ValueComboBox_SelectedIndexChanged;
            // 
            // permissionLabel
            // 
            permissionLabel.AutoSize = true;
            permissionLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            permissionLabel.Location = new System.Drawing.Point(3, 9);
            permissionLabel.Name = "permissionLabel";
            permissionLabel.Size = new System.Drawing.Size(786, 31);
            permissionLabel.TabIndex = 6;
            permissionLabel.Text = "权限节点";
            toolTip.SetToolTip(permissionLabel, "支持通配符（*）");
            // 
            // valueLabel
            // 
            valueLabel.AutoSize = true;
            valueLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            valueLabel.Location = new System.Drawing.Point(805, 9);
            valueLabel.Name = "valueLabel";
            valueLabel.Size = new System.Drawing.Size(144, 31);
            valueLabel.TabIndex = 7;
            valueLabel.Text = "值";
            // 
            // confirmButton
            // 
            confirmButton.Location = new System.Drawing.Point(409, 621);
            confirmButton.Name = "confirmButton";
            confirmButton.Size = new System.Drawing.Size(150, 46);
            confirmButton.TabIndex = 4;
            confirmButton.Text = "确认";
            confirmButton.UseVisualStyleBackColor = true;
            confirmButton.Click += ConfirmButton_Click;
            // 
            // _errorProvider
            // 
            _errorProvider.ContainerControl = this;
            // 
            // PermissionGroupEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(974, 679);
            Controls.Add(confirmButton);
            Controls.Add(tabControl);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PermissionGroupEditor";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "权限组编辑器";
            tabControl.ResumeLayout(false);
            regularTabPage.ResumeLayout(false);
            regularTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_priorityNumericUpDown).EndInit();
            memberTabPage.ResumeLayout(false);
            memberContextMenuStrip.ResumeLayout(false);
            permissionTabPage.ResumeLayout(false);
            permissionContextMenuStrip.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ComboBox _permissionComboBox;
        private System.Windows.Forms.ListView _permissionListView;
        private System.Windows.Forms.ComboBox _valueComboBox;
        private System.Windows.Forms.TextBox _descriptionTextBox;
        private System.Windows.Forms.TextBox _nameTextBox;
        private System.Windows.Forms.TextBox _idTextBox;
        private System.Windows.Forms.TextBox _parentsTextBox;
        private System.Windows.Forms.NumericUpDown _priorityNumericUpDown;
        private System.Windows.Forms.ToolStripMenuItem _deletePermissionToolStripMenuItem;
        private System.Windows.Forms.ListView _memberListView;
        private System.Windows.Forms.ToolStripMenuItem _deleteMemberToolStripMenuItem;
        private System.Windows.Forms.ErrorProvider _errorProvider;
    }
}