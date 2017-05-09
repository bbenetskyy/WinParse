namespace WinParse.UI.HomeForm
{
    partial class DefaultHomeForm
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
            this.components = new System.ComponentModel.Container();
            this.gridControlDefault = new DevExpress.XtraGrid.GridControl();
            this.gridViewDefault = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.skinRibbonGalleryBarItem1 = new DevExpress.XtraBars.SkinRibbonGalleryBarItem();
            this.barEditItemLanguage = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemLookUpEditLanguage = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.ribbonPageSettings = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rpgSkins = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpgLocalization = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDefault)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDefault)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditLanguage)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlDefault
            // 
            this.gridControlDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlDefault.Location = new System.Drawing.Point(0, 147);
            this.gridControlDefault.MainView = this.gridViewDefault;
            this.gridControlDefault.Name = "gridControlDefault";
            this.gridControlDefault.Size = new System.Drawing.Size(913, 348);
            this.gridControlDefault.TabIndex = 0;
            this.gridControlDefault.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDefault});
            // 
            // gridViewDefault
            // 
            this.gridViewDefault.GridControl = this.gridControlDefault;
            this.gridViewDefault.Name = "gridViewDefault";
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2016 Dark";
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.skinRibbonGalleryBarItem1,
            this.barEditItemLanguage});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 3;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPageSettings});
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEditLanguage});
            this.ribbonControl1.Size = new System.Drawing.Size(913, 147);
            // 
            // skinRibbonGalleryBarItem1
            // 
            this.skinRibbonGalleryBarItem1.Caption = "skinRibbonGalleryBarItem1";
            this.skinRibbonGalleryBarItem1.Id = 1;
            this.skinRibbonGalleryBarItem1.Name = "skinRibbonGalleryBarItem1";
            // 
            // barEditItemLanguage
            // 
            this.barEditItemLanguage.AutoFillWidthInMenu = DevExpress.Utils.DefaultBoolean.True;
            this.barEditItemLanguage.Caption = "Language";
            this.barEditItemLanguage.Edit = this.repositoryItemLookUpEditLanguage;
            this.barEditItemLanguage.EditWidth = 100;
            this.barEditItemLanguage.Id = 2;
            this.barEditItemLanguage.Name = "barEditItemLanguage";
            this.barEditItemLanguage.EditValueChanged += new System.EventHandler(this.barEditItemLanguage_EditValueChanged);
            // 
            // repositoryItemLookUpEditLanguage
            // 
            this.repositoryItemLookUpEditLanguage.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.repositoryItemLookUpEditLanguage.AutoHeight = false;
            this.repositoryItemLookUpEditLanguage.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEditLanguage.DisplayMember = "Description";
            this.repositoryItemLookUpEditLanguage.Name = "repositoryItemLookUpEditLanguage";
            this.repositoryItemLookUpEditLanguage.ValueMember = "Code";
            // 
            // ribbonPageSettings
            // 
            this.ribbonPageSettings.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rpgSkins,
            this.rpgLocalization});
            this.ribbonPageSettings.MergeOrder = 2;
            this.ribbonPageSettings.Name = "ribbonPageSettings";
            this.ribbonPageSettings.Text = "ribbonPage1";
            // 
            // rpgSkins
            // 
            this.rpgSkins.ItemLinks.Add(this.skinRibbonGalleryBarItem1);
            this.rpgSkins.Name = "rpgSkins";
            this.rpgSkins.Text = "Skins";
            // 
            // rpgLocalization
            // 
            this.rpgLocalization.ItemLinks.Add(this.barEditItemLanguage);
            this.rpgLocalization.Name = "rpgLocalization";
            this.rpgLocalization.Text = "ribbonPageGroup1";
            // 
            // DefaultHomeForm
            // 
            this.AllowFormGlass = DevExpress.Utils.DefaultBoolean.False;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 495);
            this.Controls.Add(this.gridControlDefault);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "DefaultHomeForm";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.DefaultHomeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDefault)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDefault)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditLanguage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageSettings;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgSkins;
        private DevExpress.XtraBars.SkinRibbonGalleryBarItem skinRibbonGalleryBarItem1;
        private DevExpress.XtraBars.BarEditItem barEditItemLanguage;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEditLanguage;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgLocalization;
        public DevExpress.XtraGrid.GridControl gridControlDefault;
        public DevExpress.XtraGrid.Views.Grid.GridView gridViewDefault;
        public DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
    }
}
