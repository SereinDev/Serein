namespace Serein.Lite.Ui.Settings
{
    partial class SettingPage
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
            System.Windows.Forms.TabControl TabControl;
            System.Windows.Forms.TabPage ReactionTabPage;
            ConnectionTabPage = new System.Windows.Forms.TabPage();
            ApplicationTabPage = new System.Windows.Forms.TabPage();
            AboutTabPage = new System.Windows.Forms.TabPage();
            TabControl = new System.Windows.Forms.TabControl();
            ReactionTabPage = new System.Windows.Forms.TabPage();
            TabControl.SuspendLayout();
            SuspendLayout();
            // 
            // TabControl
            // 
            TabControl.Controls.Add(ConnectionTabPage);
            TabControl.Controls.Add(ReactionTabPage);
            TabControl.Controls.Add(ApplicationTabPage);
            TabControl.Controls.Add(AboutTabPage);
            TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            TabControl.Location = new System.Drawing.Point(0, 0);
            TabControl.Margin = new System.Windows.Forms.Padding(0);
            TabControl.Name = "TabControl";
            TabControl.SelectedIndex = 0;
            TabControl.Size = new System.Drawing.Size(1280, 720);
            TabControl.TabIndex = 0;
            // 
            // ConnectionTabPage
            // 
            ConnectionTabPage.Location = new System.Drawing.Point(8, 45);
            ConnectionTabPage.Name = "ConnectionTabPage";
            ConnectionTabPage.Size = new System.Drawing.Size(1264, 667);
            ConnectionTabPage.TabIndex = 0;
            ConnectionTabPage.Text = "连接";
            ConnectionTabPage.UseVisualStyleBackColor = true;
            // 
            // ReactionTabPage
            // 
            ReactionTabPage.Location = new System.Drawing.Point(8, 45);
            ReactionTabPage.Name = "ReactionTabPage";
            ReactionTabPage.Size = new System.Drawing.Size(1264, 667);
            ReactionTabPage.TabIndex = 1;
            ReactionTabPage.Text = "反应";
            ReactionTabPage.UseVisualStyleBackColor = true;
            // 
            // ApplicationTabPage
            // 
            ApplicationTabPage.Location = new System.Drawing.Point(8, 45);
            ApplicationTabPage.Margin = new System.Windows.Forms.Padding(0);
            ApplicationTabPage.Name = "ApplicationTabPage";
            ApplicationTabPage.Size = new System.Drawing.Size(1264, 667);
            ApplicationTabPage.TabIndex = 2;
            ApplicationTabPage.Text = "应用";
            ApplicationTabPage.UseVisualStyleBackColor = true;
            // 
            // AboutTabPage
            // 
            AboutTabPage.Location = new System.Drawing.Point(8, 45);
            AboutTabPage.Name = "AboutTabPage";
            AboutTabPage.Size = new System.Drawing.Size(1264, 667);
            AboutTabPage.TabIndex = 3;
            AboutTabPage.Text = "关于";
            AboutTabPage.UseVisualStyleBackColor = true;
            // 
            // SettingPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(TabControl);
            Name = "SettingPage";
            Size = new System.Drawing.Size(1280, 720);
            TabControl.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabPage AboutTabPage;
        private System.Windows.Forms.TabPage ConnectionTabPage;
        private System.Windows.Forms.TabPage ApplicationTabPage;
    }
}
