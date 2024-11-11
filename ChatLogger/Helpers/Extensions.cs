using ChatLogger.User2Json;
using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Forms;
using Newtonsoft.Json;
using SteamKit2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace ChatLogger.Helpers
{
    public static class Extensions
    {

        public static string ResolveGroupName(ulong groupid)// fast way, without api key
        {
            var RespGroupName = new WebClient().DownloadString("https://steamcommunity.com/gid/" + groupid + "/memberslistxml/?xml=1"); // 6521iq
            return Between(RespGroupName, "<groupName><![CDATA[", "]]></groupName>");
        }



        public static DateTime GetTime(string timeStamp)
        {
            var dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var lTime = long.Parse($@"{timeStamp}0000000");
            var toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        #region steam related
        public static List<EPersonaState> statesList = new List<EPersonaState> {
                        EPersonaState.Offline,
                        EPersonaState.Online,
                        EPersonaState.Busy,
                        EPersonaState.Away,
                        EPersonaState.Snooze,
                        EPersonaState.LookingToTrade,
                        EPersonaState.LookingToPlay,
                        EPersonaState.Invisible };

        public static bool IsSteamid32(string input) => input.StartsWith("STEAM_0:");

        public static bool IsSteamid64(string input) => (input.Length == 17) && input.StartsWith("7656");

        public static bool IsSteamURL(string input)
        {
            string url = input.Replace("https://", "").Replace("http://", "");
            return url.Contains("steamcommunity.com/id/") || url.Contains("steamcommunity.com/profiles/");
        }
        public static string ToSteamID64(string input)
        {
            string[] split = input.Replace("STEAM_", "").Split(':');
            return (76561197960265728 + (Convert.ToInt64(split[2]) * 2) + Convert.ToInt64(split[1])).ToString();
        }
        public static string AllToSteamId64(string input)
        {
            if (IsSteamid32(input))
            {
                return ToSteamID64(input); //("765" + (input + 61197960265728)); // test ???
            }
            else if (IsSteamid64(input))
            {
                return input;
            }
            else if (IsSteamURL(input) && input.Contains("steamcommunity.com/profiles/"))
            {
                return input.Replace("https://steamcommunity.com/profiles/", "").Replace("/", "");
            }
            else if (input.Contains("steamcommunity.com/id/"))
            {
                return ResolveVanityURL(input);
            }

            return String.Empty;
        }
        public static string Between(string STR, string FirstString, string LastString)
        {
            string FinalString;
            int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
            int Pos2 = STR.IndexOf(LastString);
            FinalString = STR.Substring(Pos1, Pos2 - Pos1);
            return FinalString;
        }

        public static string ResolveVanityURL(string ProfileURL)// fast way, without api key
        {
            var RespSteamProfile = new WebClient().DownloadString(ProfileURL + "?xml=1"); // 6520iq
            return Between(RespSteamProfile, "<steamID64>", "</steamID64>");
        }
        public static string ResolveAvatar(string steamid64)// fast way, without api key
        {
            try
            {
                var RespSteamProfile = new WebClient().DownloadString("https://steamcommunity.com/profiles/" + AllToSteamId64(steamid64) + "?xml=1"); // 6521iq
                return Between(RespSteamProfile, "<avatarFull><![CDATA[", "]]></avatarFull>");
            }
            catch
            {
                return "error avatar";
            }
        }
        #endregion
        private static MetroColorStyle FormStyle;
        public static void SetStyle(this IContainer container, MetroForm ownerForm)
        {
            if (!File.Exists(Program.SettingsJsonFile))
            {
                File.WriteAllText(Program.SettingsJsonFile, "{}");
            }

            var Settingslist = JsonConvert.DeserializeObject<ChatLoggerSettings>(File.ReadAllText(Program.SettingsJsonFile));
            FormStyle = (MetroFramework.MetroColorStyle)Convert.ToUInt32(Settingslist.startupColor);

            if (container == null)
            {
                container = new System.ComponentModel.Container();
            }
            var manager = new MetroFramework.Components.MetroStyleManager(container);
            manager.Owner = ownerForm;
            container.SetDefaultStyle(ownerForm, FormStyle);
        }
        public static void SetDefaultStyle(this IContainer contr, MetroForm owner, MetroColorStyle style)
        {
            MetroStyleManager manager = FindManager(contr, owner);
            manager.Style = style;
            owner.Style = style;
        }
        public static void SetDefaultTheme(this IContainer contr, MetroForm owner, MetroThemeStyle thme)
        {
            MetroStyleManager manager = FindManager(contr, owner);
            manager.Theme = thme;
        }
        private static MetroStyleManager FindManager(IContainer contr, MetroForm owner)
        {
            MetroStyleManager manager = null;
            foreach (IComponent item in contr.Components)
                try
                {
                    if (((MetroStyleManager)item).Owner == owner)
                    {
                        manager = (MetroStyleManager)item;
                    }
                }
                catch (Exception)
                {
                    // Ignore others
                }
            return manager;
        }
    }
}