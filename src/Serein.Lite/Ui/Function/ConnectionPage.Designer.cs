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
            System.Windows.Forms.TableLayoutPanel InfoTableLayoutPanel;
            System.Windows.Forms.Label StatusLabel;
            System.Windows.Forms.Label RecvCountLabel;
            System.Windows.Forms.Label SentCountLabel;
            System.Windows.Forms.Label TimeLabel;
            TimeDynamicLabel = new System.Windows.Forms.Label();
            SentCountDynamicLabel = new System.Windows.Forms.Label();
            StatusDynamicLabel = new System.Windows.Forms.Label();
            RecvCountDynamicLabel = new System.Windows.Forms.Label();
            ControlGroupBox = new System.Windows.Forms.GroupBox();
            CloseButton = new System.Windows.Forms.Button();
            OpenButton = new System.Windows.Forms.Button();
            ConsoleWebBrowser = new Controls.ConsoleWebBrowser();
            TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            InfoGroupBox = new System.Windows.Forms.GroupBox();
            InfoTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            StatusLabel = new System.Windows.Forms.Label();
            RecvCountLabel = new System.Windows.Forms.Label();
            SentCountLabel = new System.Windows.Forms.Label();
            TimeLabel = new System.Windows.Forms.Label();
            TableLayoutPanel.SuspendLayout();
            InfoGroupBox.SuspendLayout();
            InfoTableLayoutPanel.SuspendLayout();
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
            TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 215F));
            TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanel.Size = new System.Drawing.Size(1280, 720);
            TableLayoutPanel.TabIndex = 0;
            // 
            // InfoGroupBox
            // 
            InfoGroupBox.Controls.Add(InfoTableLayoutPanel);
            InfoGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            InfoGroupBox.Location = new System.Drawing.Point(3, 3);
            InfoGroupBox.Name = "InfoGroupBox";
            InfoGroupBox.Size = new System.Drawing.Size(294, 209);
            InfoGroupBox.TabIndex = 0;
            InfoGroupBox.TabStop = false;
            InfoGroupBox.Text = "信息";
            // 
            // InfoTableLayoutPanel
            // 
            InfoTableLayoutPanel.ColumnCount = 2;
            InfoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.7083321F));
            InfoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.2916679F));
            InfoTableLayoutPanel.Controls.Add(TimeDynamicLabel, 1, 3);
            InfoTableLayoutPanel.Controls.Add(SentCountDynamicLabel, 1, 2);
            InfoTableLayoutPanel.Controls.Add(StatusLabel, 0, 0);
            InfoTableLayoutPanel.Controls.Add(RecvCountLabel, 0, 1);
            InfoTableLayoutPanel.Controls.Add(SentCountLabel, 0, 2);
            InfoTableLayoutPanel.Controls.Add(TimeLabel, 0, 3);
            InfoTableLayoutPanel.Controls.Add(StatusDynamicLabel, 1, 0);
            InfoTableLayoutPanel.Controls.Add(RecvCountDynamicLabel, 1, 1);
            InfoTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            InfoTableLayoutPanel.Location = new System.Drawing.Point(3, 34);
            InfoTableLayoutPanel.Name = "InfoTableLayoutPanel";
            InfoTableLayoutPanel.RowCount = 4;
            InfoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            InfoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            InfoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            InfoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            InfoTableLayoutPanel.Size = new System.Drawing.Size(288, 172);
            InfoTableLayoutPanel.TabIndex = 0;
            // 
            // TimeDynamicLabel
            // 
            TimeDynamicLabel.AutoSize = true;
            TimeDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            TimeDynamicLabel.Location = new System.Drawing.Point(126, 129);
            TimeDynamicLabel.Name = "TimeDynamicLabel";
            TimeDynamicLabel.Size = new System.Drawing.Size(159, 43);
            TimeDynamicLabel.TabIndex = 7;
            TimeDynamicLabel.Text = "-";
            TimeDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SentCountDynamicLabel
            // 
            SentCountDynamicLabel.AutoSize = true;
            SentCountDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            SentCountDynamicLabel.Location = new System.Drawing.Point(126, 86);
            SentCountDynamicLabel.Name = "SentCountDynamicLabel";
            SentCountDynamicLabel.Size = new System.Drawing.Size(159, 43);
            SentCountDynamicLabel.TabIndex = 6;
            SentCountDynamicLabel.Text = "0";
            SentCountDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StatusLabel
            // 
            StatusLabel.AutoSize = true;
            StatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            StatusLabel.Location = new System.Drawing.Point(3, 0);
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new System.Drawing.Size(117, 43);
            StatusLabel.TabIndex = 0;
            StatusLabel.Text = "状态";
            StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RecvCountLabel
            // 
            RecvCountLabel.AutoSize = true;
            RecvCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            RecvCountLabel.Location = new System.Drawing.Point(3, 43);
            RecvCountLabel.Name = "RecvCountLabel";
            RecvCountLabel.Size = new System.Drawing.Size(117, 43);
            RecvCountLabel.TabIndex = 1;
            RecvCountLabel.Text = "接收";
            RecvCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SentCountLabel
            // 
            SentCountLabel.AutoSize = true;
            SentCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            SentCountLabel.Location = new System.Drawing.Point(3, 86);
            SentCountLabel.Name = "SentCountLabel";
            SentCountLabel.Size = new System.Drawing.Size(117, 43);
            SentCountLabel.TabIndex = 2;
            SentCountLabel.Text = "发送";
            SentCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TimeLabel
            // 
            TimeLabel.AutoSize = true;
            TimeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            TimeLabel.Location = new System.Drawing.Point(3, 129);
            TimeLabel.Name = "TimeLabel";
            TimeLabel.Size = new System.Drawing.Size(117, 43);
            TimeLabel.TabIndex = 3;
            TimeLabel.Text = "连接时长";
            TimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusDynamicLabel
            // 
            StatusDynamicLabel.AutoSize = true;
            StatusDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            StatusDynamicLabel.Location = new System.Drawing.Point(126, 0);
            StatusDynamicLabel.Name = "StatusDynamicLabel";
            StatusDynamicLabel.Size = new System.Drawing.Size(159, 43);
            StatusDynamicLabel.TabIndex = 4;
            StatusDynamicLabel.Text = "关闭";
            StatusDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RecvCountDynamicLabel
            // 
            RecvCountDynamicLabel.AutoSize = true;
            RecvCountDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            RecvCountDynamicLabel.Location = new System.Drawing.Point(126, 43);
            RecvCountDynamicLabel.Name = "RecvCountDynamicLabel";
            RecvCountDynamicLabel.Size = new System.Drawing.Size(159, 43);
            RecvCountDynamicLabel.TabIndex = 5;
            RecvCountDynamicLabel.Text = "0";
            RecvCountDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ControlGroupBox
            // 
            ControlGroupBox.Controls.Add(CloseButton);
            ControlGroupBox.Controls.Add(OpenButton);
            ControlGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            ControlGroupBox.Location = new System.Drawing.Point(3, 218);
            ControlGroupBox.Name = "ControlGroupBox";
            ControlGroupBox.Size = new System.Drawing.Size(294, 89);
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
            CloseButton.Text = "关闭";
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
            OpenButton.Text = "开启";
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
            InfoGroupBox.ResumeLayout(false);
            InfoTableLayoutPanel.ResumeLayout(false);
            InfoTableLayoutPanel.PerformLayout();
            ControlGroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox ControlGroupBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button OpenButton;
        public Controls.ConsoleWebBrowser ConsoleWebBrowser;
        private System.Windows.Forms.Label TimeDynamicLabel;
        private System.Windows.Forms.Label SentCountDynamicLabel;
        private System.Windows.Forms.Label StatusDynamicLabel;
        private System.Windows.Forms.Label RecvCountDynamicLabel;
    }
}
