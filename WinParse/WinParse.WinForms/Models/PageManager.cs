using DXApplication1.Pages;
using System;
using System.Windows.Forms;
using WinParse.BusinessLogic.Enums;
using WinParse.BusinessLogic.Models;

namespace DXApplication1.Models
{
    public class PageManager
    {
        #region Members

        private FilterPage _filterPage;
        private SearchPage _accountingPage;
        private SearchPage _searchPage;
        private CalculatorPage _calculatorPage;
        private readonly Form _defaultMdiParent;
        public DataManager DataManager { get; set; }
        private readonly Timer _timer;
        private bool _fromSearchPage = false;

        #endregion Members

        #region CTOR

        public PageManager(Form mdiParent)
        {
            _defaultMdiParent = mdiParent;
            DataManager = new DataManager();
            _timer = new Timer
            {
                Interval = 20 * 1000 //default for 20 seconds
            };
            _timer.Tick += Timer_Tick;
        }

        #endregion CTOR

        #region Functions

        public void CloseAllPages()
        {
            _accountingPage = null;
            _calculatorPage = null;
            _filterPage = null;
            _searchPage = null;
        }

        public FilterPage GetFilterPage(Filter filter, Form mdiParent = null, bool reload = false)
        {
            _timer.Stop();
            if (filter.Id == null) DataManager.MapFilter(filter);


            return _filterPage ?? (_filterPage = new FilterPage(filter)
            {
                MdiParent = mdiParent ?? _defaultMdiParent,
                ToClose = false
            });
        }

        public CalculatorPage GetCalculatorPage(Form mdiParent = null, bool reload = false)
        {
            _timer.Stop();
            return _calculatorPage ?? (_calculatorPage = new CalculatorPage
            {
                MdiParent = mdiParent ?? _defaultMdiParent,
                ToClose = false
            });
        }

        public SearchPage GetAccountPage(Form mdiParent = null, bool reload = false)
        {
            _timer.Stop();
            if (_accountingPage == null || reload)
            {
                _accountingPage = new SearchPage(false)
                {
                    MdiParent = mdiParent ?? _defaultMdiParent,
                    ToClose = false
                };
                _accountingPage.UpdateEvent += AccountPage_Update;
                _accountingPage.CalculatorCall += AccountPage_CalculatorCall;
            }
            if (_filterPage.Filter.AutoUpdateTime != null)
                _timer.Interval = _filterPage.Filter.AutoUpdateTime.Value * 1000;

            _fromSearchPage = false;
            _timer.Start();

            return _accountingPage;
        }

        public SearchPage GetSearchPage(Form mdiParent = null, bool reload = false)
        {
            _timer.Stop();
            if (_searchPage == null || reload)
            {
                _searchPage = new SearchPage
                {
                    MdiParent = mdiParent ?? _defaultMdiParent,
                    ToClose = false
                };
                _searchPage.UpdateEvent += SearchPage_Update;
                _searchPage.CalculatorCall += AccountPage_CalculatorCall;//can be the same as for account page
            }

            if (_filterPage.Filter.AutoUpdateTime != null)
                _timer.Interval = _filterPage.Filter.AutoUpdateTime.Value * 1000;

            _fromSearchPage = true;
            _timer.Start();

            return _searchPage;
        }

        #endregion Functions

        #region Events

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_fromSearchPage && !_searchPage.IsDisposed)
                SearchPage_Update(sender, e);
            else if (!_accountingPage.IsDisposed)
                AccountPage_Update(sender, e);
        }

        private async void AccountPage_Update(object sender, EventArgs e)
        {
            _accountingPage.StartLoading();
            var resList = await DataManager.
                GetForksForAllSportsAsync(_filterPage?.Filter, ForkType.Saved).ConfigureAwait(true);
            /*
             * We should just update changed rows,
             * not all as it is now
             */
            _accountingPage.MainGridControl.DataSource = null;
            _accountingPage.MainGridControl.DataSource = resList;
            _accountingPage.MainGridControl.Refresh();
            _accountingPage.EndLoading();
        }

        /// <summary>
        /// Open calculator for Search and Account pages
        /// </summary>
        private void AccountPage_CalculatorCall(object sender, EventArgs eventArgs)
        {
            if (!(sender is Fork)) return;

            var fork = (Fork)sender;
            var profit = fork.Profit;
            fork.Profit = 100;

            GetCalculatorPage(reload: true).Fork = fork;
            GetCalculatorPage().Show();
            fork.Profit = profit;
        }

        private async void SearchPage_Update(object sender, EventArgs e)
        {
            _searchPage.StartLoading();
            var resList = await DataManager.
                GetForksForAllSportsAsync(_filterPage?.Filter, ForkType.Current).ConfigureAwait(true);
            /*
             * We should just update changed rows,
             * not all as it is now
             */
            _searchPage.MainGridControl.DataSource = null;
            _searchPage.MainGridControl.DataSource = resList;
            _searchPage.MainGridControl.Refresh();
            _searchPage.EndLoading();
        }

        #endregion Events
    }
}