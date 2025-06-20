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
            System.Windows.Forms.TabControl tabControl;
            _connectionTabPage = new System.Windows.Forms.TabPage();
            _reactionTabPage = new System.Windows.Forms.TabPage();
            _webTabPage = new System.Windows.Forms.TabPage();
            _applicationTabPage = new System.Windows.Forms.TabPage();
            _aboutTabPage = new System.Windows.Forms.TabPage();
            tabControl = new System.Windows.Forms.TabControl();
            tabControl.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(_connectionTabPage);
            tabControl.Controls.Add(_reactionTabPage);
            tabControl.Controls.Add(_webTabPage);
            tabControl.Controls.Add(_applicationTabPage);
            tabControl.Controls.Add(_aboutTabPage);
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl.Location = new System.Drawing.Point(0, 0);
            tabControl.Margin = new System.Windows.Forms.Padding(0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(1280, 720);
            tabControl.TabIndex = 0;
            // 
            // _connectionTabPage
            // 
            _connectionTabPage.Location = new System.Drawing.Point(8, 45);
            _connectionTabPage.Name = "_connectionTabPage";
            _connectionTabPage.Size = new System.Drawing.Size(1264, 667);
            _connectionTabPage.TabIndex = 0;
            _connectionTabPage.Text = "连接";
            _connectionTabPage.UseVisualStyleBackColor = true;
            // 
            // _reactionTabPage
            // 
            _reactionTabPage.Location = new System.Drawing.Point(8, 45);
            _reactionTabPage.Name = "_reactionTabPage";
            _reactionTabPage.Size = new System.Drawing.Size(1264, 667);
            _reactionTabPage.TabIndex = 1;
            _reactionTabPage.Text = "反应";
            _reactionTabPage.UseVisualStyleBackColor = true;
            // 
            // _webTabPage
            // 
            _webTabPage.Location = new System.Drawing.Point(8, 45);
            _webTabPage.Name = "_webTabPage";
            _webTabPage.Size = new System.Drawing.Size(1264, 667);
            _webTabPage.TabIndex = 4;
            _webTabPage.Text = "网页";
            _webTabPage.UseVisualStyleBackColor = true;
            // 
            // _applicationTabPage
            // 
            _applicationTabPage.Location = new System.Drawing.Point(8, 45);
            _applicationTabPage.Margin = new System.Windows.Forms.Padding(0);
            _applicationTabPage.Name = "_applicationTabPage";
            _applicationTabPage.Size = new System.Drawing.Size(1264, 667);
            _applicationTabPage.TabIndex = 2;
            _applicationTabPage.Text = "应用";
            _applicationTabPage.UseVisualStyleBackColor = true;
            // 
            // _aboutTabPage
            // 
            _aboutTabPage.Location = new System.Drawing.Point(8, 45);
            _aboutTabPage.Name = "_aboutTabPage";
            _aboutTabPage.Size = new System.Drawing.Size(1264, 667);
            _aboutTabPage.TabIndex = 3;
            _aboutTabPage.Text = "关于";
            _aboutTabPage.UseVisualStyleBackColor = true;
            // 
            // SettingPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tabControl);
            Name = "SettingPage";
            Size = new System.Drawing.Size(1280, 720);
            tabControl.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabPage _aboutTabPage;
        private System.Windows.Forms.TabPage _connectionTabPage;
        private System.Windows.Forms.TabPage _applicationTabPage;
        private System.Windows.Forms.TabPage _reactionTabPage;
        private System.Windows.Forms.TabPage _webTabPage;
    }
}
