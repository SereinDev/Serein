namespace Serein.Lite.Ui.Functions
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
            System.Windows.Forms.TabControl tabControl;
            System.Windows.Forms.TabPage consoleTabPage;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            System.Windows.Forms.GroupBox infoGroupBox;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
            System.Windows.Forms.Label countLabel;
            System.Windows.Forms.Label jsCountLabel;
            System.Windows.Forms.Label netCountLabel;
            System.Windows.Forms.GroupBox controlGroupBox;
            System.Windows.Forms.Button clearConsoleButton;
            System.Windows.Forms.Button reloadButton;
            System.Windows.Forms.TabPage manageTabPage;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ColumnHeader columnHeader3;
            System.Windows.Forms.ColumnHeader columnHeader4;
            System.Windows.Forms.ColumnHeader columnHeader5;
            System.Windows.Forms.ContextMenuStrip listViewContextMenuStrip;
            System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            System.Windows.Forms.ToolStripMenuItem lookUpDocsToolStripMenuItem;
            ConsoleWebBrowser = new Serein.Lite.Ui.Controls.ConsoleWebBrowser();
            _netCountDynamicLabel = new System.Windows.Forms.Label();
            _jsCountDynamicLabel = new System.Windows.Forms.Label();
            _countDynamicLabel = new System.Windows.Forms.Label();
            _pluginListView = new System.Windows.Forms.ListView();
            _disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pluginInfoLabel = new System.Windows.Forms.Label();
            tabControl = new System.Windows.Forms.TabControl();
            consoleTabPage = new System.Windows.Forms.TabPage();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            infoGroupBox = new System.Windows.Forms.GroupBox();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            countLabel = new System.Windows.Forms.Label();
            jsCountLabel = new System.Windows.Forms.Label();
            netCountLabel = new System.Windows.Forms.Label();
            controlGroupBox = new System.Windows.Forms.GroupBox();
            clearConsoleButton = new System.Windows.Forms.Button();
            reloadButton = new System.Windows.Forms.Button();
            manageTabPage = new System.Windows.Forms.TabPage();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            columnHeader5 = new System.Windows.Forms.ColumnHeader();
            listViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            lookUpDocsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tabControl.SuspendLayout();
            consoleTabPage.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            infoGroupBox.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            controlGroupBox.SuspendLayout();
            manageTabPage.SuspendLayout();
            listViewContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(consoleTabPage);
            tabControl.Controls.Add(manageTabPage);
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl.Location = new System.Drawing.Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(1280, 720);
            tabControl.TabIndex = 0;
            // 
            // consoleTabPage
            // 
            consoleTabPage.Controls.Add(tableLayoutPanel1);
            consoleTabPage.Location = new System.Drawing.Point(8, 45);
            consoleTabPage.Name = "consoleTabPage";
            consoleTabPage.Padding = new System.Windows.Forms.Padding(3);
            consoleTabPage.Size = new System.Drawing.Size(1264, 667);
            consoleTabPage.TabIndex = 3;
            consoleTabPage.Text = "控制台";
            consoleTabPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(ConsoleWebBrowser, 1, 0);
            tableLayoutPanel1.Controls.Add(infoGroupBox, 0, 0);
            tableLayoutPanel1.Controls.Add(controlGroupBox, 0, 1);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 149F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 159F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1258, 661);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // ConsoleWebBrowser
            // 
            ConsoleWebBrowser.AllowNavigation = false;
            ConsoleWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            ConsoleWebBrowser.IsWebBrowserContextMenuEnabled = false;
            ConsoleWebBrowser.Location = new System.Drawing.Point(303, 3);
            ConsoleWebBrowser.Name = "ConsoleWebBrowser";
            tableLayoutPanel1.SetRowSpan(ConsoleWebBrowser, 3);
            ConsoleWebBrowser.Size = new System.Drawing.Size(952, 655);
            ConsoleWebBrowser.TabIndex = 2;
            // 
            // infoGroupBox
            // 
            infoGroupBox.Controls.Add(tableLayoutPanel2);
            infoGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            infoGroupBox.Location = new System.Drawing.Point(3, 3);
            infoGroupBox.Name = "infoGroupBox";
            infoGroupBox.Size = new System.Drawing.Size(294, 143);
            infoGroupBox.TabIndex = 3;
            infoGroupBox.TabStop = false;
            infoGroupBox.Text = "信息";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.66667F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.3333321F));
            tableLayoutPanel2.Controls.Add(_netCountDynamicLabel, 1, 2);
            tableLayoutPanel2.Controls.Add(_jsCountDynamicLabel, 1, 1);
            tableLayoutPanel2.Controls.Add(countLabel, 0, 0);
            tableLayoutPanel2.Controls.Add(jsCountLabel, 0, 1);
            tableLayoutPanel2.Controls.Add(netCountLabel, 0, 2);
            tableLayoutPanel2.Controls.Add(_countDynamicLabel, 1, 0);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(3, 34);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Size = new System.Drawing.Size(288, 106);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // _netCountDynamicLabel
            // 
            _netCountDynamicLabel.AutoSize = true;
            _netCountDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _netCountDynamicLabel.Location = new System.Drawing.Point(123, 70);
            _netCountDynamicLabel.Name = "_netCountDynamicLabel";
            _netCountDynamicLabel.Size = new System.Drawing.Size(162, 36);
            _netCountDynamicLabel.TabIndex = 5;
            _netCountDynamicLabel.Text = "0";
            _netCountDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _jsCountDynamicLabel
            // 
            _jsCountDynamicLabel.AutoSize = true;
            _jsCountDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _jsCountDynamicLabel.Location = new System.Drawing.Point(123, 35);
            _jsCountDynamicLabel.Name = "_jsCountDynamicLabel";
            _jsCountDynamicLabel.Size = new System.Drawing.Size(162, 35);
            _jsCountDynamicLabel.TabIndex = 4;
            _jsCountDynamicLabel.Text = "0";
            _jsCountDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // countLabel
            // 
            countLabel.AutoSize = true;
            countLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            countLabel.Location = new System.Drawing.Point(3, 0);
            countLabel.Name = "countLabel";
            countLabel.Size = new System.Drawing.Size(114, 35);
            countLabel.TabIndex = 0;
            countLabel.Text = "插件总数";
            countLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // jsCountLabel
            // 
            jsCountLabel.AutoSize = true;
            jsCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            jsCountLabel.Location = new System.Drawing.Point(3, 35);
            jsCountLabel.Name = "jsCountLabel";
            jsCountLabel.Size = new System.Drawing.Size(114, 35);
            jsCountLabel.TabIndex = 1;
            jsCountLabel.Text = " · Js";
            jsCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // netCountLabel
            // 
            netCountLabel.AutoSize = true;
            netCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            netCountLabel.Location = new System.Drawing.Point(3, 70);
            netCountLabel.Name = "netCountLabel";
            netCountLabel.Size = new System.Drawing.Size(114, 36);
            netCountLabel.TabIndex = 2;
            netCountLabel.Text = " · Net";
            netCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _countDynamicLabel
            // 
            _countDynamicLabel.AutoSize = true;
            _countDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _countDynamicLabel.Location = new System.Drawing.Point(123, 0);
            _countDynamicLabel.Name = "_countDynamicLabel";
            _countDynamicLabel.Size = new System.Drawing.Size(162, 35);
            _countDynamicLabel.TabIndex = 3;
            _countDynamicLabel.Text = "0";
            _countDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // controlGroupBox
            // 
            controlGroupBox.Controls.Add(clearConsoleButton);
            controlGroupBox.Controls.Add(reloadButton);
            controlGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            controlGroupBox.Location = new System.Drawing.Point(3, 152);
            controlGroupBox.Name = "controlGroupBox";
            controlGroupBox.Size = new System.Drawing.Size(294, 153);
            controlGroupBox.TabIndex = 4;
            controlGroupBox.TabStop = false;
            controlGroupBox.Text = "控制";
            // 
            // clearConsoleButton
            // 
            clearConsoleButton.Location = new System.Drawing.Point(8, 95);
            clearConsoleButton.Margin = new System.Windows.Forms.Padding(5);
            clearConsoleButton.Name = "clearConsoleButton";
            clearConsoleButton.Size = new System.Drawing.Size(278, 46);
            clearConsoleButton.TabIndex = 1;
            clearConsoleButton.Text = "清空插件控制台";
            clearConsoleButton.UseVisualStyleBackColor = true;
            clearConsoleButton.Click += ClearConsoleButton_Click;
            // 
            // reloadButton
            // 
            reloadButton.Location = new System.Drawing.Point(8, 39);
            reloadButton.Margin = new System.Windows.Forms.Padding(5);
            reloadButton.Name = "reloadButton";
            reloadButton.Size = new System.Drawing.Size(278, 46);
            reloadButton.TabIndex = 0;
            reloadButton.Text = "重新加载所有插件";
            reloadButton.UseVisualStyleBackColor = true;
            reloadButton.Click += ReloadButton_Click;
            // 
            // manageTabPage
            // 
            manageTabPage.Controls.Add(_pluginListView);
            manageTabPage.Controls.Add(pluginInfoLabel);
            manageTabPage.Location = new System.Drawing.Point(8, 45);
            manageTabPage.Name = "manageTabPage";
            manageTabPage.Size = new System.Drawing.Size(1264, 667);
            manageTabPage.TabIndex = 1;
            manageTabPage.Text = "管理";
            manageTabPage.UseVisualStyleBackColor = true;
            // 
            // _pluginListView
            // 
            _pluginListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 });
            _pluginListView.ContextMenuStrip = listViewContextMenuStrip;
            _pluginListView.Dock = System.Windows.Forms.DockStyle.Fill;
            _pluginListView.FullRowSelect = true;
            _pluginListView.GridLines = true;
            _pluginListView.Location = new System.Drawing.Point(0, 0);
            _pluginListView.Margin = new System.Windows.Forms.Padding(0);
            _pluginListView.MultiSelect = false;
            _pluginListView.Name = "_pluginListView";
            _pluginListView.Size = new System.Drawing.Size(1264, 636);
            _pluginListView.TabIndex = 0;
            _pluginListView.UseCompatibleStateImageBehavior = false;
            _pluginListView.View = System.Windows.Forms.View.Details;
            _pluginListView.SelectedIndexChanged += PluginListView_SelectedIndexChanged;
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
            // listViewContextMenuStrip
            // 
            listViewContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            listViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _disableToolStripMenuItem, reloadToolStripMenuItem, clearToolStripMenuItem, toolStripSeparator1, lookUpDocsToolStripMenuItem });
            listViewContextMenuStrip.Name = "ListViewContextMenuStrip";
            listViewContextMenuStrip.Size = new System.Drawing.Size(301, 206);
            listViewContextMenuStrip.Opening += ListViewContextMenuStrip_Opening;
            // 
            // _disableToolStripMenuItem
            // 
            _disableToolStripMenuItem.Name = "_disableToolStripMenuItem";
            _disableToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            _disableToolStripMenuItem.Text = "禁用";
            _disableToolStripMenuItem.Click += DisableToolStripMenuItem_Click;
            // 
            // reloadToolStripMenuItem
            // 
            reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            reloadToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            reloadToolStripMenuItem.Text = "重新加载所有插件";
            reloadToolStripMenuItem.Click += ReloadToolStripMenuItem_Click;
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            clearToolStripMenuItem.Text = "清空控制台";
            clearToolStripMenuItem.Click += ClearToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(297, 6);
            // 
            // lookUpDocsToolStripMenuItem
            // 
            lookUpDocsToolStripMenuItem.Name = "lookUpDocsToolStripMenuItem";
            lookUpDocsToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            lookUpDocsToolStripMenuItem.Text = "查看文档";
            lookUpDocsToolStripMenuItem.Click += LookUpDocsToolStripMenuItem_Click;
            // 
            // pluginInfoLabel
            // 
            pluginInfoLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            pluginInfoLabel.Location = new System.Drawing.Point(0, 636);
            pluginInfoLabel.Name = "pluginInfoLabel";
            pluginInfoLabel.Size = new System.Drawing.Size(1264, 31);
            pluginInfoLabel.TabIndex = 1;
            pluginInfoLabel.Text = "\r\n";
            // 
            // PluginPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tabControl);
            Name = "PluginPage";
            Size = new System.Drawing.Size(1280, 720);
            tabControl.ResumeLayout(false);
            consoleTabPage.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            infoGroupBox.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            controlGroupBox.ResumeLayout(false);
            manageTabPage.ResumeLayout(false);
            listViewContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ListView _pluginListView;
        private System.Windows.Forms.ToolStripMenuItem _disableToolStripMenuItem;
        internal Controls.ConsoleWebBrowser ConsoleWebBrowser;
        private System.Windows.Forms.Label _countDynamicLabel;
        private System.Windows.Forms.Label _netCountDynamicLabel;
        private System.Windows.Forms.Label _jsCountDynamicLabel;
        private System.Windows.Forms.Label pluginInfoLabel;
    }
}
