namespace Serein.Lite.Ui.Function
{
    partial class MatchPage
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
            StatusStrip = new System.Windows.Forms.StatusStrip();
            ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            MatchListView = new System.Windows.Forms.ListView();
            StatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // StatusStrip
            // 
            StatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripStatusLabel });
            StatusStrip.Location = new System.Drawing.Point(0, 682);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Size = new System.Drawing.Size(1280, 38);
            StatusStrip.TabIndex = 0;
            StatusStrip.Text = "statusStrip1";
            // 
            // ToolStripStatusLabel
            // 
            ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            ToolStripStatusLabel.Size = new System.Drawing.Size(0, 28);
            // 
            // MatchListView
            // 
            MatchListView.Dock = System.Windows.Forms.DockStyle.Fill;
            MatchListView.Location = new System.Drawing.Point(0, 0);
            MatchListView.Name = "MatchListView";
            MatchListView.Size = new System.Drawing.Size(1280, 682);
            MatchListView.TabIndex = 1;
            MatchListView.UseCompatibleStateImageBehavior = false;
            MatchListView.SelectedIndexChanged += MatchListView_SelectedIndexChanged;
            // 
            // MatchPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(MatchListView);
            Controls.Add(StatusStrip);
            Name = "MatchPage";
            Size = new System.Drawing.Size(1280, 720);
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ListView MatchListView;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
    }
}
