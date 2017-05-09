namespace WinParse.UserManager
{
    partial class UserManagerHomeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserManagerHomeForm));
            this.toolsPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.saveGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.saveButton = new DevExpress.XtraBars.BarButtonItem();
            this.cancelButton = new DevExpress.XtraBars.BarButtonItem();
            this.refreshButton = new DevExpress.XtraBars.BarButtonItem();
            this.addGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.addButton = new DevExpress.XtraBars.BarButtonItem();
            this.removeButton = new DevExpress.XtraBars.BarButtonItem();
            this.copyButton = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.saveButton,
            this.cancelButton,
            this.refreshButton,
            this.addButton,
            this.removeButton,
            this.copyButton});
            this.ribbonControl1.MaxItemId = 1;
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.toolsPage});
            // 
            // toolsPage
            // 
            this.toolsPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.saveGroup,
            this.addGroup});
            this.toolsPage.MergeOrder = 0;
            this.toolsPage.Name = "toolsPage";
            this.toolsPage.Text = "ribbonPage1";
            // 
            // saveGroup
            // 
            this.saveGroup.ItemLinks.Add(this.saveButton);
            this.saveGroup.ItemLinks.Add(this.cancelButton);
            this.saveGroup.ItemLinks.Add(this.refreshButton);
            this.saveGroup.Name = "saveGroup";
            this.saveGroup.Text = "ribbonPageGroup1";
            // 
            // saveButton
            // 
            this.saveButton.Caption = "Save";
            this.saveButton.Id = 1;
            this.saveButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.ImageOptions.Image")));
            this.saveButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("saveButton.ImageOptions.LargeImage")));
            this.saveButton.Name = "saveButton";
            // 
            // cancelButton
            // 
            this.cancelButton.Caption = "Cancel";
            this.cancelButton.Id = 2;
            this.cancelButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("cancelButton.ImageOptions.Image")));
            this.cancelButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("cancelButton.ImageOptions.LargeImage")));
            this.cancelButton.Name = "cancelButton";
            // 
            // refreshButton
            // 
            this.refreshButton.Caption = "Refresh";
            this.refreshButton.Id = 3;
            this.refreshButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("refreshButton.ImageOptions.Image")));
            this.refreshButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("refreshButton.ImageOptions.LargeImage")));
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.refreshButton_ItemClick);
            // 
            // addGroup
            // 
            this.addGroup.ItemLinks.Add(this.addButton);
            this.addGroup.ItemLinks.Add(this.removeButton);
            this.addGroup.ItemLinks.Add(this.copyButton);
            this.addGroup.Name = "addGroup";
            this.addGroup.Text = "ribbonPageGroup1";
            // 
            // addButton
            // 
            this.addButton.Caption = "Add";
            this.addButton.Id = 4;
            this.addButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("addButton.ImageOptions.Image")));
            this.addButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("addButton.ImageOptions.LargeImage")));
            this.addButton.Name = "addButton";
            // 
            // removeButton
            // 
            this.removeButton.Caption = "Remove";
            this.removeButton.Id = 5;
            this.removeButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("removeButton.ImageOptions.Image")));
            this.removeButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("removeButton.ImageOptions.LargeImage")));
            this.removeButton.Name = "removeButton";
            // 
            // copyButton
            // 
            this.copyButton.Caption = "Copy";
            this.copyButton.Id = 6;
            this.copyButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("copyButton.ImageOptions.Image")));
            this.copyButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("copyButton.ImageOptions.LargeImage")));
            this.copyButton.Name = "copyButton";
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Name = "ribbonPage2";
            this.ribbonPage2.Text = "ribbonPage2";
            // 
            // UserManagerHomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 495);
            this.Name = "UserManagerHomeForm";
            this.Text = "UserManagerHomeForm";
            this.Load += new System.EventHandler(this.UserManagerHomeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem saveButton;
        private DevExpress.XtraBars.Ribbon.RibbonPage toolsPage;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup saveGroup;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraBars.BarButtonItem cancelButton;
        private DevExpress.XtraBars.BarButtonItem refreshButton;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup addGroup;
        private DevExpress.XtraBars.BarButtonItem addButton;
        private DevExpress.XtraBars.BarButtonItem removeButton;
        private DevExpress.XtraBars.BarButtonItem copyButton;
    }
}