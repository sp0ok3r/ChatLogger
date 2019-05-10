using ChatLogger.User2Json;
using ChatLogger.UserSettings;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using ChatLogger.Helpers;
using Win32Interop.Methods;
using System.Drawing;
using System.Linq;

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
            Region = Region.FromHrgn(Gdi32.CreateRoundRectRgn(0, 0, Width, Height, 5, 5));

            foreach (var button in this.Controls.OfType<MetroFramework.Controls.MetroButton>())
            {
                IntPtr ptr = Gdi32.CreateRoundRectRgn(1, 1, button.Width, button.Height, 5, 5);
                button.Region = Region.FromHrgn(ptr);
                Gdi32.DeleteObject(ptr);
            }

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
                    combox_defaultState.SelectedIndex = a.LoginState;
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
            if (selectedSteamID == 0)
            {
                toggle_autoLogin.Enabled = false;
                toggle_autoLogin.Checked = false;
            }
        }

        private void BTN_SUBMIT_Click(object sender, EventArgs e)
        {
            var list = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(Program.AccountsJsonFile));
            foreach (var a in list.Accounts)
            {
                if (a.username == txtBox_user.Text)
                {
                    a.LoginState = combox_defaultState.SelectedIndex;
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

        private void btn_delete_Click(object sender, EventArgs e)
        {
            var list = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(Program.AccountsJsonFile));

            list.Accounts.RemoveAll(x => x.username == Main.SelectedUser);

            File.WriteAllText(Program.AccountsJsonFile, JsonConvert.SerializeObject(list, Formatting.Indented));
            Close();
        }
    }
}
