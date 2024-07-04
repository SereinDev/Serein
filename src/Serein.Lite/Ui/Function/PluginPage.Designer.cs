namespace Serein.Lite.Ui.Function
{
    partial class PluginPage
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
            System.Windows.Forms.TabControl TabControl;
            System.Windows.Forms.TabPage ConsoleTabPage;
            System.Windows.Forms.TabPage ManageTabPage;
            System.Windows.Forms.TabPage MarketTabPage;
            System.Windows.Forms.StatusStrip ManageStatusStrip;
            ConsoleWebBrowser = new Controls.ConsoleWebBrowser();
            PluginListView = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            columnHeader5 = new System.Windows.Forms.ColumnHeader();
            ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            TabControl = new System.Windows.Forms.TabControl();
            ConsoleTabPage = new System.Windows.Forms.TabPage();
            ManageTabPage = new System.Windows.Forms.TabPage();
            MarketTabPage = new System.Windows.Forms.TabPage();
            ManageStatusStrip = new System.Windows.Forms.StatusStrip();
            TabControl.SuspendLayout();
            ConsoleTabPage.SuspendLayout();
            ManageTabPage.SuspendLayout();
            ManageStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // TabControl
            // 
            TabControl.Controls.Add(ConsoleTabPage);
            TabControl.Controls.Add(ManageTabPage);
            TabControl.Controls.Add(MarketTabPage);
            TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            TabControl.Location = new System.Drawing.Point(0, 0);
            TabControl.Name = "TabControl";
            TabControl.SelectedIndex = 0;
            TabControl.Size = new System.Drawing.Size(1280, 720);
            TabControl.TabIndex = 0;
            // 
            // ConsoleTabPage
            // 
            ConsoleTabPage.Controls.Add(ConsoleWebBrowser);
            ConsoleTabPage.Location = new System.Drawing.Point(8, 45);
            ConsoleTabPage.Name = "ConsoleTabPage";
            ConsoleTabPage.Padding = new System.Windows.Forms.Padding(3);
            ConsoleTabPage.Size = new System.Drawing.Size(1264, 667);
            ConsoleTabPage.TabIndex = 0;
            ConsoleTabPage.Text = "控制台";
            ConsoleTabPage.UseVisualStyleBackColor = true;
            // 
            // ConsoleWebBrowser
            // 
            ConsoleWebBrowser.AllowNavigation = false;
            ConsoleWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            ConsoleWebBrowser.Location = new System.Drawing.Point(3, 3);
            ConsoleWebBrowser.Name = "ConsoleWebBrowser";
            ConsoleWebBrowser.Size = new System.Drawing.Size(1258, 661);
            ConsoleWebBrowser.TabIndex = 0;
            // 
            // ManageTabPage
            // 
            ManageTabPage.Controls.Add(ManageStatusStrip);
            ManageTabPage.Controls.Add(PluginListView);
            ManageTabPage.Location = new System.Drawing.Point(8, 45);
            ManageTabPage.Name = "ManageTabPage";
            ManageTabPage.Padding = new System.Windows.Forms.Padding(3);
            ManageTabPage.Size = new System.Drawing.Size(1264, 667);
            ManageTabPage.TabIndex = 1;
            ManageTabPage.Text = "管理";
            ManageTabPage.UseVisualStyleBackColor = true;
            // 
            // MarketTabPage
            // 
            MarketTabPage.Location = new System.Drawing.Point(8, 45);
            MarketTabPage.Name = "MarketTabPage";
            MarketTabPage.Padding = new System.Windows.Forms.Padding(3);
            MarketTabPage.Size = new System.Drawing.Size(1264, 667);
            MarketTabPage.TabIndex = 2;
            MarketTabPage.Text = "市场";
            MarketTabPage.UseVisualStyleBackColor = true;
            // 
            // PluginListView
            // 
            PluginListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 });
            PluginListView.Dock = System.Windows.Forms.DockStyle.Fill;
            PluginListView.FullRowSelect = true;
            PluginListView.GridLines = true;
            PluginListView.Location = new System.Drawing.Point(3, 3);
            PluginListView.MultiSelect = false;
            PluginListView.Name = "PluginListView";
            PluginListView.Size = new System.Drawing.Size(1258, 661);
            PluginListView.TabIndex = 0;
            PluginListView.UseCompatibleStateImageBehavior = false;
            PluginListView.View = System.Windows.Forms.View.Details;
            PluginListView.SelectedIndexChanged += PluginListView_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "名称";
            columnHeader1.Width = 300;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "状态";
            columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "版本";
            columnHeader3.Width = 150;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "作者";
            columnHeader4.Width = 200;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "描述";
            columnHeader5.Width = 400;
            // 
            // ManageStatusStrip
            // 
            ManageStatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            ManageStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripStatusLabel });
            ManageStatusStrip.Location = new System.Drawing.Point(3, 642);
            ManageStatusStrip.Name = "ManageStatusStrip";
            ManageStatusStrip.Size = new System.Drawing.Size(1258, 22);
            ManageStatusStrip.TabIndex = 1;
            ManageStatusStrip.Text = "statusStrip1";
            // 
            // ToolStripStatusLabel
            // 
            ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            ToolStripStatusLabel.Size = new System.Drawing.Size(0, 28);
            // 
            // PluginPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(TabControl);
            Name = "PluginPage";
            Size = new System.Drawing.Size(1280, 720);
            TabControl.ResumeLayout(false);
            ConsoleTabPage.ResumeLayout(false);
            ManageTabPage.ResumeLayout(false);
            ManageTabPage.PerformLayout();
            ManageStatusStrip.ResumeLayout(false);
            ManageStatusStrip.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        internal Controls.ConsoleWebBrowser ConsoleWebBrowser;
        private System.Windows.Forms.ListView PluginListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
    }
}
