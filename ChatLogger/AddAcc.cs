using ChatLogger.UserSettings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ChatLogger.Helpers;
using Win32Interop.Methods;

namespace ChatLogger
{
    public partial class AddAcc : MetroFramework.Forms.MetroForm
    {
        public AddAcc()
        {
            InitializeComponent(); this.Activate();
            this.components.SetStyle(this);
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Gdi32.CreateRoundRectRgn(0, 0, Width, Height, 5, 5));

            IntPtr ptr = Gdi32.CreateRoundRectRgn(1, 1, btn_addAcc.Width, btn_addAcc.Height, 5, 5);
            btn_addAcc.Region = Region.FromHrgn(ptr);
            Gdi32.DeleteObject(ptr);

        }
        private void btn_addAcc_Click(object sender, EventArgs e)
        {
            AddAccJson(txtBox_AccUser.Text, txtBox_AccPW.Text);
        }

        public void AddAccJson(string user, string password)
        {
            try
            {
                var list = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(Program.AccountsJsonFile));
                foreach (var a in list.Accounts)
                {
                    if (a.username == user)
                    {
                        InfoForm.InfoHelper.CustomMessageBox.Show("Alert", "Duplicate username found!");
                        return;
                    }
                }

                ulong UserSteamID = 0;

                list.Accounts.Add(new UserAccounts
                {
                    LoginState = 1,
                    username = user,
                    password = password,
                    LoginKey = "0",
                    SteamID = UserSteamID
                });

                var convertedJson = JsonConvert.SerializeObject(list, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    Formatting = Formatting.Indented
                });

                File.WriteAllText(Program.AccountsJsonFile, convertedJson);
                Close();
            }
            catch
            {
                // log error
            }
        }

        #region link2json
        private void metroLink_AccountsJSONPath_Click(object sender, EventArgs e)
        {
            Process.Start(Program.AccountsJsonFile);
        }
        #endregion

    }
}
