using System;

namespace WinParse.BusinessLogic.Models
{
    public class Filter
    {
        public string Id { get; set; }

        public decimal? MinPercent { get; set; }

        public decimal? MaxPercent { get; set; }

        public bool Football { get; set; }

        public bool Basketball { get; set; }

        public bool Volleyball { get; set; }

        public bool Hockey { get; set; }

        public bool Tennis { get; set; }

        public string LicenseKey { get; set; }

        public int? AutoUpdateTime { get; set; }

        public int? RecommendedRate1 { get; set; }

        public int? RecommendedRate2 { get; set; }

        public decimal? MinRate { get; set; }

        public decimal? MaxRate { get; set; }

        public bool AutoDelete { get; set; }

        private int defaultAutoDeleteTime = 1200;

        private int? _autoDeleteTime;

        public int? AutoDeleteTime
        {
            get { return _autoDeleteTime ?? (_autoDeleteTime = defaultAutoDeleteTime); }
            set { _autoDeleteTime = value; }
        }

        public DateTime? BeforeTime { get; set; }

        public DateTime? AfterTime { get; set; }

        public bool PinnaclePlace { get; set; }
    }
}