using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Faoma4
{
    public class TestCallBack
    {
        private static FaomaModel db = new FaomaModel();

        private int invokeCount;
        private int maxCount;
        private List<serverAccount> accounts;
        public List<serverAccount> teVerwerkenAccounts = new List<serverAccount>();

       

        public TestCallBack(List<serverAccount> lijst)
        {
            invokeCount = 0;
            //maxCount = count;
            accounts = lijst;
        }

        public List<serverAccount> OptionsList
        {
            get { return teVerwerkenAccounts; }
            set { teVerwerkenAccounts = value; }
        }

        // This method is called by the timer delegate.
        public void CheckStatus(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;


            foreach (serverAccount account in accounts)
            {
                // zet lastchecked van accounts waar dat null is
                if (account.lastCheked == null)
                {
                    // haal op basis van het emailadres het contact op die het document verstuurde  
                    //string sAccount = document.verzendersEmail;

                    //listOfCompany.Where(c => c.id == 1).FirstOrDefault().Name = "Whatever Name";
                    accounts.Where(a => a.id == account.id).FirstOrDefault().lastCheked = DateTime.Now;
                    
                    //sAccount.lastCheked = DateTime.Now;
                    db.SaveChanges();

                    //account.lastCheked = DateTime.Now;
                    //db.serverAccount.SingleOrDefault(x => x.id == account.id);
                    //db.
                }

                int looptijd = account.looptijd;
                //DateTime dt = new DateTime();

                DateTime dt = (DateTime)account.lastCheked;
                dt.AddMinutes(looptijd);

                if (DateTime.Now > dt)
                {
                    teVerwerkenAccounts.Add(account);
                    
                }




                //Console.WriteLine("{0} Checking status {1,2}.",
                //DateTime.Now.ToString("h:mm:ss.fff"),
                //(++invokeCount).ToString());

                //if (invokeCount == maxCount)
                //{
                //    // Reset the counter and signal Main.
                //    invokeCount = 0;
                //    autoEvent.Set();
                //}
            }
            Thread.Sleep(900);
        }
    }
}