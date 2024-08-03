namespace Serein.Lite.Ui.Members
{
    partial class BindingPage
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
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ColumnHeader columnHeader3;
            System.Windows.Forms.StatusStrip StatusStrip;
            BindingListView = new System.Windows.Forms.ListView();
            ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            StatusStrip = new System.Windows.Forms.StatusStrip();
            StatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // BindingListView
            // 
            BindingListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            BindingListView.Dock = System.Windows.Forms.DockStyle.Fill;
            BindingListView.FullRowSelect = true;
            BindingListView.GridLines = true;
            BindingListView.Location = new System.Drawing.Point(0, 0);
            BindingListView.MultiSelect = false;
            BindingListView.Name = "BindingListView";
            BindingListView.Size = new System.Drawing.Size(1200, 720);
            BindingListView.TabIndex = 0;
            BindingListView.UseCompatibleStateImageBehavior = false;
            BindingListView.View = System.Windows.Forms.View.Details;
            BindingListView.SelectedIndexChanged += BindingListView_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "用户Id";
            columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "用户名";
            columnHeader2.Width = 300;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "绑定Id";
            columnHeader3.Width = 400;
            // 
            // StatusStrip
            // 
            StatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripStatusLabel });
            StatusStrip.Location = new System.Drawing.Point(0, 679);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Size = new System.Drawing.Size(1200, 41);
            StatusStrip.TabIndex = 1;
            StatusStrip.Text = "statusStrip1";
            // 
            // ToolStripStatusLabel
            // 
            ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            ToolStripStatusLabel.Size = new System.Drawing.Size(24, 31);
            ToolStripStatusLabel.Text = "-";
            // 
            // BindingPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(StatusStrip);
            Controls.Add(BindingListView);
            Name = "BindingPage";
            Size = new System.Drawing.Size(1200, 720);
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView BindingListView;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
    }
}
