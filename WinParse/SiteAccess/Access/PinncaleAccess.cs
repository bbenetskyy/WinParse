using Newtonsoft.Json.Linq;
using NLog;
using SiteAccess.Helpers;
using SiteAccess.Model.Bets;

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using SiteAccess.Model;

namespace SiteAccess.Access
{
    public class PinncaleAccess : AccessBase, ISiteAccess<PinnacleBet, PinncaleAccess.Result>
    {
        private HttpWebRequest _request;
        private string _username, _password, _domain, _apiDomain;
        private WebClient _testCl;
        private static Logger _logger;

        public PinncaleAccess() : base(null)
        {
            _domain = "https://www.pinnacle.com/";
            _apiDomain = "https://api.pinnaclesports.com/v1/bets/place";
            _testCl = new WebClient();
            _logger = LogManager.GetCurrentClassLogger();
        }

        public Result MakeBet(PinnacleBet bet)
        {
            string postJson =
            "{\"uniqueRequestId\":\"" + bet.Guid + "\"," +
            "\"acceptBetterLine\":\"" + bet.AcceptBetterLine.GetString() + "\"," +
            "\"stake\":" + bet.Stake.ToString().Replace(",", ".") + "," +
            "\"winRiskStake\":\"" + bet.WinRiskRate.ToString() + "\"," +
            "\"lineId\":" + bet.LineId + "," +
            "\"sportId\":" + bet.SportId + "," +
            "\"eventId\":" + bet.Eventid + "," +
            "\"periodNumber\":" + bet.PeriodNumber + "," +
            "\"betType\":\"" + bet.BetType.ToString() + "\"," +
            "\"oddsFormat\":\"" + bet.OddsFormat.ToString() + "\"";

            if (bet.BetType == BetType.TotalPoints || bet.BetType == BetType.TeamTotalPoints)
            {
                postJson += ",\"side\":\"" + bet.Side.ToString() + "\"";
            }
            else
            {
                postJson += ",\"team\":\"" + bet.TeamType.ToString() + "\"";
            }

            if (bet.SportId == 3 /*if is baseball*/)
            {
                if (bet.Pitcher1MustStart == null || bet.Pitcher2MustStart == null)
                    throw new ArgumentException("Bet must contains data about pitchers: Pitcher1MustStart or Pitcher2MustStart is null");

                postJson += ",\"pitcher1MustStart\":\"" + bet.Pitcher1MustStart.Value.GetString() + "\"," +
                    "\"Pitcher2MustStart\":\"" + bet.Pitcher2MustStart.Value.GetString() + "\"";
            }

            if (bet.CustomerReference != null)
            {
                postJson += ",\"customerReference\":\"" + bet.CustomerReference + "\"";
            }

            if (bet.AlternativeLineId != null)
            {
                postJson += ",\"altLineId\":\"" + bet.AlternativeLineId.Value.ToString() + "\"";
            }

            postJson += "}";

            byte[] byteArray = Encoding.UTF8.GetBytes(postJson);
            Stream dataStream;
            try
            {
                dataStream = _request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }
            catch
            {
                _logger.Error("Writing request stream was failed");
                return new Result("");
            }

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)_request.GetResponse();
            }
            catch (WebException ex)
            {
                _logger.Error("Error while get response");
                response = (HttpWebResponse)ex.Response;
            }

            var stream = response.GetResponseStream();
            string responseText;
            using (var reader = new StreamReader(stream))
            {
                responseText = reader.ReadToEnd();
            }

            SetHeaders();
            Login(_username, _password);

            return ReadResponse(responseText);
        }

        private Result ReadResponse(string response)
        {
            return new Result(response);
        }

        public bool CheckAvailable()
        {
            try
            {
                _testCl.DownloadString(_domain);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Login(string login, string password)
        {
            _username = login;
            _password = password;
            string credentials = String.Format("{0}:{1}", login, password);
            byte[] bytes = Encoding.UTF8.GetBytes(credentials);
            string base64 = Convert.ToBase64String(bytes);
            string authorization = String.Concat("Basic ", base64);

            _request.Headers.Add("Authorization", authorization);

            return true;
        }

        protected override void SetHeaders()
        {
            _request = (HttpWebRequest)WebRequest.Create("https://api.pinnaclesports.com/v1/bets/place");
            _request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0)";
            _request.Method = "POST";
            _request.Accept = "application/json";
            _request.ContentType = "application/json; charset=utf-8";
        }

        protected override void Connect()
        {
            throw new NotImplementedException();
        }

        public void SetHeader(string key, string val)
        {
            switch (key)
            {
                case "User-Agent": _request.UserAgent = val; break;
                case "Method": _request.Method = val; break;
                case "Content-Type": _request.ContentType = val; break;
                case "Accept": _request.Accept = val; break;
                default: _request.Headers[key] = val; break;
            }
        }

        public void SetProxy(IWebProxy proxy)
        {
            _request.Proxy = proxy;
        }

        public class Result
        {
            public decimal? Price { get; set; }
            public long? BetId { get; set; }
            public string Guid;
            public StatusEnum? Status;
            public ErrorCode? Error;
            public bool? BetterLineWasAccepted;

            public bool Success;

            public Result(string json)
            {
                // если будет ошибка, значит прислали NaN или другую нечисть, кроме null
                try
                {
                    var jo = JObject.Parse(json);
                    if (jo == null)
                        return;
                    Price = jo["price"] == null ? (decimal?)null : (decimal?)jo["price"];
                    BetId = jo["betId"] == null ? (long?)null : (long?)jo["betId"];
                    Guid = jo["uniqueRequestId"] == null ? null : (string)jo["uniqueRequestId"];
                    Status = (StatusEnum)Enum.GetNames(typeof(StatusEnum)).ToList().IndexOf((string)jo["status"]);
                    Error = (ErrorCode)Enum.GetNames(typeof(ErrorCode)).ToList().IndexOf((string)jo["errorCode"]);
                    BetterLineWasAccepted = ((string)jo["betterLineWasAccepted"])
                        == null ? (bool?)null : ((string)jo["betterLineWasAccepted"]).ToUpper() == "TRUE";

                    Success = Status.Value == StatusEnum.Accepted;
                }
                catch
                {
                    return;
                }

                #region Response

                /*
                 "{\"status\":\"ACCEPTED\",\"errorCode\":null,\"betId\":646820646,\"uniqueRequestId\":\"adb78385-9e54-4aad-b9f3-480754ff1ad9\",\"betterLineWasAccepted\":false,\"price\":3.6}"
                 */

                #endregion Response
            }
        }

        public enum StatusEnum
        {
            Accepted,
            PendingAcceptance,
            ProcessedWithError
        }

        public enum ErrorCode
        {
            AllBettingClosed,
            AllLiveBettingClosed,
            AboveEventMax,
            AboveMaxBetAmount,
            BelowMinBetAmount,
            BlockedBetting,
            BlockedClient,
            InsufficientFunds,
            InvalidCountry,
            InvalidEvent,
            InvalidOddsFormat,
            LineChanged,
            ListedPitchersSelectionError,
            OfflineEvent,
            PastCutofftime,
            RedCardsChanged,
            ScoreChanged,
            TimeRestriction
        }
    }
}