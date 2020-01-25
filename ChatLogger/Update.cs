using ChatLogger.Helpers;
using ChatLogger.User2Json;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Win32Interop.Methods;
using System.IO;

namespace ChatLogger
{
    public partial class Update : MetroFramework.Forms.MetroForm
    {
        private static string newVersion;
        public Update(string up)
        {
            InitializeComponent();
            newVersion = up;
            lbl_infoversion.Text = up;
            this.components.SetStyle(this);
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Gdi32.CreateRoundRectRgn(0, 0, Width, Height, 5, 5));
            
            IntPtr ptr = Gdi32.CreateRoundRectRgn(1, 1, btn_installupdate.Width, btn_installupdate.Height, 5, 5);
            btn_installupdate.Region = Region.FromHrgn(ptr);
            Gdi32.DeleteObject(ptr);
        }
        private void Update_Shown(object sender, EventArgs e)
        {
            this.Activate();
            RetrieveChangelog();
        }
        private void Update_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        public void RetrieveChangelog()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = System.Text.Encoding.UTF8;
                    txtBox_changelog.Text += client.DownloadString(Program.spkDomain + "update-changelog.php");
                }
            }
            catch (Exception)
            {
                InfoForm.InfoHelper.CustomMessageBox.Show("Error", "sp0ok3r.tk is down, entering in another link!");
                Process.Start("https://github.com/sp0ok3r/ChatLogger/releases");
            }
        }


        private void btn_installupdate_Click(object sender, EventArgs e)
        {
            Process.Start(Program.ExecutablePath);
            Process.Start("https://github.com/sp0ok3r/ChatLogger/");
            Process.Start("https://github.com/sp0ok3r/ChatLogger/releases/latest/ChatLogger" + newVersion + ".zip");

            var SettingsList = JsonConvert.DeserializeObject<ChatLoggerSettings>(File.ReadAllText(Program.SettingsJsonFile));
            SettingsList.LastTimeCheckedUpdate = DateTime.Now.ToString();
        }
    }
}
