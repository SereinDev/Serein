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
            System.Windows.Forms.StatusStrip statusStrip;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ContextMenuStrip listViewContextMenuStrip;
            System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginManagerForm));
            _toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            _enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _showInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _pluginListView = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            statusStrip = new System.Windows.Forms.StatusStrip();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            listViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            statusStrip.SuspendLayout();
            listViewContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _toolStripStatusLabel });
            statusStrip.Location = new System.Drawing.Point(0, 638);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(974, 41);
            statusStrip.TabIndex = 0;
            statusStrip.Text = "statusStrip1";
            // 
            // _toolStripStatusLabel
            // 
            _toolStripStatusLabel.Name = "_toolStripStatusLabel";
            _toolStripStatusLabel.Size = new System.Drawing.Size(24, 31);
            _toolStripStatusLabel.Text = "-";
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "文件大小";
            columnHeader2.Width = 200;
            // 
            // listViewContextMenuStrip
            // 
            listViewContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            listViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addToolStripMenuItem, _enableToolStripMenuItem, _disableToolStripMenuItem, _removeToolStripMenuItem, _showInExplorerToolStripMenuItem, toolStripSeparator, refreshToolStripMenuItem });
            listViewContextMenuStrip.Name = "ListViewContextMenuStrip";
            listViewContextMenuStrip.Size = new System.Drawing.Size(305, 282);
            listViewContextMenuStrip.Opening += ListViewContextMenuStrip_Opening;
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new System.Drawing.Size(304, 38);
            addToolStripMenuItem.Text = "添加";
            addToolStripMenuItem.Click += AddToolStripMenuItem_Click;
            // 
            // _enableToolStripMenuItem
            // 
            _enableToolStripMenuItem.Name = "_enableToolStripMenuItem";
            _enableToolStripMenuItem.Size = new System.Drawing.Size(304, 38);
            _enableToolStripMenuItem.Text = "启用";
            _enableToolStripMenuItem.Click += EnableToolStripMenuItem_Click;
            // 
            // _disableToolStripMenuItem
            // 
            _disableToolStripMenuItem.Name = "_disableToolStripMenuItem";
            _disableToolStripMenuItem.Size = new System.Drawing.Size(304, 38);
            _disableToolStripMenuItem.Text = "禁用";
            _disableToolStripMenuItem.Click += DisableToolStripMenuItem_Click;
            // 
            // _removeToolStripMenuItem
            // 
            _removeToolStripMenuItem.Name = "_removeToolStripMenuItem";
            _removeToolStripMenuItem.Size = new System.Drawing.Size(304, 38);
            _removeToolStripMenuItem.Text = "删除";
            _removeToolStripMenuItem.Click += RemoveToolStripMenuItem_Click;
            // 
            // _showInExplorerToolStripMenuItem
            // 
            _showInExplorerToolStripMenuItem.Name = "_showInExplorerToolStripMenuItem";
            _showInExplorerToolStripMenuItem.Size = new System.Drawing.Size(304, 38);
            _showInExplorerToolStripMenuItem.Text = "在资源管理器中显示";
            _showInExplorerToolStripMenuItem.Click += ShowInExplorerToolStripMenuItem_Click;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new System.Drawing.Size(301, 6);
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new System.Drawing.Size(304, 38);
            refreshToolStripMenuItem.Text = "刷新";
            refreshToolStripMenuItem.Click += RefreshToolStripMenuItem_Click;
            // 
            // _pluginListView
            // 
            _pluginListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2 });
            _pluginListView.ContextMenuStrip = listViewContextMenuStrip;
            _pluginListView.Dock = System.Windows.Forms.DockStyle.Fill;
            _pluginListView.FullRowSelect = true;
            _pluginListView.GridLines = true;
            _pluginListView.Location = new System.Drawing.Point(0, 0);
            _pluginListView.Name = "_pluginListView";
            _pluginListView.Size = new System.Drawing.Size(974, 638);
            _pluginListView.TabIndex = 1;
            _pluginListView.UseCompatibleStateImageBehavior = false;
            _pluginListView.View = System.Windows.Forms.View.Details;
            _pluginListView.SelectedIndexChanged += PluginListView_SelectedIndexChanged;
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
            Controls.Add(_pluginListView);
            Controls.Add(statusStrip);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MinimumSize = new System.Drawing.Size(600, 450);
            Name = "PluginManagerForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "服务器插件管理";
            DragDrop += PluginManagerForm_DragDrop;
            DragEnter += PluginManagerForm_DragEnter;
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            listViewContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel _toolStripStatusLabel;
        private System.Windows.Forms.ListView _pluginListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripMenuItem _enableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _disableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _showInExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    }
}