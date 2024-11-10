using chatlogger2.Steam;
using System.Net;
using System.Text;
using SteamKit2;
using SteamKit2.Authentication;
using System.Threading.Tasks;
using System.IO;
using System.Security.Policy;
using System;
using Win32Interop.Enums;
using static SteamKit2.Internal.CMsgRemoteClientBroadcastStatus;
using SteamKit2.Internal;
using ChatLogger.UserSettings;
using Newtonsoft.Json;
using static SteamKit2.Internal.CMsgClientClanState;
using ChatLogger.Helpers;
using ChatLogger.User2Json;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Text.Json;
using SteamKit2.Discovery;


namespace ChatLogger
{
    public class HandleLogin
    {

        public static string UserPersonaName, UserCountry, CurrentUsername;
        public static int CurrentPersonaState = 1;


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


        private WebAuthenticator _webAuthenticator = new WebAuthenticator();
        private AuthSession _authSession;

        private SteamClient steamClient;
        private CallbackManager manager;
        private SteamUser steamUser;
        private SteamFriends steamFriends;



        // TaskCompletionSource to manage the state of the connection
        private TaskCompletionSource<bool> _connectionCompletionSource;


        public HandleLogin()
        {
            // Initialize Steam client and related components
            steamClient = new SteamClient();
            manager = new CallbackManager(steamClient);
            steamUser = steamClient.GetHandler<SteamUser>();
            steamFriends = steamClient.GetHandler<SteamFriends>();

            // Subscribe to necessary callbacks
            manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);
            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);
            manager.Subscribe<SteamFriends.FriendMsgCallback>(OnFriendMsg);
            manager.Subscribe<SteamFriends.FriendMsgEchoCallback>(OnFriendEchoMsg);

            manager.Subscribe<SteamUser.AccountInfoCallback>(OnAccountInfo);


            // Start running the callback manager in a background task
            Task.Run(() => RunCallbackManager());
        }

        public async Task StartLogin(string user, string pw)
        {
            string username = user;
            string password = pw;
            string getAccessToken;


            try
            {
                if (_authSession == null)
                {
                    Console.WriteLine($"Attempting initial login for user: {username}");

                    int retryCount = 3;

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

                        byte[] tknHash = null;
                        if (File.Exists(Program.SentryFolder + user + "_tkn.bin"))
                        {
                            byte[] tknFile = File.ReadAllBytes(Program.SentryFolder + user + "_tkn.bin");
                            tknHash = CryptoHelper.SHAHash(tknFile);
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
                Console.WriteLine("[" + Program.BOTNAME + "] - Successfully logged on!");

                steamID = steamUser.SteamID.ConvertToUInt64().ToString();

                CurrentSteamID = steamUser.SteamID.ConvertToUInt64();
                //UserCountry = callback.;

                //  avatar = BitConverter.ToString(steamFriends.GetFriendAvatar(CurrentSteamID)).Replace("-", "").ToLower();


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
                Console.WriteLine($"Unable to log on to Steam: {callback.Result}");
                //_connectionCompletionSource?.SetResult(false);

                if (callback.Result == EResult.Expired)
                {
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
                }
            }
        }

        private void OnChatRoomMsg(SteamFriends.ChatMsgCallback callback)// ver isto nao alterado para grupos , id steamid name
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
                // string SHA1 = BitConverter.ToString(steamFriends.GetFriendAvatar(steamid)).Replace("-", "").ToLower();
                string SHA1 = avatar;
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
            // return steamFriends.GetFriendPersonaName((SteamID)steamid);
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
}
