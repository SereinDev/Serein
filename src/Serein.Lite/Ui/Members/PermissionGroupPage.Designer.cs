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
            System.Windows.Forms.ContextMenuStrip groupContextMenuStrip;
            System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
            _editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _groupListView = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            groupContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            groupContextMenuStrip.SuspendLayout();
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
            // groupContextMenuStrip
            // 
            groupContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            groupContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addToolStripMenuItem, _editToolStripMenuItem, _deleteToolStripMenuItem, refreshToolStripMenuItem });
            groupContextMenuStrip.Name = "GroupContextMenuStrip";
            groupContextMenuStrip.Size = new System.Drawing.Size(137, 156);
            groupContextMenuStrip.Opening += GroupContextMenuStrip_Opening;
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            addToolStripMenuItem.Text = "添加";
            addToolStripMenuItem.Click += AddToolStripMenuItem_Click;
            // 
            // _editToolStripMenuItem
            // 
            _editToolStripMenuItem.Name = "_editToolStripMenuItem";
            _editToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            _editToolStripMenuItem.Text = "编辑";
            _editToolStripMenuItem.Click += EditToolStripMenuItem_Click;
            // 
            // _deleteToolStripMenuItem
            // 
            _deleteToolStripMenuItem.Name = "_deleteToolStripMenuItem";
            _deleteToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            _deleteToolStripMenuItem.Text = "删除";
            _deleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            refreshToolStripMenuItem.Text = "刷新";
            refreshToolStripMenuItem.Click += RefreshToolStripMenuItem_Click;
            // 
            // _groupListView
            // 
            _groupListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
            _groupListView.ContextMenuStrip = groupContextMenuStrip;
            _groupListView.Dock = System.Windows.Forms.DockStyle.Fill;
            _groupListView.FullRowSelect = true;
            _groupListView.GridLines = true;
            _groupListView.Location = new System.Drawing.Point(0, 0);
            _groupListView.MultiSelect = false;
            _groupListView.Name = "_groupListView";
            _groupListView.Size = new System.Drawing.Size(1280, 720);
            _groupListView.TabIndex = 0;
            _groupListView.UseCompatibleStateImageBehavior = false;
            _groupListView.View = System.Windows.Forms.View.Details;
            // 
            // PermissionGroupPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(_groupListView);
            Name = "PermissionGroupPage";
            Size = new System.Drawing.Size(1280, 720);
            groupContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListView _groupListView;
        private System.Windows.Forms.ToolStripMenuItem _editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _deleteToolStripMenuItem;
    }
}
