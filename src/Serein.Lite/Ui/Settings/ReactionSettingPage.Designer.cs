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
            System.Windows.Forms.SplitContainer SplitContainer;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ContextMenuStrip ContextMenuStrip;
            System.Windows.Forms.ToolStripMenuItem AddToolStripMenuItem;
            EventListBox = new System.Windows.Forms.ListBox();
            CommandListView = new System.Windows.Forms.ListView();
            DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            SplitContainer = new System.Windows.Forms.SplitContainer();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            AddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)SplitContainer).BeginInit();
            SplitContainer.Panel1.SuspendLayout();
            SplitContainer.Panel2.SuspendLayout();
            SplitContainer.SuspendLayout();
            ContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // SplitContainer
            // 
            SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            SplitContainer.Location = new System.Drawing.Point(0, 0);
            SplitContainer.Name = "SplitContainer";
            // 
            // SplitContainer.Panel1
            // 
            SplitContainer.Panel1.Controls.Add(EventListBox);
            SplitContainer.Panel1MinSize = 300;
            // 
            // SplitContainer.Panel2
            // 
            SplitContainer.Panel2.Controls.Add(CommandListView);
            SplitContainer.Panel2MinSize = 300;
            SplitContainer.Size = new System.Drawing.Size(1280, 720);
            SplitContainer.SplitterDistance = 426;
            SplitContainer.TabIndex = 0;
            // 
            // EventListBox
            // 
            EventListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            EventListBox.FormattingEnabled = true;
            EventListBox.Location = new System.Drawing.Point(0, 0);
            EventListBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            EventListBox.Name = "EventListBox";
            EventListBox.Size = new System.Drawing.Size(426, 720);
            EventListBox.TabIndex = 0;
            EventListBox.SelectedIndexChanged += EventListBox_SelectedIndexChanged;
            // 
            // CommandListView
            // 
            CommandListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1 });
            CommandListView.ContextMenuStrip = ContextMenuStrip;
            CommandListView.Dock = System.Windows.Forms.DockStyle.Fill;
            CommandListView.FullRowSelect = true;
            CommandListView.GridLines = true;
            CommandListView.LabelEdit = true;
            CommandListView.Location = new System.Drawing.Point(0, 0);
            CommandListView.MultiSelect = false;
            CommandListView.Name = "CommandListView";
            CommandListView.Size = new System.Drawing.Size(850, 720);
            CommandListView.TabIndex = 0;
            CommandListView.UseCompatibleStateImageBehavior = false;
            CommandListView.View = System.Windows.Forms.View.Details;
            CommandListView.AfterLabelEdit += CommandListView_AfterLabelEdit;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "命令";
            columnHeader1.Width = 600;
            // 
            // ContextMenuStrip
            // 
            ContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { AddToolStripMenuItem, DeleteToolStripMenuItem });
            ContextMenuStrip.Name = "ContextMenuStrip";
            ContextMenuStrip.Size = new System.Drawing.Size(301, 124);
            ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            // 
            // AddToolStripMenuItem
            // 
            AddToolStripMenuItem.Name = "AddToolStripMenuItem";
            AddToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            AddToolStripMenuItem.Text = "添加";
            AddToolStripMenuItem.Click += AddToolStripMenuItem_Click;
            // 
            // DeleteToolStripMenuItem
            // 
            DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
            DeleteToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            DeleteToolStripMenuItem.Text = "删除";
            DeleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
            // 
            // ReactionPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(SplitContainer);
            Name = "ReactionPage";
            Size = new System.Drawing.Size(1280, 720);
            SplitContainer.Panel1.ResumeLayout(false);
            SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainer).EndInit();
            SplitContainer.ResumeLayout(false);
            ContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox EventListBox;
        private System.Windows.Forms.ListView CommandListView;
        private System.Windows.Forms.ToolStripMenuItem DeleteToolStripMenuItem;
    }
}
