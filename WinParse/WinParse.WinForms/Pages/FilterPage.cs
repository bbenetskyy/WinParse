//#define DisableForkDeletion
using DataSaver;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ToolsPortable;
using WinParse.BusinessLogic.Models;
using WinParse.DataSaver.Models;

namespace DXApplication1.Pages
{
    public partial class FilterPage : Form
    {
        public Filter Filter { get; }

        public LocalSaver LocalSaver { get; }

        public bool ToClose { get; set; }

        private string _userId;
        private Timer _autoDeleteTimer;

        public EventHandler FilterUpdated;
        public EventHandler ReloadingData;

        public FilterPage(Filter filter)
        {
            InitializeComponent();
            Closing += FilterPage_Closing;
            LocalSaver = new LocalSaver();
            FilterUpdated += (sender, args) => { LocalSaver.UpdateFilter(Filter); };
            ReloadingData += (sender, args) => { UpdateFilter(LocalSaver.FindFilter()); FirstBind(); UserBind(); };
            Filter = filter;
            FirstBind();
            UserBind();
            _autoDeleteTimer = new Timer();
            _autoDeleteTimer.Tick += simpleButtonDeleteForks_Click;
#if DisableForkDeletion
            layoutControlGroupAutoDelete.Visibility = LayoutVisibility.Never;
            _autoDeleteTimer.Interval = 1;
            _autoDeleteTimer.Enabled = false;

#else
            // ReSharper disable once PossibleInvalidOperationException
            _autoDeleteTimer.Interval = Filter.AutoDeleteTime.Value * 1000;
            _autoDeleteTimer.Enabled = Filter.AutoDelete;
#endif

        }

        private void UpdateFilter(Filter dbFilter)
        {
            Filter.MinPercent = dbFilter.MinPercent;
            Filter.MaxPercent = dbFilter.MaxPercent;
            Filter.MaxRate = dbFilter.MaxRate;
            Filter.MinRate = dbFilter.MinRate;
            Filter.AutoUpdateTime = dbFilter.AutoUpdateTime;
            Filter.Basketball = dbFilter.Basketball;
            Filter.Football = dbFilter.Football;
            Filter.Hockey = dbFilter.Hockey;
            Filter.LicenseKey = dbFilter.LicenseKey;
            Filter.RecommendedRate1 = dbFilter.RecommendedRate1;
            Filter.RecommendedRate2 = dbFilter.RecommendedRate2;
            Filter.Tennis = dbFilter.Tennis;
            Filter.Volleyball = dbFilter.Volleyball;
#if DisableForkDeletion
            Filter.AutoDelete = false;
            Filter.AutoDeleteTime = 0;
#else
            Filter.AutoDelete = dbFilter.AutoDelete;
            Filter.AutoDeleteTime = dbFilter.AutoDeleteTime;
#endif
            Filter.PinnaclePlace = dbFilter.PinnaclePlace;
        }

        private void UserBind()
        {
            var user = LocalSaver.FindUser();
            _userId = user?.Id;
            textEditLoginPinnacle.Text = user?.LoginPinnacle;
            textEditPasswordPinnacle.Text = user?.PasswordPinnacle;
            textEditLoginMarathon.Text = user?.LoginMarathon;
            textEditPasswordMarathon.Text = user?.PasswordMarathon;
            textEditAntiGateCode.Text = user?.AntiGateCode;
        }

        protected void FirstBind()
        {
            lock (Filter)
            {
                textEditMinPercent.EditValue = Filter.MinPercent;
                textEditMaxPercent.EditValue = Filter.MaxPercent;
                footballToggleSwitch.EditValue = Filter.Football;
                basketballToggleSwitch.EditValue = Filter.Basketball;
                volleyballToggleSwitch.EditValue = Filter.Volleyball;
                hockeyToggleSwitch.EditValue = Filter.Hockey;
                tennisToggleSwitch.EditValue = Filter.Tennis;
                textEditAutoUpdate.EditValue = Filter.AutoUpdateTime;
                textEditMinRate.EditValue = Filter.MinRate;
                textEditMaxRate.EditValue = Filter.MaxRate;
                toggleSwitchAutoDelete.EditValue = Filter.AutoDelete;
                textEditAutoDeleteTime.EditValue = Filter.AutoDeleteTime;
                dateTimePickerAfter.DateTime = Filter.AfterTime ?? DateTime.Now.Date;
                dateTimePickerBefore.DateTime = Filter.BeforeTime ?? DateTime.Now.Date;
                pinnaclePlaceToggleSwitch.EditValue = Filter.PinnaclePlace;
            }
        }


        private void SaveUser()
        {
            var user = new User
            {
                Id = _userId,
                LoginPinnacle = textEditLoginPinnacle.Text,
                PasswordPinnacle = textEditPasswordPinnacle.Text,
                LoginMarathon = textEditLoginMarathon.Text,
                PasswordMarathon = textEditPasswordMarathon.Text,
                AntiGateCode = textEditAntiGateCode.Text
            };
            if (_userId.IsBlank())
            {
                LocalSaver.AddUserToDb(user);
                _userId = user.Id;
            }
            else
                LocalSaver.UpdateUser(user);
        }

        private void FilterPage_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = !ToClose;
            if (!ToClose)
                Hide();
        }

        private void BindControlsIntoFilter()
        {
            lock (Filter)
            {
                Filter.MinPercent = textEditMinPercent.EditValue.ConvertToDecimalOrNull();
                Filter.MaxPercent = textEditMaxPercent.EditValue.ConvertToDecimalOrNull();
                Filter.Football = footballToggleSwitch.EditValue.ConvertToBool();
                Filter.Basketball = basketballToggleSwitch.EditValue.ConvertToBool();
                Filter.Volleyball = volleyballToggleSwitch.EditValue.ConvertToBool();
                Filter.Hockey = hockeyToggleSwitch.EditValue.ConvertToBool();
                Filter.Tennis = tennisToggleSwitch.EditValue.ConvertToBool();
                Filter.AutoUpdateTime = textEditAutoUpdate.EditValue.ConvertToIntOrNull();
                Filter.MinRate = textEditMinRate.EditValue.ConvertToDecimalOrNull();
                Filter.MaxRate = textEditMaxRate.EditValue.ConvertToDecimalOrNull();
                Filter.AutoDelete = toggleSwitchAutoDelete.EditValue.ConvertToBool();
                Filter.AutoDeleteTime = textEditAutoDeleteTime.EditValue.ConvertToIntOrNull();
                Filter.AfterTime = dateTimePickerAfter.DateTime;
                Filter.BeforeTime = dateTimePickerBefore.DateTime;
                Filter.PinnaclePlace = pinnaclePlaceToggleSwitch.EditValue.ConvertToBool();
            }
        }

        private bool ValidateAllControls()
        {
            var bRes = ValidateMinRate();
            bRes &= ValidateMaxRate();
            bRes &= ValidateMinPercent();
            bRes &= ValidateMaxPercent();
            //bRes &= ValidateAfterTime();
            //bRes &= ValidateBeforeTime();
            return bRes && ValidateAllNumbers();
        }

        private bool ValidateBeforeTime()
        {
            var bRes = true;
            if (dateTimePickerBefore.DateTime < dateTimePickerAfter.DateTime)
            {
                bRes = false;
                dateTimePickerBefore.ErrorIcon = DXErrorProvider.GetErrorIconInternal(ErrorType.Critical);
            }
            else
                dateTimePickerBefore.ErrorIcon = null;
            return bRes;
        }

        private bool ValidateAfterTime()
        {
            var bRes = true;
            if (dateTimePickerAfter.DateTime > dateTimePickerBefore.DateTime)
            {
                bRes = false;
                dateTimePickerAfter.ErrorIcon = DXErrorProvider.GetErrorIconInternal(ErrorType.Critical);
            }
            else
                dateTimePickerAfter.ErrorIcon = null;
            return bRes;
        }

        private bool ValidateAllNumbers()
        {
            var bRes = SetErrorIfNegative(Filter.MinPercent, textEditMinPercent);
            bRes &= SetErrorIfNegative(Filter.MaxPercent, textEditMaxPercent);
            bRes &= SetErrorIfNegative(Filter.MinRate, textEditMinRate);
            bRes &= SetErrorIfNegative(Filter.MaxRate, textEditMaxRate);
            bRes &= SetErrorIfNegative(Filter.AutoUpdateTime, textEditAutoUpdate);

            return bRes;
        }

        private bool SetErrorIfNegative(decimal? minPercent, TextEdit textEdit)
        {
            var bRes = true;

            if (minPercent < 0)
            {
                bRes = false;
                textEdit.ErrorIcon = DXErrorProvider.GetErrorIconInternal(ErrorType.Critical);
                textEdit.ErrorText = "Значение должно быть положительным";
            }
            else
            {
                textEdit.ErrorIcon = null;
                textEdit.ErrorText = null;
            }

            return bRes;
        }

        private bool ValidateMaxPercent()
        {
            if (Filter.MinPercent == null) return true;
            if (Filter.MaxPercent == null) return true;
            var bRes = Filter.MinPercent.Value <= Filter.MaxPercent.Value;

            if (!bRes)
            {
                textEditMaxPercent.ErrorIcon = DXErrorProvider.GetErrorIconInternal(ErrorType.Critical);
                textEditMaxPercent.ErrorText = "Максимальный процент меньше минимального!";
            }
            else
            {
                textEditMaxPercent.ErrorIcon = null;
                textEditMaxPercent.ErrorText = null;
            }

            return bRes;
        }

        private bool ValidateMinPercent()
        {
            if (Filter.MinPercent == null) return true;
            if (Filter.MaxPercent == null) return true;
            var bRes = Filter.MinPercent.Value <= Filter.MaxPercent.Value;

            if (!bRes)
            {
                textEditMinPercent.ErrorIcon = DXErrorProvider.GetErrorIconInternal(ErrorType.Critical);
                textEditMinPercent.ErrorText = "Минимальный процент больше максимального!";
            }
            else
            {
                textEditMinPercent.ErrorIcon = null;
                textEditMinPercent.ErrorText = null;
            }

            return bRes;
        }

        private bool ValidateMaxRate()
        {
            if (Filter.MinRate == null) return true;
            if (Filter.MaxRate == null) return true;
            var bRes = Filter.MinRate.Value <= Filter.MaxRate.Value;

            if (!bRes)
            {
                textEditMaxRate.ErrorIcon = DXErrorProvider.GetErrorIconInternal(ErrorType.Critical);
                textEditMaxRate.ErrorText = "Максимальная ставка меньше минимальной";
            }
            else
            {
                textEditMaxRate.ErrorIcon = null;
                textEditMaxRate.ErrorText = null;
            }

            return bRes;
        }

        private bool ValidateMinRate()
        {
            if (Filter.MinRate == null) return true;
            if (Filter.MaxRate == null) return true;
            var bRes = Filter.MinRate.Value <= Filter.MaxRate.Value;

            if (!bRes)
            {
                textEditMinRate.ErrorIcon = DXErrorProvider.GetErrorIconInternal(ErrorType.Critical);
                textEditMinRate.ErrorText = "Минимальная ставка больше максимальной";
            }
            else
            {
                textEditMinRate.ErrorIcon = null;
                textEditMinRate.ErrorText = null;
            }

            return bRes;
        }

        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            BindControlsIntoFilter();
            if (!ValidateAllControls()) return;
            SaveUser();
            FilterUpdated?.Invoke(sender, e);
            // ReSharper disable once PossibleInvalidOperationException
            _autoDeleteTimer.Interval = Filter.AutoDeleteTime.Value * 1000;
            _autoDeleteTimer.Enabled = Filter.AutoDelete;
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            ReloadingData?.Invoke(sender, e);
        }

        private void simpleButtonDeleteForks_Click(object sender, EventArgs e)
        {
            LocalSaver.ClearForks(LocalSaver.GetAllForkRows().ToList());
        }
    }
}