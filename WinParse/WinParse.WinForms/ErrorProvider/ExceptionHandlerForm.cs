﻿using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace WinParse.WinForms.ErrorProvider
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}