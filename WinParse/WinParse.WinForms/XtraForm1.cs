//#define UseLicense

using System.Windows.Forms;
using DataSaver;
using DevExpress.XtraEditors;
using FormulasCollection.Models;
using NLog;
using WinParse.WinForms.Models;

namespace WinParse.WinForms
{
    public partial class XtraForm1 : XtraForm
    {
        #region Members

        // ReSharper disable once InconsistentNaming
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public const string SettingsPath = "./";
        public const string SettingsFile = "Configuration.xml";

        private Filter _filter;
        private PageManager _pageManager;

        #endregion Members

        #region CTOR

        public XtraForm1()
        {
            InitializeComponent();

            IsMdiContainer = true;
            Closed += XtraForm1_Closed;
            Closing += XtraForm1_Closing;

            _pageManager = new PageManager(this); _filter = new Filter();
            _pageManager.GetFilterPage(_filter);
#if UseLicense
            var licenseForm = new LicenseForm("uk_UA", uk_UA.ResourceManager);
            if (!licenseForm.CheckInstance(_filter.LicenseKey ?? string.Empty))
                licenseForm.ShowDialog();
            if (!licenseForm.IsRegistered)
                Close();
            _filter.LicenseKey = licenseForm.LicenseKey;
#endif
        }

        #endregion CTOR

        #region Events

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var page = _pageManager.GetSearchPage();
            page.Hide(); //if already shown right now
            page.Show();
            page.OnUpdate();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var page = _pageManager.GetAccountPage();
            page.Hide(); //if already shown right now
            page.Show();
            page.OnUpdate();
        }

        private void XtraForm1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = XtraMessageBox.Show(
                "Действительно хотите выйти?",
                "Выход",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning)
                       == DialogResult.No;

            if (!e.Cancel)
                _pageManager.CloseAllPages();
        }

        private void XtraForm1_Closed(object sender, System.EventArgs e)
        {
            new LocalSaver("http://localhost:8765", "Parser").UpdateFilter(_filter);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var page = _pageManager.GetFilterPage(_filter);
            page.Hide(); //if already shown right now
            page.Show();
        }

        #endregion Events
    }
}