namespace Serein.Lite.Ui.Function
{
    partial class MatchPage
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
            System.Windows.Forms.ColumnHeader ColumnHeader1;
            System.Windows.Forms.ColumnHeader ColumnHeader2;
            System.Windows.Forms.ColumnHeader ColumnHeader3;
            System.Windows.Forms.ColumnHeader ColumnHeader4;
            System.Windows.Forms.ColumnHeader ColumnHeader5;
            System.Windows.Forms.ColumnHeader ColumnHeader6;
            System.Windows.Forms.StatusStrip StatusStrip;
            System.Windows.Forms.ToolStripMenuItem AddToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem LookUpIntroDocsToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem LookUpVariablesDocsToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem RefreshToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator;
            ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            MatchListView = new System.Windows.Forms.ListView();
            ListViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            EditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ClearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ColumnHeader1 = new System.Windows.Forms.ColumnHeader();
            ColumnHeader2 = new System.Windows.Forms.ColumnHeader();
            ColumnHeader3 = new System.Windows.Forms.ColumnHeader();
            ColumnHeader4 = new System.Windows.Forms.ColumnHeader();
            ColumnHeader5 = new System.Windows.Forms.ColumnHeader();
            ColumnHeader6 = new System.Windows.Forms.ColumnHeader();
            StatusStrip = new System.Windows.Forms.StatusStrip();
            AddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            LookUpIntroDocsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            LookUpVariablesDocsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            StatusStrip.SuspendLayout();
            ListViewContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // ColumnHeader1
            // 
            ColumnHeader1.Text = "正则表达式";
            ColumnHeader1.Width = 300;
            // 
            // ColumnHeader2
            // 
            ColumnHeader2.Text = "匹配域";
            ColumnHeader2.Width = 150;
            // 
            // ColumnHeader3
            // 
            ColumnHeader3.Text = "命令";
            ColumnHeader3.Width = 300;
            // 
            // ColumnHeader4
            // 
            ColumnHeader4.Text = "描述";
            ColumnHeader4.Width = 100;
            // 
            // ColumnHeader5
            // 
            ColumnHeader5.Text = "需要管理权限";
            ColumnHeader5.Width = 180;
            // 
            // ColumnHeader6
            // 
            ColumnHeader6.Text = "排除参数";
            ColumnHeader6.Width = 150;
            // 
            // StatusStrip
            // 
            StatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripStatusLabel });
            StatusStrip.Location = new System.Drawing.Point(0, 698);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Size = new System.Drawing.Size(1280, 22);
            StatusStrip.TabIndex = 0;
            StatusStrip.Text = "statusStrip1";
            // 
            // ToolStripStatusLabel
            // 
            ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            ToolStripStatusLabel.Size = new System.Drawing.Size(0, 12);
            // 
            // AddToolStripMenuItem
            // 
            AddToolStripMenuItem.Name = "AddToolStripMenuItem";
            AddToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            AddToolStripMenuItem.Text = "添加";
            AddToolStripMenuItem.Click += AddToolStripMenuItem_Click;
            // 
            // LookUpIntroDocsToolStripMenuItem
            // 
            LookUpIntroDocsToolStripMenuItem.Name = "LookUpIntroDocsToolStripMenuItem";
            LookUpIntroDocsToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            LookUpIntroDocsToolStripMenuItem.Text = "查看介绍文档";
            LookUpIntroDocsToolStripMenuItem.Click += LookUpIntroDocsToolStripMenuItem_Click;
            // 
            // LookUpVariablesDocsToolStripMenuItem
            // 
            LookUpVariablesDocsToolStripMenuItem.Name = "LookUpVariablesDocsToolStripMenuItem";
            LookUpVariablesDocsToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            LookUpVariablesDocsToolStripMenuItem.Text = "查看变量文档";
            LookUpVariablesDocsToolStripMenuItem.Click += LookUpVariablesDocsToolStripMenuItem_Click;
            // 
            // RefreshToolStripMenuItem
            // 
            RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
            RefreshToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            RefreshToolStripMenuItem.Text = "刷新";
            RefreshToolStripMenuItem.Click += RefreshToolStripMenuItem_Click;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new System.Drawing.Size(229, 6);
            // 
            // MatchListView
            // 
            MatchListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { ColumnHeader1, ColumnHeader2, ColumnHeader3, ColumnHeader4, ColumnHeader5, ColumnHeader6 });
            MatchListView.ContextMenuStrip = ListViewContextMenuStrip;
            MatchListView.Dock = System.Windows.Forms.DockStyle.Fill;
            MatchListView.FullRowSelect = true;
            MatchListView.GridLines = true;
            MatchListView.Location = new System.Drawing.Point(0, 0);
            MatchListView.Name = "MatchListView";
            MatchListView.Size = new System.Drawing.Size(1280, 698);
            MatchListView.TabIndex = 1;
            MatchListView.UseCompatibleStateImageBehavior = false;
            MatchListView.View = System.Windows.Forms.View.Details;
            MatchListView.SelectedIndexChanged += MatchListView_SelectedIndexChanged;
            MatchListView.KeyDown += MatchListView_KeyDown;
            // 
            // ListViewContextMenuStrip
            // 
            ListViewContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            ListViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { AddToolStripMenuItem, EditToolStripMenuItem, DeleteToolStripMenuItem, ClearToolStripMenuItem, RefreshToolStripMenuItem, toolStripSeparator, LookUpIntroDocsToolStripMenuItem, LookUpVariablesDocsToolStripMenuItem });
            ListViewContextMenuStrip.Name = "ContextMenuStrip";
            ListViewContextMenuStrip.Size = new System.Drawing.Size(233, 314);
            ListViewContextMenuStrip.Opening += ContextMenuStrip_Opening;
            // 
            // EditToolStripMenuItem
            // 
            EditToolStripMenuItem.Name = "EditToolStripMenuItem";
            EditToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            EditToolStripMenuItem.Text = "编辑";
            EditToolStripMenuItem.Click += EditToolStripMenuItem_Click;
            // 
            // DeleteToolStripMenuItem
            // 
            DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
            DeleteToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            DeleteToolStripMenuItem.Text = "删除";
            DeleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
            // 
            // ClearToolStripMenuItem
            // 
            ClearToolStripMenuItem.Name = "ClearToolStripMenuItem";
            ClearToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            ClearToolStripMenuItem.Text = "清空";
            ClearToolStripMenuItem.Click += ClearToolStripMenuItem_Click;
            // 
            // MatchPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(MatchListView);
            Controls.Add(StatusStrip);
            Name = "MatchPage";
            Size = new System.Drawing.Size(1280, 720);
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            ListViewContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView MatchListView;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
        private System.Windows.Forms.ContextMenuStrip ListViewContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem EditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearToolStripMenuItem;
    }
}
