using SteamKit2.Authentication;
using System;
using System.Threading.Tasks;

namespace chatlogger2.Steam
{
    public class WebAuthenticator : IAuthenticator
    {
        private TaskCompletionSource<string> _authCodeCompletionSource;
        private TaskCompletionSource<bool> _deviceConfirmationCompletionSource;

        // This method will be called by Steam when a 2FA code is needed
        public Task<string> GetDeviceCodeAsync(bool previousCodeWasIncorrect)
        {
            _authCodeCompletionSource = new TaskCompletionSource<string>();
            Console.WriteLine("Waiting for the user to provide 2FA code...");
            return _authCodeCompletionSource.Task;
        }

        // This method will be called when Steam requires a Steam Guard code sent via email
        public Task<string> GetEmailCodeAsync(string email, bool previousCodeWasIncorrect)
        {
            _authCodeCompletionSource = new TaskCompletionSource<string>();
            Console.WriteLine($"Waiting for the Steam Guard code sent to email: {email}...");
            return _authCodeCompletionSource.Task;
        }

        // Called when Steam requires confirmation for new device login
        public Task<bool> AcceptDeviceConfirmationAsync()
        {
            _deviceConfirmationCompletionSource = new TaskCompletionSource<bool>();
            Console.WriteLine("Device confirmation required...");
            _deviceConfirmationCompletionSource.SetResult(true); // Automatically accept for this example
            return _deviceConfirmationCompletionSource.Task;
        }

        // Provide the 2FA code when the user inputs it via web form
        public void ProvideDeviceCode(string code)
        {
            _authCodeCompletionSource?.SetResult(code);
        }

        // Provide the Steam Guard code when the user inputs it via web form
        public void ProvideEmailCode(string code)
        {
            _authCodeCompletionSource?.SetResult(code);
        }
    }
}
