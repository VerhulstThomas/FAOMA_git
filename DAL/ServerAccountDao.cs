using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faoma4;

namespace DAL
{
    public class ServerAccountDao : IServerAccountDao
    {
        private FaomaModel db = new FaomaModel();
        public void CreateServerAccount(serverAccount serverAccount)
        {
            db.serverAccount.Add(serverAccount);
            db.SaveChanges();

            db.Entry(serverAccount).GetDatabaseValues();
            long id = serverAccount.id;
            serverAccount sAccount = db.serverAccount.Find(id);
          

            sAccount.username = tempId.tmpId;
            // voeg ook de datum toe als eerste waarde voor lastCheked
            sAccount.lastCheked = DateTime.Now;

            Guid guid = Guid.NewGuid();
            sAccount.password = guid.ToString();

            db.SaveChanges();
            db.Dispose();
        }

        public void DeleteServerAccount(long id)
        {
            throw new NotImplementedException();
        }

        public void EditServerAccount(serverAccount serverAccount, long? id)
        {
             serverAccount origineelServerAccount = db.serverAccount.Find(id);
             origineelServerAccount.teBeherenEmail = serverAccount.teBeherenEmail;
             origineelServerAccount.teBeherenEmailPW = serverAccount.teBeherenEmailPW;
             origineelServerAccount.looptijd = serverAccount.looptijd;
             origineelServerAccount.beheerdersEmail = serverAccount.beheerdersEmail;
            //// db.Entry(serverAccount).State = EntityState.Modified;
             db.SaveChanges();
            db.Dispose();

        }

        public serverAccount FindServerAccountWithId(long? id)
        {
            serverAccount serverAccount = db.serverAccount.Find(id);
            db.Dispose();
            return serverAccount;
            
        }

        public IList<serverAccount> ListOfServerAccounts()
        {
            throw new NotImplementedException();
        }

        public void EditServerAccount()
        {
            throw new NotImplementedException();
        }
    }
}
