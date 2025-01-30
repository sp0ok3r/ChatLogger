using ChatLogger.Helpers;
using ChatLogger.User2Json;
using ChatLogger.UserSettings;
using Newtonsoft.Json;
using SteamKit2;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace ChatLogger
{
    public class AccountLogin
    {
        public static string UserPersonaName, UserCountry, CurrentUsername;
        public static int CurrentPersonaState = 1;

        public static SteamClient steamClient;
        public static SteamUser steamUser;
        public static SteamFriends steamFriends;
        public static CallbackManager ChatLoggerManager;

        public static bool ChatLogger = false;
        private static EResult LastLogOnResult;

        public static bool IsLoggedIn { get; private set; }
        public static bool isRunning = false;

        private static int DisconnectedCounter;
        private static int MaxDisconnects = 5;


        private static string NewloginKey = null;

        public static string user, pass;


        public static string authCode, twoFactorAuth;
        public static string steamID, LastMessageReceived, LastMessageSent;

        public static ulong CurrentSteamID = 0;

        public static string AvatarPrefix = "http://cdn.akamai.steamstatic.com/steamcommunity/public/images/avatars/";
        public static string AvatarSuffix = "_full.jpg";

        public static void UserSettingsGather(string username, string password)
        {
            try
            {
                var list = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(Program.AccountsJsonFile));

                user = username;
                CurrentUsername = username;
                foreach (var a in list.Accounts)
                {
                    if (a.username == username)
                    {
                        pass = a.password;
                    }
                }

                Login();
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + Program.BOTNAME + "] - " + e);
            }
        }
        public static void Login()
        {
            Console.WriteLine("[" + Program.BOTNAME + "] - Starting Login...");

            isRunning = true;

            steamClient = new SteamClient();

            ChatLoggerManager = new CallbackManager(steamClient);

            // DebugLog.AddListener(new MyListener());
            //  DebugLog.Enabled = true;

            #region Callbacks
            steamUser = steamClient.GetHandler<SteamUser>();
            steamFriends = steamClient.GetHandler<SteamFriends>();

            ChatLoggerManager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            ChatLoggerManager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);

            ChatLoggerManager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            ChatLoggerManager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);
            ChatLoggerManager.Subscribe<SteamUser.AccountInfoCallback>(OnAccountInfo);
            ChatLoggerManager.Subscribe<SteamFriends.FriendMsgCallback>(OnFriendMsg);
            ChatLoggerManager.Subscribe<SteamFriends.FriendMsgEchoCallback>(OnFriendEchoMsg);

           // ChatLoggerManager.Subscribe<SteamFriends.ChatMsgCallback>(OnChatRoomMsg); soon
            #endregion

            Console.WriteLine("[" + Program.BOTNAME + "] - Connecting to Steam...");


            steamClient.Connect();
            while (isRunning)
            {
                ChatLoggerManager.RunWaitCallbacks(TimeSpan.FromMilliseconds(500));
            }
        }

        static void OnConnected(SteamClient.ConnectedCallback callback)
        {
            if (callback.ToString() != "SteamKit2.SteamClient+ConnectedCallback")
            {
                Console.WriteLine("[" + Program.BOTNAME + "] - Unable to connect to Steam: {0}", callback.ToString());

                isRunning = false;
                return;
            }
            //Sucess
            Console.WriteLine("[" + Program.BOTNAME + "] - Connected to Steam! Logging in '{0}'...", user);

            byte[] sentryHash = null;
            if (File.Exists(Program.SentryFolder + user + ".bin"))
            {
                byte[] sentryFile = File.ReadAllBytes(Program.SentryFolder + user + ".bin");
                sentryHash = CryptoHelper.SHAHash(sentryFile);
            }

            //Set LoginKey for user
            var list = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(Program.AccountsJsonFile));
            foreach (var a in list.Accounts)
            {
                if (a.username == user)
                {
                    if (string.IsNullOrEmpty(a.LoginKey) || a.LoginKey.ToString() == "undefined")// redo
                    {
                    }
                    else
                    {
                        NewloginKey = a.LoginKey;
                        File.WriteAllText(Program.AccountsJsonFile, JsonConvert.SerializeObject(list, Formatting.Indented)); // update login key
                    }
                }
            }


            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = user,
                Password = pass,
                //
                AuthCode = authCode,
                TwoFactorCode = twoFactorAuth,
                //SentryFileHash = sentryHash,
                //
                LoginID = 1337,
                ShouldRememberPassword = true,
                LoginKey = NewloginKey
            });
        }

        static void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            LastLogOnResult = callback.Result;


            bool isSteamGuard = callback.Result == EResult.AccountLogonDenied;
            bool is2FA = callback.Result == EResult.AccountLoginDeniedNeedTwoFactor;
            bool isLoginKey = callback.Result == EResult.InvalidPassword && NewloginKey != null;

            if (isSteamGuard || is2FA || isLoginKey)
            {

                if (!isLoginKey)
                {
                    Console.WriteLine("[" + Program.BOTNAME + "] - This account is SteamGuard protected!");
                }

                if (is2FA)
                {
                    SteamGuard SteamGuard = new SteamGuard("Phone", user,false);
                    SteamGuard.ShowDialog();

                    bool UserInputCode = true;
                    while (UserInputCode)
                    {
                        if (SteamGuard.AuthCode.Length == 5) // Wait for user input
                        {
                            UserInputCode = false;
                        }
                    }

                    twoFactorAuth = SteamGuard.AuthCode;

                }
                else if (isLoginKey)
                {
                    var list = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(Program.AccountsJsonFile));
                    foreach (var a in list.Accounts)
                    {
                        if (a.username == user)
                        {
                            a.LoginKey = "";
                            Console.WriteLine("[" + Program.BOTNAME + "] - Removed old loginkey!");
                        }
                    }
                    string output = JsonConvert.SerializeObject(list, Formatting.Indented);
                    File.WriteAllText(Program.AccountsJsonFile, output);

                    NewloginKey = "";

                    if (pass != null)
                    {
                        Console.WriteLine("[" + Program.BOTNAME + "] - Login key expired. Connecting with user password.");
                        InfoForm.InfoHelper.CustomMessageBox.Show("Info", "Login key expired or wrong Password. Connecting with user password. Wait 3secs...");

                    }
                    else
                    {
                        Console.WriteLine("[" + Program.BOTNAME + "] - Login key expired.");
                        InfoForm.InfoHelper.CustomMessageBox.Show("Info", "Login key expired! Wait 3secs...");
                    }
                }
                else
                {
                    Console.Write("[" + Program.BOTNAME + "] - Please enter the auth code sent to the email at {0}: ", callback.EmailDomain);

                    SteamGuard SteamGuard = new SteamGuard(callback.EmailDomain, user,false);
                    SteamGuard.ShowDialog();

                    bool UserInputCode = true;
                    while (UserInputCode)
                    {
                        if (SteamGuard.AuthCode.Length == 5) // Wait for user input
                        {
                            UserInputCode = false;
                        }
                    }
                    authCode = SteamGuard.AuthCode;
                }
                return;

            }
            else if (callback.Result != EResult.OK)
            {
                Console.WriteLine("[" + Program.BOTNAME + "] - Unable to logon to Steam: {0} / {1}", callback.Result, callback.ExtendedResult);
                InfoForm.InfoHelper.CustomMessageBox.Show("Error", "Unable to logon to Steam: " + callback.Result);
                isRunning = false;
                return;
            }

            // LOGGED:
            Console.WriteLine("[" + Program.BOTNAME + "] - Successfully logged on!");

            steamID = steamClient.SteamID.ConvertToUInt64().ToString();
            CurrentSteamID = steamClient.SteamID.ConvertToUInt64();
            UserCountry = callback.IPCountryCode;

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

        static void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            if (callback.UserInitiated)
            {
                return;
            }

            EResult lastLogOnResult = LastLogOnResult;
            LastLogOnResult = EResult.Invalid;

            DisconnectedCounter++;
            CurrentPersonaState = 0;

            if (isRunning)
            {
                if (DisconnectedCounter >= MaxDisconnects)
                {
                    Console.WriteLine("[" + Program.BOTNAME + "] - Too many disconnects occured in a short period of time. Wait 1 minute...");
                    InfoForm.InfoHelper.CustomMessageBox.Show("Error", "Too many disconnects occured in a short period of time. Wait 1 minute...");
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    DisconnectedCounter = 0;
                    steamClient.Disconnect();
                }
            }


            Console.WriteLine("[" + Program.BOTNAME + "] - Reconnecting in 1min ...");
            Thread.Sleep(TimeSpan.FromMinutes(1));

            steamClient.Connect();

        }

        /*
        switch (lastLogOnResult)
        {
            case EResult.AccountDisabled:
                // Do not attempt to reconnect, those failures are permanent
                return;
            case EResult.InvalidPassword:
            case EResult.NoConnection:
            case EResult.ServiceUnavailable:
            case EResult.Timeout:
            case EResult.TryAnotherCM:
            case EResult.TwoFactorCodeMismatch:
                //retry
                TimeSpan.FromSeconds(5);
                steamClient.Connect();
                Console.WriteLine("Disconnected from steam, reconnecting in 5 sec... [" + lastLogOnResult + "]");

                break;
            case EResult.AccountLoginDeniedNeedTwoFactor:
                steamClient.Connect();
                break;

            case EResult.RateLimitExceeded:
                //retry
                Console.WriteLine("Disconnected from steam, please try again in 30min [" + lastLogOnResult + "]");

                break;
                // default: //test
                //   TimeSpan.FromSeconds(5);
                //   steamClient.Connect();
                //   Console.WriteLine("Disconnected from steam, reconnecting in 5 sec... [" + lastLogOnResult + "]");
                //    break;
        }
        */
  

        static void OnLoginKey(SteamUser.LoginKeyCallback callback)
        {
            steamUser.AcceptNewLoginKey(callback);


            var list = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(Program.AccountsJsonFile));
            foreach (var a in list.Accounts)
            {
                if (a.username == user)
                {
                    a.LoginKey = callback.LoginKey; // check this
                    NewloginKey = callback.LoginKey;// check this

                    if (a.SteamID.ToString() == "0")//add null or empty?
                    {
                        a.SteamID = steamClient.SteamID.ConvertToUInt64();
                    }
                    Console.WriteLine("[" + Program.BOTNAME + "] - Got Login-Key, setting in config!");
                }
            }
            File.WriteAllText(Program.AccountsJsonFile, JsonConvert.SerializeObject(list, Formatting.Indented));
        }

        static void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            LastLogOnResult = callback.Result;


            IsLoggedIn = false;
            CurrentPersonaState = 0;
            Console.WriteLine("[" + Program.BOTNAME + "] - Logged off of Steam: {0}", callback.Result);
            InfoForm.InfoHelper.CustomMessageBox.Show("Error", "Logged off of Steam:" + callback.Result);
        }

        static void OnMachineAuth(SteamUser.UpdateMachineAuthCallback callback)
        {
            Console.WriteLine("[" + Program.BOTNAME + "] - Updating sentryfile...");

            byte[] sentryHash = CryptoHelper.SHAHash(callback.Data);
            File.WriteAllBytes(Program.SentryFolder + user + ".bin", callback.Data);

            steamUser.SendMachineAuthResponse(new SteamUser.MachineAuthDetails { JobID = callback.JobID, FileName = callback.FileName, BytesWritten = callback.BytesToWrite, FileSize = callback.Data.Length, Offset = callback.Offset, Result = EResult.OK, LastError = 0, OneTimePassword = callback.OneTimePassword, SentryFileHash = sentryHash, });
            Console.WriteLine("[" + Program.BOTNAME + "] - Sentry updated!");
        }


        static void OnAccountInfo(SteamUser.AccountInfoCallback callback)
        {
            UserPersonaName = callback.PersonaName;
            UserCountry = callback.Country;
        }

        static void OnFriendMsg(SteamFriends.FriendMsgCallback callback)
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
                }
            }
        }

        static void OnFriendEchoMsg(SteamFriends.FriendMsgEchoCallback callback)
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
                }
            }
        }

        static void OnChatRoomMsg(SteamFriends.ChatMsgCallback callback)// ver isto nao alterado para grupos , id steamid name
        {
            if (callback.ChatMsgType == EChatEntryType.ChatMsg)
            {

                var Settingslist = JsonConvert.DeserializeObject<ChatLoggerSettings>(File.ReadAllText(Program.SettingsJsonFile));

                string GroupName = Extensions.ResolveGroupName(callback.ChatRoomID);

                ulong FriendID = callback.ChatterID;
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
                }
            }
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

        public static string GetPersonaName(ulong steamid)
        {
            return steamFriends.GetFriendPersonaName((SteamID)steamid);
        }

        public static void Logout()
        {
            user = null;
            isRunning = false;
            IsLoggedIn = false;
            steamUser.LogOff();
            CurrentPersonaState = 0;
            CurrentUsername = null;
            DisconnectedCounter = 0;
    }
    }

}

class MyListener : IDebugListener
{
    public void WriteLine(string category, string msg)
    {
        Console.WriteLine("[IDebug] 💔 " + category + ": " + msg);
    }
}