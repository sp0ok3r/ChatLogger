using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using SteamKit2;
using System.Collections;
using MetroFramework;
using MetroFramework.Forms;


namespace MistClient
{
    public partial class Friends : MetroForm
    {
        /*
        public static bool chat_opened = false;
        public static bool keepLog = true;

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        public byte[] AvatarHash { get; set; } // checking if update is necessary
        public static string MistVersion = Application.ProductVersion.Remove(Application.ProductVersion.LastIndexOf('.'));        
        bool minimizeToTray = true;
        bool statsSent = false;
        ulong contextMenuSteamId = 0;
        Form MiniProfile = new Form();

        public static List<MetroFramework.Components.MetroStyleManager> globalThemeManager = new List<MetroFramework.Components.MetroStyleManager>();
        public static MetroFramework.Components.MetroStyleManager GlobalStyleManager = new MetroFramework.Components.MetroStyleManager();

        public Friends(SteamBot.Bot bot, string username)
        {
            InitializeComponent();
            this.MouseWheel += Friends_MouseWheel;
            webControl1.MouseWheel += Friends_MouseWheel;
            webControl1.Source = ("file://" + Application.StartupPath + "/Resources/Friends.html").ToUri();
            webControl1.DocumentReady += webControl1_DocumentReady;
            webControl1.ShowContextMenu += webControl1_ShowContextMenu;
            this.StyleManager = Friends.GlobalStyleManager;
            this.StyleManager.OnThemeChanged += StyleManager_OnThemeChanged;
            Util.LoadTheme(this, this.Controls);
            this.shareUsageStatsToolStripMenuItem.Checked = Properties.Settings.Default.ShareUsageStats;
            if (Properties.Settings.Default.ShareUsageStats)
            {
                Util.SendUsageStats(bot.SteamUser.SteamID);
                statsSent = true;
            }
            this.Text = "Friends";
            this.steam_name.Text = username;
            this.bot = bot;
            this.steam_name.ContextMenuStrip = menu_status;
            this.steam_status.ContextMenuStrip = menu_status;
            this.label_settings.ContextMenuStrip = menu_status;
            this.text_search.Text = "";
            this.steam_status.TextChanged += steam_status_TextChanged;
            this.minimizeToTrayToolStripMenuItem.Checked = Properties.Settings.Default.MinimizeToTray;
            this.showOnlineFriendsOnlyToolStripMenuItem.Checked = Properties.Settings.Default.OnlineOnly;
            logConversationsToolStripMenuItem.Checked = Properties.Settings.Default.KeepLog;
            keepLog = logConversationsToolStripMenuItem.Checked;          
            // Set colors
            var onlineColor = ColorTranslator.FromHtml("#5db2ff");
            steam_name.ForeColor = onlineColor;
            steam_status.ForeColor = onlineColor;

            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Show", OnTrayIconDoubleClick);
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Mist";
            Bitmap bmp = Properties.Resources.mist_icon;
            trayIcon.Icon = Icon.FromHandle(bmp.GetHicon());

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = false;

            trayIcon.DoubleClick += new System.EventHandler(this.OnTrayIconDoubleClick);
        }

        void Friends_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!webControl1.Focused)
                webControl1.Focus();
        }

        void StyleManager_OnThemeChanged(object sender, EventArgs e)
        {
            menu_friend.StyleManager = this.StyleManager;
            menu_friendreq.StyleManager = this.StyleManager;
            menu_status.StyleManager = this.StyleManager;
            if (webControl1 == null) return;

            if (webControl1.IsDocumentReady)
            {
                if (this.StyleManager.Theme == MetroFramework.MetroThemeStyle.Dark)
                    webControl1.ExecuteJavascript("updateTheme('dark');");
                else
                    webControl1.ExecuteJavascript("updateTheme('light');");
            }
        }

        void steam_status_TextChanged(object sender, EventArgs e)
        {
            if (steam_status.Text == "Offline")
            {
                steam_name.ForeColor = SystemColors.ControlDark;
                steam_status.ForeColor = SystemColors.ControlDark;
            }
            else
            {
                var onlineColor = ColorTranslator.FromHtml("#5db2ff");
                steam_name.ForeColor = onlineColor;
                steam_status.ForeColor = onlineColor;
            }
        }

        void webControl1_ShowContextMenu(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }

        void webControl1_DocumentReady(object sender, UrlEventArgs e)
        {            
            webControl1.DocumentReady -= webControl1_DocumentReady;
            while (webControl1.ExecuteJavascriptWithResult("document.body.innerHTML").IsUndefined)
            {
                WebCore.Update();
            }
            StyleManager_OnThemeChanged(null, EventArgs.Empty);

            JSObject jsobject = webControl1.CreateGlobalJavascriptObject("Mist");
            jsobject.Bind("OpenChat", false, OpenChatHandler);
            jsobject.Bind("ShowMenu", false, ShowMenuHandler);
            jsobject.Bind("ShowMiniProfile", false, ShowMiniProfile);
            jsobject.Bind("HideMiniProfile", false, HideMiniProfile);
            jsobject.Bind("ShowFriendReqMenu", false, ShowFriendReqMenu);

            UpdateFriendsHTML();
            UpdateFriendRequestsHTML();            
        }

        void ShowFriendReqMenu(object sender, JavascriptMethodEventArgs e)
        {
            try
            {
                contextMenuSteamId = Convert.ToUInt64(e.Arguments[0].ToString());
                menu_friendreq.Show(Cursor.Position);
            }
            catch
            {

            }
        }
        void webControlMini_DocumentReady(object sender, UrlEventArgs e, Form webControlForm)
        {
            var webControl = (Awesomium.Windows.Forms.WebControl)sender;
            while (webControl.ExecuteJavascriptWithResult("document.body.innerHTML").IsUndefined)
            {
                WebCore.Update();
            }
            var function = @"function getHeight() {
                                var body = document.body,
                                html = document.documentElement;

                                var height = Math.max( body.scrollHeight, body.offsetHeight, 
                                                       html.clientHeight, html.scrollHeight, html.offsetHeight );
                                return height;
                                }";
            int height = Convert.ToInt32(webControl.ExecuteJavascriptWithResult(function + @"getHeight();").ToString());
            webControlForm.Height = height;
        }

        void ShowMenuHandler(object sender, JavascriptMethodEventArgs e)
        {
            try
            {
                contextMenuSteamId = Convert.ToUInt64(e.Arguments[0].ToString());
                menu_friend.Show(Cursor.Position);
            }
            catch
            {

            }        
        }

        void OpenChatHandler(object sender, JavascriptMethodEventArgs e)
        {
            try
            {
                bot.main.Invoke((Action)(() =>
                {
                    ulong steamId = Convert.ToUInt64(e.Arguments[0].ToString());
                    string selected = bot.SteamFriends.GetFriendPersonaName(steamId);
                    if (!chat_opened)
                    {
                        chat = new Chat(bot);
                        chat.AddChat(selected, steamId);
                        chat.Show();
                        chat.Activate();
                        chat_opened = true;
                    }
                    else
                    {
                        bool found = false;
                        foreach (TabPage tab in chat.ChatTabControl.TabPages)
                        {
                            if ((SteamID)tab.Tag == new SteamID(steamId))
                            {
                                chat.ChatTabControl.SelectedTab = tab;
                                chat.Activate();
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            chat.AddChat(selected, steamId);
                            chat.Activate();
                        }
                    }
                }));
            }
            catch
            {

            }            
        }

        public void UpdateFriendsHTML()
        {
            bot.main.Invoke((Action)(() =>
            {
                if (!webControl1.IsDocumentReady) return;

                while (webControl1.ExecuteJavascriptWithResult("document.body.innerHTML").IsUndefined)
                {
                    WebCore.Update();
                }
                webControl1.ExecuteJavascript("updateFriends(\"" + System.Web.HttpUtility.JavaScriptStringEncode(GetFriendsHTML()) + "\");");
            }));            
        }

        private string GetFriendsHTML()
        {
            string output = "";
            string gameFriends = "";
            string onlineFriends = "";
            string offlineFriends = "";
            string scammerFriends = "";
            foreach (var friend in ListFriends.Get(Properties.Settings.Default.OnlineOnly))
            {
                string statusStyle = "";
                string html = GetFriendHTML(friend.SID, out statusStyle);
                switch (statusStyle)
                {
                    case "offline":
                        offlineFriends += html;
                        break;
                    case "ingame":
                        gameFriends += html;
                        break;
                    case "online":
                        onlineFriends += html;
                        break;
                    case "scammer":
                        scammerFriends += html;
                        break;
                }
            }
            output += scammerFriends + gameFriends + onlineFriends + offlineFriends;
            return output;
        }

        public void UpdateFriendHTML(ulong steamId)
        {
            bot.main.Invoke((Action)(() =>
            {
                while (webControl1.ExecuteJavascriptWithResult("document.body.innerHTML").IsUndefined)
                {
                    WebCore.Update();
                }
                var empty = "";
                var html = GetFriendHTML(steamId, out empty);
                if (!string.IsNullOrEmpty(html))
                    webControl1.ExecuteJavascript("updateUser(\"" + steamId + "\", \"" + System.Web.HttpUtility.JavaScriptStringEncode(html) + "\");");
            }));
        }

        public string GetFriendHTML(ulong steamId, out string statusStyle)
        {
            var friend = ListFriends.GetFriend(steamId);
            try
            {
                string friendName = System.Web.HttpUtility.HtmlEncode(friend.Name);
                byte[] avatarHash = bot.SteamFriends.GetFriendAvatar(friend.SID);
                bool validHash = avatarHash != null && !IsZeros(avatarHash);
                string hashStr = BitConverter.ToString(avatarHash).Replace("-", "").ToLower();
                string hashPrefix = hashStr.Substring(0, 2);
                string avatarUrl = string.Format("http://media.steampowered.com/steamcommunity/public/images/avatars/{0}/{1}.jpg", hashPrefix, hashStr);
                string currentGame = System.Web.HttpUtility.HtmlEncode(bot.SteamFriends.GetFriendGamePlayedName(friend.SID));
                if (friend.Status == "Offline")
                    statusStyle = "offline";
                else if (!string.IsNullOrEmpty(currentGame))
                    statusStyle = "ingame";
                else
                    statusStyle = "online";
                string steamRep = Util.GetSteamRepStatus(friend.SID);
                if (steamRep.Contains("SCAMMER"))
                {
                    statusStyle = "scammer";
                    friendName = "[WARNING: SCAMMER] " + friendName;
                }
                string status = string.IsNullOrEmpty(currentGame) ? friend.Status : "In-Game (" + friend.Status + ")";
                string html = string.Format(@"<tr id='id-{5}' data-playername='{1}' ondblclick='Mist.OpenChat(""{5}"");' oncontextmenu='Mist.ShowMenu(""{5}"");return false;'><td class='avatar'><img class='{4}' src='{0}' onmouseover='Mist.ShowMiniProfile(""{5}"");' onmouseout='Mist.HideMiniProfile();'/></td><td class='playerinfo'><div class='playername {4}'><span class='text'>{1}</span><span class='nickname'>{6}</span><span class='playeroptions'></span></div><div class='status {4}'>{2}</div><div class='gamename {4}'>{3}</div></td></tr>", avatarUrl, friendName, status, currentGame, statusStyle, friend.SID, friend.Nickname);
                return html;
            }
            catch
            {
                statusStyle = "";
                return "";
            }            
        }

        public void UpdateFriendRequestsHTML()
        {
            if (ListFriendRequests.Get().Count == 0) return;
            bot.main.Invoke((Action)(() =>
            {
                while (webControl1.ExecuteJavascriptWithResult("document.body.innerHTML").IsUndefined)
                {
                    WebCore.Update();
                }
                webControl1.ExecuteJavascript("updateFriendRequests(\"" + System.Web.HttpUtility.JavaScriptStringEncode(GetFriendRequestsHTML()) + "\");");
                webControl1.ExecuteJavascript("showFriendRequests();");
            }));
        }

        private string GetFriendRequestsHTML()
        {
            string output = "";
            string gameFriends = "";
            string onlineFriends = "";
            string offlineFriends = "";
            string scammerFriends = "";
            foreach (var friend in ListFriendRequests.Get())
            {
                string friendName = System.Web.HttpUtility.HtmlEncode(friend.Name);
                byte[] avatarHash = bot.SteamFriends.GetFriendAvatar(friend.SteamID);
                bool validHash = avatarHash != null && !IsZeros(avatarHash);
                string hashStr = BitConverter.ToString(avatarHash).Replace("-", "").ToLower();
                string hashPrefix = hashStr.Substring(0, 2);
                string avatarUrl = string.Format("http://media.steampowered.com/steamcommunity/public/images/avatars/{0}/{1}.jpg", hashPrefix, hashStr);
                string currentGame = System.Web.HttpUtility.HtmlEncode(bot.SteamFriends.GetFriendGamePlayedName(friend.SteamID));
                string statusStyle = "";                
                if (friend.Status == "Offline")
                    statusStyle = "offline";
                else if (!string.IsNullOrEmpty(currentGame))
                    statusStyle = "ingame";
                else
                    statusStyle = "online";                
                string steamRep = Util.GetSteamRepStatus(friend.SteamID);
                if (steamRep.Contains("SCAMMER"))
                {
                    statusStyle = "scammer";
                    friendName = "[WARNING: SCAMMER] " + friendName;
                }                    
                string status = string.IsNullOrEmpty(currentGame) ? friend.Status : "In-Game (" + friend.Status + ")";
                string html = string.Format(@"<tr data-playername='{1}' ondblclick='Mist.ShowFriendReqMenu(""{5}"");' oncontextmenu='Mist.ShowFriendReqMenu(""{5}"");return false;'><td class='avatar'><img class='{4}' src='{0}' onmouseover='Mist.ShowMiniProfile(""{5}"");' onmouseout='Mist.HideMiniProfile();'/></td><td class='playerinfo'><div class='playername {4}'><span class='text'>{1}</span><span class='playeroptions'></span></div><div class='status {4}'>{2}</div><div class='gamename {4}'>{3}</div></td></tr>", avatarUrl, friendName, status, currentGame, statusStyle, friend.SteamID);
                switch (statusStyle)
                {
                    case "offline":
                        offlineFriends += html;
                        break;
                    case "ingame":
                        gameFriends += html;
                        break;
                    case "online":
                        onlineFriends += html;
                        break;
                    case "scammer":
                        scammerFriends += html;
                        break;
                }
            }
            output += scammerFriends + gameFriends + onlineFriends + offlineFriends;
            return output;
        }

        public int GetNumFriendsDisplayed()
        {
            var result = -1;
            bot.main.Invoke((Action)(() =>
            {
                if (!webControl1.IsDocumentReady) return;
                while (webControl1.ExecuteJavascriptWithResult("document.body.innerHTML").IsUndefined)
                {
                    WebCore.Update();
                }
                result = Convert.ToInt32(webControl1.ExecuteJavascriptWithResult("document.getElementsByTagName('table')[1].rows.length;").ToString());
            }));
            return result;
        }

        private bool IsInGame()
        {
            try
            {
                string gameName = bot.SteamFriends.GetFriendGamePlayedName(bot.SteamUser.SteamID);
                return !string.IsNullOrEmpty(gameName);
            }
            catch
            {
                return false;
            }
        }

        private bool IsOnline()
        {
            try
            {
                return bot.SteamFriends.GetPersonaState() != SteamKit2.EPersonaState.Offline;
            }
            catch { return false; }
        }

        private Bitmap ComposeAvatar(string path)
        {
            Bitmap holder = MistClient.Properties.Resources.IconOnline;
            Bitmap avatar = Util.GetAvatar(path);

            Graphics gfx = null;
            try
            {
                gfx = Graphics.FromImage(holder);
                gfx.DrawImage(avatar, new Rectangle(2, 2, avatarBox.Width - 4, avatarBox.Height - 4));
            }
            finally
            {
                gfx.Dispose();
            }

            return holder;
        }

        void AvatarDownloaded(AvatarDownloadDetails details)
        {
            try
            {
                if (avatarBox.InvokeRequired)
                {
                    avatarBox.Invoke(new MethodInvoker(() =>
                    {
                        AvatarDownloaded(details);
                    }
                    ));
                }
                else
                {
                    avatarBox.Image = ComposeAvatar((details.Success ? details.Filename : null));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("FriendControl", "Unable to compose avatar: {0}", ex.Message);
            }
        }

        public static bool IsZeros(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] != 0)
                    return false;
            }
            return true;
        }

        void OnTrayIconDoubleClick(object sender, EventArgs e)
        {
            this.Show();            
            trayIcon.Visible = false;
        }        

        private void Friends_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (minimizeToTray)
            {                
                this.Hide();
                e.Cancel = true;
                if (trayIcon != null)
                {
                    try
                    {
                        trayIcon.Visible = true;
                        trayIcon.ShowBalloonTip(5000, "Mist has been minimized to tray", "To restore Mist, double-click the tray icon.", ToolTipIcon.Info);
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                OnExit(sender, e);
            }
        }

        private void Friends_FormClosed(object sender, FormClosedEventArgs e)
        {
            trayIcon.Icon = null;
            trayIcon.Visible = false;
            trayIcon.Dispose();
        }

        private void OnExit(object sender, EventArgs e)
        {
            try
            {
                trayIcon.Visible = false;
                trayIcon.Icon = null;
                trayIcon.Dispose();
                Application.Exit();
                Environment.Exit(0);
            }
            catch
            {

            }
        }

        public void UpdateState()
        {
            steam_name.Invoke(new Action(() => steam_name.Text = SteamBot.Bot.DisplayName));
            byte[] avatarHash = bot.SteamFriends.GetFriendAvatar(bot.SteamUser.SteamID);
            bool validHash = avatarHash != null && !IsZeros(avatarHash);
            if ((AvatarHash == null && !validHash && avatarBox.Image != null) || (AvatarHash != null && AvatarHash.SequenceEqual(avatarHash)))
            {
                // avatar is already up to date, no operations necessary
            }
            else if (validHash)
            {
                AvatarHash = avatarHash;
                CDNCache.DownloadAvatar(bot.SteamUser.SteamID, avatarHash, AvatarDownloaded);
            }
            else
            {
                AvatarHash = null;
                avatarBox.Image = ComposeAvatar(null);
            }            
        }        

        private void label1_MouseHover(object sender, EventArgs e)
        {
            if (GlobalStyleManager.Theme == MetroFramework.MetroThemeStyle.Dark)
                label_settings.ForeColor = Color.WhiteSmoke;
            else
                label_settings.ForeColor = SystemColors.ControlText;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label_settings.ForeColor = SystemColors.ControlDarkDark;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            menu_status.Show(label_settings, Cursor.HotSpot.X + 4, Cursor.HotSpot.Y + 21);
        }

        private void onlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bot.SteamFriends.SetPersonaState(SteamKit2.EPersonaState.Online);
            this.steam_status.Text = bot.SteamFriends.GetPersonaState().ToString();
        }

        private void awayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bot.SteamFriends.SetPersonaState(SteamKit2.EPersonaState.Away);
            this.steam_status.Text = bot.SteamFriends.GetPersonaState().ToString();
        }

        private void busyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bot.SteamFriends.SetPersonaState(SteamKit2.EPersonaState.Busy);
            this.steam_status.Text = bot.SteamFriends.GetPersonaState().ToString();
        }

        private void lookingToPlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bot.SteamFriends.SetPersonaState(SteamKit2.EPersonaState.LookingToPlay);
            this.steam_status.Text = bot.SteamFriends.GetPersonaState().ToString();
        }

        private void lookingToTradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bot.SteamFriends.SetPersonaState(SteamKit2.EPersonaState.LookingToTrade);
            this.steam_status.Text = bot.SteamFriends.GetPersonaState().ToString();
        }

        private void snoozeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bot.SteamFriends.SetPersonaState(SteamKit2.EPersonaState.Snooze);
            this.steam_status.Text = bot.SteamFriends.GetPersonaState().ToString();
        }

        private void offlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bot.SteamFriends.SetPersonaState(SteamKit2.EPersonaState.Offline);
            this.steam_status.Text = bot.SteamFriends.GetPersonaState().ToString();
        }

        private void changeProfileNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProfileName changeProfile = new ProfileName(bot);
            changeProfile.ShowDialog();
        }

        private void label_addfriend_Click(object sender, EventArgs e)
        {
            AddFriend addFriend = new AddFriend(bot);
            addFriend.ShowDialog();
        }

        private void showBackpackToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Your backpack
            ulong sid = bot.SteamUser.SteamID;
            ShowBackpack showBP = new ShowBackpack(bot, sid);
            showBP.Show();
            showBP.Activate();
        }

        private void showBackpackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Friend's backpack
            ShowBackpack showBP = new ShowBackpack(bot, contextMenuSteamId);
            showBP.Show();
            showBP.Activate();
        }

        private void openChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bot.main.Invoke((Action)(() =>
            {
                SteamID steamId = new SteamID(contextMenuSteamId);
                string selected = bot.SteamFriends.GetFriendPersonaName(contextMenuSteamId);
                if (!chat_opened)
                {
                    chat = new Chat(bot);
                    chat.AddChat(selected, contextMenuSteamId);
                    chat.Show();
                    chat.Focus();
                    chat_opened = true;
                }
                else
                {
                    bool found = false;
                    foreach (TabPage tab in chat.ChatTabControl.TabPages)
                    {
                        if ((SteamID)tab.Tag == steamId)
                        {
                            chat.ChatTabControl.SelectedTab = tab;
                            chat.Focus();
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        chat.AddChat(selected, contextMenuSteamId);
                        chat.Focus();
                    }
                }
            }));
        }

        private void inviteToTradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bot.main.Invoke((Action)(() =>
            {
                string selected = bot.SteamFriends.GetFriendPersonaName(contextMenuSteamId);
                if (!chat_opened)
                {
                    chat = new Chat(bot);
                    chat.AddChat(selected, contextMenuSteamId);
                    chat.Show();
                    chat.Focus();
                    chat_opened = true;
                    chat.chatTab.tradeClicked();
                }
                else
                {
                    bool found = false;
                    foreach (TabPage tab in Friends.chat.ChatTabControl.TabPages)
                    {
                        if ((SteamID)tab.Tag == new SteamID(contextMenuSteamId))
                        {
                            found = true;
                            tab.Invoke((Action)(() =>
                            {
                                foreach (var item in tab.Controls)
                                {
                                    chat.chatTab = (ChatTab)item;
                                    chat.chatTab.tradeClicked();
                                }
                            }));
                            return;
                        }
                    }
                    if (!found)
                    {
                        chat.AddChat(selected, contextMenuSteamId);
                        chat.Focus();
                        chat.chatTab.tradeClicked();
                    }
                }
            }));
        }

        private void removeFriendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ulong sid = contextMenuSteamId;
            string name = bot.SteamFriends.GetFriendPersonaName(sid);
            var dr = MetroFramework.MetroMessageBox.Show(this, "Are you sure you want to remove " + name + "?",
                    "Remove Friend",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                ListFriends.Remove(sid);
                bot.friends.Remove(sid);
                bot.SteamFriends.RemoveFriend(sid);
                MetroFramework.MetroMessageBox.Show(this, "You have removed " + name + ".",
                        "Remove Friend",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
            }            
        }

        private void blockFriendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ulong sid = contextMenuSteamId;
            string selected = bot.SteamFriends.GetFriendPersonaName(sid);
            var dr = MetroFramework.MetroMessageBox.Show(this, "Are you sure you want to block " + selected + "?",
                    "Block Friend",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                ListFriends.Remove(sid);
                bot.friends.Remove(sid);
                bot.SteamFriends.IgnoreFriend(sid);
                MetroFramework.MetroMessageBox.Show(this, "You have blocked " + selected + ".",
                        "Block Friend",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
            }            
        }        

        private void steamRepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string status = Util.GetSteamRepStatus(contextMenuSteamId);
            if (status == "")
            {
                MetroFramework.MetroMessageBox.Show(this, "User has no special reputation.",
                "SteamRep Status",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
            }
            else
            {
                var icon = status.Contains("SCAMMER") ? MessageBoxIcon.Warning : MessageBoxIcon.Information;
                MetroFramework.MetroMessageBox.Show(this, status,
                "SteamRep Status",
                MessageBoxButtons.OK,
                icon,
                MessageBoxDefaultButton.Button1);
            }
        }                    

        private void aboutMistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MetroFramework.MetroMessageBox.Show(this, " - Mist v" + MistVersion + " is created by waylaidwanderer.\r\nView http://steamcommunity.com/groups/MistClient for more information.",
                        "About",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string response = SteamTrade.SteamWeb.Fetch(Util.UpdateCheckUrl);
            if (response != "")
            {
                string latestVer = Util.ParseBetween(response, "<version>", "</version>");                                
                if (latestVer != Friends.MistVersion)
                {
                    string[] changelog = Util.GetStringInBetween("<changelog>", "</changelog>", response, false, false);
                    if (!string.IsNullOrEmpty(changelog[0]))
                    {
                        Updater updater = new Updater(latestVer, changelog[0], bot.log);
                        updater.ShowDialog();
                        updater.Activate();
                    }
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Congratulations, Mist is up-to-date! Thank you for using Mist :)",
                        "Latest Version",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Failed to connect to the update server! Please try again later.",
                        "Update Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
            }
        }

        private void Friends_Load(object sender, EventArgs e)
        {
            byte[] avatarHash = bot.SteamFriends.GetFriendAvatar(bot.SteamUser.SteamID);
            bool validHash = avatarHash != null && !IsZeros(avatarHash);

            if ((AvatarHash == null && !validHash && avatarBox.Image != null) || (AvatarHash != null && AvatarHash.SequenceEqual(avatarHash)))
            {
                // avatar is already up to date, no operations necessary
            }
            else if (validHash)
            {
                AvatarHash = avatarHash;
                CDNCache.DownloadAvatar(bot.SteamUser.SteamID, avatarHash, AvatarDownloaded);
            }
            else
            {
                AvatarHash = null;
                avatarBox.Image = ComposeAvatar(null);
            }
        }

        private void viewProfileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Util.ShowSteamProfile(bot, contextMenuSteamId);
        }

        private void acceptFriendRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bot.SteamFriends.AddFriend(contextMenuSteamId);
            ListFriendRequests.Remove(contextMenuSteamId);
            if (ListFriendRequests.Get().Count == 0)
                webControl1.ExecuteJavascript("hideFriendRequests();");
        }

        private void denyFriendRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            bot.SteamFriends.RemoveFriend(contextMenuSteamId);
            ListFriendRequests.Remove(contextMenuSteamId);
            if (ListFriendRequests.Get().Count == 0)
                webControl1.ExecuteJavascript("hideFriendRequests();");
        }

        private void viewProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.ShowSteamProfile(bot, contextMenuSteamId);
        }

        private void showBackpackToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ShowBackpack showBP = new ShowBackpack(bot, contextMenuSteamId);
            showBP.Show();
            showBP.Activate();
        }

        private void steamRepStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // This is a proxy for SteamRep's beta API. Not recommended for heavy/wide usage.
            try
            {
                string url = "http://scam.io/profiles/" + contextMenuSteamId;
                string response = SteamTrade.SteamWeb.Fetch(url);
                if (response != "")
                {
                    string status = Util.ParseBetween(response, "<reputation>", "</reputation>");
                    if (status == "")
                    {
                        MetroFramework.MetroMessageBox.Show(this, "User has no special reputation.",
                        "SteamRep Status",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, status,
                        "SteamRep Status",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1);
                    }
                }
            }
            catch
            {

            }
        }

        private void minimizeToTrayOnCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            minimizeToTray = minimizeToTrayToolStripMenuItem.Checked;
            Properties.Settings.Default.MinimizeToTray = minimizeToTray;
            Properties.Settings.Default.Save();
        }

        private void text_search_TextChanged(object sender, EventArgs e)
        {
            if (webControl1.IsDocumentReady)
            {
                webControl1.ExecuteJavascript("searchUser(\"" + text_search.Text + "\");");
            }
        }

        private void viewChatLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ulong sid = contextMenuSteamId;
            string selected = bot.SteamFriends.GetFriendPersonaName(sid);
            string logDir = Path.Combine(Application.StartupPath, "logs");
            string file = Path.Combine(logDir, sid.ToString() + ".txt");
            if (!File.Exists(file))
            {
                ChatLog chatLog = new ChatLog(selected, sid.ToString());
                chatLog.Show();
                chatLog.Activate();
            }
            else
            {
                string[] log = Util.ReadAllLines(file);
                StringBuilder sb = new StringBuilder();
                foreach (string line in log)
                {
                    sb.Append(line + Environment.NewLine);
                }
                ChatLog chatLog = new ChatLog(selected, sid.ToString(), sb.ToString());
                chatLog.Show();
                chatLog.Activate();
            }
        }

        private void logConversationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            keepLog = logConversationsToolStripMenuItem.Checked;
            Properties.Settings.Default.KeepLog = keepLog;
            Properties.Settings.Default.Save();
        }

        private void text_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                text_search.Clear();
            }
        }

        private void exitMistToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            minimizeToTray = false;
            this.Dispose();
            this.Close(); 
            Environment.Exit(0);
        }

        private void Friends_Leave(object sender, EventArgs e)
        {
            text_search.ResetText();
            //this.friends_list.SetObjects(ListFriends.Get(showOnlineFriendsOnlyToolStripMenuItem.Checked));
            label_settings.Select();
        }

        private void minimizeToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            minimizeToTray = minimizeToTrayToolStripMenuItem.Checked;
            Properties.Settings.Default.MinimizeToTray = minimizeToTray;
            Properties.Settings.Default.Save();
        }

        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalStyleManager.Theme = MetroFramework.MetroThemeStyle.Light;
            try
            {
                Properties.Settings.Default.Theme = GlobalStyleManager.Theme.ToString();
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Util.LoadTheme(this, this.Controls);
            menu_status.Close();
        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalStyleManager.Theme = MetroFramework.MetroThemeStyle.Dark;
            Properties.Settings.Default.Theme = GlobalStyleManager.Theme.ToString();
            Properties.Settings.Default.Save();
            Util.LoadTheme(this, this.Controls);
            menu_status.Close();
        }

        private void setColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StyleChooser styleChooser = new StyleChooser("Light");
            styleChooser.ShowDialog();
            styleChooser.Activate();
        }

        private void setColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StyleChooser styleChooser = new StyleChooser("Dark");
            styleChooser.ShowDialog();
            styleChooser.Activate();
        }        

        private void showOnlineFriendsOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool checkState = showOnlineFriendsOnlyToolStripMenuItem.Checked;
            Properties.Settings.Default.OnlineOnly = checkState;
            Properties.Settings.Default.Save();
            UpdateFriendsHTML();
        }

        private void tradeOffersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tradeOffersForm = new MetroForm();
            tradeOffersForm.Icon = MistClient.Properties.Resources.Icon;
            tradeOffersForm.Width = 1012;
            tradeOffersForm.Height = 500;
            tradeOffersForm.Text = "Trade Offers";
            tradeOffersForm.Style = this.Style;
            tradeOffersForm.Theme = this.Theme;
            tradeOffersForm.ShadowType = MetroFormShadowType.DropShadow;
            var tradeOffersWeb = new Awesomium.Windows.Forms.WebControl();
            tradeOffersWeb.Dock = DockStyle.Fill;
            string cookies = string.Format("steamLogin={0}; sessionid={1}", bot.token, bot.sessionId);
            tradeOffersWeb.WebSession = WebCore.CreateWebSession(new WebPreferences());
            tradeOffersWeb.WebSession.SetCookie("http://steamcommunity.com".ToUri(), cookies, true, true);
            tradeOffersWeb.Source = ("http://steamcommunity.com/profiles/" + bot.SteamUser.SteamID.ConvertToUInt64() + "/tradeoffers/").ToUri();
            tradeOffersWeb.DocumentReady += tradeOffersWeb_DocumentReady;
            tradeOffersForm.Controls.Add(tradeOffersWeb);
            tradeOffersForm.Show();
            tradeOffersForm.Focus();
        }

        void tradeOffersWeb_DocumentReady(object sender, UrlEventArgs e)
        {
            var webControl = (Awesomium.Windows.Forms.WebControl)sender;
            while (webControl.ExecuteJavascriptWithResult("document.body.innerHTML").IsUndefined)
            {                
                WebCore.Update();
            }            
            var script = @"var $j = jQuery.noConflict();
                            $j(function() {
                                $j('#global_header').hide();
                                $j('.profile_small_header_bg').hide();
                                $j('.inventory_links').hide();
                                $j('#footer_spacer').hide();
                                $j('#footer').hide();
                            });";
            webControl.ExecuteJavascript(script);
            script = @" var scrollbarCSS = '::-webkit-scrollbar { width: 14px !important; height: 14px !important; } ::-webkit-scrollbar-track { background-color: #111111 !important;	} ::-webkit-scrollbar-thumb { background-color: #444444 !important; } ::-webkit-scrollbar-thumb:hover { background-color: #5e5e5e !important; } ::-webkit-scrollbar-corner { background-color: #111111 !important; }';              
                            var head = document.getElementsByTagName('head')[0];
                            var style = document.createElement('style');
                            style.type = 'text/css';
                            style.innerHTML = scrollbarCSS;                            
                            head.appendChild(style);
                            ";
            webControl.ExecuteJavascript(script);
        }

        private void sendTradeOfferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sid = new SteamID(contextMenuSteamId);
            var url = "http://steamcommunity.com/tradeoffer/new/?partner=" + sid.AccountID;
            var tradeOffersForm = new MetroForm();
            tradeOffersForm.Icon = MistClient.Properties.Resources.Icon;
            tradeOffersForm.Width = 720;
            tradeOffersForm.Height = 730;
            tradeOffersForm.Text = "Trade Offer with " + bot.SteamFriends.GetFriendPersonaName(sid);
            tradeOffersForm.Style = this.Style;
            tradeOffersForm.Theme = this.Theme;
            tradeOffersForm.ShadowType = MetroFormShadowType.DropShadow;
            var tradeOffersWeb = new Awesomium.Windows.Forms.WebControl();
            tradeOffersWeb.Dock = DockStyle.Fill;
            string cookies = string.Format("steamLogin={0}; sessionid={1}", bot.token, bot.sessionId);
            tradeOffersWeb.WebSession = WebCore.CreateWebSession(new WebPreferences());
            tradeOffersWeb.WebSession.SetCookie("http://steamcommunity.com".ToUri(), cookies, true, true);
            tradeOffersWeb.Source = url.ToUri();
            tradeOffersWeb.DocumentReady += tradeOffersWeb_DocumentReady;
            tradeOffersForm.Controls.Add(tradeOffersWeb);
            tradeOffersForm.Show();
            tradeOffersForm.Focus();
        }

        private void viewSteamIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSteamInfo(bot, contextMenuSteamId);
        }

        private void viewSteamInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSteamInfo(bot, bot.SteamUser.SteamID.ConvertToUInt64());
        }

        public void ShowSteamInfo(SteamBot.Bot bot, ulong steamId)
        {
            var steamInfo = new MetroForm();
            var steamInfoTextBox = new MetroFramework.Controls.MetroTextBox();
            steamInfoTextBox.Multiline = true;
            steamInfoTextBox.Dock = DockStyle.Fill;
            steamInfoTextBox.Text = Util.GetSteamIDInfo(bot, steamId);
            steamInfoTextBox.Theme = this.StyleManager.Theme;
            steamInfoTextBox.Style = this.StyleManager.Style;
            steamInfo.Icon = MistClient.Properties.Resources.Icon;
            steamInfo.Width = 435;
            steamInfo.Height = 155;
            steamInfo.MaximizeBox = false;
            steamInfo.MinimizeBox = false;
            steamInfo.Resizable = false;
            steamInfo.Text = "Steam Info";
            steamInfo.Theme = this.StyleManager.Theme;
            steamInfo.Style = this.StyleManager.Style;
            steamInfo.ShadowType = MetroFormShadowType.DropShadow;
            steamInfo.Controls.Add(steamInfoTextBox);
            steamInfo.ShowDialog();
        }

        private void viewProfileToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Util.ShowSteamProfile(bot, bot.SteamUser.SteamID.ConvertToUInt64());
        }

        private void steamCommunityMarketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MetroForm();
            form.Text = "Steam Community Market";
            form.Width = 1025;
            form.Height = 600;
            form.Style = Friends.GlobalStyleManager.Style;
            form.Theme = Friends.GlobalStyleManager.Theme;
            form.Icon = MistClient.Properties.Resources.Icon;
            form.ShadowType = MetroFormShadowType.DropShadow;
            var webControl = new Awesomium.Windows.Forms.WebControl();
            form.Controls.Add(webControl);
            webControl.Dock = System.Windows.Forms.DockStyle.Fill;
            string cookies = string.Format("steamLogin={0}; sessionid={1}", bot.token, bot.sessionId);
            webControl.WebSession = Awesomium.Core.WebCore.CreateWebSession(new Awesomium.Core.WebPreferences());
            webControl.WebSession.SetCookie(new Uri("http://steamcommunity.com"), cookies, true, true);
            webControl.Source = new Uri("http://steamcommunity.com/market");
            webControl.DocumentReady += steamCommunityMarket_DocumentReady;
            webControl.TitleChanged += (s, ev) => webControl_TitleChanged(s, ev, form);            
            form.Show();
        }

        void steamCommunityMarket_DocumentReady(object sender, UrlEventArgs e)
        {
            var webControl = (Awesomium.Windows.Forms.WebControl)sender;
            while (webControl.ExecuteJavascriptWithResult("document.body.innerHTML").IsUndefined)
            {
                WebCore.Update();
            }
            var script = @"var $j = jQuery.noConflict();
                            $j(function() {
                                $j('#global_header').hide();
                                $j('.profile_small_header_bg').hide();
                                $j('.inventory_links').hide();
                                $j('#footer_spacer').hide();
                                $j('#footer').hide();
                            });";
            webControl.ExecuteJavascript(script);
            script = @" var scrollbarCSS = '::-webkit-scrollbar { width: 14px !important; height: 14px !important; } ::-webkit-scrollbar-track { background-color: #111111 !important;	} ::-webkit-scrollbar-thumb { background-color: #444444 !important; } ::-webkit-scrollbar-thumb:hover { background-color: #5e5e5e !important; } ::-webkit-scrollbar-corner { background-color: #111111 !important; }';              
                            var head = document.getElementsByTagName('head')[0];
                            var style = document.createElement('style');
                            style.type = 'text/css';
                            style.innerHTML = scrollbarCSS;                            
                            head.appendChild(style);
                            ";
            webControl.ExecuteJavascriptWithResult(script);
            var prevSize = webControl.Parent.Size;
            webControl.Parent.Size = new Size(webControl.Parent.Size.Width + 100, webControl.Parent.Size.Height);
            webControl.Parent.Size = prevSize;
        }

        private static void webControl_TitleChanged(object sender, Awesomium.Core.TitleChangedEventArgs e, MetroForm parentForm)
        {
            parentForm.Text = e.Title;
            parentForm.Refresh();
            ((Awesomium.Windows.Forms.WebControl)sender).TitleChanged -= (s, ev) => webControl_TitleChanged(s, ev, parentForm);
        }

        private void shareUsageStatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (shareUsageStatsToolStripMenuItem.Checked)
            {
                if (!statsSent)
                {
                    Util.SendUsageStats(bot.SteamUser.SteamID);
                    statsSent = true;
                }
            }
        }
    }
        */
    }
}
