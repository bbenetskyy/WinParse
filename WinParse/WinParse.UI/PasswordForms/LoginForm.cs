﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WinParse.Resources;

namespace WinParse.UI.PasswordForms
{
    public partial class LoginForm : XtraForm
    {
        public LoginForm()
        {
            InitializeComponent();
            ResMan.ResourceChaned += ResMan_ResourceChaned;
        }


        private void ResMan_ResourceChaned(object sender, EventArgs e)
        {
            layoutControlItemLoginTextEdit.Text = ResMan.GetString(ResKeys.LoginForm_LoginTextEdit_Text);
            layoutControlItemLoginTextEdit.CustomizationFormText = ResMan.GetString(ResKeys.LoginForm_LoginTextEdit_CustomizationFormText);

            layoutControlItemPasswordTextEdit.Text = ResMan.GetString(ResKeys.LoginForm_PasswordTextEdit_Text);
            layoutControlItemPasswordTextEdit.CustomizationFormText = ResMan.GetString(ResKeys.LoginForm_PasswordTextEdit_CustomizationFormText);

            layoutControlItemSaveCheckBox.Text = ResMan.GetString(ResKeys.LoginForm_SaveCheckBox_Text);
            layoutControlItemSaveCheckBox.CustomizationFormText = ResMan.GetString(ResKeys.LoginForm_SaveCheckBox_CustomizationFormText);

            simpleButtonOk.ToolTip = ResMan.GetString(ResKeys.LoginForm_ButtonOk_ToolTip);
            simpleButtonCancel.ToolTip = ResMan.GetString(ResKeys.LoginForm_ButtonCancel_ToolTip);
        }

        /// <summary>
        /// Get data from filled forms
        /// </summary>
        /// <returns>Dynamic type with Login(string), Password(string), Url(string), SaveMe(bool)</returns>
        public dynamic GetCredentials
            =>
                new
                {
                    Login = textEditLogin.Text,
                    Password = textEditPassword.Text,
                    Url = textEditUrl.Text,
                    SaveMe = checkBoxSave.Checked
                };
    }
}