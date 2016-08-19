using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Faoma4;

namespace BL
{
    interface IServerAccountService
    {
        IList<serverAccount> ListOfServerAccounts();
        serverAccount FindServerAccountWithId(long? id);
        void EditServerAccount(serverAccount serverAccount, long? id);
        //void EditDocument(Document document, long? id);
        void DeleteServerAccount(long id);
        void CreateServerAccount(serverAccount serverAccount);
    }
    
}
