using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using WinParse.UI.HomeForm;
using WinParse.UserManager.Properties;

namespace WinParse.UserManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            var homeForm = new DefaultHomeForm
            {
                Language = Settings.Default.Language,
                Skin = Settings.Default.Skin
            };
            Application.Run(homeForm);
            Settings.Default.Language = homeForm.Language;
            Settings.Default.Skin = homeForm.Skin;
        }
    }
}
