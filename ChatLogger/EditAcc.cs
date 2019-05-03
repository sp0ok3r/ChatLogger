using ChatLogger.User2Json;
using ChatLogger.UserSettings;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using ChatLogger.Helpers;

namespace ChatLogger
{
    public partial class EditAcc : MetroFramework.Forms.MetroForm
    {
        public EditAcc()
        {
            InitializeComponent();
            this.components.SetStyle(this);

            this.Activate();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Helpers.Extensions.CreateRoundRectRgn(0, 0, Width, Height, 5, 5));
        }

        private void EditAcc_Load(object sender, EventArgs e)
        {
            EditSelected(Main.SelectedUser);
        }
        private ulong selectedSteamID = 0;
        public void EditSelected(string user)
        {
            var list = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(Program.AccountsJsonFile));
            foreach (var a in list.Accounts)
            {
                if (a.username == user)
                {
                    txtBox_user.Text = a.username;
                    txtBox_pw.Text = a.password;
                    selectedSteamID = a.SteamID;
                }
            }

            var Settingslist = JsonConvert.DeserializeObject<ChatLoggerSettings>(File.ReadAllText(Program.SettingsJsonFile));

            if (Settingslist.startupAcc == selectedSteamID && Settingslist.startupAcc.ToString().Length > 0)
            {
                toggle_autoLogin.Enabled = true;
                toggle_autoLogin.Checked = true;
            }
            if (Settingslist.startupAcc != 0 && Settingslist.startupAcc != selectedSteamID)
            {
                toggle_autoLogin.Enabled = false;
            }
        }

        private void BTN_SUBMIT_Click(object sender, EventArgs e)
        {

            var list = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(Program.AccountsJsonFile));
            foreach (var a in list.Accounts)
            {
                if (a.username == txtBox_user.Text)
                {
                    a.password = txtBox_pw.Text;
                }
            }
            File.WriteAllText(Program.AccountsJsonFile, JsonConvert.SerializeObject(list, Formatting.Indented));

            var Settingslist = JsonConvert.DeserializeObject<ChatLoggerSettings>(File.ReadAllText(Program.SettingsJsonFile));

            if (!toggle_autoLogin.Checked)
            {
                Settingslist.startupAcc = 0;
            }
            else
            {
                Settingslist.startupAcc = selectedSteamID;
            }

            File.WriteAllText(Program.SettingsJsonFile, JsonConvert.SerializeObject(Settingslist, Formatting.Indented));
            Close();
        }

        private void MetroLink_AccountsJSONPath_Click(object sender, EventArgs e)
        {
            Process.Start(Program.AccountsJsonFile);
        }
    }
}
