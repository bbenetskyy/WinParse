using DevExpress.LookAndFeel;
using DXApplication1;
using System;
using System.Windows.Forms;

namespace WinParse.WinForms
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application. 
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");

            Application.Run(new XtraForm1());
        }
    }
}