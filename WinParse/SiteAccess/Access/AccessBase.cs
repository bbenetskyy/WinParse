using Common.Modules.AntiCaptha;
using System;
using System.Timers;

namespace SiteAccess.Access
{
    public abstract class AccessBase
    {
        private IAntiCaptcha _ac;
        private Timer _updateTimer;

        public AccessBase(IAntiCaptcha ac)
        {
            _ac = ac;

            SetHeaders();
        }

        protected abstract void SetHeaders();

        protected abstract void Connect();

        protected string SolveCaptcha(byte[] data)
        {
            return _ac.GetAnswer(data);
        }

        protected void SetupAutoConnect(TimeSpan interval)
        {
            _updateTimer?.Dispose();
            _updateTimer = new Timer(interval.TotalMilliseconds);
            _updateTimer.Elapsed += delegate { Connect(); };
            _updateTimer.Start();
        }
    }
}