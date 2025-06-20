namespace Serein.Lite.Ui.Settings
{
    partial class ReactionSettingPage
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
            System.Windows.Forms.SplitContainer splitContainer;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ContextMenuStrip contextMenuStrip;
            System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
            _eventListBox = new System.Windows.Forms.ListBox();
            _commandListView = new System.Windows.Forms.ListView();
            _deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            splitContainer = new System.Windows.Forms.SplitContainer();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer.Location = new System.Drawing.Point(0, 0);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(_eventListBox);
            splitContainer.Panel1MinSize = 300;
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(_commandListView);
            splitContainer.Panel2MinSize = 300;
            splitContainer.Size = new System.Drawing.Size(1280, 720);
            splitContainer.SplitterDistance = 426;
            splitContainer.TabIndex = 0;
            // 
            // _eventListBox
            // 
            _eventListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _eventListBox.FormattingEnabled = true;
            _eventListBox.Location = new System.Drawing.Point(0, 0);
            _eventListBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            _eventListBox.Name = "_eventListBox";
            _eventListBox.Size = new System.Drawing.Size(426, 720);
            _eventListBox.TabIndex = 0;
            _eventListBox.SelectedIndexChanged += EventListBox_SelectedIndexChanged;
            // 
            // _commandListView
            // 
            _commandListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1 });
            _commandListView.ContextMenuStrip = contextMenuStrip;
            _commandListView.Dock = System.Windows.Forms.DockStyle.Fill;
            _commandListView.FullRowSelect = true;
            _commandListView.GridLines = true;
            _commandListView.LabelEdit = true;
            _commandListView.Location = new System.Drawing.Point(0, 0);
            _commandListView.MultiSelect = false;
            _commandListView.Name = "_commandListView";
            _commandListView.Size = new System.Drawing.Size(850, 720);
            _commandListView.TabIndex = 0;
            _commandListView.UseCompatibleStateImageBehavior = false;
            _commandListView.View = System.Windows.Forms.View.Details;
            _commandListView.AfterLabelEdit += CommandListView_AfterLabelEdit;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "命令";
            columnHeader1.Width = 600;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addToolStripMenuItem, _deleteToolStripMenuItem });
            contextMenuStrip.Name = "ContextMenuStrip";
            contextMenuStrip.Size = new System.Drawing.Size(137, 80);
            contextMenuStrip.Opening += ContextMenuStrip_Opening;
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            addToolStripMenuItem.Text = "添加";
            addToolStripMenuItem.Click += AddToolStripMenuItem_Click;
            // 
            // _deleteToolStripMenuItem
            // 
            _deleteToolStripMenuItem.Name = "_deleteToolStripMenuItem";
            _deleteToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            _deleteToolStripMenuItem.Text = "删除";
            _deleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
            // 
            // ReactionSettingPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(splitContainer);
            Name = "ReactionSettingPage";
            Size = new System.Drawing.Size(1280, 720);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox _eventListBox;
        private System.Windows.Forms.ListView _commandListView;
        private System.Windows.Forms.ToolStripMenuItem _deleteToolStripMenuItem;
    }
}
