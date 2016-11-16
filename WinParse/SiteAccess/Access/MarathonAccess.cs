
using Common.Core.Helpers;
using Common.Core.Parsing;
using Common.Modules.AntiCaptha;
using Newtonsoft.Json.Linq;
using NLog;
using SiteAccess.Model;
using SiteAccess.Model.Bets;
using System;
using System.Net;
using System.Text;
using System.Web;

namespace SiteAccess.Access
{
    public class MarathonAccess : ContentLoader, ISiteAccess<MarathonBet, bool>
    {
        private IAntiCaptcha _ac;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private static int countAttemptToLogin = 5;

        public MarathonAccess( IAntiCaptcha anti ) : base(null)
        {
            _ac = anti;
            Encoding = Encoding.UTF8;
            Timeout = 5000;
            HomeReq();
        }

        public void HomeReq( )
        {
            Headers.Clear();
            Headers["Accept"] = "application/json, text/javascript, */*; q=0.01";
            Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36"; ;
            _html = DownloadString(_domain);

            Headers.Clear();
            Headers["Accept"] = "application/json, text/javascript, */*; q=0.01";
            Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            _html = DownloadString("pagemessages.htm".TrueLink(_domain));

            Headers.Clear();
            Headers["Accept"] = "application/json, text/javascript, */*; q=0.01";
            Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            _html = UploadString("/betslip/view/current.htm".TrueLink(_domain), "firstLoad=true");
        }

        public JObject SetBet( MarathonBet bet )
        {
            Clear();
            try
            {
                Add(bet);
                Save(bet);
            }
            catch (Exception e)   
            {
                _logger.Error(e, $"Can't add or save bet", bet);
                return null;
            }
            Headers["Accept"] = "text/plain, */*; q=0.01";
            Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            Headers["User-Agent"] =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            Headers["X-Requested-With"] = "XMLHttpRequest";
            Headers["X-NewRelic-ID"] = _xpid;
            Headers["Content-Type"] = "application/x-www-form-urlencoded";
            Headers["Referer"] = "https://www.marathonbet.com/";
            Encoding = Encoding.UTF8;

            var data =
                "schd=false&p=SINGLES&b=" + bet.GetBetData();

            try
            {
                var str = UploadString("/betslip/placebet.htm".TrueLink(_domain.OriginalString), data);
                var jo = JObject.Parse(str);
                return jo;
            }
            catch(Exception e)
            {
                _logger.Error(e, $"Can't place bet", bet);
                return null;
            }
        }

        public JObject SetBetOneClick( MarathonBet bet )
        {
            Headers["Accept"] = "text/plain, */*; q=0.01";
            Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            Headers["User-Agent"] =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            Headers["X-Requested-With"] = "XMLHttpRequest";
            Headers["X-NewRelic-ID"] = _xpid;
            Headers["Content-Type"] = "application/x-www-form-urlencoded";
            Headers["Referer"] = "https://www.marathonbet.com/";
            Encoding = Encoding.UTF8;

            var data = $"ch={bet.GetAddData()}&st={bet.Stake}&chd=true";
            var jo = UploadJObject("/betslip/placebetinoneclick.htm".TrueLink(_domain.OriginalString), data);

            return jo;
        }

        public JObject PlaceTicket( string id )
        {
            Headers["Accept"] = "text/plain, */*; q=0.01";
            Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            Headers["User-Agent"] =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            Headers["X-Requested-With"] = "XMLHttpRequest";
            Headers["X-NewRelic-ID"] = _xpid;
            Headers["Content-Type"] = "application/x-www-form-urlencoded";
            Headers["Referer"] = "https://www.marathonbet.com/";
            Encoding = Encoding.UTF8;

            var data = $"t={id}&oneClick=true";
            var jo = UploadJObject("/betslip/placeticket.htm".TrueLink(_domain.OriginalString), data);
            return jo;
        }

        public bool Login( string login, string password )
        {


            Headers.Clear();
            Headers["Accept"] = "application/json, text/javascript, */*; q=0.01";
            Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            Headers["User-Agent"] =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            Headers["X-Requested-With"] = "XMLHttpRequest";
            Headers["Content-Type"] = "application/x-www-form-urlencoded; charset=UTF-8";
            Headers["Referer"] = _domain.OriginalString;
            Headers["Origin"] = _domain.Host;

            var data =
                $"login={HttpUtility.UrlEncode(login)}&login_password={HttpUtility.UrlEncode(password)}&loginUrl=https%3A%2F%2Fwww.marathonbet.com%3A443%2Fsu%2Flogin.htm";
            var jo = UploadJObject("/login.htm".TrueLink(_domain.OriginalString), data);
            if(jo == null)
            {
                _logger.Error("Answer from server was null, when login");
                return false;
            }

            try
            {
                if((string)jo["loginResult"] == "FAIL")
                {
                    _logger.Error("Login result is FAIL");
                    return false;
                }

                if((string)jo["loginResult"] == "CAPTCHA_EXPECTED")
                {
                    LoginWithCapcha(login, password);
                }

                SetStateAfterLogin();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void LoginWithCapcha( string login, string password )
        {
            for(int i = 0; i < countAttemptToLogin; i++)
            {
                // Получение капчи
                Headers.Clear();
                Headers["Accept"] = "image/webp,image/*,*/*;q=0.8";
                Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
                Headers["User-Agent"] =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
                var byteArr = DownloadData($"/captcha.htm?{GetTime()}".TrueLink(_domain));
                var res = _ac.GetAnswer(byteArr);

                // Запрос на вход с капчей
                Headers.Clear();
                Headers["Accept"] = "application/json, text/javascript, */*; q=0.01";
                Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
                Headers["User-Agent"] =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
                Headers["X-Requested-With"] = "XMLHttpRequest";
                Headers["Content-Type"] = "application/x-www-form-urlencoded; charset=UTF-8";
                Headers["Referer"] = _domain.OriginalString;

                var data =
                $"login={HttpUtility.UrlEncode(login)}&login_password={HttpUtility.UrlEncode(password)}&captcha={res}&loginUrl=https%3A%2F%2Fwww.marathonbet.com%3A443%2Fsu%2Flogin.htm";


                var jo = UploadJObject("/login.htm".TrueLink(_domain.OriginalString), data);

                if((string)jo["loginResult"] != "SUCCESS")
                {
                    _logger.Error("Can't login with captcha in " + (i + 1) + "attempt.");
                    continue;
                }

                break;
            }
        }

        private void Add( MarathonBet bet )
        {
            Headers["Accept"] = "text/plain, */*; q=0.01";
            Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            Headers["User-Agent"] =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            Headers["X-Requested-With"] = "XMLHttpRequest";
            Headers["X-NewRelic-ID"] = _xpid;
            Headers["Content-Type"] = "application/x-www-form-urlencoded";
            Headers["Referer"] = "https://www.marathonbet.com/";

            var data =
                "ch=" + bet.GetAddData() + "&url=https%3A%2F%2Fwww.marathonbet.com%2Fsu%2Fall-events.htm&ws=true";

            var res = UploadString("/betslip/add.htm".TrueLink(_domain), data);

        }

        private void Save( MarathonBet bet )
        {
            Headers["Accept"] = "text/plain, */*; q=0.01";
            Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            Headers["User-Agent"] =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            Headers["X-Requested-With"] = "XMLHttpRequest";
            Headers["X-NewRelic-ID"] = _xpid;
            Headers["Content-Type"] = "application/x-www-form-urlencoded";
            Headers["Referer"] = "https://www.marathonbet.com/";

            var data =
                "schd=false&u=" + bet.Id + "&st=" + bet.Stake + "&ew=false&p=SINGLES&v=falsee";

            UploadString("/betslip/save.htm".TrueLink(_domain), data);
        }

        private void Clear( )
        {
            Headers["Accept"] = "text/plain, */*; q=0.01";
            Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            Headers["User-Agent"] =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            Headers["X-Requested-With"] = "XMLHttpRequest";
            Headers["X-NewRelic-ID"] = _xpid;
            Headers["Content-Type"] = "application/x-www-form-urlencoded";
            Headers["Referer"] = "https://www.marathonbet.com/";

            UploadString("/betslip/clear.htm".TrueLink(_domain), "");
        }

        private void SetStateAfterLogin( )
        {
            Headers["Accept"] = "*/*";
            Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            Headers["User-Agent"] =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            Headers["X-Requested-With"] = "XMLHttpRequest";
            Headers["X-NewRelic-ID"] = _xpid;
            Headers["Content-Type"] = "application/x-www-form-urlencoded; charset=UTF-8";


            var data = "betPolicy=EqualsToCurrent";
            UploadString("/betslip/updatebetplacingmode.htm".TrueLink(_domain), data);
        }

        private Int64 GetTime( )
        {
            Int64 retval = 0;
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = (DateTime.Now.ToUniversalTime() - st);
            retval = (Int64)(t.TotalMilliseconds + 0.5);
            return retval;
        }

        public bool CheckAvailable( )
        {
            throw new NotImplementedException();
        }

        public bool MakeBet( MarathonBet bet )
        {
            var res = SetBet(bet);
            if(res == null)
            {
                _logger.Error("Place bet failed due to previous errors.", bet);
                return false;
            }
            if(res.ToString().Contains("ERROR"))
            {
                _logger.Error("Place bet result is ERROR.", res);
                return false;
            }
            _logger.Info<MarathonBet>("Place bet success", bet);
            return true;


            #region results
            /*
             {{
  "result": "ERROR",
  "previouspage": "SHORT",
  "availableFreebets": [],
  "amendedstake": false,
  "messages": [
    "Извините, Ваша ставка не принята.",
    "На Вашем счёте недостаточно средств. Ваш текущий баланс: $&nbsp;0.00"
  ],
  "currentpage": "SINGLES",
  "oom": true,
  "selectionuids": [
    "4050540,Match_Result.1"
  ],
  "sessionExpire": 7198099,
  "eventIdsInTicket": "[4050540]",
  "content": "<div class=\"betslip-content\" id=\"betslip-content\">\n  <input id=\"isPriceChangesSubscriptionEnabled\" value=\"true\" type=\"hidden\">\n<table class=\"table-header\">\n<tr>\n<td id='header_bet_slip' class=\"not-radius\" onclick=\"toggleBetslip();\">\n<span class=\"bet-slip-selection-count\">(выбрано 1) &nbsp; &nbsp; <span class=\"betslip-arrow\"></span></span>\nКупон\n</td>\n</tr>\n</table>\n<table id=\"coupon_body\" class=\"bet-slip-page hidden\">\n <tr>\n <td>\n <div class=\"bet-slip-open\">\n\n<table class=\"type-bet\">\n<tr>\n   <td  class=\"active\"  id=\"button_singles\" \n onClick=\"toggleBetslipTab('SINGLES'); return false;\">\n <a href=\"#\">Одиночные</a>\n </td>\n  \n  \n  \n \n </tr>\n\n</table>\n <div id=\"tab_singles\">\n <div class=\"hidden\">\n <input id=\"min.vip\" value=\"9.22337203685477E12\" type=\"hidden\">\n </div>\n <div class=\"scroll-selections\">\n <table class=\"select-bet\" id=\"single-container\">\n <tr>\n <td class=\"del\">\n <a href=\"javascript: getBetslip().remove('4050540,Match_Result.1')\">x</a>\r\n\r\n </td>\n <td class=\"\">\n<div class=\"hint-box\">\r\n <span class=\"underline\" data-tooltip='{\"position\":{\"container\":\".header-content\",\"at\":\"center left\",\"my\":\"center right\"},\"style\":{\"classes\":\"bet-slip-hint\"}}'>\r\n Bolton Wanderers vs Blackpool\r\n </span>.\r\n <div class=\"tooltip carrying-over\">Футбол. Англия. Трофей Футбольной лиги. Групповой этап. Болтон Уондерерз - Блэкпул</div>\r\n Match Result: <span id=\"selection.name.4050540,Match_Result.1\" class=\"bold\">Bolton Wanderers To Win</span>\r\n</div> <table>\n <tr>\n <td>\n <div class=\"hidden\">\n <input data-betslip-selection-uid=\"4050540,Match_Result.1\" type=\"hidden\" />\n <input id=\"coeff.id.4050540,Match_Result.1\" value=\"10345303346\" type=\"hidden\">\n <input id=\"vip.avaliable.4050540,Match_Result.1\" value=\"false\" type=\"hidden\">\n <input id=\"ew.fraction.4050540,Match_Result.1\" value=\"1.0\" type=\"hidden\">\n <input id=\"ew.enabled.4050540,Match_Result.1\" value=\"false\" type=\"hidden\">\n <input id=\"halfplace.4050540,Match_Result.1\" value=\"9.22337203685477E12\" type=\"hidden\">\n <input id=\"eprice.4050540,Match_Result.1\" value=\"0.7272727272727273\" type=\"hidden\">\n <input id=\"max.4050540,Match_Result.1\" value=\"1.12\" type=\"hidden\" >\n <input id=\"vip.enabled.4050540,Match_Result.1\" value=\"false\" type=\"hidden\" >\n <input id=\"choice.status.4050540,Match_Result.1\" value=\"NONE\" type=\"hidden\" >\n <input id=\"actualSelection.4050540,Match_Result.1\" data-accepted-selection-uid=\"4050540,Match_Result.1\" value=\"4050540,Match_Result.1\" data-accepted-selection-type=\"CP\" >\n </div>\n<div class=\"ew\">\n <div class=\"tooltip\">\nСтавка на победу/место\n </div>\n</div>\n </td>\n <td class=\"choice-price\">\nКоэфф.:\n<span id=\"price.presentation.4050540,Match_Result.1\" class=\"bold\n\">\n1.727\n</span>\n\n </td>\n <td class=\"stake\">\n <input\n id=\"stake.4050540,Match_Result.1\"\n name=\"stake\"\n rel=\"Сумма:\"\n type=\"text\"\n class=\"stake stake-input js-focusable\"\n value=\"5\"\n onpaste=\"return checkPaste(this, event, true, false, 2);\"\n onkeypress=\"return checkChar(this, event, true, false, 2);\"\n onblur=\"if (isEmpty(this)) this.value='Сумма:';\"\n onkeyup=\"if (noValue(event)) return true; extractSum(this, 2, false); getBetslip().checkBet('4050540,Match_Result.1');\"\n onfocus=\"if (isEmpty(this)) this.value='';\"\n />\n </td>\n </tr>\n <tr>\n <td colspan=\"2\"></td>\n <td class=\"p-0\">\n </td>\n </tr>\n </table>\n <div class=\"min-max-stake\">\nМакс. сумма: \n <span id=\"max-stake-4050540,Match_Result.1\"\n >\n<a href=\"javascript://\" data-max-stake=\"1.12\" onclick=\"var el = $$('stake.4050540,Match_Result.1'); jQuery(el).focus(); setValue(el, jQuery(this).data('maxStake')); extractSum(el, 2, false); if (isStake(el)) getBetslip().checkBet('4050540,Match_Result.1');\">\n1.12\n </a>\n</span>\n<span id=\"max-stake-inactive-4050540,Match_Result.1\"\n class=\"hidden\"\n >\n1.12\n</span>\n &nbsp;&nbsp;&nbsp;\nМин. сумма:  <span id=\"min-stake-4050540,Match_Result.1\">0.22</span>\n </div>\n <div id=\"display.is_not_active.4050540,Match_Result.1\" class=\"red\" style=\"display:  none \">\nВыбор неактивен.\n </div>\n <div class=\"estimate-return\">\nПотенциальная выплата: \n<span id=\"returns.singles.4050540,Match_Result.1\" class=\"bold\">\n8.64\n</span>\n </div>\n </td>\n </tr>\n </table>\n </div>\n\n </div>\n\n<div class=\"result-bet\">\n<div class=\"cl-right\">\n<div class=\"total carrying-over\"><span id=\"total_cost\">5.00</span><img id=\"total_cost_progress\" class=\"event-loading hidden\" src=\"/cdn/3-0-461-340/images/loading.gif\" alt=\"\" /></div>\n<div class=\"total-name\">Общая сумма ставки: </div>\n</div>\n<div class=\"cl-right\">\n<div class=\"total carrying-over\"><span id=\"total_returns\">8.64</span><img id=\"total_returns_progress\" class=\"event-loading hidden\" src=\"/cdn/3-0-461-340/images/loading.gif\" alt=\"\" /></div>\n<div class=\"total-name\">Общая потенц. выплата: </div>\n</div>\n\n</div>\n<div id=\"betslip_has_removed_choices_block\" class=\"has-removed\">\nКупон содержит неактивные выборы.\n</div>\n<div id=\"betslip_apply_choices_block\" class=\"apply-choices\">\nИзменились условия.\n<span id=\"betslip_apply_choices\" class=\"button\">Принять</span>\n</div>\n\n<table class=\"panel-bet\">\n<tr>\n<td>\n<span onclick=\"getBetslip().removeAll();return false;\" class=\"button btn-remove\">\nУдалить все<span>x</span>\n</span>\n</td>\n<td class=\"panel-bet-cell\">\n<span\nid=\"betslip_placebet_btn_id\"\nonclick=\"\ngetBetslip().confirmPlaceBet(true, true);\n return false;\n\" \nclass=\"button btn-place-bet\n \"\ndata-enabled=\"true\"\n>\nСделать ставку\n</span>\n</td>\n</tr>\n<tr>\n<td colspan=\"2\" class=\"retain\">\n<input type=\"checkbox\" id=\"betslip_retain\" onchange=\"getBetslip().setRetain(this.checked);\"\n/>\n<label for=\"betslip_retain\">Сохранить выборы в купоне после размещения ставки</label>\n</td>\n</tr>\n</table>\n<div class=\"bet-slip-policy\">\n<div class=\"js-place-mode-label bet-slip-place-mode-label\" onclick=\"showHideBetslipPlaceModeBlock()\">\n<span class=\"mode-label\">Соглашаться с изменением коэфф.</span>\n<span class=\"arrow\"></span>\n</div>\n<div id=\"betslipPlacingModeBlockId\">\n\n \n<table id=\"betPlacingModeRadioBlockId\" class=\"radio-btn \">\n <tr>\n <td class=\"radio-btn-greater\">\n <input type=\"radio\" value=\"GreaterOrEqualsToCurrent\" id=\"betPlacingModeRadio_GreaterOrEqualsToCurrent\"\n />\n<label for=\"betPlacingModeRadio_GreaterOrEqualsToCurrent\" data-ellipsis=\"{}\">при повышении</label>\n  </td>\n <td class=\"radio-btn-any\">\n <input type=\"radio\" value=\"Any\" id=\"betPlacingModeRadio_Any\"\n />\n<label for=\"betPlacingModeRadio_Any\" data-ellipsis=\"{}\">всегда</label>\n  </td>\n <td class=\"radio-btn-equals\">\n <input type=\"radio\" value=\"EqualsToCurrent\" id=\"betPlacingModeRadio_EqualsToCurrent\"\nchecked=\"checked\"\n />\n<label for=\"betPlacingModeRadio_EqualsToCurrent\" data-ellipsis=\"{}\">никогда</label>\n  </td>\n </tr>\n</table>\n</div>\n</div>\n\n </div>\n</td>\n </tr>\n</table>\n </div>",
  "changedterms": false
}}

            {{
  "amendedstake": false,
  "currentpage": "SHORT",
  "ticket_id": "749388585",
  "oom": false,
  "selectionuids": [],
  "sessionExpire": 7199997,
  "content": "<div class=\"betslip-content\" id=\"betslip-content\">\n  <input id=\"isPriceChangesSubscriptionEnabled\" value=\"true\" type=\"hidden\">\n<table class=\"table-header\">\n<tr>\n<td id='header_bet_slip' class=\"not-radius\" onclick=\"toggleBetslip();\">\n<span class=\"bet-slip-selection-count\">(выбрано 0) &nbsp; &nbsp; <span class=\"betslip-arrow\"></span></span>\nКупон\n</td>\n</tr>\n</table>\n<table id=\"coupon_body\" class=\"bet-slip-page hidden\">\n <tr>\n <td>\n <div id=\"stakeNoWideScreenId\" class=\"stake-no hidden\">\n <div class=\"bet-slip-nostake\">Пожалуйста, выберите исход</div>\n <div class=\"bet-slip-info\">Максимумы по ставкам растут с приближением начала матчей.</div>\n </div>\n<div id=\"stakeNoNotWideScreenId\" class=\"stake-no hidden\">\n <div class=\"bet-slip-nostake-not-wide\">Пожалуйста, выберите исход</div>\n </div>\n</td>\n </tr>\n</table>\n </div>",
  "changedterms": false,
  "result": "OK",
  "balance": "$ 32.26",
  "previouspage": "SINGLES",
  "availableFreebets": [],
  "messages": [
    "Ваша ставка принята, спасибо"
  ],
  "eventIdsInTicket": "[4050540]"
}}
             */

            #endregion
        }

        public void SetHeader( string key, string val )
        {
            Headers[key] = val;
        }

        public void SetProxy( IWebProxy proxy )
        {
            Proxy = proxy as WebProxy;
        }

        private User _user;
        private readonly Uri _domain = new Uri("https://www.marathonbet.com/su");
        private string _xpid;
        private string _html;
    }
}
