using SteamKit2;
using System;
using System.Threading.Tasks;

namespace chatlogger2.Steam
{
    public class Auth
    {
        private readonly SteamClient steamClient;
        private readonly CallbackManager manager;
        private readonly SteamUser steamUser;
        private TaskCompletionSource<bool> loginTaskCompletionSource;

        private bool isLoggedIn;

        private string _username;
        private string _password;
        private string _twoFactorCode;
        private string _steamGuardCode;

        public bool IsLoggedIn => isLoggedIn;

        public Auth()
        {
            steamClient = new SteamClient();
            manager = new CallbackManager(steamClient);
            steamUser = steamClient.GetHandler<SteamUser>();

            // Subscribe to callbacks
            manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);
            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);
        }

        public async Task ConnectAndLoginAsync(string username, string password, string twoFactorCode = "", string steamGuardCode = "")
        {
            _username = username;
            _password = password;
            _twoFactorCode = twoFactorCode;
            _steamGuardCode = steamGuardCode;

            loginTaskCompletionSource = new TaskCompletionSource<bool>();

            if (!steamClient.IsConnected)
            {
                Console.WriteLine("Connecting to Steam...");
                steamClient.Connect();
            }

            while (!loginTaskCompletionSource.Task.IsCompleted)
            {
                manager.RunWaitCallbacks(TimeSpan.FromMilliseconds(100));
                await Task.Delay(100); // Ensure the loop doesn't block the async context entirely
            }

            await loginTaskCompletionSource.Task; // Wait for the login result
        }

        private void OnConnected(SteamClient.ConnectedCallback callback)
        {
            Console.WriteLine("Connected to Steam. Logging in...");
            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = _username,
                Password = _password,
                TwoFactorCode = _twoFactorCode,
                AuthCode = _steamGuardCode
            });
        }

        private void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            Console.WriteLine("Disconnected from Steam.");
            isLoggedIn = false;

            if (!loginTaskCompletionSource.Task.IsCompleted)
            {
                loginTaskCompletionSource.TrySetResult(false);
            }
        }

        private void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            if (callback.Result == EResult.OK)
            {
                Console.WriteLine("Successfully logged in to Steam!");
                isLoggedIn = true;
                loginTaskCompletionSource.TrySetResult(true);
            }
            else if (callback.Result == EResult.AccountLogonDenied)
            {
                Console.WriteLine("Steam Guard code required (check your email).");
                loginTaskCompletionSource.TrySetException(new SteamGuardRequiredException("Steam Guard code required."));
            }
            else if (callback.Result == EResult.AccountLoginDeniedNeedTwoFactor)
            {
                Console.WriteLine("Two-factor authentication code required.");
                loginTaskCompletionSource.TrySetException(new TwoFactorCodeRequiredException("Two-factor authentication code required."));
            }
            else
            {
                Console.WriteLine($"Failed to log in: {callback.Result}");
                isLoggedIn = false;
                loginTaskCompletionSource.TrySetResult(false);
            }
        }

        private void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {
            Console.WriteLine("Logged off from Steam.");
            isLoggedIn = false;
        }
    }

    // Custom exceptions to handle 2FA or Steam Guard requirements
    public class SteamGuardRequiredException : Exception
    {
        public SteamGuardRequiredException(string message) : base(message) { }
    }

    public class TwoFactorCodeRequiredException : Exception
    {
        public TwoFactorCodeRequiredException(string message) : base(message) { }
    }
}
