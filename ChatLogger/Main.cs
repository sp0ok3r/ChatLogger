using ChatLogger.UserSettings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using ChatLogger.Helpers;
using ChatLogger.User2Json;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Media;
using MetroFramework;

namespace ChatLogger
{
    public partial class Main : MetroFramework.Forms.MetroForm
    {
        public static string usernameJSON;
        public static string passwordJSON;
        public static string SelectedUser = "";
        private static List<SteamLoginUsers> _users;

        public Main()
        {
            InitializeComponent();
            metroTabControl.SelectedTab = metroTab_AddAcc;
            this.components.SetStyle(this);

            Region = System.Drawing.Region.FromHrgn(Helpers.Extensions.CreateRoundRectRgn(0, 0, Width, Height, 5, 5));

            lbl_connecting.Visible = false;
            lbl_currentUsername.Visible = false;
            btnLabel_PersonaAndFlag.Visible = false;
            btn_logout.Visible = false;
            panel_steamStates.Visible = false;
            picBox_SteamAvatar.Visible = false;

        }

        private void Main_Shown(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Tick += new EventHandler(Trolha_Tick);
            t.Interval = 2000;
            t.Start();

            var Settingslist = JsonConvert.DeserializeObject<ChatLoggerSettings>(File.ReadAllText(Program.SettingsJsonFile));

            //metroStyleManager.Style= (MetroFramework.MetroColorStyle)Convert.ToUInt32(Settingslist.startupColor);
            combox_Colors.SelectedIndex = Settingslist.startupColor;

            if (Settingslist.startup)
            {
                toggle_startWindows.Checked = true;
            }
            else
            {
                toggle_startWindows.Checked = false;
            }

            if (Settingslist.startMinimized)
            {
                chck_Minimized.Checked = true;
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                chck_Minimized.Checked = false;
                this.WindowState = FormWindowState.Normal;
            }

            if (Settingslist.playsound)
            {
                toggle_playSound.Checked = true;
                //  Stream str = Properties.Resources.;
                // SoundPlayer snd = new SoundPlayer(str);
                //  snd.Play();
            }
            else { toggle_playSound.Checked = false; }
        }

        public void RefreshAccountList()
        {
            AccountsList_Grid.Rows.Clear();
            foreach (var a in JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(Program.AccountsJsonFile)).Accounts)
            {
                bool LoginK = true;
                if (string.IsNullOrEmpty(a.LoginKey) || a.LoginKey == "0")
                {
                    LoginK = false;
                }

                string[] row = { a.username, (a.SteamID).ToString(), (LoginK).ToString() };
                AccountsList_Grid.Rows.Add(row);

            }
            Acc_ScrollBar.Maximum = AccountsList_Grid.Rows.Count;
            AccountsList_Grid.ClearSelection();
        }

        public static void LoginusersVDF_ToFile()
        {
            _users = GetLoginUsers().ToList();

            if (_users.Count == 0)
            {
                throw new IOException(@"Cannot find saved users!");
            }
            foreach (var user in _users)
            {
                var list = JsonConvert.DeserializeObject<UserSettings.RootObject>(File.ReadAllText(Program.AccountsJsonFile));
                foreach (var a in list.Accounts)
                {
                    if (a.username == user.AccountName)
                    {
                        return;
                    }
                }
                list.Accounts.Add(new UserAccounts
                {
                    username = user.AccountName,
                    password = "",
                    SteamID = user.SteamId64,
                    LoginKey = "0"
                });
                var convertedJson = JsonConvert.SerializeObject(list, new JsonSerializerSettings { Formatting = Formatting.Indented });
                File.WriteAllText(Program.AccountsJsonFile, convertedJson);
            }
        }


        public static IEnumerable<User2Json.SteamLoginUsers> GetLoginUsers()
        {
            if (SteamPath.SteamLocation == null)
            {
                SteamPath.Init();
            }

            dynamic volvo = VdfConvert.Deserialize(File.ReadAllText(SteamPath.SteamLocation + @"\config\loginusers.vdf"));
            VToken v2 = volvo.Value;
            return v2.Children().Select(child => new User2Json.SteamLoginUsers(child)).Where(user => user.RememberPassword).OrderByDescending(user => user.LastLoginTime).ToList();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(Program.AccountsJsonFile))
            {
                var DefaultJson = "{Accounts: []}";
                File.WriteAllText(Program.AccountsJsonFile, DefaultJson);
            }
            try // Saved some gamers (900-10)iq
            {
                LoginusersVDF_ToFile();
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory not found, but starting anyway...");
            }
            if (!File.Exists(Program.SettingsJsonFile))
            {
                var DefaultJson = "{}";
                File.WriteAllText(Program.SettingsJsonFile, DefaultJson);
            }

            if (!File.Exists(Program.SentryFolder))
            {
                Directory.CreateDirectory(Program.SentryFolder);
            }

            if (!File.Exists(Program.ChatLogsFolder))
            {
                Directory.CreateDirectory(Program.ChatLogsFolder);
            }
            RefreshAccountList();

            txtBox_logDir.Text = Program.ChatLogsFolder;
        }
        private void HandleFormAddAccClosed(Object sender, FormClosedEventArgs e)
        {
            RefreshAccountList();
            btn_addAcc.Enabled = true;
        }
        private void HandleFormEditAccClosed(Object sender, FormClosedEventArgs e)
        {
            RefreshAccountList();
            btn_editAcc.Enabled = true;
        }
        private void btn_addAcc_Click(object sender, EventArgs e)
        {
            btn_addAcc.Enabled = false;
            Form AddAcc = new AddAcc();
            AddAcc.FormClosed += HandleFormAddAccClosed;
            AddAcc.Show();
        }

        private void btn_editAcc_Click(object sender, EventArgs e)
        {
            if (AccountsList_Grid.SelectedRows.Count > 0)
            {
                SelectedUser = AccountsList_Grid.SelectedRows[0].Cells[0].Value.ToString();
                btn_editAcc.Enabled = false;
                Form EditAcc = new EditAcc();
                EditAcc.FormClosed += HandleFormEditAccClosed;
                EditAcc.Show();
            }
            else
            {
                InfoForm.InfoHelper.CustomMessageBox.Show("Error", "Please select an account!");
            }
        }

        private void AccountsList_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedUser = AccountsList_Grid.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void btn_login2selected_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(SelectedUser))
            {
                InfoForm.InfoHelper.CustomMessageBox.Show("Info", "Please select an account!");
                return;
            }

            var list = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(Program.AccountsJsonFile));

            foreach (var a in list.Accounts)
            {
                if (a.username == SelectedUser)
                {
                    if (string.IsNullOrEmpty(a.password))
                    {
                        InfoForm.InfoHelper.CustomMessageBox.Show("Info", "Please add password to: " + a.username);
                        return;
                    }
                    usernameJSON = a.username;
                    passwordJSON = a.password;
                }
            }
            // Start Login
            Thread doLogin = new Thread(() => AccountLogin.UserSettingsGather(usernameJSON, passwordJSON));
            doLogin.Start();
            btn_login2selected.Enabled = false;

        }

        private void Acc_ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue >= AccountsList_Grid.Rows.Count)
            {
                return;
            }
            AccountsList_Grid.FirstDisplayedScrollingRowIndex = e.NewValue;
        }

        private void metroLink_spk_Click(object sender, EventArgs e)
        {
            Process.Start("http://steamcommunity.com/profiles/76561198041931474");
        }

        private void metroLink_spkMusic_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/watch?v=3mACun803qU");
        }

        private void Trolha_Tick(object sender, EventArgs e)
        {
            try
            {
                if (AccountLogin.IsLoggedIn == true)
                {
                    btn_login2selected.Enabled = false;

                    lbl_connecting.Visible = true;
                    lbl_currentUsername.Visible = true;
                    btnLabel_PersonaAndFlag.Visible = true;
                    btn_logout.Visible = true;
                    panel_steamStates.Visible = true;
                    picBox_SteamAvatar.Visible = true;

                    byte[] data = new WebClient().DownloadData("https://www.countryflags.io/" + AccountLogin.UserCountry + "/flat/16.png");
                    MemoryStream ms = new MemoryStream(data);
                    btnLabel_PersonaAndFlag.Image = Image.FromStream(ms);

                    btnLabel_PersonaAndFlag.Invoke(new Action(() => btnLabel_PersonaAndFlag.Text = AccountLogin.UserPersonaName));

                    panel_steamStates.BackColor = Color.LightSkyBlue;
                    picBox_SteamAvatar.ImageLocation = AccountLogin.GetAvatarLink(AccountLogin.CurrentSteamID);
                    lbl_currentUsername.Invoke(new Action(() => lbl_currentUsername.Text = AccountLogin.CurrentUsername));
                }
                else
                {
                    lbl_connecting.Visible = false;
                    lbl_currentUsername.Visible = false;
                    btnLabel_PersonaAndFlag.Visible = false;
                    btn_logout.Visible = false;
                    panel_steamStates.Visible = false;
                    picBox_SteamAvatar.Visible = false;


                    btn_login2selected.Enabled = true;
                    btnLabel_PersonaAndFlag.Image = Properties.Resources.notloggedFlag;
                    btnLabel_PersonaAndFlag.Invoke(new Action(() => btnLabel_PersonaAndFlag.Text = "None"));


                    panel_steamStates.BackColor = Color.Gray;
                    picBox_SteamAvatar.BackColor = Color.FromArgb(255, 25, 25, 25);
                    lbl_currentUsername.Invoke(new Action(() => lbl_currentUsername.Text = "None"));
                    //  return;
                }
            }
            catch (Exception ewe)
            {
                InfoForm.InfoHelper.CustomMessageBox.Show("Error", e.ToString());
                Console.WriteLine(ewe);
            }
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            if (AccountLogin.IsLoggedIn == true)
            {
                AccountLogin.Logout();
            }
            else
            {
                InfoForm.InfoHelper.CustomMessageBox.Show("Error", "Not logged!");
            }
        }

        private void toggle_startWindows_CheckedChanged(object sender, EventArgs e)
        {
            var Settingslist = JsonConvert.DeserializeObject<ChatLoggerSettings>(File.ReadAllText(Program.SettingsJsonFile));

            if (toggle_startWindows.Checked)
            {
                Settingslist.startup = true;

                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                key.SetValue("ChatLogger", Program.ExecutablePath + @"\ChatLogger.exe");
            }
            else
            {
                Settingslist.startup = false;

                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                key.DeleteValue("ChatLogger", false);
            }
            File.WriteAllText(Program.SettingsJsonFile, JsonConvert.SerializeObject(Settingslist, Formatting.Indented));
        }

        private void chck_Minimized_CheckedChanged(object sender, EventArgs e)
        {
            var Settingslist = JsonConvert.DeserializeObject<ChatLoggerSettings>(File.ReadAllText(Program.SettingsJsonFile));

            if (chck_Minimized.Checked)
            {
                Settingslist.startMinimized = true;
            }
            else
            {
                Settingslist.startMinimized = false;
            }
            File.WriteAllText(Program.SettingsJsonFile, JsonConvert.SerializeObject(Settingslist, Formatting.Indented));
        }

        private void toggle_playSound_CheckedChanged(object sender, EventArgs e)
        {
            var Settingslist = JsonConvert.DeserializeObject<ChatLoggerSettings>(File.ReadAllText(Program.SettingsJsonFile));
            if (toggle_playSound.Checked)
            {
                Settingslist.playsound = true;
            }
            else
            {
                Settingslist.playsound = false;
            }
            File.WriteAllText(Program.SettingsJsonFile, JsonConvert.SerializeObject(Settingslist, Formatting.Indented));
        }

        private void btn_separationSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtBox_saveSeparator.Text))
            {
                var Settingslist = JsonConvert.DeserializeObject<ChatLoggerSettings>(File.ReadAllText(Program.SettingsJsonFile));

                Settingslist.Separator = txtBox_saveSeparator.Text;

                File.WriteAllText(Program.SettingsJsonFile, JsonConvert.SerializeObject(Settingslist, Formatting.Indented));
            }
            else
            {
                InfoForm.InfoHelper.CustomMessageBox.Show("Error", "Please write something :/");
            }
        }

        private void metroLink_ChatLogsPath_Click(object sender, EventArgs e)
        {
            if (AccountLogin.IsLoggedIn == true)
            {
                Process.Start(Program.ChatLogsFolder + @"\" + AccountLogin.steamID);
            }
            else
            {
                Process.Start(Program.ChatLogsFolder);
            }
        }

        private void combox_Colors_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Settingslist = JsonConvert.DeserializeObject<ChatLoggerSettings>(File.ReadAllText(Program.SettingsJsonFile));

            Settingslist.startupColor = combox_Colors.SelectedIndex;

            File.WriteAllText(Program.SettingsJsonFile, JsonConvert.SerializeObject(Settingslist, new JsonSerializerSettings { Formatting = Formatting.Indented }));

            this.components.SetStyle(this);
            this.Refresh();
        }
    }
}

