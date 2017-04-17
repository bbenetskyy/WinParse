using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DataSaver;
using DevExpress.XtraGrid;

namespace WinParse.WinForms.Pages
{
    public partial class SearchPage : Form
    {
        public EventHandler UpdateEvent;
        public EventHandler CalculatorCall;
        private LocalSaver _localSaver;

        public bool ToClose { get; set; }
        public GridControl MainGridControl { get; private set; }

        public virtual void OnUpdate() => UpdateEvent?.Invoke(null, null);

        public virtual void OnCalculatorCall() => CalculatorCall?.Invoke(gridView1.GetFocusedRow(), null);

        public SearchPage(bool isSearchPage = true)
        {
            InitializeComponent();
            Closing += SearchPage_Closing;
            backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
            backgroundWorker1.WorkerSupportsCancellation = true;
            _localSaver = new LocalSaver("http://localhost:8765", "Parser");

            if (isSearchPage)
                InitSearchPage();
            else
                InitAccountingPage();
        }

        private void InitAccountingPage()
        {
            Text = "Учет";
            Icon = Icon.FromHandle(((Bitmap)imageList1.Images[3]).GetHicon());
        }

        private void InitSearchPage()
        {
            Text = "Поиск вилок";
            Icon = Icon.FromHandle(((Bitmap)imageList1.Images[2]).GetHicon());
            colMarRate.Visible = false;
            colMarSuccess.Visible = false;
            colPinRate.Visible = false;
            colPinSuccess.Visible = false;
        }

        private void BackgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            for (var i = progressBarControl1.Properties.Minimum; i <= progressBarControl1.Properties.Maximum; i++)
            {
                Thread.Sleep(5);
                progressBarControl1.Invoke(new MethodInvoker(delegate
                { progressBarControl1.EditValue = i; }));
            }
        }

        protected virtual void SearchPage_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EndLoading();
            e.Cancel = !ToClose;
            if (!ToClose)
                Hide();
        }

        protected virtual void bUpdate_Click(object sender, EventArgs e)
        {
            OnUpdate();
        }

        protected virtual void simpleButton1_Click(object sender, EventArgs e) => OnCalculatorCall();

        public void StartLoading()
        {
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
        }

        public void EndLoading()
        {
            if (backgroundWorker1.IsBusy)
                backgroundWorker1.CancelAsync();
        }
    }
}