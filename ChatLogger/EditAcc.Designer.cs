namespace ChatLogger
{
    partial class EditAcc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditAcc));
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.toggle_autoLogin = new MetroFramework.Controls.MetroToggle();
            this.metroLink_AccountsJSONPath = new MetroFramework.Controls.MetroLink();
            this.BTN_SUBMIT = new MetroFramework.Controls.MetroButton();
            this.txtBox_pw = new MetroFramework.Controls.MetroTextBox();
            this.txtBox_user = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.combox_defaultState = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btn_delete = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(24, 62);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(104, 19);
            this.metroLabel4.TabIndex = 40;
            this.metroLabel4.Text = "Login at startup:";
            this.metroLabel4.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // toggle_autoLogin
            // 
            this.toggle_autoLogin.AutoSize = true;
            this.toggle_autoLogin.Location = new System.Drawing.Point(131, 64);
            this.toggle_autoLogin.Name = "toggle_autoLogin";
            this.toggle_autoLogin.Size = new System.Drawing.Size(80, 17);
            this.toggle_autoLogin.TabIndex = 39;
            this.toggle_autoLogin.Text = "Off";
            this.toggle_autoLogin.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.toggle_autoLogin.UseSelectable = true;
            this.toggle_autoLogin.UseStyleColors = true;
            // 
            // metroLink_AccountsJSONPath
            // 
            this.metroLink_AccountsJSONPath.Location = new System.Drawing.Point(132, 206);
            this.metroLink_AccountsJSONPath.Name = "metroLink_AccountsJSONPath";
            this.metroLink_AccountsJSONPath.Size = new System.Drawing.Size(31, 31);
            this.metroLink_AccountsJSONPath.TabIndex = 36;
            this.metroLink_AccountsJSONPath.Text = "📁";
            this.metroLink_AccountsJSONPath.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLink_AccountsJSONPath.UseCustomBackColor = true;
            this.metroLink_AccountsJSONPath.UseSelectable = true;
            this.metroLink_AccountsJSONPath.Click += new System.EventHandler(this.MetroLink_AccountsJSONPath_Click);
            // 
            // BTN_SUBMIT
            // 
            this.BTN_SUBMIT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.BTN_SUBMIT.ForeColor = System.Drawing.Color.White;
            this.BTN_SUBMIT.Location = new System.Drawing.Point(169, 206);
            this.BTN_SUBMIT.Name = "BTN_SUBMIT";
            this.BTN_SUBMIT.Size = new System.Drawing.Size(90, 31);
            this.BTN_SUBMIT.TabIndex = 35;
            this.BTN_SUBMIT.Text = "SUBMIT";
            this.BTN_SUBMIT.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.BTN_SUBMIT.UseCustomBackColor = true;
            this.BTN_SUBMIT.UseSelectable = true;
            this.BTN_SUBMIT.UseStyleColors = true;
            this.BTN_SUBMIT.Click += new System.EventHandler(this.BTN_SUBMIT_Click);
            // 
            // txtBox_pw
            // 
            // 
            // 
            // 
            this.txtBox_pw.CustomButton.Image = null;
            this.txtBox_pw.CustomButton.Location = new System.Drawing.Point(129, 1);
            this.txtBox_pw.CustomButton.Name = "";
            this.txtBox_pw.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtBox_pw.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtBox_pw.CustomButton.TabIndex = 1;
            this.txtBox_pw.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtBox_pw.CustomButton.UseSelectable = true;
            this.txtBox_pw.CustomButton.Visible = false;
            this.txtBox_pw.ForeColor = System.Drawing.Color.White;
            this.txtBox_pw.Lines = new string[0];
            this.txtBox_pw.Location = new System.Drawing.Point(108, 162);
            this.txtBox_pw.MaxLength = 32767;
            this.txtBox_pw.Name = "txtBox_pw";
            this.txtBox_pw.PasswordChar = '●';
            this.txtBox_pw.PromptText = "123456";
            this.txtBox_pw.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtBox_pw.SelectedText = "";
            this.txtBox_pw.SelectionLength = 0;
            this.txtBox_pw.SelectionStart = 0;
            this.txtBox_pw.ShortcutsEnabled = true;
            this.txtBox_pw.Size = new System.Drawing.Size(151, 23);
            this.txtBox_pw.TabIndex = 30;
            this.txtBox_pw.TabStop = false;
            this.txtBox_pw.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtBox_pw.UseCustomBackColor = true;
            this.txtBox_pw.UseCustomForeColor = true;
            this.txtBox_pw.UseSelectable = true;
            this.txtBox_pw.UseStyleColors = true;
            this.txtBox_pw.UseSystemPasswordChar = true;
            this.txtBox_pw.WaterMark = "123456";
            this.txtBox_pw.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtBox_pw.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtBox_user
            // 
            // 
            // 
            // 
            this.txtBox_user.CustomButton.Image = null;
            this.txtBox_user.CustomButton.Location = new System.Drawing.Point(129, 1);
            this.txtBox_user.CustomButton.Name = "";
            this.txtBox_user.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtBox_user.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtBox_user.CustomButton.TabIndex = 1;
            this.txtBox_user.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtBox_user.CustomButton.UseSelectable = true;
            this.txtBox_user.CustomButton.Visible = false;
            this.txtBox_user.ForeColor = System.Drawing.Color.White;
            this.txtBox_user.Lines = new string[0];
            this.txtBox_user.Location = new System.Drawing.Point(108, 133);
            this.txtBox_user.MaxLength = 32767;
            this.txtBox_user.Name = "txtBox_user";
            this.txtBox_user.PasswordChar = '\0';
            this.txtBox_user.PromptText = "ma name jeff";
            this.txtBox_user.ReadOnly = true;
            this.txtBox_user.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtBox_user.SelectedText = "";
            this.txtBox_user.SelectionLength = 0;
            this.txtBox_user.SelectionStart = 0;
            this.txtBox_user.ShortcutsEnabled = true;
            this.txtBox_user.Size = new System.Drawing.Size(151, 23);
            this.txtBox_user.TabIndex = 29;
            this.txtBox_user.TabStop = false;
            this.txtBox_user.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtBox_user.UseCustomBackColor = true;
            this.txtBox_user.UseCustomForeColor = true;
            this.txtBox_user.UseSelectable = true;
            this.txtBox_user.UseStyleColors = true;
            this.txtBox_user.WaterMark = "ma name jeff";
            this.txtBox_user.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtBox_user.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(20, 162);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(85, 19);
            this.metroLabel2.TabIndex = 32;
            this.metroLabel2.Text = "🔑 password:";
            this.metroLabel2.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(19, 137);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(87, 19);
            this.metroLabel1.TabIndex = 31;
            this.metroLabel1.Text = "👤 username:";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroStyleManager
            // 
            this.metroStyleManager.Owner = this;
            this.metroStyleManager.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // combox_defaultState
            // 
            this.combox_defaultState.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.combox_defaultState.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.combox_defaultState.FormattingEnabled = true;
            this.combox_defaultState.ItemHeight = 19;
            this.combox_defaultState.Items.AddRange(new object[] {
            "Offline",
            "Online",
            "Busy",
            "Away",
            "Snooze",
            "LookingToTrade",
            "LookingToPlay",
            "Invisible"});
            this.combox_defaultState.Location = new System.Drawing.Point(132, 87);
            this.combox_defaultState.Name = "combox_defaultState";
            this.combox_defaultState.Size = new System.Drawing.Size(125, 25);
            this.combox_defaultState.TabIndex = 59;
            this.combox_defaultState.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.combox_defaultState.UseSelectable = true;
            this.combox_defaultState.UseStyleColors = true;
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.Location = new System.Drawing.Point(87, 90);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(41, 19);
            this.metroLabel6.TabIndex = 58;
            this.metroLabel6.Text = "State:";
            this.metroLabel6.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.richTextBox1.DetectUrls = false;
            this.richTextBox1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.richTextBox1.Location = new System.Drawing.Point(49, 95);
            this.richTextBox1.MaxLength = 50;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(42, 14);
            this.richTextBox1.TabIndex = 60;
            this.richTextBox1.Text = "└───";
            // 
            // btn_delete
            // 
            this.btn_delete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btn_delete.ForeColor = System.Drawing.Color.White;
            this.btn_delete.Location = new System.Drawing.Point(19, 206);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(90, 31);
            this.btn_delete.TabIndex = 61;
            this.btn_delete.Text = "DELETE";
            this.btn_delete.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btn_delete.UseCustomBackColor = true;
            this.btn_delete.UseSelectable = true;
            this.btn_delete.UseStyleColors = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // EditAcc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 252);
            this.Controls.Add(this.btn_delete);
            this.Controls.Add(this.combox_defaultState);
            this.Controls.Add(this.metroLabel6);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.toggle_autoLogin);
            this.Controls.Add(this.metroLink_AccountsJSONPath);
            this.Controls.Add(this.BTN_SUBMIT);
            this.Controls.Add(this.txtBox_pw);
            this.Controls.Add(this.txtBox_user);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.richTextBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "EditAcc";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Style = MetroFramework.MetroColorStyle.Default;
            this.Text = "EDIT ACCOUNT";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.EditAcc_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroToggle toggle_autoLogin;
        private MetroFramework.Controls.MetroLink metroLink_AccountsJSONPath;
        private MetroFramework.Controls.MetroButton BTN_SUBMIT;
        private MetroFramework.Controls.MetroTextBox txtBox_pw;
        private MetroFramework.Controls.MetroTextBox txtBox_user;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
        private MetroFramework.Controls.MetroComboBox combox_defaultState;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private MetroFramework.Controls.MetroButton btn_delete;
    }
}