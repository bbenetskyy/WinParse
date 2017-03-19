﻿using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace DXApplication1.ErrorProvider
{
    public partial class ExceptionHandlerForm : XtraForm
    {
        public ExceptionHandlerForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        public string StackTrace
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }

        public static void ShowException(Exception ex)
        {
            var form = new ExceptionHandlerForm();

            form.StackTrace = ex.Message;
            form.StackTrace += ex.StackTrace;
            form.ShowDialog();
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}