using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SiteAccess.Access
{
    public interface ISiteAccess<Bet, Result>
    {
        bool CheckAvailable( );
        Result MakeBet( Bet bet );
        bool Login(string login, string password);
        void SetHeader( string key, string val );
        void SetProxy( IWebProxy proxy );

    }
}
