namespace ChatLogger
{
    partial class SteamGuard
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SteamGuard));
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.btn_submit = new MetroFramework.Controls.MetroButton();
            this.txtBox_Code = new MetroFramework.Controls.MetroTextBox();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel1.ForeColor = System.Drawing.Color.White;
            this.metroLabel1.Location = new System.Drawing.Point(6, 73);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(208, 26);
            this.metroLabel1.Style = MetroFramework.MetroColorStyle.Purple;
            this.metroLabel1.TabIndex = 2;
            this.metroLabel1.Text = "Enter Steam-Guard Code:\r\n";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel1.UseCustomForeColor = true;
            this.metroLabel1.UseStyleColors = true;
            // 
            // btn_submit
            // 
            this.btn_submit.Location = new System.Drawing.Point(220, 108);
            this.btn_submit.Name = "btn_submit";
            this.btn_submit.Size = new System.Drawing.Size(89, 27);
            this.btn_submit.TabIndex = 3;
            this.btn_submit.Text = "SUBMIT";
            this.btn_submit.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btn_submit.UseSelectable = true;
            this.btn_submit.Click += new System.EventHandler(this.btn_submit_Click);
            // 
            // txtBox_Code
            // 
            // 
            // 
            // 
            this.txtBox_Code.CustomButton.Image = null;
            this.txtBox_Code.CustomButton.Location = new System.Drawing.Point(67, 1);
            this.txtBox_Code.CustomButton.Name = "";
            this.txtBox_Code.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtBox_Code.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtBox_Code.CustomButton.TabIndex = 1;
            this.txtBox_Code.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtBox_Code.CustomButton.UseSelectable = true;
            this.txtBox_Code.CustomButton.Visible = false;
            this.txtBox_Code.ForeColor = System.Drawing.Color.White;
            this.txtBox_Code.Lines = new string[0];
            this.txtBox_Code.Location = new System.Drawing.Point(220, 76);
            this.txtBox_Code.MaxLength = 10;
            this.txtBox_Code.Name = "txtBox_Code";
            this.txtBox_Code.PasswordChar = '\0';
            this.txtBox_Code.PromptText = "GABEN";
            this.txtBox_Code.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtBox_Code.SelectedText = "";
            this.txtBox_Code.SelectionLength = 0;
            this.txtBox_Code.SelectionStart = 0;
            this.txtBox_Code.ShortcutsEnabled = true;
            this.txtBox_Code.Size = new System.Drawing.Size(89, 23);
            this.txtBox_Code.TabIndex = 0;
            this.txtBox_Code.UseCustomBackColor = true;
            this.txtBox_Code.UseCustomForeColor = true;
            this.txtBox_Code.UseSelectable = true;
            this.txtBox_Code.UseStyleColors = true;
            this.txtBox_Code.WaterMark = "GABEN";
            this.txtBox_Code.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtBox_Code.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroStyleManager
            // 
            this.metroStyleManager.Owner = this;
            this.metroStyleManager.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 36);
            this.label1.TabIndex = 4;
            this.label1.Text = "📱";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(23, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(140, 36);
            this.panel1.TabIndex = 5;
            // 
            // SteamGuard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 140);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtBox_Code);
            this.Controls.Add(this.btn_submit);
            this.Controls.Add(this.metroLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SteamGuard";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Style = MetroFramework.MetroColorStyle.Default;
            this.Text = "Steam Guard";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Load += new System.EventHandler(this.SteamGuard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroButton btn_submit;
        private MetroFramework.Controls.MetroTextBox txtBox_Code;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}