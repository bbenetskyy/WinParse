namespace WinParse.UI.PasswordForms
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.textEditLogin = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlItemLoginTextEdit = new DevExpress.XtraLayout.LayoutControlItem();
            this.textEditPassword = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlItemPasswordTextEdit = new DevExpress.XtraLayout.LayoutControlItem();
            this.textEditUrl = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlItemUrlTextEdit = new DevExpress.XtraLayout.LayoutControlItem();
            this.checkBoxSave = new System.Windows.Forms.CheckBox();
            this.layoutControlItemSaveCheckBox = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleButtonOk = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItemOkButton = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItemCancelButton = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditLogin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLoginTextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemPasswordTextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditUrl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUrlTextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSaveCheckBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemOkButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCancelButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
            this.SuspendLayout();
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.simpleButtonCancel);
            this.dataLayoutControl1.Controls.Add(this.simpleButtonOk);
            this.dataLayoutControl1.Controls.Add(this.checkBoxSave);
            this.dataLayoutControl1.Controls.Add(this.textEditUrl);
            this.dataLayoutControl1.Controls.Add(this.textEditPassword);
            this.dataLayoutControl1.Controls.Add(this.textEditLogin);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 0);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.layoutControlGroup1;
            this.dataLayoutControl1.Size = new System.Drawing.Size(288, 165);
            this.dataLayoutControl1.TabIndex = 0;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemLoginTextEdit,
            this.layoutControlItemPasswordTextEdit,
            this.layoutControlItemUrlTextEdit,
            this.layoutControlItemSaveCheckBox,
            this.emptySpaceItem1,
            this.emptySpaceItem3,
            this.emptySpaceItem4,
            this.layoutControlItemOkButton,
            this.layoutControlItemCancelButton});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(288, 165);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // textEditLogin
            // 
            this.textEditLogin.Location = new System.Drawing.Point(64, 12);
            this.textEditLogin.Name = "textEditLogin";
            this.textEditLogin.Size = new System.Drawing.Size(212, 20);
            this.textEditLogin.StyleController = this.dataLayoutControl1;
            this.textEditLogin.TabIndex = 4;
            // 
            // layoutControlItemLoginTextEdit
            // 
            this.layoutControlItemLoginTextEdit.Control = this.textEditLogin;
            this.layoutControlItemLoginTextEdit.CustomizationFormText = "Login";
            this.layoutControlItemLoginTextEdit.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemLoginTextEdit.Name = "layoutControlItemLoginTextEdit";
            this.layoutControlItemLoginTextEdit.Size = new System.Drawing.Size(268, 24);
            this.layoutControlItemLoginTextEdit.Text = "Login";
            this.layoutControlItemLoginTextEdit.TextSize = new System.Drawing.Size(48, 13);
            // 
            // textEditPassword
            // 
            this.textEditPassword.Location = new System.Drawing.Point(64, 36);
            this.textEditPassword.Name = "textEditPassword";
            this.textEditPassword.Properties.PasswordChar = '*';
            this.textEditPassword.Size = new System.Drawing.Size(212, 20);
            this.textEditPassword.StyleController = this.dataLayoutControl1;
            this.textEditPassword.TabIndex = 5;
            // 
            // layoutControlItemPasswordTextEdit
            // 
            this.layoutControlItemPasswordTextEdit.Control = this.textEditPassword;
            this.layoutControlItemPasswordTextEdit.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItemPasswordTextEdit.Name = "layoutControlItemPasswordTextEdit";
            this.layoutControlItemPasswordTextEdit.Size = new System.Drawing.Size(268, 24);
            this.layoutControlItemPasswordTextEdit.Text = "Password";
            this.layoutControlItemPasswordTextEdit.TextSize = new System.Drawing.Size(48, 13);
            // 
            // textEditUrl
            // 
            this.textEditUrl.Location = new System.Drawing.Point(64, 60);
            this.textEditUrl.Name = "textEditUrl";
            this.textEditUrl.Properties.Mask.BeepOnError = true;
            this.textEditUrl.Properties.Mask.EditMask = "(http|https)://\\d{1,3}[.]\\d{1,3}[.]\\d{1,3}[.]\\d{1,3}[:]\\d{4}2";
            this.textEditUrl.Properties.Mask.IgnoreMaskBlank = false;
            this.textEditUrl.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.textEditUrl.Properties.Mask.PlaceHolder = '+';
            this.textEditUrl.Properties.Mask.ShowPlaceHolders = false;
            this.textEditUrl.Size = new System.Drawing.Size(212, 20);
            this.textEditUrl.StyleController = this.dataLayoutControl1;
            this.textEditUrl.TabIndex = 6;
            // 
            // layoutControlItemUrlTextEdit
            // 
            this.layoutControlItemUrlTextEdit.Control = this.textEditUrl;
            this.layoutControlItemUrlTextEdit.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItemUrlTextEdit.Name = "layoutControlItemUrlTextEdit";
            this.layoutControlItemUrlTextEdit.Size = new System.Drawing.Size(268, 24);
            this.layoutControlItemUrlTextEdit.Text = "Server Url";
            this.layoutControlItemUrlTextEdit.TextSize = new System.Drawing.Size(48, 13);
            // 
            // checkBoxSave
            // 
            this.checkBoxSave.Location = new System.Drawing.Point(66, 84);
            this.checkBoxSave.Name = "checkBoxSave";
            this.checkBoxSave.Size = new System.Drawing.Size(210, 20);
            this.checkBoxSave.TabIndex = 7;
            this.checkBoxSave.Text = "Save Credential";
            this.checkBoxSave.UseVisualStyleBackColor = true;
            // 
            // layoutControlItemSaveCheckBox
            // 
            this.layoutControlItemSaveCheckBox.Control = this.checkBoxSave;
            this.layoutControlItemSaveCheckBox.Location = new System.Drawing.Point(54, 72);
            this.layoutControlItemSaveCheckBox.Name = "layoutControlItemSaveCheckBox";
            this.layoutControlItemSaveCheckBox.Size = new System.Drawing.Size(214, 24);
            this.layoutControlItemSaveCheckBox.Text = "Save Credential";
            this.layoutControlItemSaveCheckBox.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemSaveCheckBox.TextVisible = false;
            // 
            // simpleButtonOk
            // 
            this.simpleButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.simpleButtonOk.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonOk.ImageUri.Uri = "Apply;Office2013";
            this.simpleButtonOk.Location = new System.Drawing.Point(66, 108);
            this.simpleButtonOk.Name = "simpleButtonOk";
            this.simpleButtonOk.Size = new System.Drawing.Size(66, 38);
            this.simpleButtonOk.StyleController = this.dataLayoutControl1;
            this.simpleButtonOk.TabIndex = 8;
            // 
            // layoutControlItemOkButton
            // 
            this.layoutControlItemOkButton.Control = this.simpleButtonOk;
            this.layoutControlItemOkButton.Location = new System.Drawing.Point(54, 96);
            this.layoutControlItemOkButton.Name = "layoutControlItemOkButton";
            this.layoutControlItemOkButton.Size = new System.Drawing.Size(70, 49);
            this.layoutControlItemOkButton.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemOkButton.TextVisible = false;
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButtonCancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonCancel.ImageUri.Uri = "Cancel;Office2013";
            this.simpleButtonCancel.Location = new System.Drawing.Point(136, 108);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(71, 38);
            this.simpleButtonCancel.StyleController = this.dataLayoutControl1;
            this.simpleButtonCancel.TabIndex = 9;
            // 
            // layoutControlItemCancelButton
            // 
            this.layoutControlItemCancelButton.Control = this.simpleButtonCancel;
            this.layoutControlItemCancelButton.Location = new System.Drawing.Point(124, 96);
            this.layoutControlItemCancelButton.Name = "layoutControlItemCancelButton";
            this.layoutControlItemCancelButton.Size = new System.Drawing.Size(75, 49);
            this.layoutControlItemCancelButton.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemCancelButton.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(199, 96);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(69, 49);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(0, 72);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(54, 24);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem4
            // 
            this.emptySpaceItem4.AllowHotTrack = false;
            this.emptySpaceItem4.Location = new System.Drawing.Point(0, 96);
            this.emptySpaceItem4.Name = "emptySpaceItem4";
            this.emptySpaceItem4.Size = new System.Drawing.Size(54, 49);
            this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 165);
            this.Controls.Add(this.dataLayoutControl1);
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditLogin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLoginTextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemPasswordTextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditUrl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUrlTextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSaveCheckBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemOkButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCancelButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonCancel;
        private DevExpress.XtraEditors.SimpleButton simpleButtonOk;
        private System.Windows.Forms.CheckBox checkBoxSave;
        private DevExpress.XtraEditors.TextEdit textEditUrl;
        private DevExpress.XtraEditors.TextEdit textEditPassword;
        private DevExpress.XtraEditors.TextEdit textEditLogin;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemLoginTextEdit;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemPasswordTextEdit;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemUrlTextEdit;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSaveCheckBox;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemOkButton;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCancelButton;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
    }
}