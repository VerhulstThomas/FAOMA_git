using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data.Entity;

using Faoma4;



namespace LoopEmailChecker
{
    //private static Faoma4.FaomaModel db = new FaomaModel();
    //private static List<serverAccount> accounts = db.serverAccount.ToList();

    public partial class Service1 : ServiceBase
    {
        //Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;

        private static Faoma4.FaomaModel db = new FaomaModel();
        //private FaomaModel db = new FaomaModel();
        // laad alle accounts
        //private List<serverAccount> accounts;//= db.serverAccount.ToList();

        // de lijst voor de te verwerken accounts
        //private List<serverAccount> teVerwerkenAccounts = new List<serverAccount>();

        private Timer timer = new Timer(OnTick, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

        private static void OnTick(object state)
        {
            var accounts = db.serverAccount.ToList();
            var teVerwerkenAccounts = new List<serverAccount>();

            foreach (var account in accounts)
            {
                // zet lastchecked van accounts waar dat null is
                if (account.lastCheked == null)
                {
                    account.lastCheked = DateTime.Now;
                }
                // pak de lustijd
                int looptijd = account.looptijd;

                DateTime tmp = (DateTime)account.lastCheked;

                tmp.AddMinutes(looptijd);

                // als lastCheked + looptijd groter is dan dateTime.Now
                if (DateTime.Now > tmp)
                {
                    teVerwerkenAccounts.Add(account);
                }
            }

            db.SaveChanges();
            // stuur de lijst met accounts die aan de beurt zijn door              
            LoopUtils.getAllUnseenMails(teVerwerkenAccounts);

            // kijk om het half uur of er nieuwe accouns zijn
            //DateTime dt = new DateTime();
            //if ((dt.Minute / 30) == 0)
            //{
            //    //de reeks nieuwe accounts wordt aan de lisjt toegevoegd                 
            //    accounts = db.serverAccount.ToList();
            //}
            // haal alle accounts uit de lijst
            teVerwerkenAccounts.Clear();
        }

        public Service1()
        {

            ///db =Faoma4.FaomaModel;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Debugger.Break();
        }

        protected override void OnStop()
        {
            
        }
    }
}
