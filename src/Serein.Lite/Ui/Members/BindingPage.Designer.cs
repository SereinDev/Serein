namespace Serein.Lite.Ui.Members
{
    partial class BindingPage
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
            System.Windows.Forms.StatusStrip statusStrip;
            System.Windows.Forms.ContextMenuStrip contextMenuStrip;
            System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
            _toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            _bindingListView = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            statusStrip = new System.Windows.Forms.StatusStrip();
            contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            statusStrip.SuspendLayout();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "用户Id";
            columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "用户名";
            columnHeader2.Width = 300;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "绑定Id";
            columnHeader3.Width = 400;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _toolStripStatusLabel });
            statusStrip.Location = new System.Drawing.Point(0, 679);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(1200, 41);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip1";
            // 
            // _toolStripStatusLabel
            // 
            _toolStripStatusLabel.Name = "_toolStripStatusLabel";
            _toolStripStatusLabel.Size = new System.Drawing.Size(24, 31);
            _toolStripStatusLabel.Text = "-";
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { refreshToolStripMenuItem });
            contextMenuStrip.Name = "ContextMenuStrip";
            contextMenuStrip.Size = new System.Drawing.Size(137, 42);
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            refreshToolStripMenuItem.Text = "刷新";
            refreshToolStripMenuItem.Click += RefreshToolStripMenuItem_Click;
            // 
            // _bindingListView
            // 
            _bindingListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            _bindingListView.ContextMenuStrip = contextMenuStrip;
            _bindingListView.Dock = System.Windows.Forms.DockStyle.Fill;
            _bindingListView.FullRowSelect = true;
            _bindingListView.GridLines = true;
            _bindingListView.Location = new System.Drawing.Point(0, 0);
            _bindingListView.MultiSelect = false;
            _bindingListView.Name = "_bindingListView";
            _bindingListView.Size = new System.Drawing.Size(1200, 720);
            _bindingListView.TabIndex = 0;
            _bindingListView.UseCompatibleStateImageBehavior = false;
            _bindingListView.View = System.Windows.Forms.View.Details;
            _bindingListView.SelectedIndexChanged += BindingListView_SelectedIndexChanged;
            // 
            // BindingPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(statusStrip);
            Controls.Add(_bindingListView);
            Name = "BindingPage";
            Size = new System.Drawing.Size(1200, 720);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView _bindingListView;
        private System.Windows.Forms.ToolStripStatusLabel _toolStripStatusLabel;
    }
}
