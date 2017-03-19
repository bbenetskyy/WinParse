using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ToolsPortable;
using WinParse.BusinessLogic.Models;

namespace DXApplication1.Pages
{
    public partial class OpenCalculatorForm : Form
    {
        public Fork SelectedEvent { get; set; }
        private List<Fork> dataSource;

        public OpenCalculatorForm()
        {
            InitializeComponent();
            dataSource = new List<Fork>();
        }

        public OpenCalculatorForm(List<Fork> dataList)
            : this()
        {
            dataSource.AddRange(dataList);
            gridControl1.DataSource = dataList;

            if (gridView1.RowCount == 0)
                buttonOpen.Enabled = false;
            else
            {
                gridView1.FocusedRowHandle = 0;
                SelectedEvent = gridView1.GetFocusedRow() as Fork;
            }
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            if (textEditEvent.Text.IsNotBlank() ||
                textEditTypeFirst.Text.IsNotBlank() ||
                textEditTypeSecond.Text.IsNotBlank())
            {
                gridControl1.DataSource = MakeSearch(
                    textEditEvent.Text,
                    textEditTypeFirst.Text,
                    textEditTypeSecond.Text);
                gridControl1.Refresh();
            }
        }

        private List<Fork> MakeSearch(string eventCriteria, string typeFirstCriteria, string typeSecondCriteria)
        {
            var resList = new List<Fork>();
            resList.AddRange(dataSource);

            if (eventCriteria.IsNotBlank())
                resList = resList.Where(f => f.Event.Contains(eventCriteria)).ToList();
            if (typeFirstCriteria.IsNotBlank())
                resList = resList.Where(f => f.TypeFirst.Contains(typeFirstCriteria)).ToList();
            if (typeSecondCriteria.IsNotBlank())
                resList = resList.Where(f => f.TypeSecond.Contains(typeSecondCriteria)).ToList();

            return resList;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textEditEvent.Text = string.Empty;
            textEditTypeSecond.Text = string.Empty;
            textEditTypeFirst.Text = string.Empty;
            gridControl1.DataSource = dataSource;
            gridControl1.Refresh();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                buttonOpen.Enabled = true;
                SelectedEvent = gridView1.GetFocusedRow() as Fork;
            }
        }
    }
}