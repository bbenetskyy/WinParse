using System;
using DevExpress.LookAndFeel;
using DevExpress.XtraBars.Ribbon;
using WinParse.Resources;
using WinParse.UI.Properties;

namespace WinParse.UI.HomeForm
{
    public partial class DefaultHomeForm : RibbonForm
    {
        public DefaultHomeForm()
        {
            InitializeComponent();
        }

        private void barEditItemLanguage_EditValueChanged(object sender, EventArgs e)
        {
            ResMan.SetResource(ResMan.GetResourceByName(barEditItemLanguage.EditValue as string));
        }

        private void DefaultHomeForm_Load(object sender, EventArgs e)
        {
            LoadLanguages();
            ResMan.ResourceChaned += ResMan_ResourceChaned;
            barEditItemLanguage.EditValue = Settings.Default.Language;
            UserLookAndFeel.Default.SkinName = Settings.Default.Skin;
        }

        private void ResMan_ResourceChaned(object sender, EventArgs e)
        {
            Name = ResMan.GetString(ResKeys.DefaultHomeForm_Name);
            barEditItemLanguage.Caption = ResMan.GetString(ResKeys.DefaultHomeForm_Language_Caption);
            ribbonPageSettings.Text = ResMan.GetString(ResKeys.DefaultHomeForm_PageSettings_Text);
            rpgSkins.Text = ResMan.GetString(ResKeys.DefaultHomeForm_PageGroupSkins_Text);
            rpgLocalization.Text = ResMan.GetString(ResKeys.DefaultHomeForm_PageGroupLocalization_Text);
            repositoryItemLookUpEditLanguage.Columns["columnCode"].Caption =
                ResMan.GetString(ResKeys.DefaultHomeForm_Language_CodeColumn_Caption);
            repositoryItemLookUpEditLanguage.Columns["columnDescription"].Caption =
                ResMan.GetString(ResKeys.DefaultHomeForm_Language_Description_Caption);
        }

        private void LoadLanguages()
        {
            repositoryItemLookUpEditLanguage.DataSource = new[]
            {
                new {Code = "en-GB", Description = "English"},
                new {Code = "ru-RU", Description = "Русский"},
                new {Code = "uk-UA", Description = "Українська"},
            };
        }

        private void DefaultHomeForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Settings.Default.Language = barEditItemLanguage.EditValue as string;
            Settings.Default.Skin = UserLookAndFeel.Default.SkinName;
        }
    }
}
