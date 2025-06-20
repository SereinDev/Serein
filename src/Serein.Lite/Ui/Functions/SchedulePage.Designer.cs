namespace Serein.Lite.Ui.Functions
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
            System.Windows.Forms.StatusStrip statusStrip;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ColumnHeader columnHeader3;
            System.Windows.Forms.ContextMenuStrip listViewContextMenuStrip;
            System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            System.Windows.Forms.ToolStripMenuItem lookUpIntroDocsToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem lookUpVariablesDocsToolStripMenuItem;
            _toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            _editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _scheduleListView = new System.Windows.Forms.ListView();
            statusStrip = new System.Windows.Forms.StatusStrip();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            listViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            lookUpIntroDocsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            lookUpVariablesDocsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            statusStrip.SuspendLayout();
            listViewContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _toolStripStatusLabel });
            statusStrip.Location = new System.Drawing.Point(0, 698);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(1280, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip1";
            // 
            // _toolStripStatusLabel
            // 
            _toolStripStatusLabel.Name = "_toolStripStatusLabel";
            _toolStripStatusLabel.Size = new System.Drawing.Size(0, 12);
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
            // listViewContextMenuStrip
            // 
            listViewContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            listViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addToolStripMenuItem, _editToolStripMenuItem, _deleteToolStripMenuItem, _clearToolStripMenuItem, refreshToolStripMenuItem, toolStripSeparator1, lookUpIntroDocsToolStripMenuItem, lookUpVariablesDocsToolStripMenuItem });
            listViewContextMenuStrip.Name = "ContextMenuStrip";
            listViewContextMenuStrip.Size = new System.Drawing.Size(301, 320);
            listViewContextMenuStrip.Opening += ListViewContextMenuStrip_Opening;
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            addToolStripMenuItem.Text = "添加";
            addToolStripMenuItem.Click += AddToolStripMenuItem_Click;
            // 
            // _editToolStripMenuItem
            // 
            _editToolStripMenuItem.Name = "_editToolStripMenuItem";
            _editToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            _editToolStripMenuItem.Text = "编辑";
            _editToolStripMenuItem.Click += EditToolStripMenuItem_Click;
            // 
            // _deleteToolStripMenuItem
            // 
            _deleteToolStripMenuItem.Name = "_deleteToolStripMenuItem";
            _deleteToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            _deleteToolStripMenuItem.Text = "删除";
            _deleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
            // 
            // _clearToolStripMenuItem
            // 
            _clearToolStripMenuItem.Name = "_clearToolStripMenuItem";
            _clearToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            _clearToolStripMenuItem.Text = "清空";
            _clearToolStripMenuItem.Click += ClearToolStripMenuItem_Click;
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            refreshToolStripMenuItem.Text = "刷新";
            refreshToolStripMenuItem.Click += RefreshToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(297, 6);
            // 
            // lookUpIntroDocsToolStripMenuItem
            // 
            lookUpIntroDocsToolStripMenuItem.Name = "lookUpIntroDocsToolStripMenuItem";
            lookUpIntroDocsToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            lookUpIntroDocsToolStripMenuItem.Text = "查看介绍文档";
            lookUpIntroDocsToolStripMenuItem.Click += LookUpIntroDocsToolStripMenuItem_Click;
            // 
            // lookUpVariablesDocsToolStripMenuItem
            // 
            lookUpVariablesDocsToolStripMenuItem.Name = "lookUpVariablesDocsToolStripMenuItem";
            lookUpVariablesDocsToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            lookUpVariablesDocsToolStripMenuItem.Text = "查看变量文档";
            lookUpVariablesDocsToolStripMenuItem.Click += LookUpVariablesDocsToolStripMenuItem_Click;
            // 
            // _scheduleListView
            // 
            _scheduleListView.CheckBoxes = true;
            _scheduleListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            _scheduleListView.ContextMenuStrip = listViewContextMenuStrip;
            _scheduleListView.Dock = System.Windows.Forms.DockStyle.Fill;
            _scheduleListView.FullRowSelect = true;
            _scheduleListView.GridLines = true;
            _scheduleListView.Location = new System.Drawing.Point(0, 0);
            _scheduleListView.Name = "_scheduleListView";
            _scheduleListView.Size = new System.Drawing.Size(1280, 720);
            _scheduleListView.TabIndex = 0;
            _scheduleListView.UseCompatibleStateImageBehavior = false;
            _scheduleListView.View = System.Windows.Forms.View.Details;
            _scheduleListView.ItemChecked += ScheduleListView_ItemChecked;
            _scheduleListView.SelectedIndexChanged += ScheduleListView_SelectedIndexChanged;
            _scheduleListView.KeyDown += ScheduleListView_KeyDown;
            // 
            // SchedulePage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(statusStrip);
            Controls.Add(_scheduleListView);
            Name = "SchedulePage";
            Size = new System.Drawing.Size(1280, 720);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            listViewContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView _scheduleListView;
        private System.Windows.Forms.ToolStripStatusLabel _toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem _editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _clearToolStripMenuItem;
    }
}
