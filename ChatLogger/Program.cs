using System;
using System.IO;
using System.Windows.Forms;

namespace ChatLogger
{
    static class Program
    {
        public static readonly string BOTNAME = "ChatLogger";

        public static readonly string spkDomain = "http://sp0ok3r.tk/ChatLogger/";
        public static readonly string Version = "1.0.3c";


        public static readonly string ExecutablePath = Path.GetDirectoryName(Application.ExecutablePath);
        public static readonly string AccountsJsonFile = ExecutablePath + @"\Accounts.json";
        public static readonly string SettingsJsonFile = ExecutablePath + @"\Settings.json";
        public static readonly string SentryFolder = ExecutablePath + @"\Sentry\";
        public static readonly string ChatLogsFolder = ExecutablePath + @"\ChatLogs\";


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
