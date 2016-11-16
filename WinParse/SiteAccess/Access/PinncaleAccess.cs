using Newtonsoft.Json.Linq;
using NLog;
using SiteAccess.Enums;
using SiteAccess.Helpers;
using SiteAccess.Model.Bets;

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SiteAccess.Access
{
    public class PinncaleAccess : AccessBase, ISiteAccess<PinnacleBet, PinncaleAccess.Result>
    {

        private HttpWebRequest request;
        private string _username, _password, Domain, ApiDomain;
        private WebClient _testCl;
        private static Logger _logger;

        public PinncaleAccess( ) : base(null)
        {
            Domain = "https://www.pinnacle.com/";
            ApiDomain = "https://api.pinnaclesports.com/v1/bets/place";
            _testCl = new WebClient();
            _logger = LogManager.GetCurrentClassLogger();
        }


        public PinncaleAccess.Result MakeBet( PinnacleBet bet )
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

            if(bet.BetType == BetType.TOTAL_POINTS || bet.BetType == BetType.TEAM_TOTAL_POINTS)
            {
                postJson += ",\"side\":\"" + bet.Side.ToString() + "\"";
            }
            else
            {
                postJson += ",\"team\":\"" + bet.TeamType.ToString() + "\"";
            }

            if(bet.SportId == 3 /*if is baseball*/)
            {
                if(bet.Pitcher1MustStart == null || bet.Pitcher2MustStart == null)
                    throw new ArgumentException("Bet must contains data about pitchers: Pitcher1MustStart or Pitcher2MustStart is null");

                postJson += ",\"pitcher1MustStart\":\"" + bet.Pitcher1MustStart.Value.GetString() + "\"," +
                    "\"Pitcher2MustStart\":\"" + bet.Pitcher2MustStart.Value.GetString() + "\"";
            }

            if(bet.CustomerReference != null)
            {
                postJson += ",\"customerReference\":\"" + bet.CustomerReference + "\"";

            }

            if(bet.AlternativeLineId != null)
            {
                postJson += ",\"altLineId\":\"" + bet.AlternativeLineId.Value.ToString() + "\"";
            }

            postJson += "}";


            byte[] byteArray = Encoding.UTF8.GetBytes(postJson);
            Stream dataStream;
            try
            {
                dataStream = request.GetRequestStream();
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
                response = (HttpWebResponse)request.GetResponse();
            }
            catch(WebException ex)
            {
                _logger.Error("Error while get response");
                response = (HttpWebResponse)ex.Response;
            }

            var stream = response.GetResponseStream();
            string responseText;
            using(var reader = new StreamReader(stream))
            {
                responseText = reader.ReadToEnd();
            }

            SetHeaders();
            Login(_username, _password);
            
            return ReadResponse(responseText);
        }

        private Result ReadResponse( string response )
        {
            return new Result(response);
        }

        public bool CheckAvailable( )
        {
            try
            {
                _testCl.DownloadString(Domain);
            }
            catch
            {
                return false;
            }
            return true;
        }


        public bool Login( string login, string password )
        {
            _username = login;
            _password = password;
            string credentials = String.Format("{0}:{1}", login, password);
            byte[] bytes = Encoding.UTF8.GetBytes(credentials);
            string base64 = Convert.ToBase64String(bytes);
            string authorization = String.Concat("Basic ", base64);

            request.Headers.Add("Authorization", authorization);

            return true;
        }

        protected override void SetHeaders( )
        {
            request = (HttpWebRequest)WebRequest.Create("https://api.pinnaclesports.com/v1/bets/place");
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0)";
            request.Method = "POST";
            request.Accept = "application/json";
            request.ContentType = "application/json; charset=utf-8";
        }

        protected override void Connect( )
        {
            throw new NotImplementedException();
        }

        public void SetHeader( string key, string val )
        {
            switch(key)
            {
                case "User-Agent": request.UserAgent = val; break;
                case "Method": request.Method = val; break;
                case "Content-Type": request.ContentType = val; break;
                case "Accept": request.Accept = val; break;
                default: request.Headers[key] = val; break;
            }

        }

        public void SetProxy( IWebProxy proxy )
        {
            request.Proxy = proxy;
        }

        public class Result
        {
            public decimal? Price { get; set; }
            public long? BetId { get; set; }
            public string GUID;
            public StatusEnum? Status;
            public ErrorCode? Error;
            public bool? BetterLineWasAccepted;

            public bool Success;
            public Result( string json )
            {
                // если будет ошибка, значит прислали NaN или другую нечисть, кроме null
                try
                {
                    var jo = JObject.Parse(json);
                    if(jo == null)
                        return;
                    Price = jo["price"] == null ? (decimal?)null : (decimal?)jo["price"];
                    BetId = jo["betId"] == null ? (long?)null : (long?)jo["betId"];
                    GUID = jo["uniqueRequestId"] == null ? null : (string)jo["uniqueRequestId"];
                    Status = (StatusEnum)Enum.GetNames(typeof(StatusEnum)).ToList().IndexOf((string)jo["status"]);
                    Error = (ErrorCode)Enum.GetNames(typeof(ErrorCode)).ToList().IndexOf((string)jo["errorCode"]);
                    BetterLineWasAccepted = ((string)jo["betterLineWasAccepted"])
                        == null ? (bool?)null : ((string)jo["betterLineWasAccepted"]).ToUpper() == "TRUE";

                    Success = Status.Value == StatusEnum.ACCEPTED;
                }
                catch
                {
                    return;
                }
                #region Response
                /*
                 "{\"status\":\"ACCEPTED\",\"errorCode\":null,\"betId\":646820646,\"uniqueRequestId\":\"adb78385-9e54-4aad-b9f3-480754ff1ad9\",\"betterLineWasAccepted\":false,\"price\":3.6}"
                 */
                #endregion
            }
        }

        public enum StatusEnum
        {
            ACCEPTED,
            PENDING_ACCEPTANCE,
            PROCESSED_WITH_ERROR
        }

        public enum ErrorCode
        {
            ALL_BETTING_CLOSED,
            ALL_LIVE_BETTING_CLOSED,
            ABOVE_EVENT_MAX,
            ABOVE_MAX_BET_AMOUNT,
            BELOW_MIN_BET_AMOUNT,
            BLOCKED_BETTING,
            BLOCKED_CLIENT,
            INSUFFICIENT_FUNDS,
            INVALID_COUNTRY,
            INVALID_EVENT,
            INVALID_ODDS_FORMAT,
            LINE_CHANGED,
            LISTED_PITCHERS_SELECTION_ERROR,
            OFFLINE_EVENT,
            PAST_CUTOFFTIME,
            RED_CARDS_CHANGED,
            SCORE_CHANGED,
            TIME_RESTRICTION
        }
    }
}
