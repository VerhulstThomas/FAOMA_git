using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Faoma4
{
    public class Loop
    {
        private static FaomaModel db = new FaomaModel();
        private static Mailk mail = new Mailk();
        
        //public string test() {
         //   db.serverAccount.ToList()
        //        }
        //bij het starten van de server moet voor alle accounts de loop worden opestart
        // opgelet, bij het aanmaken van een nieuw account moet dit ook nog gebeuren!
        //https://msdn.microsoft.com/en-us/library/ff963552.aspx
        public void loopInitializer()
        {
           // IEnumerable<serverAccount> myEnumerable = ;

           // Parallel.ForEach(myEnumerable, myEnumerable lus(2)){
           
           // }
        }
        

        static void UpdatePredictionsParallel()
        {
            //Mailk mails = new Mailk();
            // haal een lijst op met alle geregistreerde accounts
            List<serverAccount> accounts = db.serverAccount.ToList();
            
            //start parallelle loops
            Parallel.ForEach(accounts, account =>
            {
                int looptijd = account.looptijd;


                while (true)
                {
                    Thread.Sleep(60 * looptijd * 1000);
                    //Console.WriteLine("*** calling MyMethod *** ");
                    //MyMethod();
                    

                }


                //Trend trend = SampleUtilities.Fit(account.Balance);
                //double prediction = trend.Predict(
                //                 account.Balance.Length + NumberOfMonths);
                //account.ParPrediction = prediction;
                //account.ParWarning = prediction < account.Overdraft;
            });
        }



        public void lus(int time)
        {

        }


    }
}