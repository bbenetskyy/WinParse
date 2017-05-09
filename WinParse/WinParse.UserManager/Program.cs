using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DataSaver;
using DataSaver.Models;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using ToolsPortable;
using WinParse.UI.HomeForm;
using WinParse.UI.PasswordForms;
using WinParse.UserManager.Properties;

namespace WinParse.UserManager
{
    static class Program
    {
        public static User CurrentUser = null;
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

            if (!LogIn()) return;

            var homeForm = new UserManagerHomeForm
            {
                Language = Settings.Default.Language,
                Skin = Settings.Default.Skin
            };
            Application.Run(homeForm);
            Settings.Default.Language = homeForm.Language;
            Settings.Default.Skin = homeForm.Skin;
        }

        private static bool LogIn()
        {
            if (Settings.Default.Login.IsNotBlank() &&
                Settings.Default.Password.IsNotBlank() &&
                Settings.Default.Url.IsNotBlank())
            {
                CurrentUser = new LocalSaver(Settings.Default.Url, "Parser")
                    .AuthenticateUser(Settings.Default.Login, Settings.Default.Password);

                return CurrentUser != null;
            }

            var loginForm = new LoginForm {Language = Settings.Default.Language};
            if (loginForm.ShowDialog() != DialogResult.OK) return false;

            var credentials = loginForm.GetCredentials;
            if (credentials.Url.IsBlank()) return false;

            CurrentUser = new LocalSaver(credentials.Url, "Parser")
                .AuthenticateUser(credentials.Login, credentials.Password);

            if (credentials.SaveMe)
            {
                Settings.Default.Login = credentials.Login;
                Settings.Default.Password = credentials.Password;
                Settings.Default.Url = credentials.Url;
            }
            else
            {
                Settings.Default.Login = null;
                Settings.Default.Password= null;
                Settings.Default.Url = null;;
            }

            return CurrentUser != null;
        }
    }
}
