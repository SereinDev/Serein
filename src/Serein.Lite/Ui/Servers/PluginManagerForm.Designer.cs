namespace Serein.Lite.Ui.Servers
{
    partial class PluginManagerForm
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
            System.Windows.Forms.StatusStrip StatusStrip;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ContextMenuStrip ListViewContextMenuStrip;
            System.Windows.Forms.ToolStripMenuItem AddToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator ToolStripSeparator;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginManagerForm));
            ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            EnableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DisableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            RemoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ShowInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            PluginListView = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            StatusStrip = new System.Windows.Forms.StatusStrip();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            ListViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            AddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            StatusStrip.SuspendLayout();
            ListViewContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // StatusStrip
            // 
            StatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripStatusLabel });
            StatusStrip.Location = new System.Drawing.Point(0, 638);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Size = new System.Drawing.Size(974, 41);
            StatusStrip.TabIndex = 0;
            StatusStrip.Text = "statusStrip1";
            // 
            // ToolStripStatusLabel
            // 
            ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            ToolStripStatusLabel.Size = new System.Drawing.Size(24, 31);
            ToolStripStatusLabel.Text = "-";
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "文件大小";
            columnHeader2.Width = 200;
            // 
            // ListViewContextMenuStrip
            // 
            ListViewContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            ListViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { AddToolStripMenuItem, EnableToolStripMenuItem, DisableToolStripMenuItem, RemoveToolStripMenuItem, ShowInExplorerToolStripMenuItem, ToolStripSeparator, RefreshToolStripMenuItem });
            ListViewContextMenuStrip.Name = "ListViewContextMenuStrip";
            ListViewContextMenuStrip.Size = new System.Drawing.Size(305, 238);
            ListViewContextMenuStrip.Opening += ListViewContextMenuStrip_Opening;
            // 
            // AddToolStripMenuItem
            // 
            AddToolStripMenuItem.Name = "AddToolStripMenuItem";
            AddToolStripMenuItem.Size = new System.Drawing.Size(304, 38);
            AddToolStripMenuItem.Text = "添加";
            AddToolStripMenuItem.Click += AddToolStripMenuItem_Click;
            // 
            // EnableToolStripMenuItem
            // 
            EnableToolStripMenuItem.Name = "EnableToolStripMenuItem";
            EnableToolStripMenuItem.Size = new System.Drawing.Size(304, 38);
            EnableToolStripMenuItem.Text = "启用";
            EnableToolStripMenuItem.Click += EnableToolStripMenuItem_Click;
            // 
            // DisableToolStripMenuItem
            // 
            DisableToolStripMenuItem.Name = "DisableToolStripMenuItem";
            DisableToolStripMenuItem.Size = new System.Drawing.Size(304, 38);
            DisableToolStripMenuItem.Text = "禁用";
            DisableToolStripMenuItem.Click += DisableToolStripMenuItem_Click;
            // 
            // RemoveToolStripMenuItem
            // 
            RemoveToolStripMenuItem.Name = "RemoveToolStripMenuItem";
            RemoveToolStripMenuItem.Size = new System.Drawing.Size(304, 38);
            RemoveToolStripMenuItem.Text = "删除";
            RemoveToolStripMenuItem.Click += RemoveToolStripMenuItem_Click;
            // 
            // ShowInExplorerToolStripMenuItem
            // 
            ShowInExplorerToolStripMenuItem.Name = "ShowInExplorerToolStripMenuItem";
            ShowInExplorerToolStripMenuItem.Size = new System.Drawing.Size(304, 38);
            ShowInExplorerToolStripMenuItem.Text = "在资源管理器中显示";
            ShowInExplorerToolStripMenuItem.Click += ShowInExplorerToolStripMenuItem_Click;
            // 
            // ToolStripSeparator
            // 
            ToolStripSeparator.Name = "ToolStripSeparator";
            ToolStripSeparator.Size = new System.Drawing.Size(301, 6);
            // 
            // RefreshToolStripMenuItem
            // 
            RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
            RefreshToolStripMenuItem.Size = new System.Drawing.Size(304, 38);
            RefreshToolStripMenuItem.Text = "刷新";
            RefreshToolStripMenuItem.Click += RefreshToolStripMenuItem_Click;
            // 
            // PluginListView
            // 
            PluginListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2 });
            PluginListView.ContextMenuStrip = ListViewContextMenuStrip;
            PluginListView.Dock = System.Windows.Forms.DockStyle.Fill;
            PluginListView.FullRowSelect = true;
            PluginListView.GridLines = true;
            PluginListView.Location = new System.Drawing.Point(0, 0);
            PluginListView.Name = "PluginListView";
            PluginListView.Size = new System.Drawing.Size(974, 638);
            PluginListView.TabIndex = 1;
            PluginListView.UseCompatibleStateImageBehavior = false;
            PluginListView.View = System.Windows.Forms.View.Details;
            PluginListView.SelectedIndexChanged += PluginListView_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "插件名称";
            columnHeader1.Width = 500;
            // 
            // PluginManagerForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(974, 679);
            Controls.Add(PluginListView);
            Controls.Add(StatusStrip);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MinimumSize = new System.Drawing.Size(600, 450);
            Name = "PluginManagerForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "服务器插件管理";
            DragDrop += PluginManagerForm_DragDrop;
            DragEnter += PluginManagerForm_DragEnter;
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            ListViewContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
        private System.Windows.Forms.ListView PluginListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripMenuItem EnableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DisableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RemoveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowInExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RefreshToolStripMenuItem;
    }
}