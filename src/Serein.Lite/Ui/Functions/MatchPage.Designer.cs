namespace Serein.Lite.Ui.Functions
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
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ColumnHeader columnHeader3;
            System.Windows.Forms.ColumnHeader columnHeader4;
            System.Windows.Forms.ColumnHeader columnHeader5;
            System.Windows.Forms.ColumnHeader columnHeader6;
            System.Windows.Forms.StatusStrip statusStrip;
            System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem lookUpIntroDocsToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem lookUpVariablesDocsToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator;
            System.Windows.Forms.ContextMenuStrip listViewContextMenuStrip;
            _toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            _matchListView = new System.Windows.Forms.ListView();
            _editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            columnHeader5 = new System.Windows.Forms.ColumnHeader();
            columnHeader6 = new System.Windows.Forms.ColumnHeader();
            statusStrip = new System.Windows.Forms.StatusStrip();
            addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            lookUpIntroDocsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            lookUpVariablesDocsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            listViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            statusStrip.SuspendLayout();
            listViewContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "正则表达式";
            columnHeader1.Width = 300;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "匹配域";
            columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "命令";
            columnHeader3.Width = 300;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "描述";
            columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "需要管理权限";
            columnHeader5.Width = 180;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "排除参数";
            columnHeader6.Width = 150;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _toolStripStatusLabel });
            statusStrip.Location = new System.Drawing.Point(0, 682);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(1280, 38);
            statusStrip.TabIndex = 0;
            statusStrip.Text = "statusStrip1";
            // 
            // _toolStripStatusLabel
            // 
            _toolStripStatusLabel.Name = "_toolStripStatusLabel";
            _toolStripStatusLabel.Size = new System.Drawing.Size(0, 28);
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            addToolStripMenuItem.Text = "添加";
            addToolStripMenuItem.Click += AddToolStripMenuItem_Click;
            // 
            // lookUpIntroDocsToolStripMenuItem
            // 
            lookUpIntroDocsToolStripMenuItem.Name = "lookUpIntroDocsToolStripMenuItem";
            lookUpIntroDocsToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            lookUpIntroDocsToolStripMenuItem.Text = "查看介绍文档";
            lookUpIntroDocsToolStripMenuItem.Click += LookUpIntroDocsToolStripMenuItem_Click;
            // 
            // lookUpVariablesDocsToolStripMenuItem
            // 
            lookUpVariablesDocsToolStripMenuItem.Name = "lookUpVariablesDocsToolStripMenuItem";
            lookUpVariablesDocsToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            lookUpVariablesDocsToolStripMenuItem.Text = "查看变量文档";
            lookUpVariablesDocsToolStripMenuItem.Click += LookUpVariablesDocsToolStripMenuItem_Click;
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            refreshToolStripMenuItem.Text = "刷新";
            refreshToolStripMenuItem.Click += RefreshToolStripMenuItem_Click;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new System.Drawing.Size(229, 6);
            // 
            // _matchListView
            // 
            _matchListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6 });
            _matchListView.ContextMenuStrip = listViewContextMenuStrip;
            _matchListView.Dock = System.Windows.Forms.DockStyle.Fill;
            _matchListView.FullRowSelect = true;
            _matchListView.GridLines = true;
            _matchListView.Location = new System.Drawing.Point(0, 0);
            _matchListView.Name = "_matchListView";
            _matchListView.Size = new System.Drawing.Size(1280, 682);
            _matchListView.TabIndex = 1;
            _matchListView.UseCompatibleStateImageBehavior = false;
            _matchListView.View = System.Windows.Forms.View.Details;
            _matchListView.SelectedIndexChanged += MatchListView_SelectedIndexChanged;
            _matchListView.KeyDown += MatchListView_KeyDown;
            // 
            // listViewContextMenuStrip
            // 
            listViewContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            listViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addToolStripMenuItem, _editToolStripMenuItem, _deleteToolStripMenuItem, _clearToolStripMenuItem, refreshToolStripMenuItem, toolStripSeparator, lookUpIntroDocsToolStripMenuItem, lookUpVariablesDocsToolStripMenuItem });
            listViewContextMenuStrip.Name = "ContextMenuStrip";
            listViewContextMenuStrip.Size = new System.Drawing.Size(233, 276);
            listViewContextMenuStrip.Opening += ContextMenuStrip_Opening;
            // 
            // _editToolStripMenuItem
            // 
            _editToolStripMenuItem.Name = "_editToolStripMenuItem";
            _editToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            _editToolStripMenuItem.Text = "编辑";
            _editToolStripMenuItem.Click += EditToolStripMenuItem_Click;
            // 
            // _deleteToolStripMenuItem
            // 
            _deleteToolStripMenuItem.Name = "_deleteToolStripMenuItem";
            _deleteToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            _deleteToolStripMenuItem.Text = "删除";
            _deleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
            // 
            // _clearToolStripMenuItem
            // 
            _clearToolStripMenuItem.Name = "_clearToolStripMenuItem";
            _clearToolStripMenuItem.Size = new System.Drawing.Size(232, 38);
            _clearToolStripMenuItem.Text = "清空";
            _clearToolStripMenuItem.Click += ClearToolStripMenuItem_Click;
            // 
            // MatchPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_matchListView);
            Controls.Add(statusStrip);
            Name = "MatchPage";
            Size = new System.Drawing.Size(1280, 720);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            listViewContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView _matchListView;
        private System.Windows.Forms.ToolStripStatusLabel _toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem _editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _clearToolStripMenuItem;
    }
}
