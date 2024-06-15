namespace Serein.Lite.Ui.Function
{
    partial class PluginPage
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
            System.Windows.Forms.TabPage ConsoleTabPage;
            System.Windows.Forms.TabPage ManageTabPage;
            System.Windows.Forms.TabPage MarketTabPage;
            ConsoleRichTextBox = new System.Windows.Forms.RichTextBox();
            TabControl = new System.Windows.Forms.TabControl();
            ConsoleTabPage = new System.Windows.Forms.TabPage();
            ManageTabPage = new System.Windows.Forms.TabPage();
            MarketTabPage = new System.Windows.Forms.TabPage();
            TabControl.SuspendLayout();
            ConsoleTabPage.SuspendLayout();
            SuspendLayout();
            // 
            // TabControl
            // 
            TabControl.Controls.Add(ConsoleTabPage);
            TabControl.Controls.Add(ManageTabPage);
            TabControl.Controls.Add(MarketTabPage);
            TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            TabControl.Location = new System.Drawing.Point(0, 0);
            TabControl.Name = "TabControl";
            TabControl.SelectedIndex = 0;
            TabControl.Size = new System.Drawing.Size(1280, 720);
            TabControl.TabIndex = 0;
            // 
            // ConsoleTabPage
            // 
            ConsoleTabPage.Controls.Add(ConsoleRichTextBox);
            ConsoleTabPage.Location = new System.Drawing.Point(8, 45);
            ConsoleTabPage.Name = "ConsoleTabPage";
            ConsoleTabPage.Padding = new System.Windows.Forms.Padding(3);
            ConsoleTabPage.Size = new System.Drawing.Size(1264, 667);
            ConsoleTabPage.TabIndex = 0;
            ConsoleTabPage.Text = "控制台";
            ConsoleTabPage.UseVisualStyleBackColor = true;
            // 
            // ConsoleRichTextBox
            // 
            ConsoleRichTextBox.BackColor = System.Drawing.Color.White;
            ConsoleRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            ConsoleRichTextBox.Location = new System.Drawing.Point(3, 3);
            ConsoleRichTextBox.Name = "ConsoleRichTextBox";
            ConsoleRichTextBox.ReadOnly = true;
            ConsoleRichTextBox.Size = new System.Drawing.Size(1258, 661);
            ConsoleRichTextBox.TabIndex = 0;
            ConsoleRichTextBox.Text = "";
            // 
            // ManageTabPage
            // 
            ManageTabPage.Location = new System.Drawing.Point(8, 45);
            ManageTabPage.Name = "ManageTabPage";
            ManageTabPage.Padding = new System.Windows.Forms.Padding(3);
            ManageTabPage.Size = new System.Drawing.Size(1264, 667);
            ManageTabPage.TabIndex = 1;
            ManageTabPage.Text = "管理";
            ManageTabPage.UseVisualStyleBackColor = true;
            // 
            // MarketTabPage
            // 
            MarketTabPage.Location = new System.Drawing.Point(8, 45);
            MarketTabPage.Name = "MarketTabPage";
            MarketTabPage.Padding = new System.Windows.Forms.Padding(3);
            MarketTabPage.Size = new System.Drawing.Size(1264, 667);
            MarketTabPage.TabIndex = 2;
            MarketTabPage.Text = "市场";
            MarketTabPage.UseVisualStyleBackColor = true;
            // 
            // PluginPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(TabControl);
            Name = "PluginPage";
            Size = new System.Drawing.Size(1280, 720);
            TabControl.ResumeLayout(false);
            ConsoleTabPage.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.RichTextBox ConsoleRichTextBox;
    }
}
