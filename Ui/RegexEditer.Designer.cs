
namespace Serein
{
    partial class RegexEditer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegexEditer));
            this.Area = new System.Windows.Forms.ComboBox();
            this.AreaLabel = new System.Windows.Forms.Label();
            this.IsAdmin = new System.Windows.Forms.CheckBox();
            this.RegexTextBox = new System.Windows.Forms.TextBox();
            this.CommandTextBox = new System.Windows.Forms.TextBox();
            this.RemarkTextBox = new System.Windows.Forms.TextBox();
            this.RegexLabel = new System.Windows.Forms.Label();
            this.RemarkLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Confirm = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Area
            // 
            this.Area.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Area.FormattingEnabled = true;
            this.Area.Items.AddRange(new object[] {
            resources.GetString("Area.Items"),
            resources.GetString("Area.Items1"),
            resources.GetString("Area.Items2"),
            resources.GetString("Area.Items3")});
            resources.ApplyResources(this.Area, "Area");
            this.Area.Name = "Area";
            this.Area.SelectedIndexChanged += new System.EventHandler(this.Area_SelectedIndexChanged);
            // 
            // AreaLabel
            // 
            resources.ApplyResources(this.AreaLabel, "AreaLabel");
            this.AreaLabel.Name = "AreaLabel";
            // 
            // IsAdmin
            // 
            resources.ApplyResources(this.IsAdmin, "IsAdmin");
            this.IsAdmin.Name = "IsAdmin";
            this.IsAdmin.UseVisualStyleBackColor = true;
            // 
            // RegexTextBox
            // 
            resources.ApplyResources(this.RegexTextBox, "RegexTextBox");
            this.RegexTextBox.Name = "RegexTextBox";
            this.RegexTextBox.TextChanged += new System.EventHandler(this.Regex_TextChanged);
            // 
            // CommandTextBox
            // 
            resources.ApplyResources(this.CommandTextBox, "CommandTextBox");
            this.CommandTextBox.Name = "CommandTextBox";
            this.CommandTextBox.TextChanged += new System.EventHandler(this.Command_TextChanged);
            // 
            // RemarkTextBox
            // 
            resources.ApplyResources(this.RemarkTextBox, "RemarkTextBox");
            this.RemarkTextBox.Name = "RemarkTextBox";
            this.RemarkTextBox.TextChanged += new System.EventHandler(this.Remark_TextChanged);
            // 
            // RegexLabel
            // 
            resources.ApplyResources(this.RegexLabel, "RegexLabel");
            this.RegexLabel.Name = "RegexLabel";
            // 
            // RemarkLabel
            // 
            resources.ApplyResources(this.RemarkLabel, "RemarkLabel");
            this.RemarkLabel.Name = "RemarkLabel";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Confirm
            // 
            resources.ApplyResources(this.Confirm, "Confirm");
            this.Confirm.Name = "Confirm";
            this.Confirm.UseVisualStyleBackColor = true;
            this.Confirm.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // Cancel
            // 
            resources.ApplyResources(this.Cancel, "Cancel");
            this.Cancel.Name = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // RegexEditer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RemarkLabel);
            this.Controls.Add(this.RegexLabel);
            this.Controls.Add(this.RemarkTextBox);
            this.Controls.Add(this.CommandTextBox);
            this.Controls.Add(this.RegexTextBox);
            this.Controls.Add(this.IsAdmin);
            this.Controls.Add(this.AreaLabel);
            this.Controls.Add(this.Area);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegexEditer";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox Area;
        public System.Windows.Forms.Label AreaLabel;
        public System.Windows.Forms.CheckBox IsAdmin;
        public System.Windows.Forms.TextBox RegexTextBox;
        public System.Windows.Forms.TextBox CommandTextBox;
        public System.Windows.Forms.TextBox RemarkTextBox;
        public System.Windows.Forms.Label RegexLabel;
        public System.Windows.Forms.Label RemarkLabel;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button Confirm;
        public System.Windows.Forms.Button Cancel;
    }
}