namespace Serein.Lite.Ui.Servers
{
    partial class Panel
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
            System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
            System.Windows.Forms.GroupBox InfoGroupBox;
            System.Windows.Forms.GroupBox ControlGroupBox;
            System.Windows.Forms.TableLayoutPanel ControlTableLayoutPanel;
            System.Windows.Forms.GroupBox ConsoleGroupBox;
            System.Windows.Forms.TableLayoutPanel ConsoleTableLayoutPanel;
            StartButton = new System.Windows.Forms.Button();
            StopButton = new System.Windows.Forms.Button();
            RestartButton = new System.Windows.Forms.Button();
            TerminateButton = new System.Windows.Forms.Button();
            InputTextBox = new System.Windows.Forms.TextBox();
            EnterButton = new System.Windows.Forms.Button();
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            InfoGroupBox = new System.Windows.Forms.GroupBox();
            ControlGroupBox = new System.Windows.Forms.GroupBox();
            ControlTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ConsoleGroupBox = new System.Windows.Forms.GroupBox();
            ConsoleTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            MainTableLayoutPanel.SuspendLayout();
            ControlGroupBox.SuspendLayout();
            ControlTableLayoutPanel.SuspendLayout();
            ConsoleGroupBox.SuspendLayout();
            ConsoleTableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            MainTableLayoutPanel.ColumnCount = 2;
            MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            MainTableLayoutPanel.Controls.Add(InfoGroupBox, 0, 0);
            MainTableLayoutPanel.Controls.Add(ControlGroupBox, 0, 1);
            MainTableLayoutPanel.Controls.Add(ConsoleGroupBox, 1, 0);
            MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            MainTableLayoutPanel.RowCount = 3;
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66667F));
            MainTableLayoutPanel.Size = new System.Drawing.Size(1280, 720);
            MainTableLayoutPanel.TabIndex = 0;
            // 
            // InfoGroupBox
            // 
            InfoGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            InfoGroupBox.Location = new System.Drawing.Point(3, 3);
            InfoGroupBox.Name = "InfoGroupBox";
            InfoGroupBox.Size = new System.Drawing.Size(294, 183);
            InfoGroupBox.TabIndex = 0;
            InfoGroupBox.TabStop = false;
            InfoGroupBox.Text = "信息";
            // 
            // ControlGroupBox
            // 
            ControlGroupBox.Controls.Add(ControlTableLayoutPanel);
            ControlGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            ControlGroupBox.Location = new System.Drawing.Point(3, 192);
            ControlGroupBox.Name = "ControlGroupBox";
            ControlGroupBox.Size = new System.Drawing.Size(294, 144);
            ControlGroupBox.TabIndex = 1;
            ControlGroupBox.TabStop = false;
            ControlGroupBox.Text = "控制";
            // 
            // ControlTableLayoutPanel
            // 
            ControlTableLayoutPanel.ColumnCount = 2;
            ControlTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            ControlTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            ControlTableLayoutPanel.Controls.Add(StartButton, 0, 0);
            ControlTableLayoutPanel.Controls.Add(StopButton, 1, 0);
            ControlTableLayoutPanel.Controls.Add(RestartButton, 0, 1);
            ControlTableLayoutPanel.Controls.Add(TerminateButton, 1, 1);
            ControlTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            ControlTableLayoutPanel.Location = new System.Drawing.Point(3, 34);
            ControlTableLayoutPanel.Name = "ControlTableLayoutPanel";
            ControlTableLayoutPanel.RowCount = 2;
            ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            ControlTableLayoutPanel.Size = new System.Drawing.Size(288, 107);
            ControlTableLayoutPanel.TabIndex = 0;
            // 
            // StartButton
            // 
            StartButton.Dock = System.Windows.Forms.DockStyle.Fill;
            StartButton.Location = new System.Drawing.Point(3, 3);
            StartButton.Name = "StartButton";
            StartButton.Size = new System.Drawing.Size(138, 47);
            StartButton.TabIndex = 0;
            StartButton.Text = "启动";
            StartButton.UseVisualStyleBackColor = true;
            StartButton.Click += StartButton_Click;
            // 
            // StopButton
            // 
            StopButton.Dock = System.Windows.Forms.DockStyle.Fill;
            StopButton.Location = new System.Drawing.Point(147, 3);
            StopButton.Name = "StopButton";
            StopButton.Size = new System.Drawing.Size(138, 47);
            StopButton.TabIndex = 1;
            StopButton.Text = "停止";
            StopButton.UseVisualStyleBackColor = true;
            StopButton.Click += StopButton_Click;
            // 
            // RestartButton
            // 
            RestartButton.Dock = System.Windows.Forms.DockStyle.Fill;
            RestartButton.Location = new System.Drawing.Point(3, 56);
            RestartButton.Name = "RestartButton";
            RestartButton.Size = new System.Drawing.Size(138, 48);
            RestartButton.TabIndex = 2;
            RestartButton.Text = "重启";
            RestartButton.UseVisualStyleBackColor = true;
            RestartButton.Click += RestartButton_Click;
            // 
            // TerminateButton
            // 
            TerminateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            TerminateButton.Location = new System.Drawing.Point(147, 56);
            TerminateButton.Name = "TerminateButton";
            TerminateButton.Size = new System.Drawing.Size(138, 48);
            TerminateButton.TabIndex = 3;
            TerminateButton.Text = "强制结束";
            TerminateButton.UseVisualStyleBackColor = true;
            TerminateButton.Click += TerminateButton_Click;
            // 
            // ConsoleGroupBox
            // 
            ConsoleGroupBox.Controls.Add(ConsoleTableLayoutPanel);
            ConsoleGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            ConsoleGroupBox.Location = new System.Drawing.Point(303, 3);
            ConsoleGroupBox.Name = "ConsoleGroupBox";
            MainTableLayoutPanel.SetRowSpan(ConsoleGroupBox, 3);
            ConsoleGroupBox.Size = new System.Drawing.Size(974, 714);
            ConsoleGroupBox.TabIndex = 2;
            ConsoleGroupBox.TabStop = false;
            ConsoleGroupBox.Text = "控制台";
            // 
            // ConsoleTableLayoutPanel
            // 
            ConsoleTableLayoutPanel.ColumnCount = 2;
            ConsoleTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ConsoleTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            ConsoleTableLayoutPanel.Controls.Add(InputTextBox, 0, 1);
            ConsoleTableLayoutPanel.Controls.Add(EnterButton, 1, 1);
            ConsoleTableLayoutPanel.Controls.Add(richTextBox1, 0, 0);
            ConsoleTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            ConsoleTableLayoutPanel.Location = new System.Drawing.Point(3, 34);
            ConsoleTableLayoutPanel.Name = "ConsoleTableLayoutPanel";
            ConsoleTableLayoutPanel.RowCount = 2;
            ConsoleTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ConsoleTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            ConsoleTableLayoutPanel.Size = new System.Drawing.Size(968, 677);
            ConsoleTableLayoutPanel.TabIndex = 0;
            // 
            // InputTextBox
            // 
            InputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            InputTextBox.Location = new System.Drawing.Point(3, 632);
            InputTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
            InputTextBox.Name = "InputTextBox";
            InputTextBox.Size = new System.Drawing.Size(910, 38);
            InputTextBox.TabIndex = 0;
            InputTextBox.KeyDown += InputTextBox_KeyDown;
            // 
            // EnterButton
            // 
            EnterButton.Dock = System.Windows.Forms.DockStyle.Fill;
            EnterButton.Location = new System.Drawing.Point(921, 632);
            EnterButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            EnterButton.Name = "EnterButton";
            EnterButton.Size = new System.Drawing.Size(44, 40);
            EnterButton.TabIndex = 1;
            EnterButton.Text = "▲";
            EnterButton.UseVisualStyleBackColor = true;
            EnterButton.Click += EnterButton_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            ConsoleTableLayoutPanel.SetColumnSpan(richTextBox1, 2);
            richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            richTextBox1.Location = new System.Drawing.Point(3, 3);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(962, 623);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            // 
            // Panel
            // 
            BackColor = System.Drawing.Color.White;
            Controls.Add(MainTableLayoutPanel);
            Name = "Panel";
            Size = new System.Drawing.Size(1280, 720);
            MainTableLayoutPanel.ResumeLayout(false);
            ControlGroupBox.ResumeLayout(false);
            ControlTableLayoutPanel.ResumeLayout(false);
            ConsoleGroupBox.ResumeLayout(false);
            ConsoleTableLayoutPanel.ResumeLayout(false);
            ConsoleTableLayoutPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.Button EnterButton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button RestartButton;
        private System.Windows.Forms.Button TerminateButton;
    }
}
