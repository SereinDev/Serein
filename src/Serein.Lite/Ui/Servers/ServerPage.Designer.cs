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
            MainTabControl = new System.Windows.Forms.TabControl();
            StatusStrip = new System.Windows.Forms.StatusStrip();
            ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            StatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // MainTabControl
            // 
            MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            MainTabControl.Location = new System.Drawing.Point(0, 0);
            MainTabControl.Name = "MainTabControl";
            MainTabControl.SelectedIndex = 0;
            MainTabControl.Size = new System.Drawing.Size(1000, 750);
            MainTabControl.TabIndex = 0;
            // 
            // StatusStrip
            // 
            StatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripStatusLabel });
            StatusStrip.Location = new System.Drawing.Point(0, 709);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Size = new System.Drawing.Size(1000, 41);
            StatusStrip.TabIndex = 1;
            // 
            // ToolStripStatusLabel
            // 
            ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            ToolStripStatusLabel.Size = new System.Drawing.Size(710, 31);
            ToolStripStatusLabel.Text = "当前没有服务器配置。你可以在左上角的服务器菜单栏添加或导入";
            // 
            // ServerPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(StatusStrip);
            Controls.Add(MainTabControl);
            Name = "ServerPage";
            Size = new System.Drawing.Size(1000, 750);
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
        internal System.Windows.Forms.TabControl MainTabControl;
    }
}
