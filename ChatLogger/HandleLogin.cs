using SteamKit2;
using SteamKit2.Internal;
using SteamKit2.Authentication;
using System.Threading.Tasks;
using System.IO;
using System;
using ChatLogger.UserSettings;
using Newtonsoft.Json;
using ChatLogger.Helpers;
using ChatLogger.User2Json;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading;
using MercuryBOT.FriendsList;
using System.Data;


/*
 * 
 *          todo
 *  se trocar de conta o chatlogger nao faz folder novo com o nome da conta
 * 
 * guardar msgs de groups
 * 
 * 
 */


namespace ChatLogger
{
    public class HandleLogin
    {

        public static List<SteamID> Friends { get; private set; }

        public static string UserPersonaName, UserCountry, CurrentUsername;
        public static int CurrentPersonaState = 1;

        public static bool updateFriendsStatus = false;


        public static bool ChatLogger = false;
        private static EResult LastLogOnResult;

        public static bool IsLoggedIn { get; private set; }
        public static bool isRunning = false;

        public static string user, pass;
        public string getGuardData;

        public static string authCode, twoFactorAuth;
        public static string steamID, LastMessageReceived, LastMessageSent;

        public static ulong CurrentSteamID = 0;

        public static string AvatarPrefix = "http://cdn.akamai.steamstatic.com/steamcommunity/public/images/avatars/";
        public static string AvatarSuffix = "_full.jpg";

        public static string avatar;
        public static string LastErrorLogin = "";

        public ClientPlayerNicknameListHandler SteamNicknames;
        //private WebAuthenticator _webAuthenticator = new WebAuthenticator();
        private AuthSession _authSession;

        private SteamClient steamClient;
        private CallbackManager manager;
        private SteamUser steamUser;
        // private SteamFriends steamFriends;
        private SteamUnifiedMessages steamUnified;

        public static SteamFriends steamFriends { get; set; }
        // TaskCompletionSource to manage the state of the connection
        private TaskCompletionSource<bool> _connectionCompletionSource;


        Dictionary<ulong, string> PlayerNicknames = new Dictionary<ulong, string>();

        public HandleLogin()
        {
            DebugLog.AddListener(new MyListener());
            DebugLog.Enabled = true;

            // Initialize Steam client and related components
            steamClient = new SteamClient();
            manager = new CallbackManager(steamClient);
            steamUnified = steamClient.GetHandler<SteamUnifiedMessages>();


            // Subscribe to necessary callbacks
            manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);

            steamUser = steamClient.GetHandler<SteamUser>();
            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);
            manager.Subscribe<SteamUser.AccountInfoCallback>(OnAccountInfo);

            steamFriends = steamClient.GetHandler<SteamFriends>();
            manager.Subscribe<SteamFriends.PersonaStateCallback>(OnPersonaState);

            manager.Subscribe<SteamFriends.FriendMsgCallback>(OnFriendMsg);
            manager.Subscribe<SteamFriends.FriendMsgEchoCallback>(OnFriendEchoMsg);
            manager.Subscribe<SteamFriends.ChatMsgCallback>(OnChatRoomMsg);
            manager.Subscribe<SteamFriends.ChatRoomInfoCallback>(OnChatRoomInfo);

            manager.Subscribe<SteamFriends.FriendsListCallback>(OnFriendsList);


            steamClient.AddHandler(new ClientPlayerNicknameListHandler());
            SteamNicknames = steamClient.GetHandler<ClientPlayerNicknameListHandler>();


            manager.Subscribe<SteamUnifiedMessages.ServiceMethodNotification>(OnServiceMethod);
            manager.Subscribe<SteamUnifiedMessages.ServiceMethodResponse>(OnMethodResponse);

            // Start running the callback manager in a background task
            Task.Run(() => RunCallbackManager());
        }

        public async Task StartLogin(string user, string pw)
        {
            string username = user;
            string password = pw;
            CurrentUsername = username;

            try
            {
                if (_authSession == null)
                {
                    Console.WriteLine($"Attempting initial login for user: {username}");

                    int retryCount = 5;

                    while (retryCount > 0)
                    {
                        _connectionCompletionSource = new TaskCompletionSource<bool>();

                        // Connect to Steam
                        steamClient.Connect();

                        // Await connection completion asynchronously
                        if (await WaitForSteamConnectionAsync(TimeSpan.FromSeconds(10)))
                        {
                            Console.WriteLine("Connected to Steam successfully.");
                            break; // Exit retry loop on success
                        }

                        Console.WriteLine("Failed to connect to Steam: Connection timed out. Retrying...");
                        InfoForm.InfoHelper.CustomMessageBox.Show("Error", "Failed to connect to Steam: Connection timed out. Retrying... (Maybe Steam Maintenance..)");
                        retryCount--;
                    }

                    if (retryCount == 0)
                    {
                        Console.WriteLine("Failed to connect to Steam after multiple attempts.");
                        return;
                    }


                    bool tkn_datacheck = File.Exists(Program.SentryFolder + user + "_tkn.data");

                    if (File.Exists(Program.SentryFolder + user + "_tkn.data"))
                    {
                        var listUserSettings = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(Program.AccountsJsonFile));

                        var account = listUserSettings.Accounts.Find(a => a.username == user);
                        if (account != null)
                        {
                            account.password = "";
                        }

                        File.WriteAllText(Program.AccountsJsonFile, JsonConvert.SerializeObject(listUserSettings, Formatting.Indented));

                        // test this


                        //var ListUserSettings = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(Program.AccountsJsonFile));
                        //foreach (var a in ListUserSettings.Accounts)
                        //{
                        //    if (a.username == user)
                        //    {
                        //        a.password = "";
                        //    }
                        //}
                        //File.WriteAllText(Program.AccountsJsonFile, JsonConvert.SerializeObject(ListUserSettings, Formatting.Indented));

                        steamUser.LogOn(new SteamUser.LogOnDetails
                        {
                            Username = username,
                            AccessToken = File.ReadAllText(Program.SentryFolder + user + "_tkn.data"),

                            // condition ? consequent : alternative
                            ShouldRememberPassword = true,
                            LoginID = (uint)(new Random().Next(10000, 10000000)),
                        });
                    }
                    else
                    {
                        // Begin the authentication session after a successful connection
                        _authSession = await steamClient.Authentication.BeginAuthSessionViaCredentialsAsync(new AuthSessionDetails
                        {
                            Username = username,
                            Password = password,
                            Authenticator = new DeviceAuth(),
                            IsPersistentSession = true,
                            //GuardData = getGuardData
                        });

                        // Start polling for the authentication result
                        var pollResponse = await _authSession.PollingWaitForResultAsync();

                        if (pollResponse.NewGuardData != null)
                        {
                            // When using certain two factor methods (such as email 2fa), guard data may be provided by Steam
                            // for use in future authentication sessions to avoid triggering 2FA again (this works similarly to the old sentry file system).
                            // Do note that this guard data is also a JWT token and has an expiration date.
                            getGuardData = pollResponse.NewGuardData;
                        }

                        // Log on to Steam with the access token we received
                        steamUser.LogOn(new SteamUser.LogOnDetails
                        {
                            Username = pollResponse.AccountName,
                            AccessToken = pollResponse.RefreshToken,//a ? read : 

                            // condition ? consequent : alternative
                            ShouldRememberPassword = true,
                            LoginID = (uint)(new Random().Next(10000, 10000000)),
                        });

                        if (!tkn_datacheck)
                        {
                            File.WriteAllText(Program.SentryFolder + user + "_tkn.data", pollResponse.RefreshToken);
                        }

                        Console.WriteLine("Login successful!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
            }
        }

        private void OnConnected(SteamClient.ConnectedCallback callback)
        {
            Console.WriteLine("Connected to Steam!");
            _connectionCompletionSource?.SetResult(true);
        }

        private void OnPersonaState(SteamFriends.PersonaStateCallback callback)
        {
            if (callback == null)
            {
                return;
            }


            if (updateFriendsStatus && callback.FriendID.ConvertToUInt64().ToString().Substring(0, 1) == "7")
            {

                Console.WriteLine(callback.FriendID.ConvertToUInt64() + "~state:" + callback.State.ToString());

                var status = callback.State;
                var sid = callback.FriendID;
                ListFriends.UpdateStatus(sid, status.ToString());
            }


            if (callback.FriendID == CurrentSteamID)
            {
                string avatarHash = null;

                if ((callback.AvatarHash != null) && (callback.AvatarHash.Length > 0) && callback.AvatarHash.Any(singleByte => singleByte != 0))
                {
                    avatarHash = BitConverter.ToString(callback.AvatarHash).Replace("-", "").ToLowerInvariant();

                    if (string.IsNullOrEmpty(avatarHash) || avatarHash.All(singleChar => singleChar == '0'))
                    {
                        avatarHash = null;
                    }
                }
            }
        }




        private void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            Console.WriteLine($"Disconnected from Steam. UserInitiated: {callback.UserInitiated}");
            if (!callback.UserInitiated)
            {
                // Retry connection if disconnected unexpectedly
                _connectionCompletionSource?.SetResult(false);
            }
        }

        private void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            if (callback.Result == EResult.OK)
            {

                //new Thread(() =>
                //{
                //    while (true)
                //    {
                //        if (showFriends != null)
                //        {
                //            var numFriendsDisplayed = showFriends.GetNumFriendsDisplayed();
                //            var numSteamFriendCount = steamFriends.GetFriendCount();
                //            if (numFriendsDisplayed != -1 && numFriendsDisplayed != ListFriends.Get().Count)
                //            {
                //                LoadFriends();
                //                showFriends.UpdateFriendsHTML();
                //            }
                //            System.Threading.Thread.Sleep(10000);
                //        }

                //    }
                //}).Start();


                Console.WriteLine("[" + Program.BOTNAME + "] - Successfully logged on!");
                LastErrorLogin = "ok";
                steamID = steamUser.SteamID.ConvertToUInt64().ToString();

                CurrentSteamID = steamUser.SteamID.ConvertToUInt64();
                UserCountry = callback.IPCountryCode;

                // LoadFriends();

                IsLoggedIn = true;
                var ListUserSettings = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(Program.AccountsJsonFile));
                foreach (var a in ListUserSettings.Accounts)
                {
                    if (a.username == user)
                    {
                        steamFriends.SetPersonaState(Extensions.statesList[a.LoginState]);
                        a.LastLoginTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                        CurrentPersonaState = a.LoginState;
                    }
                }
                File.WriteAllText(Program.AccountsJsonFile, JsonConvert.SerializeObject(ListUserSettings, Formatting.Indented));
            }
            else
            {
                Console.WriteLine("Unable to logon to Steam: {0} / {1}", callback.Result, callback.ExtendedResult);
                _connectionCompletionSource?.SetResult(false);
                LastErrorLogin = "Unable to logon to Steam: " + callback.Result + " / " + callback.ExtendedResult;

                InfoForm.InfoHelper.CustomMessageBox.Show("Error", callback.Result + " / " + callback.ExtendedResult);


                if (callback.Result == EResult.Expired)
                {

                    InfoForm.InfoHelper.CustomMessageBox.Show("Error", "Login Session Expired, please login again.");


                    if (File.Exists(Program.SentryFolder + user + "_tkn.data"))
                    {
                        File.Delete(Program.SentryFolder + user + "_tkn.data");
                    }
                }
            }
        }

        private void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {
            Console.WriteLine($"Logged off from Steam: {callback.Result}");
            _connectionCompletionSource?.SetResult(false);
        }

        // Utility method to wait for the connection
        private async Task<bool> WaitForSteamConnectionAsync(TimeSpan timeout)
        {
            var completedTask = await Task.WhenAny(_connectionCompletionSource.Task, Task.Delay(timeout));
            return completedTask == _connectionCompletionSource.Task && _connectionCompletionSource.Task.Result;
        }

        // Utility method to run the callback manager
        private void RunCallbackManager()
        {
            while (true)
            {
                manager.RunWaitCallbacks(TimeSpan.FromMilliseconds(100));
            }
        }

        private void OnAccountInfo(SteamUser.AccountInfoCallback callback)
        {
            UserPersonaName = callback.PersonaName;
            UserCountry = callback.Country;
        }

        private void OnFriendMsg(SteamFriends.FriendMsgCallback callback)
        {
            if (callback.EntryType == EChatEntryType.ChatMsg)
            {

                var Settingslist = JsonConvert.DeserializeObject<ChatLoggerSettings>(File.ReadAllText(Program.SettingsJsonFile));


                ulong FriendID = callback.Sender;
                string Message = callback.Message;

                string FriendName = steamFriends.GetFriendPersonaName(FriendID);
                string nameClean = Regex.Replace(FriendName, "[^A-Za-z0-9 _]", "");

                string FriendIDName = @"\[" + FriendID + "] - " + nameClean + ".txt";
                string pathLog = Settingslist.PathLogs + @"\" + steamClient.SteamID.ConvertToUInt64() + FriendIDName;

                string FinalMsg = "[" + DateTime.Now + "] " + FriendName + ": " + Message;

                // Console.WriteLine("\nYou received a message by " + FriendName + "\n Telling you: " + Message);

                LastMessageReceived = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + FriendName.Replace(":", "") + ": " + Message;

                if (!Directory.Exists(Settingslist.PathLogs + @"\" + steamClient.SteamID.ConvertToUInt64()))
                {
                    Directory.CreateDirectory(Settingslist.PathLogs + @"\" + steamClient.SteamID.ConvertToUInt64());
                }

                string[] files = Directory.GetFiles(Settingslist.PathLogs + @"\" + steamClient.SteamID.ConvertToUInt64(), "[" + FriendID + "]*.txt");

                if (files.Length > 0)//file exist
                {
                    string[] LastDate = File.ReadLines(files[0]).Last().Split(' '); LastDate[0] = LastDate[0].Substring(1);

                    using (var tw = new StreamWriter(files[0], true))
                        if (LastDate[0] != DateTime.Now.Date.ToShortDateString())
                        {
                            tw.WriteLine(Settingslist.Separator + "\n" + FinalMsg);
                        }
                        else
                        {
                            tw.WriteLine(FinalMsg);
                        }
                }
                else
                {
                    FileInfo file = new FileInfo(pathLog);
                    file.Directory.Create();
                    File.WriteAllText(pathLog, FinalMsg + "\n");
                    //File.SetAttributes(pathLog, File.GetAttributes(pathLog) | FileAttributes.ReadOnly);
                }
            }
        }

        private void OnFriendEchoMsg(SteamFriends.FriendMsgEchoCallback callback)
        {
            if (callback.EntryType == EChatEntryType.ChatMsg)
            {
                
                var Settingslist = JsonConvert.DeserializeObject<ChatLoggerSettings>(File.ReadAllText(Program.SettingsJsonFile));

                ulong FriendID = callback.Recipient;
                string Message = callback.Message;

                string FriendName = steamFriends.GetFriendPersonaName(FriendID);

                string nameClean = Regex.Replace(FriendName, "[^A-Za-z0-9 _]", "");

                string FriendIDName = @"\[" + FriendID + "] - " + nameClean + ".txt";
                string pathLog = Settingslist.PathLogs + @"\" + steamClient.SteamID.ConvertToUInt64() + FriendIDName;


                string FinalMsg = "[" + DateTime.Now + "] " + steamFriends.GetPersonaName() + ": " + Message;


                LastMessageSent = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + steamFriends.GetFriendPersonaName(CurrentSteamID).Replace(":", "") + ": " + Message;

                Console.WriteLine("\nYou sent a message to " + FriendName + "\n Saying: " + Message);

                if (!Directory.Exists(Settingslist.PathLogs + @"\" + steamClient.SteamID.ConvertToUInt64()))
                {
                    Directory.CreateDirectory(Settingslist.PathLogs + @"\" + steamClient.SteamID.ConvertToUInt64());
                }

                string[] files = Directory.GetFiles(Settingslist.PathLogs + @"\" + steamClient.SteamID.ConvertToUInt64(), "[" + FriendID + "]*.txt");

                if (files.Length > 0)//file exist
                {
                    string[] LastDate = File.ReadLines(files[0]).Last().Split(' '); LastDate[0] = LastDate[0].Substring(1);
                    using (var tw = new StreamWriter(files[0], true))
                    {
                        if (LastDate[0] != DateTime.Now.Date.ToShortDateString())
                        {
                            tw.WriteLine(Settingslist.Separator + "\n" + FinalMsg);
                        }
                        else
                        {
                            tw.WriteLine(FinalMsg);
                        }
                    }
                }
                else
                {
                    FileInfo file = new FileInfo(pathLog);
                    file.Directory.Create();
                    File.WriteAllText(pathLog, FinalMsg + "\n");
                    //File.SetAttributes(pathLog, File.GetAttributes(pathLog) | FileAttributes.ReadOnly);
                }
            }
        }

        private void OnChatRoomMsg(SteamFriends.ChatMsgCallback callback)
        {


            if (callback.ChatMsgType == EChatEntryType.ChatMsg)
            {
                var Settingslist = JsonConvert.DeserializeObject<ChatLoggerSettings>(File.ReadAllText(Program.SettingsJsonFile));

                string GroupName = Extensions.ResolveGroupName(callback.ChatRoomID);


                ulong FriendID = callback.ChatterID;
                string Message = callback.Message;

                string chatroomID = callback.ChatRoomID.ToString();

                string FriendName = steamFriends.GetFriendPersonaName(FriendID);
                string nameClean = Regex.Replace(FriendName, "[^A-Za-z0-9 _]", "");

                string FriendIDName = @"\[" + GroupName + "] - All Messages.txt";
                string pathLog = Settingslist.PathLogs + @"\" + GroupName;


                string FinalMsg = "[" + DateTime.Now + "] " + steamFriends.GetPersonaName() + ": " + Message;


                LastMessageSent = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + steamFriends.GetFriendPersonaName(CurrentSteamID).Replace(":", "") + ": " + Message;

                Console.WriteLine("\nYou sent a message to " + FriendName + "\n Saying: " + Message);

                if (!Directory.Exists(Settingslist.PathLogs + @"\" + steamClient.SteamID.ConvertToUInt64()))
                {
                    Directory.CreateDirectory(Settingslist.PathLogs + @"\" + steamClient.SteamID.ConvertToUInt64());
                }

                string[] files = Directory.GetFiles(Settingslist.PathLogs + @"\" + steamClient.SteamID.ConvertToUInt64(), "[" + FriendID + "]*.txt");

                if (files.Length > 0)//file exist
                {
                    string[] LastDate = File.ReadLines(files[0]).Last().Split(' '); LastDate[0] = LastDate[0].Substring(1);
                    using (var tw = new StreamWriter(files[0], true))
                    {
                        if (LastDate[0] != DateTime.Now.Date.ToShortDateString())
                        {
                            tw.WriteLine(Settingslist.Separator + "\n" + FinalMsg);
                        }
                        else
                        {
                            tw.WriteLine(FinalMsg);
                        }
                    }
                }
                else
                {
                    FileInfo file = new FileInfo(pathLog);
                    file.Directory.Create();
                    File.WriteAllText(pathLog, FinalMsg + "\n");
                }
            }
        }


        private void OnChatRoomInfo(SteamFriends.ChatRoomInfoCallback callback)
        {
        }


        public static string GetAvatarLink(ulong steamid)
        {
            try
            {
                string SHA1 = BitConverter.ToString(steamFriends.GetFriendAvatar(steamid)).Replace("-", "").ToLower();

                string PreURL = SHA1.Substring(1, 2);
                return AvatarPrefix + PreURL + "/" + SHA1 + AvatarSuffix;
            }
            catch (Exception)
            {
                return Extensions.ResolveAvatar(steamid.ToString());
            }
        }

        public static void GetPersonaName(ulong steamid)
        {
            //  return steamFriends.GetFriendPersonaName((SteamID)steamid);
        }



        private void OnFriendsList(SteamFriends.FriendsListCallback obj)
        {
            LoadFriends();

            Console.WriteLine("[" + Program.BOTNAME + "] Recorded steam friends : {0}", steamFriends.GetFriendCount());
        }

        public void CreateFriendsListIfNecessary()
        {
            if (Friends != null)
                return;

            Friends = new List<SteamID>();
            for (int i = 0; i < steamFriends.GetFriendCount(); i++)
            {

                SteamID allfriends = steamFriends.GetFriendByIndex(i);
                var id = allfriends.ConvertToUInt64().ToString();
                if (id.StartsWith("7") && allfriends.ConvertToUInt64() != 0 && steamFriends.GetFriendRelationship(allfriends.ConvertToUInt64()) == EFriendRelationship.Friend)
                {
                    Friends.Add(allfriends.ConvertToUInt64());
                }
            }
        }


        public void RequestFriendInfo(IEnumerable<SteamID> steamIdList, EClientPersonaStateFlag requestedInfo = EClientPersonaStateFlag.Status)
        {
            if (steamIdList == null)
            {
                throw new ArgumentNullException(nameof(steamIdList));
            }


            var request = new ClientMsgProtobuf<CMsgClientRequestFriendData>(EMsg.ClientRequestFriendData);

            request.Body.friends.AddRange(steamIdList.Select(sID => sID.ConvertToUInt64()));
            request.Body.persona_state_requested = (uint)requestedInfo;

            steamClient.Send(request);

        }

        public static void LoadFriends()
        {
            ListFriends.Clear();
            //List<SteamID> steamIdList = new List<SteamID>();
            var steamListFriends = new List<SteamID>();
            Console.WriteLine("[" + Program.BOTNAME + "] - Loading all friends...");
            for (int index = 0; index < steamFriends.GetFriendCount(); index++)
            {
                steamListFriends.Add(steamFriends.GetFriendByIndex(index));
                Thread.Sleep(25);
            }

            for (int index = 0; index < steamListFriends.Count; index++)
            {
                SteamID steamId = steamListFriends[index];
                if (steamFriends.GetFriendRelationship(steamId) == EFriendRelationship.Friend)
                {
                    string friendPersonaName = steamFriends.GetFriendPersonaName(steamId);
                    // string Relationship = steamFriends.GetFriendRelationship(steamId).ToString();
                    string Relationship = "gamer1";
                    // string status = steamFriends.GetFriendPersonaState(steamId).ToString();
                    string status = "";


                    //string statis = 
                    ListFriends.Add(friendPersonaName, (ulong)steamId, Relationship, status);
                    steamFriends.RequestFriendInfo(steamId, EClientPersonaStateFlag.Status);

                }
            }
            updateFriendsStatus = true;
            Console.WriteLine("Done! {0} friends.", ListFriends.Get().Count);
            //  FriendsLoaded = true;
        }

        private void OnServiceMethod(SteamUnifiedMessages.ServiceMethodNotification notification)
        {
            if (notification == null)
            {
                //nameof(notification);

                return;
            }

            //manager.SubscribeServiceNotification<ChatRoomClient, CChatRoom_IncomingChatMessage_Notification>(OnIncomingChatMessage);
            switch (notification.MethodName)
            {
                case "ChatRoomClient.NotifyIncomingChatMessage#1":
                    OnIncomingChatMessage((CChatRoom_IncomingChatMessage_Notification)notification.Body);

                    break;
            }
        }

        private void OnIncomingChatMessage(CChatRoom_IncomingChatMessage_Notification notification)
        {
            if (notification == null)
            {

                return;
            }



            string message;

            // Prefer to use message without bbcode, but only if it's available
            if (!string.IsNullOrEmpty(notification.message_no_bbcode))
            {
                message = notification.message_no_bbcode;
            }
            else if (!string.IsNullOrEmpty(notification.message))
            {
                message = UnEscape(notification.message);
            }
            else
            {
                return;
            }

            // ArchiLogger.LogChatMessage(false, message, notification.chat_group_id, notification.chat_id, notification.steamid_sender);
            //  if ((notification.chat_group_id != MasterChatGroupID) || (BotConfig.OnlineStatus == EPersonaState.Offline))
            //  {
            //     return;
            //   }

            // await HandleMessage(notification.chat_group_id, notification.chat_id, notification.steamid_sender, message).ConfigureAwait(false);            
            Console.WriteLine(notification.chat_group_id + notification.chat_id + notification.steamid_sender);

        }


        static void OnMethodResponse(SteamUnifiedMessages.ServiceMethodResponse callback)
        {
            if (callback.Result != EResult.OK)
            {
                Console.WriteLine($"Unified service request failed with {callback.Result}");
                return;
            }



        }


        private static string UnEscape(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return null;
            }
            return message.Replace("\\[", "[").Replace("\\\\", "\\");
        }


        public void Logout()
        {
            user = null;
            isRunning = false;
            IsLoggedIn = false;
            steamUser.LogOff();
            CurrentPersonaState = 0;
            CurrentUsername = null;
            //DisconnectedCounter = 0;
        }
    }
    class MyListener : IDebugListener
    {
        public void WriteLine(string category, string msg)
        {
            // this function will be called when internal steamkit components write to the debuglog

            // for this example, we'll print the output to the console
            Console.WriteLine("MyListener - {0}: {1}", category, msg);
            File.AppendAllText(Program.ExecutablePath + "/logs.txt", DateTime.Now.ToString() + "  " + category + msg + "\n");
        }
    }
}
