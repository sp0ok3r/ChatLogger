using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SteamKit2.GC.Dota.Internal.CMsgDOTABotDebugInfo;
using MetroFramework.Forms;
using SteamKit2;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Web;
using MercuryBOT.FriendsList;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Awesomium.Core;
using Awesomium.Windows.Forms;
using chatlogger2.Steam;

namespace ChatLogger.ViewChats
{
    public partial class ChatsVisualizer : MetroForm
    {

        Awesomium.Windows.Forms.WebControl webControl1 = new Awesomium.Windows.Forms.WebControl();

        public ChatsVisualizer()
        {
            InitializeComponent();

            this.webControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));


            this.Controls.Add(webControl1);
            this.webControl1.Location = new System.Drawing.Point(23, 63);
            this.webControl1.Size = new System.Drawing.Size(1035, 474);

            webControl1.Source = ("file://" + Application.StartupPath + "/Friends.html").ToUri();
            webControl1.DocumentReady += webControl1_DocumentReady;
            webControl1.ShowContextMenu += webControl1_ShowContextMenu;
            // Add the browser to the form's controls.


        }
        private void ChatsVisualizer_Load(object sender, EventArgs e)
        {

            FriendsHTML();
        }

        private void FriendsHTML()
        {
            //Util.LoadTheme(this, this.Controls);


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
                WebCore.Run();
            }


            // JSObject jsobject = webControl1.CreateGlobalJavascriptObject("Mist");
            //jsobject.Bind("OpenChat", false, OpenChatHandler);
            //jsobject.Bind("ShowMenu", false, ShowMenuHandler);
            //jsobject.Bind("ShowMiniProfile", false, ShowMiniProfile);
            //jsobject.Bind("HideMiniProfile", false, HideMiniProfile);
            //jsobject.Bind("ShowFriendReqMenu", false, ShowFriendReqMenu);

            UpdateFriendsHTML();
            //UpdateFriendRequestsHTML();
        }

        private string GetFriendsHTML()
        {
            string output = "";
            string gameFriends = "";
            string onlineFriends = "";
            string offlineFriends = "";
            foreach (var friend in ListFriends.Get(false))
            {
                string statusStyle = "";
                string html = GetFriendHTML(friend.SID, out statusStyle);
                switch (statusStyle)
                {
                    case "Offline":
                    case "offline":
                        offlineFriends += html;
                        break;
                    case "Ingame":
                        gameFriends += html;
                        break;
                    case "Online":
                        onlineFriends += html;
                        break;
                }
            }
            output += gameFriends + onlineFriends + offlineFriends;
            return output;
        }

        public string GetFriendHTML(ulong steamId, out string statusStyle)
        {
            var friend = ListFriends.GetFriend(steamId);
            try
            {
                //byte[] avatarHash = HandleLogin.steamFriends.GetFriendAvatar(friend.SID);
                //bool validHash = avatarHash != null && !IsZeros(avatarHash);
                //string hashStr = BitConverter.ToString(avatarHash).Replace("-", "").ToLower();
                //string hashPrefix = hashStr.Substring(0, 2);
                //string avatarUrl = string.Format("http://media.steampowered.com/steamcommunity/public/images/avatars/{0}/{1}.jpg", hashPrefix, hashStr);
                //string currentGame = HttpUtility.HtmlEncode(HandleLogin.steamFriends.GetFriendGamePlayedName(friend.SID));
                //if (friend.Status == "Offline")
                //    statusStyle = "offline";
                //else if (!string.IsNullOrEmpty(currentGame))
                //    statusStyle = "ingame";
                //else
                //    statusStyle = "online";
                //string status = string.IsNullOrEmpty(currentGame) ? friend.Status : "In-Game (" + friend.Status + ")";

                string friendName = HttpUtility.HtmlEncode(friend.Name);
                string avatarUrl= HandleLogin.GetAvatarLink(friend.SID);

                var getFriendGame = HttpUtility.HtmlEncode(HandleLogin.steamFriends.GetFriendGamePlayedName(friend.SID));

                string currentGame = (getFriendGame != null) ? getFriendGame : "Unknown";


                if (friend.Status == "offline" || friend.Status == "Offline")
                    statusStyle = "offline";
                else if (currentGame != "Unknown")
                    statusStyle = "ingame";
                else
                    statusStyle = "online";
                string status = currentGame=="Unknown" ? friend.Status : "In-Game (" + friend.Status + ")";



                string nickname = "fff";




                //string html = string.Format(@"<tr id='id-{5}' data-playername='{1}' ondblclick='Mist.OpenChat(""{5}"");' oncontextmenu='Mist.ShowMenu(""{5}"");return false;'><td class='avatar'><img class='{4}' src='{0}' onmouseover='Mist.ShowMiniProfile(""{5}"");' onmouseout='Mist.HideMiniProfile();'/></td><td class='playerinfo'><div class='playername {4}'><span class='text'>{1}</span><span class='nickname'>{6}</span><span class='playeroptions'></span></div><div class='status {4}'>{2}</div><div class='gamename {4}'>{3}</div></td></tr>", avatarUrl, friendName, currentGame, statusStyle, friend.SID);//removed ,status friend.Nickname
                string html = string.Format(@"
<tr id='id-{5}' data-playername='{1}' 
    ondblclick='Mist.OpenChat(""{5}"");' 
    oncontextmenu='Mist.ShowMenu(""{5}""); return false;'>
    <td class='avatar'>
        <img class='{4}' src='{0}' 
             onmouseover='Mist.ShowMiniProfile(""{5}"");' 
             onmouseout='Mist.HideMiniProfile();'/>
    </td>
    <td class='playerinfo'>
        <div class='playername {4}'>
            <span class='text'>{1}</span>
            <span class='nickname'>{6}</span>
            <span class='playeroptions'></span>
        </div>
        <div class='status {4}'>{2}</div>
        <div class='gamename {4}'>{3}</div>
    </td>
</tr>",
                    avatarUrl,        // {0} - URL of the player's avatar image
                    friendName,       // {1} - Name of the player
                    currentGame,      // {2} - Player's status (e.g., online, offline)
                    statusStyle,      // {3} - CSS class for styling based on status
                    friend.Status,       // {4} - Unique identifier for the player (e.g., Steam ID)
                    friend.SID,       // {5} - Unique identifier for the player (e.g., Steam ID)
                    nickname          // {6} - The nickname of the player
                );
                return html;
            }
            catch
            {
                statusStyle = "";
                return "";
            }
        }

        public void UpdateFriendsHTML()
        {
            if (!webControl1.IsDocumentReady) return;

            while (webControl1.ExecuteJavascriptWithResult("document.body.innerHTML").IsUndefined)
            {
                WebCore.Run();
            }
            webControl1.ExecuteJavascript("updateFriends(\"" + HttpUtility.JavaScriptStringEncode(GetFriendsHTML()) + "\");");
            Console.WriteLine("execity javascript");


            Console.WriteLine(GetFriendsHTML());

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

    }
}
