using ChatLogger.User2Json;
using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Forms;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace ChatLogger.Helpers
{
    public static class Extensions
    {
        public static DateTime GetTime(string timeStamp)
        {
            var dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var lTime = long.Parse($@"{timeStamp}0000000");
            var toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
               (
                   int nLeftRect,     // x-coordinate of upper-left corner
                   int nTopRect,      // y-coordinate of upper-left corner
                   int nRightRect,    // x-coordinate of lower-right corner
                   int nBottomRect,   // y-coordinate of lower-right corner
                   int nWidthEllipse, // height of ellipse
                   int nHeightEllipse // width of ellipse
               );

        //What is your style        
        private static MetroColorStyle FormStyle;
        
        public static void SetStyle(this IContainer container, MetroForm ownerForm)
        {
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
            {
                if (((MetroStyleManager)item).Owner == owner)
                {
                    manager = (MetroStyleManager)item;
                }
            }
            return manager;
        }
    }
}