
namespace Serein
{
    partial class main
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.Panel = new System.Windows.Forms.TabPage();
            this.PanelTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.PanelInfo = new System.Windows.Forms.GroupBox();
            this.PanelControls = new System.Windows.Forms.GroupBox();
            this.PanelConsole = new System.Windows.Forms.GroupBox();
            this.PanelPanel = new System.Windows.Forms.Panel();
            this.ConsoleWebBrowser = new System.Windows.Forms.WebBrowser();
            this.PanelConsolePanel1 = new System.Windows.Forms.Panel();
            this.PanelConsoleEnter = new System.Windows.Forms.Button();
            this.PanelConsoleInput = new System.Windows.Forms.TextBox();
            this.About = new System.Windows.Forms.TabPage();
            this.SereinIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.MainTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl.SuspendLayout();
            this.Panel.SuspendLayout();
            this.PanelTableLayout.SuspendLayout();
            this.PanelConsole.SuspendLayout();
            this.PanelPanel.SuspendLayout();
            this.PanelConsolePanel1.SuspendLayout();
            this.MainTableLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.Panel);
            this.tabControl.Controls.Add(this.About);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // Panel
            // 
            this.Panel.Controls.Add(this.PanelTableLayout);
            resources.ApplyResources(this.Panel, "Panel");
            this.Panel.Name = "Panel";
            this.Panel.UseVisualStyleBackColor = true;
            // 
            // PanelTableLayout
            // 
            resources.ApplyResources(this.PanelTableLayout, "PanelTableLayout");
            this.PanelTableLayout.Controls.Add(this.PanelInfo, 0, 0);
            this.PanelTableLayout.Controls.Add(this.PanelControls, 0, 1);
            this.PanelTableLayout.Controls.Add(this.PanelConsole, 1, 0);
            this.PanelTableLayout.Name = "PanelTableLayout";
            this.PanelTableLayout.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // PanelInfo
            // 
            resources.ApplyResources(this.PanelInfo, "PanelInfo");
            this.PanelInfo.Name = "PanelInfo";
            this.PanelInfo.TabStop = false;
            this.PanelInfo.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // PanelControls
            // 
            resources.ApplyResources(this.PanelControls, "PanelControls");
            this.PanelControls.Name = "PanelControls";
            this.PanelControls.TabStop = false;
            // 
            // PanelConsole
            // 
            this.PanelConsole.Controls.Add(this.PanelPanel);
            resources.ApplyResources(this.PanelConsole, "PanelConsole");
            this.PanelConsole.Name = "PanelConsole";
            this.PanelTableLayout.SetRowSpan(this.PanelConsole, 2);
            this.PanelConsole.TabStop = false;
            this.PanelConsole.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // PanelPanel
            // 
            this.PanelPanel.Controls.Add(this.ConsoleWebBrowser);
            this.PanelPanel.Controls.Add(this.PanelConsolePanel1);
            resources.ApplyResources(this.PanelPanel, "PanelPanel");
            this.PanelPanel.Name = "PanelPanel";
            // 
            // ConsoleWebBrowser
            // 
            resources.ApplyResources(this.ConsoleWebBrowser, "ConsoleWebBrowser");
            this.ConsoleWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.ConsoleWebBrowser.Name = "ConsoleWebBrowser";
            this.ConsoleWebBrowser.ScrollBarsEnabled = false;
            this.ConsoleWebBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // PanelConsolePanel1
            // 
            resources.ApplyResources(this.PanelConsolePanel1, "PanelConsolePanel1");
            this.PanelConsolePanel1.Controls.Add(this.PanelConsoleEnter);
            this.PanelConsolePanel1.Controls.Add(this.PanelConsoleInput);
            this.PanelConsolePanel1.Name = "PanelConsolePanel1";
            // 
            // PanelConsoleEnter
            // 
            resources.ApplyResources(this.PanelConsoleEnter, "PanelConsoleEnter");
            this.PanelConsoleEnter.Name = "PanelConsoleEnter";
            this.PanelConsoleEnter.UseVisualStyleBackColor = true;
            // 
            // PanelConsoleInput
            // 
            resources.ApplyResources(this.PanelConsoleInput, "PanelConsoleInput");
            this.PanelConsoleInput.Name = "PanelConsoleInput";
            // 
            // About
            // 
            resources.ApplyResources(this.About, "About");
            this.About.Name = "About";
            this.About.UseVisualStyleBackColor = true;
            // 
            // SereinIcon
            // 
            resources.ApplyResources(this.SereinIcon, "SereinIcon");
            this.SereinIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // MainTableLayout
            // 
            resources.ApplyResources(this.MainTableLayout, "MainTableLayout");
            this.MainTableLayout.Controls.Add(this.tabControl, 0, 0);
            this.MainTableLayout.Name = "MainTableLayout";
            // 
            // main
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayout);
            this.Name = "main";
            this.tabControl.ResumeLayout(false);
            this.Panel.ResumeLayout(false);
            this.Panel.PerformLayout();
            this.PanelTableLayout.ResumeLayout(false);
            this.PanelConsole.ResumeLayout(false);
            this.PanelPanel.ResumeLayout(false);
            this.PanelConsolePanel1.ResumeLayout(false);
            this.PanelConsolePanel1.PerformLayout();
            this.MainTableLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage Panel;
        private System.Windows.Forms.TabPage About;
        private System.Windows.Forms.NotifyIcon SereinIcon;
        private System.Windows.Forms.TableLayoutPanel MainTableLayout;
        private System.Windows.Forms.TableLayoutPanel PanelTableLayout;
        private System.Windows.Forms.GroupBox PanelInfo;
        private System.Windows.Forms.GroupBox PanelControls;
        private System.Windows.Forms.GroupBox PanelConsole;
        private System.Windows.Forms.Panel PanelConsolePanel1;
        private System.Windows.Forms.TextBox PanelConsoleInput;
        private System.Windows.Forms.Button PanelConsoleEnter;
        private System.Windows.Forms.Panel PanelPanel;
        private System.Windows.Forms.WebBrowser ConsoleWebBrowser;
    }
}

