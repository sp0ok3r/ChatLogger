﻿namespace ChatLogger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SteamGuard));
            this.lbl_infoemailorPhone = new MetroFramework.Controls.MetroLabel();
            this.btn_submit = new MetroFramework.Controls.MetroButton();
            this.txtBox_Code = new MetroFramework.Controls.MetroTextBox();
            this.lbl_emojiInfo = new System.Windows.Forms.Label();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.btn_cancel = new MetroFramework.Controls.MetroButton();
            this.lbl_account = new MetroFramework.Controls.MetroLabel();
            this.MongoToolTip = new MetroFramework.Components.MetroToolTip();
            this.SuspendLayout();
            // 
            // lbl_infoemailorPhone
            // 
            this.lbl_infoemailorPhone.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_infoemailorPhone.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lbl_infoemailorPhone.ForeColor = System.Drawing.Color.Silver;
            this.lbl_infoemailorPhone.Location = new System.Drawing.Point(12, 71);
            this.lbl_infoemailorPhone.Name = "lbl_infoemailorPhone";
            this.lbl_infoemailorPhone.Size = new System.Drawing.Size(247, 19);
            this.lbl_infoemailorPhone.Style = MetroFramework.MetroColorStyle.Purple;
            this.lbl_infoemailorPhone.TabIndex = 2;
            this.lbl_infoemailorPhone.Text = "Enter your two-factor authentication code\r\n";
            this.lbl_infoemailorPhone.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.lbl_infoemailorPhone.UseCustomForeColor = true;
            this.lbl_infoemailorPhone.UseStyleColors = true;
            // 
            // btn_submit
            // 
            this.btn_submit.Location = new System.Drawing.Point(207, 114);
            this.btn_submit.Name = "btn_submit";
            this.btn_submit.Size = new System.Drawing.Size(151, 36);
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
            this.txtBox_Code.CustomButton.Location = new System.Drawing.Point(69, 1);
            this.txtBox_Code.CustomButton.Name = "";
            this.txtBox_Code.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtBox_Code.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtBox_Code.CustomButton.TabIndex = 1;
            this.txtBox_Code.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtBox_Code.CustomButton.UseSelectable = true;
            this.txtBox_Code.CustomButton.Visible = false;
            this.txtBox_Code.ForeColor = System.Drawing.Color.White;
            this.txtBox_Code.Lines = new string[0];
            this.txtBox_Code.Location = new System.Drawing.Point(267, 68);
            this.txtBox_Code.MaxLength = 5;
            this.txtBox_Code.Name = "txtBox_Code";
            this.txtBox_Code.PasswordChar = '\0';
            this.txtBox_Code.PromptText = "GABEN";
            this.txtBox_Code.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtBox_Code.SelectedText = "";
            this.txtBox_Code.SelectionLength = 0;
            this.txtBox_Code.SelectionStart = 0;
            this.txtBox_Code.ShortcutsEnabled = true;
            this.txtBox_Code.Size = new System.Drawing.Size(91, 23);
            this.txtBox_Code.TabIndex = 1;
            this.txtBox_Code.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBox_Code.UseCustomBackColor = true;
            this.txtBox_Code.UseCustomForeColor = true;
            this.txtBox_Code.UseSelectable = true;
            this.txtBox_Code.UseStyleColors = true;
            this.txtBox_Code.WaterMark = "GABEN";
            this.txtBox_Code.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtBox_Code.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lbl_emojiInfo
            // 
            this.lbl_emojiInfo.AutoSize = true;
            this.lbl_emojiInfo.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_emojiInfo.ForeColor = System.Drawing.Color.White;
            this.lbl_emojiInfo.Location = new System.Drawing.Point(159, 16);
            this.lbl_emojiInfo.Name = "lbl_emojiInfo";
            this.lbl_emojiInfo.Size = new System.Drawing.Size(37, 36);
            this.lbl_emojiInfo.TabIndex = 4;
            this.lbl_emojiInfo.Text = "📱";
            // 
            // metroLabel2
            // 
            this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel2.ForeColor = System.Drawing.Color.Gainsboro;
            this.metroLabel2.Location = new System.Drawing.Point(13, 86);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(52, 19);
            this.metroLabel2.Style = MetroFramework.MetroColorStyle.Purple;
            this.metroLabel2.TabIndex = 6;
            this.metroLabel2.Text = "Account:";
            this.metroLabel2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel2.UseCustomForeColor = true;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(13, 114);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(151, 36);
            this.btn_cancel.TabIndex = 7;
            this.btn_cancel.Text = "CANCEL";
            this.btn_cancel.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btn_cancel.UseSelectable = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // lbl_account
            // 
            this.lbl_account.AutoSize = true;
            this.lbl_account.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbl_account.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_account.Location = new System.Drawing.Point(62, 86);
            this.lbl_account.Name = "lbl_account";
            this.lbl_account.Size = new System.Drawing.Size(16, 15);
            this.lbl_account.TabIndex = 8;
            this.lbl_account.Text = "...";
            this.lbl_account.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.lbl_account.UseStyleColors = true;
            // 
            // MongoToolTip
            // 
            this.MongoToolTip.Style = MetroFramework.MetroColorStyle.Default;
            this.MongoToolTip.StyleManager = null;
            this.MongoToolTip.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // SteamGuard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 156);
            this.Controls.Add(this.lbl_emojiInfo);
            this.Controls.Add(this.lbl_account);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.txtBox_Code);
            this.Controls.Add(this.btn_submit);
            this.Controls.Add(this.lbl_infoemailorPhone);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SteamGuard";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Style = MetroFramework.MetroColorStyle.Default;
            this.Text = "Steam Guard";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SteamGuard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroLabel lbl_infoemailorPhone;
        private MetroFramework.Controls.MetroButton btn_submit;
        private MetroFramework.Controls.MetroTextBox txtBox_Code;
        private System.Windows.Forms.Label lbl_emojiInfo;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroButton btn_cancel;
        private MetroFramework.Controls.MetroLabel lbl_account;
        private MetroFramework.Components.MetroToolTip MongoToolTip;
    }
}