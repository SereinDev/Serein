namespace Serein.Lite.Ui.Members
{
    partial class PermissionGroupPage
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
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ColumnHeader columnHeader3;
            System.Windows.Forms.ColumnHeader columnHeader4;
            System.Windows.Forms.ContextMenuStrip GroupContextMenuStrip;
            System.Windows.Forms.ToolStripMenuItem AddToolStripMenuItem;
            EditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            GroupListView = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            GroupContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            AddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            GroupContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Id";
            columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "名称";
            columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "描述";
            columnHeader3.Width = 400;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "优先级";
            columnHeader4.Width = 150;
            // 
            // GroupContextMenuStrip
            // 
            GroupContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            GroupContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { AddToolStripMenuItem, EditToolStripMenuItem, DeleteToolStripMenuItem, RefreshToolStripMenuItem });
            GroupContextMenuStrip.Name = "GroupContextMenuStrip";
            GroupContextMenuStrip.Size = new System.Drawing.Size(301, 200);
            GroupContextMenuStrip.Opening += GroupContextMenuStrip_Opening;
            // 
            // AddToolStripMenuItem
            // 
            AddToolStripMenuItem.Name = "AddToolStripMenuItem";
            AddToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            AddToolStripMenuItem.Text = "添加";
            AddToolStripMenuItem.Click += AddToolStripMenuItem_Click;
            // 
            // EditToolStripMenuItem
            // 
            EditToolStripMenuItem.Name = "EditToolStripMenuItem";
            EditToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            EditToolStripMenuItem.Text = "编辑";
            EditToolStripMenuItem.Click += EditToolStripMenuItem_Click;
            // 
            // DeleteToolStripMenuItem
            // 
            DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
            DeleteToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            DeleteToolStripMenuItem.Text = "删除";
            DeleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
            // 
            // RefreshToolStripMenuItem
            // 
            RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
            RefreshToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            RefreshToolStripMenuItem.Text = "刷新";
            RefreshToolStripMenuItem.Click += RefreshToolStripMenuItem_Click;
            // 
            // GroupListView
            // 
            GroupListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
            GroupListView.ContextMenuStrip = GroupContextMenuStrip;
            GroupListView.Dock = System.Windows.Forms.DockStyle.Fill;
            GroupListView.FullRowSelect = true;
            GroupListView.GridLines = true;
            GroupListView.Location = new System.Drawing.Point(0, 0);
            GroupListView.MultiSelect = false;
            GroupListView.Name = "GroupListView";
            GroupListView.Size = new System.Drawing.Size(1280, 720);
            GroupListView.TabIndex = 0;
            GroupListView.UseCompatibleStateImageBehavior = false;
            GroupListView.View = System.Windows.Forms.View.Details;
            // 
            // PermissionGroupPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(GroupListView);
            Name = "PermissionGroupPage";
            Size = new System.Drawing.Size(1280, 720);
            GroupContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListView GroupListView;
        private System.Windows.Forms.ToolStripMenuItem EditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RefreshToolStripMenuItem;
    }
}
