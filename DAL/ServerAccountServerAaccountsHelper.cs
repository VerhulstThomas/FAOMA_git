using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Faoma4.DAL
{
    // fout bij opstarten service
    //https://support.threattracksecurity.com/support/solutions/articles/1000071019-error-1053-the-service-did-not-respond-in-a-timely-fashion-when-attempting-to-start-stop-or-pause
    // deze klasse dient om op basis van  de string Account.id, ServerAccount.id te vinden
    public class ServerAccountServerAaccountsHelper
    {
        private string serveraccountId;
        private FaomaModel db = new FaomaModel();

        public ServerAccountServerAaccountsHelper(string id)
        {
            this.serveraccountId = id;
        }

        public long serverAccountsId()
        {
           

                //            return db.serverAccount.Find(X => X. :;
                serverAccount sa = (from x in db.serverAccount
                                    where x.username == serveraccountId
                                    select x).FirstOrDefault();

            if (!(sa ==null))
            {
                return sa.id;
            }
            return 1;
                

            
        }

    }
}