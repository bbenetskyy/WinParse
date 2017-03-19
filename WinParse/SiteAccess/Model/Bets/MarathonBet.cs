using Common.Core.Helpers;
using System;
using System.Web;

namespace SiteAccess.Model.Bets
{
    public class MarathonBet : ICloneable
    {
        /// <summary>
        /// Это ИД события, его можно получить например через такой XPath :
        /// "//div[@class='category-container']//tbody[@data-event-name]//span[@data-selection-key]" Если означает победу
        /// 1-ого, то в конце 1, ничью - draw, победу 2 - 3
        /// Пример: 4050540@Match_Result.1, 4050540@Match_Result.draw, 4050540@Match_Result.3
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { _id = value.Replace("@", ","); } // нужен перевод для ставки
        }

        private string _id;

        public double Stake { get; set; }

        /// <summary>
        /// Имя события, через XPath получать так //div[@class='category-container']//tbody[@data-event-name]"
        /// Пример: Bolton Wanderers vs Blackpool
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дополнительные данные о ставке. Получаем с внутренней части того элемента Name, таких элемента 3: 1 - победа
        /// 1 2 - ничья, 3 - победа - 2, XPath //div[@class='category-container']//tbody[@data-event-name]//td[@dat
        /// a-sel] - список из 3
        /// Пример: {\"sn\":\"Bolton Wanderers To Win\",\"mn\":\"Match Result\",\"ewc\":\"1/1
        ///         1\",\"cid\":10345303346,\"prt\":\"CP\",\"ewf\":\"1.0\",\"epr\":\"1.7272727272727273\",\"prices\":{\"0\":\"8/11\",\"1\":\"1.727\",\"2\":\"-138\",\"3\":\"0.727\",\"4\":\"0.727\",\"5\":\"-1.38\"},\"mid\":\"4050540\",\"u\":\"4050540,Match_Result.1\",\"en\":\"Bolton
        ///         Wanderers vs Blackpool\",\"l\":false}
        /// </summary>
        public string AddData { get; set; }

        public MarathonBet(string id, string addData, string name)
        {
            Id = id;
            AddData = addData;
            Name = name;
        }

        public MarathonBet()
        {
        }

        public string GetBetData()
        {
            return HttpUtility.UrlEncode("[{\"url\":\"" + Id + "\",\"stake\":" + Stake + ",\"vip\":false,\"ew\":false}]");
        }

        public string GetAddData()
        {
            return
                HttpUtility.UrlEncode(AddData.TrimEnd('}') +
                "}" + $",\"mid\":\"{Id.StopWord(",")}\",\"u\":\"{Id}\",\"en\":\"{Name}\"" +
                "}");
        }

        public object Clone()
        {
            return new MarathonBet()
            {
                Id = Id,
                AddData = AddData,
                Stake = Stake
            };
        }
    }
}