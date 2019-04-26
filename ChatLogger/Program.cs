using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatLogger
{
    static class Program
    {
        public static readonly string BOTNAME = "ChatLogger";
        public static readonly string BOTNAMELowerCaps = "ChatLogger";

        public static readonly string spkDomain = "https://github.com/sp0ok3r/SteamChatLogger";
        public static readonly string Version = "1.0.0";


        public static readonly string ExecutablePath = Path.GetDirectoryName(Application.ExecutablePath);
        public static readonly string AccountsJsonFile = ExecutablePath + @"\Accounts.json";
        public static readonly string SettingsJsonFile = ExecutablePath + @"\Settings.json";
        public static readonly string SentryFolder = ExecutablePath + @"\Sentry\";
        public static readonly string ChatLogsFolder = ExecutablePath + @"\ChatLogs\";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
