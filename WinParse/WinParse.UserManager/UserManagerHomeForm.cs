using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraPrinting.Native;
using WinParse.Resources;
using WinParse.UI.HomeForm;

namespace WinParse.UserManager
{
    public partial class UserManagerHomeForm : DefaultHomeForm
    {
        public UserManagerHomeForm()
        {
            InitializeComponent();
        }

        private void UserManagerHomeForm_Load(object sender, EventArgs e)
        {
            //we need to place toolPage like first Page
            var pages = ribbonControl1.Pages.ToList();
            ribbonControl1.Pages.Clear();
            ribbonControl1.Pages.Add(toolsPage);
            pages.Where(p=>p != toolsPage).ForEach(p=>ribbonControl1.Pages.Add(p));
            ResMan.ResourceChaned += ResMan_ResourceChaned;
        }

        private void ResMan_ResourceChaned(object sender, EventArgs e)
        {
            Text = ResMan.GetString(ResKeys.UserManagerHomeForm_Text);
            addButton.Caption =     ResMan.GetString(ResKeys.UserManagerHomeForm_AddButton_Caption);
            removeButton.Caption =  ResMan.GetString(ResKeys.UserManagerHomeForm_RemoveButton_Caption);
            copyButton.Caption =    ResMan.GetString(ResKeys.UserManagerHomeForm_CopyButton_Caption);
            saveButton.Caption =    ResMan.GetString(ResKeys.UserManagerHomeForm_SaveButton_Caption);
            cancelButton.Caption =  ResMan.GetString(ResKeys.UserManagerHomeForm_CancelButton_Caption);
            refreshButton.Caption = ResMan.GetString(ResKeys.UserManagerHomeForm_RefreshButton_Caption);
            addGroup.Text = ResMan.GetString(ResKeys.UserManagerHomeForm_AddGroup_Text);
            saveGroup.Text = ResMan.GetString(ResKeys.UserManagerHomeForm_SaveGroup_Text);
            toolsPage.Text = ResMan.GetString(ResKeys.UserManagerHomeForm_ToolsPage_Text);
        }

        private void refreshButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}
