using ChatLogger.Helpers;
using System;
using System.IO;
using System.Media;
using System.Windows.Forms;
using Win32Interop.Methods;
using System.Drawing;

namespace ChatLogger
{
    public partial class Info :  MetroFramework.Forms.MetroForm
    {

        //https://stackoverflow.com/questions/6932792/how-to-create-a-custom-messagebox
        public Info(string title, string description)
        {
            InitializeComponent();
            this.components.SetStyle(this);
            this.lbl_title.Text = title;
            this.txtBox_info.Text = description;
            this.FormBorderStyle = FormBorderStyle.None;
            Region = Region.FromHrgn(Gdi32.CreateRoundRectRgn(0, 0, Width, Height, 5, 5));

            IntPtr ptr = Gdi32.CreateRoundRectRgn(1, 1, btn_okinfo.Width, btn_okinfo.Height, 5, 5);
            btn_okinfo.Region = Region.FromHrgn(ptr);
            Gdi32.DeleteObject(ptr);
        }
        
        private void Btn_okinfo_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Info_Shown(object sender, EventArgs e)
        {
            Stream str = Properties.Resources.ChatLogger_Alert;
            SoundPlayer snd = new SoundPlayer(str);
            snd.Play();
        }
    }
}
