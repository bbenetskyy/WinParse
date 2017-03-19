using System.Net;

namespace SiteAccess.Access
{
    public interface ISiteAccess<Bet, Result>
    {
        bool CheckAvailable();

        Result MakeBet(Bet bet);

        bool Login(string login, string password);

        void SetHeader(string key, string val);

        void SetProxy(IWebProxy proxy);
    }
}