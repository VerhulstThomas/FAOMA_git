using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Faoma4;

namespace DAL
{
    interface IServerAccountDao
    {
        IList<serverAccount> ListOfServerAccounts();
        serverAccount FindServerAccountWithId(long? id);
        void EditServerAccount();
        void EditServerAccount(serverAccount serverAccount, long? id);
        void DeleteServerAccount(long id);
        void CreateServerAccount(serverAccount serverAccount);
    }
}
