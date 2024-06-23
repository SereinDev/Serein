namespace Serein.Lite.Ui.Function
{
    partial class ConnectionPage
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
            System.Windows.Forms.TableLayoutPanel TableLayoutPanel;
            System.Windows.Forms.GroupBox InfoGroupBox;
            ControlGroupBox = new System.Windows.Forms.GroupBox();
            CloseButton = new System.Windows.Forms.Button();
            OpenButton = new System.Windows.Forms.Button();
            ConsoleWebBrowser = new Controls.ConsoleWebBrowser();
            TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            InfoGroupBox = new System.Windows.Forms.GroupBox();
            TableLayoutPanel.SuspendLayout();
            ControlGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // TableLayoutPanel
            // 
            TableLayoutPanel.ColumnCount = 2;
            TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanel.Controls.Add(InfoGroupBox, 0, 0);
            TableLayoutPanel.Controls.Add(ControlGroupBox, 0, 1);
            TableLayoutPanel.Controls.Add(ConsoleWebBrowser, 1, 0);
            TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            TableLayoutPanel.Name = "TableLayoutPanel";
            TableLayoutPanel.RowCount = 3;
            TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 191F));
            TableLayoutPanel.Size = new System.Drawing.Size(1280, 720);
            TableLayoutPanel.TabIndex = 0;
            // 
            // InfoGroupBox
            // 
            InfoGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            InfoGroupBox.Location = new System.Drawing.Point(3, 3);
            InfoGroupBox.Name = "InfoGroupBox";
            InfoGroupBox.Size = new System.Drawing.Size(294, 413);
            InfoGroupBox.TabIndex = 0;
            InfoGroupBox.TabStop = false;
            InfoGroupBox.Text = "信息";
            // 
            // ControlGroupBox
            // 
            ControlGroupBox.Controls.Add(CloseButton);
            ControlGroupBox.Controls.Add(OpenButton);
            ControlGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            ControlGroupBox.Location = new System.Drawing.Point(3, 422);
            ControlGroupBox.Name = "ControlGroupBox";
            ControlGroupBox.Size = new System.Drawing.Size(294, 104);
            ControlGroupBox.TabIndex = 1;
            ControlGroupBox.TabStop = false;
            ControlGroupBox.Text = "控制";
            // 
            // CloseButton
            // 
            CloseButton.Location = new System.Drawing.Point(161, 37);
            CloseButton.Margin = new System.Windows.Forms.Padding(10);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new System.Drawing.Size(120, 48);
            CloseButton.TabIndex = 1;
            CloseButton.Text = "断开";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // OpenButton
            // 
            OpenButton.Location = new System.Drawing.Point(13, 37);
            OpenButton.Margin = new System.Windows.Forms.Padding(10);
            OpenButton.Name = "OpenButton";
            OpenButton.Size = new System.Drawing.Size(120, 48);
            OpenButton.TabIndex = 0;
            OpenButton.Text = "连接";
            OpenButton.UseVisualStyleBackColor = true;
            OpenButton.Click += OpenButton_Click;
            // 
            // ConsoleWebBrowser
            // 
            ConsoleWebBrowser.AllowNavigation = false;
            ConsoleWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            ConsoleWebBrowser.IsWebBrowserContextMenuEnabled = false;
            ConsoleWebBrowser.Location = new System.Drawing.Point(303, 3);
            ConsoleWebBrowser.Name = "ConsoleWebBrowser";
            TableLayoutPanel.SetRowSpan(ConsoleWebBrowser, 3);
            ConsoleWebBrowser.Size = new System.Drawing.Size(974, 714);
            ConsoleWebBrowser.TabIndex = 2;
            // 
            // ConnectionPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(TableLayoutPanel);
            Name = "ConnectionPage";
            Size = new System.Drawing.Size(1280, 720);
            TableLayoutPanel.ResumeLayout(false);
            ControlGroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox ControlGroupBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button OpenButton;
        public Controls.ConsoleWebBrowser ConsoleWebBrowser;
    }
}
