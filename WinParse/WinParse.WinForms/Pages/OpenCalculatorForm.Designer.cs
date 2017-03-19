namespace DXApplication1.Pages
{
    partial class OpenCalculatorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenCalculatorForm));
            this.buttonFind = new DevExpress.XtraEditors.SimpleButton();
            this.buttonClear = new DevExpress.XtraEditors.SimpleButton();
            this.textEditEvent = new DevExpress.XtraEditors.TextEdit();
            this.textEditTypeSecond = new DevExpress.XtraEditors.TextEdit();
            this.textEditTypeFirst = new DevExpress.XtraEditors.TextEdit();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colEvent = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colType1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCoef1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colType2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCoef2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.buttonOpen = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textEditEvent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditTypeSecond.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditTypeFirst.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonFind
            // 
            this.buttonFind.Location = new System.Drawing.Point(480, 20);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(75, 23);
            this.buttonFind.TabIndex = 0;
            this.buttonFind.Text = "Найти";
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(480, 49);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 1;
            this.buttonClear.Text = "Очистить";
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // textEditEvent
            // 
            this.textEditEvent.Location = new System.Drawing.Point(124, 22);
            this.textEditEvent.Name = "textEditEvent";
            this.textEditEvent.Size = new System.Drawing.Size(331, 20);
            this.textEditEvent.TabIndex = 2;
            // 
            // textEditTypeSecond
            // 
            this.textEditTypeSecond.Location = new System.Drawing.Point(124, 72);
            this.textEditTypeSecond.Name = "textEditTypeSecond";
            this.textEditTypeSecond.Size = new System.Drawing.Size(331, 20);
            this.textEditTypeSecond.TabIndex = 3;
            // 
            // textEditTypeFirst
            // 
            this.textEditTypeFirst.Location = new System.Drawing.Point(124, 46);
            this.textEditTypeFirst.Name = "textEditTypeFirst";
            this.textEditTypeFirst.Size = new System.Drawing.Size(331, 20);
            this.textEditTypeFirst.TabIndex = 4;
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(12, 118);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(567, 169);
            this.gridControl1.TabIndex = 5;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colEvent,
            this.colType1,
            this.colCoef1,
            this.colType2,
            this.colCoef2});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // colEvent
            // 
            this.colEvent.Caption = "Событие";
            this.colEvent.FieldName = "Event";
            this.colEvent.Name = "colEvent";
            this.colEvent.OptionsColumn.ReadOnly = true;
            this.colEvent.Visible = true;
            this.colEvent.VisibleIndex = 0;
            // 
            // colType1
            // 
            this.colType1.Caption = "Тип 1";
            this.colType1.FieldName = "TypeFirst";
            this.colType1.Name = "colType1";
            this.colType1.OptionsColumn.ReadOnly = true;
            this.colType1.Visible = true;
            this.colType1.VisibleIndex = 1;
            // 
            // colCoef1
            // 
            this.colCoef1.Caption = "Коэффициент 1";
            this.colCoef1.FieldName = "CoefFirst";
            this.colCoef1.Name = "colCoef1";
            this.colCoef1.OptionsColumn.ReadOnly = true;
            this.colCoef1.Visible = true;
            this.colCoef1.VisibleIndex = 2;
            // 
            // colType2
            // 
            this.colType2.Caption = "Тип 2";
            this.colType2.FieldName = "TypeSecond";
            this.colType2.Name = "colType2";
            this.colType2.OptionsColumn.ReadOnly = true;
            this.colType2.Visible = true;
            this.colType2.VisibleIndex = 3;
            // 
            // colCoef2
            // 
            this.colCoef2.Caption = "Коэффициент 2";
            this.colCoef2.FieldName = "CoefSecond";
            this.colCoef2.Name = "colCoef2";
            this.colCoef2.OptionsColumn.ReadOnly = true;
            this.colCoef2.Visible = true;
            this.colCoef2.VisibleIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Событие";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(83, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Тип 1";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(83, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Тип 2";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(499, 293);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Отменить";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(418, 293);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonOpen.TabIndex = 11;
            this.buttonOpen.Text = "Открыть";
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // OpenCalculatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 328);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.textEditTypeFirst);
            this.Controls.Add(this.textEditTypeSecond);
            this.Controls.Add(this.textEditEvent);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonFind);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenCalculatorForm";
            this.Text = "OpenCalculatorForm";
            ((System.ComponentModel.ISupportInitialize)(this.textEditEvent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditTypeSecond.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditTypeFirst.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton buttonFind;
        private DevExpress.XtraEditors.SimpleButton buttonClear;
        private DevExpress.XtraEditors.TextEdit textEditEvent;
        private DevExpress.XtraEditors.TextEdit textEditTypeSecond;
        private DevExpress.XtraEditors.TextEdit textEditTypeFirst;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.SimpleButton buttonCancel;
        private DevExpress.XtraEditors.SimpleButton buttonOpen;
        private DevExpress.XtraGrid.Columns.GridColumn colEvent;
        private DevExpress.XtraGrid.Columns.GridColumn colCoef1;
        private DevExpress.XtraGrid.Columns.GridColumn colType1;
        private DevExpress.XtraGrid.Columns.GridColumn colType2;
        private DevExpress.XtraGrid.Columns.GridColumn colCoef2;
    }
}