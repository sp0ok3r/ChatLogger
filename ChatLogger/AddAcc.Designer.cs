﻿namespace ChatLogger
{
    partial class AddAcc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddAcc));
            this.metroLink_AccountsJSONPath = new MetroFramework.Controls.MetroLink();
            this.txtBox_AccPW = new MetroFramework.Controls.MetroTextBox();
            this.txtBox_AccUser = new MetroFramework.Controls.MetroTextBox();
            this.btn_addAcc = new MetroFramework.Controls.MetroButton();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroLink_AccountsJSONPath
            // 
            this.metroLink_AccountsJSONPath.FontSize = MetroFramework.MetroLinkSize.Tall;
            this.metroLink_AccountsJSONPath.Location = new System.Drawing.Point(137, 133);
            this.metroLink_AccountsJSONPath.Name = "metroLink_AccountsJSONPath";
            this.metroLink_AccountsJSONPath.Size = new System.Drawing.Size(43, 31);
            this.metroLink_AccountsJSONPath.TabIndex = 23;
            this.metroLink_AccountsJSONPath.Text = "📁";
            this.metroLink_AccountsJSONPath.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLink_AccountsJSONPath.UseCustomBackColor = true;
            this.metroLink_AccountsJSONPath.UseSelectable = true;
            this.metroLink_AccountsJSONPath.Click += new System.EventHandler(this.metroLink_AccountsJSONPath_Click);
            // 
            // txtBox_AccPW
            // 
            // 
            // 
            // 
            this.txtBox_AccPW.CustomButton.Image = null;
            this.txtBox_AccPW.CustomButton.Location = new System.Drawing.Point(137, 1);
            this.txtBox_AccPW.CustomButton.Name = "";
            this.txtBox_AccPW.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtBox_AccPW.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtBox_AccPW.CustomButton.TabIndex = 1;
            this.txtBox_AccPW.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtBox_AccPW.CustomButton.UseSelectable = true;
            this.txtBox_AccPW.CustomButton.Visible = false;
            this.txtBox_AccPW.ForeColor = System.Drawing.Color.White;
            this.txtBox_AccPW.Lines = new string[0];
            this.txtBox_AccPW.Location = new System.Drawing.Point(117, 95);
            this.txtBox_AccPW.MaxLength = 64;
            this.txtBox_AccPW.Name = "txtBox_AccPW";
            this.txtBox_AccPW.PasswordChar = '●';
            this.txtBox_AccPW.PromptText = "123456";
            this.txtBox_AccPW.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtBox_AccPW.SelectedText = "";
            this.txtBox_AccPW.SelectionLength = 0;
            this.txtBox_AccPW.SelectionStart = 0;
            this.txtBox_AccPW.ShortcutsEnabled = true;
            this.txtBox_AccPW.Size = new System.Drawing.Size(159, 23);
            this.txtBox_AccPW.TabIndex = 22;
            this.txtBox_AccPW.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtBox_AccPW.UseCustomBackColor = true;
            this.txtBox_AccPW.UseCustomForeColor = true;
            this.txtBox_AccPW.UseSelectable = true;
            this.txtBox_AccPW.UseStyleColors = true;
            this.txtBox_AccPW.UseSystemPasswordChar = true;
            this.txtBox_AccPW.WaterMark = "123456";
            this.txtBox_AccPW.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtBox_AccPW.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtBox_AccUser
            // 
            // 
            // 
            // 
            this.txtBox_AccUser.CustomButton.Image = null;
            this.txtBox_AccUser.CustomButton.Location = new System.Drawing.Point(137, 1);
            this.txtBox_AccUser.CustomButton.Name = "";
            this.txtBox_AccUser.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtBox_AccUser.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtBox_AccUser.CustomButton.TabIndex = 1;
            this.txtBox_AccUser.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtBox_AccUser.CustomButton.UseSelectable = true;
            this.txtBox_AccUser.CustomButton.Visible = false;
            this.txtBox_AccUser.ForeColor = System.Drawing.Color.White;
            this.txtBox_AccUser.Lines = new string[0];
            this.txtBox_AccUser.Location = new System.Drawing.Point(117, 66);
            this.txtBox_AccUser.MaxLength = 32;
            this.txtBox_AccUser.Name = "txtBox_AccUser";
            this.txtBox_AccUser.PasswordChar = '\0';
            this.txtBox_AccUser.PromptText = "ma name jeff";
            this.txtBox_AccUser.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtBox_AccUser.SelectedText = "";
            this.txtBox_AccUser.SelectionLength = 0;
            this.txtBox_AccUser.SelectionStart = 0;
            this.txtBox_AccUser.ShortcutsEnabled = true;
            this.txtBox_AccUser.Size = new System.Drawing.Size(159, 23);
            this.txtBox_AccUser.TabIndex = 21;
            this.txtBox_AccUser.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtBox_AccUser.UseCustomBackColor = true;
            this.txtBox_AccUser.UseCustomForeColor = true;
            this.txtBox_AccUser.UseSelectable = true;
            this.txtBox_AccUser.UseStyleColors = true;
            this.txtBox_AccUser.WaterMark = "ma name jeff";
            this.txtBox_AccUser.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtBox_AccUser.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // btn_addAcc
            // 
            this.btn_addAcc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btn_addAcc.ForeColor = System.Drawing.Color.White;
            this.btn_addAcc.Location = new System.Drawing.Point(186, 133);
            this.btn_addAcc.Name = "btn_addAcc";
            this.btn_addAcc.Size = new System.Drawing.Size(90, 31);
            this.btn_addAcc.TabIndex = 20;
            this.btn_addAcc.TabStop = false;
            this.btn_addAcc.Text = "SUBMIT";
            this.btn_addAcc.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btn_addAcc.UseCustomBackColor = true;
            this.btn_addAcc.UseSelectable = true;
            this.btn_addAcc.UseStyleColors = true;
            this.btn_addAcc.Click += new System.EventHandler(this.btn_addAcc_Click);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(28, 95);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(90, 19);
            this.metroLabel2.TabIndex = 19;
            this.metroLabel2.Text = "🔑 password:";
            this.metroLabel2.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(26, 70);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(89, 19);
            this.metroLabel1.TabIndex = 18;
            this.metroLabel1.Text = "👤 username:";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // AddAcc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 175);
            this.Controls.Add(this.metroLink_AccountsJSONPath);
            this.Controls.Add(this.txtBox_AccPW);
            this.Controls.Add(this.txtBox_AccUser);
            this.Controls.Add(this.btn_addAcc);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AddAcc";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Style = MetroFramework.MetroColorStyle.Default;
            this.Text = "ADD ACCOUNT";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroLink metroLink_AccountsJSONPath;
        private MetroFramework.Controls.MetroTextBox txtBox_AccPW;
        private MetroFramework.Controls.MetroTextBox txtBox_AccUser;
        private MetroFramework.Controls.MetroButton btn_addAcc;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel1;
    }
}