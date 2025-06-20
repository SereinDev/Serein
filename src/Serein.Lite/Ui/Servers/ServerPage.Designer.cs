namespace Serein.Lite.Ui.Servers
{
    partial class ServerPage
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
            System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
            _statusStrip = new System.Windows.Forms.StatusStrip();
            _mainTabControl = new System.Windows.Forms.TabControl();
            toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            _statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _statusStrip
            // 
            _statusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            _statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel });
            _statusStrip.Location = new System.Drawing.Point(0, 709);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Size = new System.Drawing.Size(1000, 41);
            _statusStrip.TabIndex = 1;
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new System.Drawing.Size(710, 31);
            toolStripStatusLabel.Text = "当前没有服务器配置。你可以在左上角的服务器菜单栏添加或导入";
            // 
            // _mainTabControl
            // 
            _mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            _mainTabControl.Location = new System.Drawing.Point(0, 0);
            _mainTabControl.Name = "_mainTabControl";
            _mainTabControl.SelectedIndex = 0;
            _mainTabControl.Size = new System.Drawing.Size(1000, 750);
            _mainTabControl.TabIndex = 0;
            // 
            // ServerPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_statusStrip);
            Controls.Add(_mainTabControl);
            Name = "ServerPage";
            Size = new System.Drawing.Size(1000, 750);
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.StatusStrip _statusStrip;
        internal System.Windows.Forms.TabControl _mainTabControl;
    }
}
