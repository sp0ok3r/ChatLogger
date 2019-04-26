using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Forms;
using System;
using System.ComponentModel;
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

        
    }
}