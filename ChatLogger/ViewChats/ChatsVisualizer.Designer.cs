using Awesomium.Windows.Forms;

namespace ChatLogger.ViewChats
{
    partial class ChatsVisualizer
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
            this.text_search = new MetroFramework.Controls.MetroTextBox();
            this.SuspendLayout();
            // 
            // text_search
            // 
            this.text_search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.text_search.CustomButton.Image = null;
            this.text_search.CustomButton.Location = new System.Drawing.Point(210, 1);
            this.text_search.CustomButton.Name = "";
            this.text_search.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.text_search.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.text_search.CustomButton.TabIndex = 1;
            this.text_search.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.text_search.CustomButton.UseSelectable = true;
            this.text_search.CustomButton.Visible = false;
            this.text_search.Lines = new string[] {
        "Search"};
            this.text_search.Location = new System.Drawing.Point(23, 18);
            this.text_search.MaxLength = 32767;
            this.text_search.Name = "text_search";
            this.text_search.PasswordChar = '\0';
            this.text_search.PromptText = "Search...";
            this.text_search.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.text_search.SelectedText = "";
            this.text_search.SelectionLength = 0;
            this.text_search.SelectionStart = 0;
            this.text_search.ShortcutsEnabled = true;
            this.text_search.Size = new System.Drawing.Size(248, 23);
            this.text_search.Style = MetroFramework.MetroColorStyle.Blue;
            this.text_search.TabIndex = 2;
            this.text_search.TabStop = false;
            this.text_search.Text = "Search";
            this.text_search.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.text_search.UseSelectable = true;
            this.text_search.WaterMark = "Search...";
            this.text_search.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.text_search.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // ChatsVisualizer
            // 
            this.ClientSize = new System.Drawing.Size(1081, 560);
            this.Controls.Add(this.text_search);
            this.Name = "ChatsVisualizer";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTextBox text_search;
    }
}