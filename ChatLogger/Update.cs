using ChatLogger.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatLogger
{
    public partial class Update : MetroFramework.Forms.MetroForm
    {
        public Update(string up)
        {
            InitializeComponent();
            lbl_infoversion.Text = up;
            this.components.SetStyle(this);
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Helpers.Extensions.CreateRoundRectRgn(0, 0, Width, Height, 5, 5));
        }
        private void Update_Shown(object sender, EventArgs e)
        {
            this.BringToFront();
            this.Activate();
            RetrieveChangelog();
        }
        private void Update_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private string DownloadLink;
        private void RetrieveChangelog()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Steam Chat");
                    client.Encoding = Encoding.UTF8;

                    Uri uri = new Uri(Program.GITHUB_PROJECT);
                    string releases = client.DownloadString(uri);
                    foreach (var g in JsonConvert.DeserializeObject<List<User2Json.GitHubApi.GithubRelease>>(releases))
                    {
                        txtBox_changelog.Text += g.body;
                        DownloadLink = g.assets[0].browser_download_url;
                    }
                }
            }
            catch (Exception)
            {
                InfoForm.InfoHelper.CustomMessageBox.Show("Error", "github error");
                Process.Start("https://github.com/sp0ok3r/Mercury/releases");
            }
        }


        private void btn_installupdate_Click(object sender, EventArgs e)
        {
            Process.Start(Program.ExecutablePath);
            Process.Start(DownloadLink);
        }
    }
}
