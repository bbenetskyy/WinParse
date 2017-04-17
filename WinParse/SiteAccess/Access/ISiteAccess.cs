using System.Net;

namespace SiteAccess.Access
{
    public interface ISiteAccess<TBet, TResult>
    {
        bool CheckAvailable();

        TResult MakeBet(TBet bet);

        bool Login(string login, string password);

        void SetHeader(string key, string val);

        void SetProxy(IWebProxy proxy);
    }
}