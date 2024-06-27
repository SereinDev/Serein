namespace Serein.Lite.Ui.Function
{
    partial class SchedulePage
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
            System.Windows.Forms.StatusStrip StatusStrip;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ColumnHeader columnHeader3;
            System.Windows.Forms.ContextMenuStrip ListViewContextMenuStrip;
            System.Windows.Forms.ToolStripMenuItem AddToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem ImportToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem RefreshToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
            System.Windows.Forms.ToolStripMenuItem LookUpIntroDocsToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem LookUpVariablesDocsToolStripMenuItem;
            ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            EditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ClearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ScheduleListView = new System.Windows.Forms.ListView();
            StatusStrip = new System.Windows.Forms.StatusStrip();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            ListViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            AddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            LookUpIntroDocsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            LookUpVariablesDocsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            StatusStrip.SuspendLayout();
            ListViewContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // StatusStrip
            // 
            StatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripStatusLabel });
            StatusStrip.Location = new System.Drawing.Point(0, 698);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Size = new System.Drawing.Size(1280, 22);
            StatusStrip.TabIndex = 1;
            StatusStrip.Text = "statusStrip1";
            // 
            // ToolStripStatusLabel
            // 
            ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            ToolStripStatusLabel.Size = new System.Drawing.Size(0, 12);
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Cron表达式";
            columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "命令";
            columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "描述";
            columnHeader3.Width = 150;
            // 
            // ListViewContextMenuStrip
            // 
            ListViewContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            ListViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { AddToolStripMenuItem, EditToolStripMenuItem, DeleteToolStripMenuItem, ClearToolStripMenuItem, ImportToolStripMenuItem, RefreshToolStripMenuItem, ToolStripSeparator1, LookUpIntroDocsToolStripMenuItem, LookUpVariablesDocsToolStripMenuItem });
            ListViewContextMenuStrip.Name = "ContextMenuStrip";
            ListViewContextMenuStrip.Size = new System.Drawing.Size(301, 358);
            ListViewContextMenuStrip.Opening += ListViewContextMenuStrip_Opening;
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
            // ClearToolStripMenuItem
            // 
            ClearToolStripMenuItem.Name = "ClearToolStripMenuItem";
            ClearToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            ClearToolStripMenuItem.Text = "清空";
            ClearToolStripMenuItem.Click += ClearToolStripMenuItem_Click;
            // 
            // ImportToolStripMenuItem
            // 
            ImportToolStripMenuItem.Name = "ImportToolStripMenuItem";
            ImportToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            ImportToolStripMenuItem.Text = "导入...";
            ImportToolStripMenuItem.Click += ImportToolStripMenuItem_Click;
            // 
            // RefreshToolStripMenuItem
            // 
            RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
            RefreshToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            RefreshToolStripMenuItem.Text = "刷新";
            RefreshToolStripMenuItem.Click += RefreshToolStripMenuItem_Click;
            // 
            // ToolStripSeparator1
            // 
            ToolStripSeparator1.Name = "ToolStripSeparator1";
            ToolStripSeparator1.Size = new System.Drawing.Size(297, 6);
            // 
            // LookUpIntroDocsToolStripMenuItem
            // 
            LookUpIntroDocsToolStripMenuItem.Name = "LookUpIntroDocsToolStripMenuItem";
            LookUpIntroDocsToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            LookUpIntroDocsToolStripMenuItem.Text = "查看介绍文档";
            LookUpIntroDocsToolStripMenuItem.Click += LookUpIntroDocsToolStripMenuItem_Click;
            // 
            // LookUpVariablesDocsToolStripMenuItem
            // 
            LookUpVariablesDocsToolStripMenuItem.Name = "LookUpVariablesDocsToolStripMenuItem";
            LookUpVariablesDocsToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            LookUpVariablesDocsToolStripMenuItem.Text = "查看变量文档";
            LookUpVariablesDocsToolStripMenuItem.Click += LookUpVariablesDocsToolStripMenuItem_Click;
            // 
            // ScheduleListView
            // 
            ScheduleListView.CheckBoxes = true;
            ScheduleListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            ScheduleListView.ContextMenuStrip = ListViewContextMenuStrip;
            ScheduleListView.Dock = System.Windows.Forms.DockStyle.Fill;
            ScheduleListView.FullRowSelect = true;
            ScheduleListView.GridLines = true;
            ScheduleListView.Location = new System.Drawing.Point(0, 0);
            ScheduleListView.Name = "ScheduleListView";
            ScheduleListView.Size = new System.Drawing.Size(1280, 720);
            ScheduleListView.TabIndex = 0;
            ScheduleListView.UseCompatibleStateImageBehavior = false;
            ScheduleListView.View = System.Windows.Forms.View.Details;
            ScheduleListView.ItemChecked += ScheduleListView_ItemChecked;
            ScheduleListView.SelectedIndexChanged += ScheduleListView_SelectedIndexChanged;
            ScheduleListView.KeyDown += ScheduleListView_KeyDown;
            // 
            // SchedulePage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(StatusStrip);
            Controls.Add(ScheduleListView);
            Name = "SchedulePage";
            Size = new System.Drawing.Size(1280, 720);
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            ListViewContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView ScheduleListView;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem EditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearToolStripMenuItem;
    }
}
