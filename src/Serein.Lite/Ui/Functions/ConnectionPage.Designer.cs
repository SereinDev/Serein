namespace Serein.Lite.Ui.Functions
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
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
            System.Windows.Forms.GroupBox infoGroupBox;
            System.Windows.Forms.TableLayoutPanel infoTableLayoutPanel;
            System.Windows.Forms.Label statusLabel;
            System.Windows.Forms.Label recvCountLabel;
            System.Windows.Forms.Label sentCountLabel;
            System.Windows.Forms.Label timeLabel;
            System.Windows.Forms.GroupBox controlGroupBox;
            System.Windows.Forms.Button closeButton;
            System.Windows.Forms.Button openButton;
            _timeDynamicLabel = new System.Windows.Forms.Label();
            _sentCountDynamicLabel = new System.Windows.Forms.Label();
            _statusDynamicLabel = new System.Windows.Forms.Label();
            _recvCountDynamicLabel = new System.Windows.Forms.Label();
            ConsoleWebBrowser = new Serein.Lite.Ui.Controls.ConsoleWebBrowser();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            infoGroupBox = new System.Windows.Forms.GroupBox();
            infoTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            statusLabel = new System.Windows.Forms.Label();
            recvCountLabel = new System.Windows.Forms.Label();
            sentCountLabel = new System.Windows.Forms.Label();
            timeLabel = new System.Windows.Forms.Label();
            controlGroupBox = new System.Windows.Forms.GroupBox();
            closeButton = new System.Windows.Forms.Button();
            openButton = new System.Windows.Forms.Button();
            tableLayoutPanel.SuspendLayout();
            infoGroupBox.SuspendLayout();
            infoTableLayoutPanel.SuspendLayout();
            controlGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(infoGroupBox, 0, 0);
            tableLayoutPanel.Controls.Add(controlGroupBox, 0, 1);
            tableLayoutPanel.Controls.Add(ConsoleWebBrowser, 1, 0);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 3;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 215F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.Size = new System.Drawing.Size(1280, 720);
            tableLayoutPanel.TabIndex = 0;
            // 
            // infoGroupBox
            // 
            infoGroupBox.Controls.Add(infoTableLayoutPanel);
            infoGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            infoGroupBox.Location = new System.Drawing.Point(3, 3);
            infoGroupBox.Name = "infoGroupBox";
            infoGroupBox.Size = new System.Drawing.Size(294, 209);
            infoGroupBox.TabIndex = 0;
            infoGroupBox.TabStop = false;
            infoGroupBox.Text = "信息";
            // 
            // infoTableLayoutPanel
            // 
            infoTableLayoutPanel.ColumnCount = 2;
            infoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.7083321F));
            infoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.2916679F));
            infoTableLayoutPanel.Controls.Add(_timeDynamicLabel, 1, 3);
            infoTableLayoutPanel.Controls.Add(_sentCountDynamicLabel, 1, 2);
            infoTableLayoutPanel.Controls.Add(statusLabel, 0, 0);
            infoTableLayoutPanel.Controls.Add(recvCountLabel, 0, 1);
            infoTableLayoutPanel.Controls.Add(sentCountLabel, 0, 2);
            infoTableLayoutPanel.Controls.Add(timeLabel, 0, 3);
            infoTableLayoutPanel.Controls.Add(_statusDynamicLabel, 1, 0);
            infoTableLayoutPanel.Controls.Add(_recvCountDynamicLabel, 1, 1);
            infoTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            infoTableLayoutPanel.Location = new System.Drawing.Point(3, 34);
            infoTableLayoutPanel.Name = "infoTableLayoutPanel";
            infoTableLayoutPanel.RowCount = 4;
            infoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            infoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            infoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            infoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            infoTableLayoutPanel.Size = new System.Drawing.Size(288, 172);
            infoTableLayoutPanel.TabIndex = 0;
            // 
            // _timeDynamicLabel
            // 
            _timeDynamicLabel.AutoSize = true;
            _timeDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _timeDynamicLabel.Location = new System.Drawing.Point(126, 129);
            _timeDynamicLabel.Name = "_timeDynamicLabel";
            _timeDynamicLabel.Size = new System.Drawing.Size(159, 43);
            _timeDynamicLabel.TabIndex = 7;
            _timeDynamicLabel.Text = "-";
            _timeDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _sentCountDynamicLabel
            // 
            _sentCountDynamicLabel.AutoSize = true;
            _sentCountDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _sentCountDynamicLabel.Location = new System.Drawing.Point(126, 86);
            _sentCountDynamicLabel.Name = "_sentCountDynamicLabel";
            _sentCountDynamicLabel.Size = new System.Drawing.Size(159, 43);
            _sentCountDynamicLabel.TabIndex = 6;
            _sentCountDynamicLabel.Text = "0";
            _sentCountDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            statusLabel.Location = new System.Drawing.Point(3, 0);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new System.Drawing.Size(117, 43);
            statusLabel.TabIndex = 0;
            statusLabel.Text = "状态";
            statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // recvCountLabel
            // 
            recvCountLabel.AutoSize = true;
            recvCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            recvCountLabel.Location = new System.Drawing.Point(3, 43);
            recvCountLabel.Name = "recvCountLabel";
            recvCountLabel.Size = new System.Drawing.Size(117, 43);
            recvCountLabel.TabIndex = 1;
            recvCountLabel.Text = "接收";
            recvCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sentCountLabel
            // 
            sentCountLabel.AutoSize = true;
            sentCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            sentCountLabel.Location = new System.Drawing.Point(3, 86);
            sentCountLabel.Name = "sentCountLabel";
            sentCountLabel.Size = new System.Drawing.Size(117, 43);
            sentCountLabel.TabIndex = 2;
            sentCountLabel.Text = "发送";
            sentCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timeLabel
            // 
            timeLabel.AutoSize = true;
            timeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            timeLabel.Location = new System.Drawing.Point(3, 129);
            timeLabel.Name = "timeLabel";
            timeLabel.Size = new System.Drawing.Size(117, 43);
            timeLabel.TabIndex = 3;
            timeLabel.Text = "连接时长";
            timeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _statusDynamicLabel
            // 
            _statusDynamicLabel.AutoSize = true;
            _statusDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _statusDynamicLabel.Location = new System.Drawing.Point(126, 0);
            _statusDynamicLabel.Name = "_statusDynamicLabel";
            _statusDynamicLabel.Size = new System.Drawing.Size(159, 43);
            _statusDynamicLabel.TabIndex = 4;
            _statusDynamicLabel.Text = "关闭";
            _statusDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _recvCountDynamicLabel
            // 
            _recvCountDynamicLabel.AutoSize = true;
            _recvCountDynamicLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _recvCountDynamicLabel.Location = new System.Drawing.Point(126, 43);
            _recvCountDynamicLabel.Name = "_recvCountDynamicLabel";
            _recvCountDynamicLabel.Size = new System.Drawing.Size(159, 43);
            _recvCountDynamicLabel.TabIndex = 5;
            _recvCountDynamicLabel.Text = "0";
            _recvCountDynamicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // controlGroupBox
            // 
            controlGroupBox.Controls.Add(closeButton);
            controlGroupBox.Controls.Add(openButton);
            controlGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            controlGroupBox.Location = new System.Drawing.Point(3, 218);
            controlGroupBox.Name = "controlGroupBox";
            controlGroupBox.Size = new System.Drawing.Size(294, 89);
            controlGroupBox.TabIndex = 1;
            controlGroupBox.TabStop = false;
            controlGroupBox.Text = "控制";
            // 
            // closeButton
            // 
            closeButton.Location = new System.Drawing.Point(161, 37);
            closeButton.Margin = new System.Windows.Forms.Padding(10);
            closeButton.Name = "closeButton";
            closeButton.Size = new System.Drawing.Size(120, 48);
            closeButton.TabIndex = 1;
            closeButton.Text = "关闭";
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += CloseButton_Click;
            // 
            // openButton
            // 
            openButton.Location = new System.Drawing.Point(13, 37);
            openButton.Margin = new System.Windows.Forms.Padding(10);
            openButton.Name = "openButton";
            openButton.Size = new System.Drawing.Size(120, 48);
            openButton.TabIndex = 0;
            openButton.Text = "开启";
            openButton.UseVisualStyleBackColor = true;
            openButton.Click += OpenButton_Click;
            // 
            // ConsoleWebBrowser
            // 
            ConsoleWebBrowser.AllowNavigation = false;
            ConsoleWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            ConsoleWebBrowser.IsWebBrowserContextMenuEnabled = false;
            ConsoleWebBrowser.Location = new System.Drawing.Point(303, 3);
            ConsoleWebBrowser.Name = "ConsoleWebBrowser";
            tableLayoutPanel.SetRowSpan(ConsoleWebBrowser, 3);
            ConsoleWebBrowser.Size = new System.Drawing.Size(974, 714);
            ConsoleWebBrowser.TabIndex = 2;
            // 
            // ConnectionPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(tableLayoutPanel);
            Name = "ConnectionPage";
            Size = new System.Drawing.Size(1280, 720);
            tableLayoutPanel.ResumeLayout(false);
            infoGroupBox.ResumeLayout(false);
            infoTableLayoutPanel.ResumeLayout(false);
            infoTableLayoutPanel.PerformLayout();
            controlGroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        public Controls.ConsoleWebBrowser ConsoleWebBrowser;
        private System.Windows.Forms.Label _timeDynamicLabel;
        private System.Windows.Forms.Label _sentCountDynamicLabel;
        private System.Windows.Forms.Label _statusDynamicLabel;
        private System.Windows.Forms.Label _recvCountDynamicLabel;
    }
}
