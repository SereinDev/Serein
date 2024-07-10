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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.TabControl TabControl;
            System.Windows.Forms.TabPage ConsoleTabPage;
            System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
            System.Windows.Forms.TableLayoutPanel TableLayoutPanel2;
            System.Windows.Forms.Label CountLabel;
            System.Windows.Forms.Label JsCountLabel;
            System.Windows.Forms.Label NetCountLabel;
            System.Windows.Forms.Button ClearConsoleButton;
            System.Windows.Forms.Button ReloadButton;
            System.Windows.Forms.TabPage ManageTabPage;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ColumnHeader columnHeader3;
            System.Windows.Forms.ColumnHeader columnHeader4;
            System.Windows.Forms.ColumnHeader columnHeader5;
            System.Windows.Forms.ContextMenuStrip ListViewContextMenuStrip;
            System.Windows.Forms.ToolStripMenuItem ReloadToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem ClearToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
            System.Windows.Forms.ToolStripMenuItem LookUpDocsToolStripMenuItem;
            System.Windows.Forms.StatusStrip ManageStatusStrip;
            System.Windows.Forms.TabPage MarketTabPage;
            ConsoleWebBrowser = new Controls.ConsoleWebBrowser();
            InfoGroupBox = new System.Windows.Forms.GroupBox();
            NetCountDynamicLabel = new System.Windows.Forms.Label();
            JsCountDynamicLabel = new System.Windows.Forms.Label();
            CountDynamicLabel = new System.Windows.Forms.Label();
            ControlGroupBox = new System.Windows.Forms.GroupBox();
            PluginListView = new System.Windows.Forms.ListView();
            DisableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            TabControl = new System.Windows.Forms.TabControl();
            ConsoleTabPage = new System.Windows.Forms.TabPage();
            TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            TableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            CountLabel = new System.Windows.Forms.Label();
            JsCountLabel = new System.Windows.Forms.Label();
            NetCountLabel = new System.Windows.Forms.Label();
            ClearConsoleButton = new System.Windows.Forms.Button();
            ReloadButton = new System.Windows.Forms.Button();
            ManageTabPage = new System.Windows.Forms.TabPage();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            columnHeader5 = new System.Windows.Forms.ColumnHeader();
            ListViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            ReloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ClearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            LookUpDocsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ManageStatusStrip = new System.Windows.Forms.StatusStrip();
            MarketTabPage = new System.Windows.Forms.TabPage();
            TabControl.SuspendLayout();
            ConsoleTabPage.SuspendLayout();
            TableLayoutPanel1.SuspendLayout();
            InfoGroupBox.SuspendLayout();
            TableLayoutPanel2.SuspendLayout();
            ControlGroupBox.SuspendLayout();
            ManageTabPage.SuspendLayout();
            ListViewContextMenuStrip.SuspendLayout();
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
            ConsoleTabPage.Controls.Add(TableLayoutPanel1);
            ConsoleTabPage.Location = new System.Drawing.Point(8, 45);
            ConsoleTabPage.Name = "ConsoleTabPage";
            ConsoleTabPage.Padding = new System.Windows.Forms.Padding(3);
            ConsoleTabPage.Size = new System.Drawing.Size(1264, 667);
            ConsoleTabPage.TabIndex = 3;
            ConsoleTabPage.Text = "控制台";
            ConsoleTabPage.UseVisualStyleBackColor = true;
            // 
            // TableLayoutPanel1
            // 
            TableLayoutPanel1.ColumnCount = 2;
            TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanel1.Controls.Add(ConsoleWebBrowser, 1, 0);
            TableLayoutPanel1.Controls.Add(InfoGroupBox, 0, 0);
            TableLayoutPanel1.Controls.Add(ControlGroupBox, 0, 1);
            TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            TableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            TableLayoutPanel1.Name = "TableLayoutPanel1";
            TableLayoutPanel1.RowCount = 3;
            TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 149F));
            TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 159F));
            TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanel1.Size = new System.Drawing.Size(1258, 661);
            TableLayoutPanel1.TabIndex = 0;
            // 
            // ConsoleWebBrowser
            // 
            ConsoleWebBrowser.AllowNavigation = false;
            ConsoleWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            ConsoleWebBrowser.IsWebBrowserContextMenuEnabled = false;
            ConsoleWebBrowser.Location = new System.Drawing.Point(303, 3);
            ConsoleWebBrowser.Name = "ConsoleWebBrowser";
            TableLayoutPanel1.SetRowSpan(ConsoleWebBrowser, 3);
            ConsoleWebBrowser.Size = new System.Drawing.Size(952, 655);
            ConsoleWebBrowser.TabIndex = 2;
            // 
            // InfoGroupBox
            // 
            InfoGroupBox.Controls.Add(TableLayoutPanel2);
            InfoGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            InfoGroupBox.Location = new System.Drawing.Point(3, 3);
            InfoGroupBox.Name = "InfoGroupBox";
            InfoGroupBox.Size = new System.Drawing.Size(294, 143);
            InfoGroupBox.TabIndex = 3;
            InfoGroupBox.TabStop = false;
            InfoGroupBox.Text = "信息";
            // 
            // TableLayoutPanel2
            // 
            TableLayoutPanel2.ColumnCount = 2;
            TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.66667F));
            TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.3333321F));
            TableLayoutPanel2.Controls.Add(NetCountDynamicLabel, 1, 2);
            TableLayoutPanel2.Controls.Add(JsCountDynamicLabel, 1, 1);
            TableLayoutPanel2.Controls.Add(CountLabel, 0, 0);
            TableLayoutPanel2.Controls.Add(JsCountLabel, 0, 1);
            TableLayoutPanel2.Controls.Add(NetCountLabel, 0, 2);
            TableLayoutPanel2.Controls.Add(CountDynamicLabel, 1, 0);
            TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            TableLayoutPanel2.Location = new System.Drawing.Point(3, 34);
            TableLayoutPanel2.Name = "TableLayoutPanel2";
            TableLayoutPanel2.RowCount = 3;
            TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333321F));
            TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333321F));
            TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333321F));
            TableLayoutPanel2.Size = new System.Drawing.Size(288, 106);
            TableLayoutPanel2.TabIndex = 0;
            // 
            // NetCountDynamicLabel
            // 
            NetCountDynamicLabel.AutoSize = true;
            NetCountDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            NetCountDynamicLabel.Location = new System.Drawing.Point(123, 70);
            NetCountDynamicLabel.Name = "NetCountDynamicLabel";
            NetCountDynamicLabel.Size = new System.Drawing.Size(162, 36);
            NetCountDynamicLabel.TabIndex = 5;
            NetCountDynamicLabel.Text = "0";
            NetCountDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // JsCountDynamicLabel
            // 
            JsCountDynamicLabel.AutoSize = true;
            JsCountDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            JsCountDynamicLabel.Location = new System.Drawing.Point(123, 35);
            JsCountDynamicLabel.Name = "JsCountDynamicLabel";
            JsCountDynamicLabel.Size = new System.Drawing.Size(162, 35);
            JsCountDynamicLabel.TabIndex = 4;
            JsCountDynamicLabel.Text = "0";
            JsCountDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CountLabel
            // 
            CountLabel.AutoSize = true;
            CountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            CountLabel.Location = new System.Drawing.Point(3, 0);
            CountLabel.Name = "CountLabel";
            CountLabel.Size = new System.Drawing.Size(114, 35);
            CountLabel.TabIndex = 0;
            CountLabel.Text = "插件总数";
            CountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // JsCountLabel
            // 
            JsCountLabel.AutoSize = true;
            JsCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            JsCountLabel.Location = new System.Drawing.Point(3, 35);
            JsCountLabel.Name = "JsCountLabel";
            JsCountLabel.Size = new System.Drawing.Size(114, 35);
            JsCountLabel.TabIndex = 1;
            JsCountLabel.Text = " · Js";
            JsCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NetCountLabel
            // 
            NetCountLabel.AutoSize = true;
            NetCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            NetCountLabel.Location = new System.Drawing.Point(3, 70);
            NetCountLabel.Name = "NetCountLabel";
            NetCountLabel.Size = new System.Drawing.Size(114, 36);
            NetCountLabel.TabIndex = 2;
            NetCountLabel.Text = " · Net";
            NetCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CountDynamicLabel
            // 
            CountDynamicLabel.AutoSize = true;
            CountDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            CountDynamicLabel.Location = new System.Drawing.Point(123, 0);
            CountDynamicLabel.Name = "CountDynamicLabel";
            CountDynamicLabel.Size = new System.Drawing.Size(162, 35);
            CountDynamicLabel.TabIndex = 3;
            CountDynamicLabel.Text = "0";
            CountDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ControlGroupBox
            // 
            ControlGroupBox.Controls.Add(ClearConsoleButton);
            ControlGroupBox.Controls.Add(ReloadButton);
            ControlGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            ControlGroupBox.Location = new System.Drawing.Point(3, 152);
            ControlGroupBox.Name = "ControlGroupBox";
            ControlGroupBox.Size = new System.Drawing.Size(294, 153);
            ControlGroupBox.TabIndex = 4;
            ControlGroupBox.TabStop = false;
            ControlGroupBox.Text = "控制";
            // 
            // ClearConsoleButton
            // 
            ClearConsoleButton.Location = new System.Drawing.Point(8, 95);
            ClearConsoleButton.Margin = new System.Windows.Forms.Padding(5);
            ClearConsoleButton.Name = "ClearConsoleButton";
            ClearConsoleButton.Size = new System.Drawing.Size(278, 46);
            ClearConsoleButton.TabIndex = 1;
            ClearConsoleButton.Text = "清空插件控制台";
            ClearConsoleButton.UseVisualStyleBackColor = true;
            ClearConsoleButton.Click += ClearConsoleButton_Click;
            // 
            // ReloadButton
            // 
            ReloadButton.Location = new System.Drawing.Point(8, 39);
            ReloadButton.Margin = new System.Windows.Forms.Padding(5);
            ReloadButton.Name = "ReloadButton";
            ReloadButton.Size = new System.Drawing.Size(278, 46);
            ReloadButton.TabIndex = 0;
            ReloadButton.Text = "重新加载所有插件";
            ReloadButton.UseVisualStyleBackColor = true;
            ReloadButton.Click += ReloadButton_Click;
            // 
            // ManageTabPage
            // 
            ManageTabPage.Controls.Add(PluginListView);
            ManageTabPage.Controls.Add(ManageStatusStrip);
            ManageTabPage.Location = new System.Drawing.Point(8, 45);
            ManageTabPage.Name = "ManageTabPage";
            ManageTabPage.Size = new System.Drawing.Size(1264, 667);
            ManageTabPage.TabIndex = 1;
            ManageTabPage.Text = "管理";
            ManageTabPage.UseVisualStyleBackColor = true;
            // 
            // PluginListView
            // 
            PluginListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 });
            PluginListView.ContextMenuStrip = ListViewContextMenuStrip;
            PluginListView.Dock = System.Windows.Forms.DockStyle.Fill;
            PluginListView.FullRowSelect = true;
            PluginListView.GridLines = true;
            PluginListView.Location = new System.Drawing.Point(0, 0);
            PluginListView.Margin = new System.Windows.Forms.Padding(0);
            PluginListView.MultiSelect = false;
            PluginListView.Name = "PluginListView";
            PluginListView.Size = new System.Drawing.Size(1264, 626);
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
            // ListViewContextMenuStrip
            // 
            ListViewContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            ListViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { DisableToolStripMenuItem, ReloadToolStripMenuItem, ClearToolStripMenuItem, ToolStripSeparator1, LookUpDocsToolStripMenuItem });
            ListViewContextMenuStrip.Name = "ListViewContextMenuStrip";
            ListViewContextMenuStrip.Size = new System.Drawing.Size(281, 162);
            ListViewContextMenuStrip.Opening += ListViewContextMenuStrip_Opening;
            // 
            // DisableToolStripMenuItem
            // 
            DisableToolStripMenuItem.Name = "DisableToolStripMenuItem";
            DisableToolStripMenuItem.Size = new System.Drawing.Size(280, 38);
            DisableToolStripMenuItem.Text = "禁用";
            DisableToolStripMenuItem.Click += DisableToolStripMenuItem_Click;
            // 
            // ReloadToolStripMenuItem
            // 
            ReloadToolStripMenuItem.Name = "ReloadToolStripMenuItem";
            ReloadToolStripMenuItem.Size = new System.Drawing.Size(280, 38);
            ReloadToolStripMenuItem.Text = "重新加载所有插件";
            ReloadToolStripMenuItem.Click += ReloadToolStripMenuItem_Click;
            // 
            // ClearToolStripMenuItem
            // 
            ClearToolStripMenuItem.Name = "ClearToolStripMenuItem";
            ClearToolStripMenuItem.Size = new System.Drawing.Size(280, 38);
            ClearToolStripMenuItem.Text = "清空控制台";
            ClearToolStripMenuItem.Click += ClearToolStripMenuItem_Click;
            // 
            // ToolStripSeparator1
            // 
            ToolStripSeparator1.Name = "ToolStripSeparator1";
            ToolStripSeparator1.Size = new System.Drawing.Size(277, 6);
            // 
            // LookUpDocsToolStripMenuItem
            // 
            LookUpDocsToolStripMenuItem.Name = "LookUpDocsToolStripMenuItem";
            LookUpDocsToolStripMenuItem.Size = new System.Drawing.Size(280, 38);
            LookUpDocsToolStripMenuItem.Text = "查看文档";
            LookUpDocsToolStripMenuItem.Click += LookUpDocsToolStripMenuItem_Click;
            // 
            // ManageStatusStrip
            // 
            ManageStatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            ManageStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripStatusLabel });
            ManageStatusStrip.Location = new System.Drawing.Point(0, 626);
            ManageStatusStrip.Name = "ManageStatusStrip";
            ManageStatusStrip.Size = new System.Drawing.Size(1264, 41);
            ManageStatusStrip.TabIndex = 1;
            ManageStatusStrip.Text = "statusStrip1";
            // 
            // ToolStripStatusLabel
            // 
            ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            ToolStripStatusLabel.Size = new System.Drawing.Size(24, 31);
            ToolStripStatusLabel.Text = "-";
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
            // PluginPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(TabControl);
            Name = "PluginPage";
            Size = new System.Drawing.Size(1280, 720);
            TabControl.ResumeLayout(false);
            ConsoleTabPage.ResumeLayout(false);
            TableLayoutPanel1.ResumeLayout(false);
            InfoGroupBox.ResumeLayout(false);
            TableLayoutPanel2.ResumeLayout(false);
            TableLayoutPanel2.PerformLayout();
            ControlGroupBox.ResumeLayout(false);
            ManageTabPage.ResumeLayout(false);
            ManageTabPage.PerformLayout();
            ListViewContextMenuStrip.ResumeLayout(false);
            ManageStatusStrip.ResumeLayout(false);
            ManageStatusStrip.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ListView PluginListView;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem DisableToolStripMenuItem;
        internal Controls.ConsoleWebBrowser ConsoleWebBrowser;
        private System.Windows.Forms.GroupBox InfoGroupBox;
        private System.Windows.Forms.Label CountDynamicLabel;
        private System.Windows.Forms.Label NetCountDynamicLabel;
        private System.Windows.Forms.Label JsCountDynamicLabel;
        private System.Windows.Forms.GroupBox ControlGroupBox;
    }
}
