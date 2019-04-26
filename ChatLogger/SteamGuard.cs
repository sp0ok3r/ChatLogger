
using System;
using System.Windows.Forms;
using ChatLogger.Helpers;

namespace ChatLogger
{
    public partial class SteamGuard : MetroFramework.Forms.MetroForm
    {
        public static string AuthCode;

        public SteamGuard()
        {
            InitializeComponent(); this.Activate();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Helpers.Extensions.CreateRoundRectRgn(0, 0, Width, Height, 5, 5));
           
        }

        private void SteamGuard_Load(object sender, EventArgs e)
        {
            txtBox_Code.Focus();
        }

        private void btn_submit_Click(object sender, EventArgs e)
        {
            if (AuthCode != "")
            {
                AuthCode = txtBox_Code.Text;
                this.Close();
            }
        }
    }
}
