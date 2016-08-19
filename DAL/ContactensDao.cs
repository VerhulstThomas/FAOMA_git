using Faoma4;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public class ContactensDao
    {
        private FaomaModel db = new FaomaModel();

        public IList<contacten> getContactsFromThisUser()
        {
            var onbewerkteContactenLijst = db.serveraccount_contacten.ToList();
            List<serveraccount_contacten> bewerkteContactenLijst = new List<serveraccount_contacten>();

            foreach (var contact in onbewerkteContactenLijst)
            {
                if (!(contact.serverAccountId == tmpServerAccountsId.tmpServerAccountId))
                {

                }
                else { bewerkteContactenLijst.Add(contact); }
            }

            List<contacten> doorTeGevenContacten = new List<contacten>();
           
            foreach (var bewerktContact in bewerkteContactenLijst)
            {
                long tmp = bewerktContact.contactenId;
                foreach (var contact in db.contacten.ToList())
                {
                    if (tmp == contact.id)
                    {
                        doorTeGevenContacten.Add(contact);
                    }
                }


            }
            db.Dispose();
            return doorTeGevenContacten;
        }

        public void EditContact(contacten contact)
        {
            try
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public void deleteContact(long id)
        {
            try
            {
                contacten contacten = db.contacten.Find(id);

                serveraccount_contacten sc = (from x in db.serveraccount_contacten
                                              where x.contactenId == contacten.id
                                              select x).FirstOrDefault();

                db.serveraccount_contacten.Remove(sc);

                db.contacten.Remove(contacten);
                db.SaveChanges();
                db.Dispose();

            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public void createContact(contacten contact)
        {
            try
            {
                db.contacten.Add(contact);
                db.SaveChanges();

                // bij toevoegen contact moet ook de colom  serveraccount_contacten worden aangevuld
                // http://stackoverflow.com/questions/5212751/how-can-i-get-id-of-inserted-entity-in-entity-framework
                serveraccount_contacten sc = new serveraccount_contacten();
                db.Entry(contact).GetDatabaseValues();
                long id = contact.id;
                sc.contactenId = id;
                //sc.serverAccountId = 3;// hardcoaded, moet eigenlijk aan login hangen
                // zoals hierboven, toegepast
                sc.serverAccountId = tmpServerAccountsId.tmpServerAccountId;
                db.serveraccount_contacten.Add(sc);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public contacten FindContactWithId(long? id)
        {
            try
            {
               contacten contacten = db.contacten.Find(id);
                db.Dispose();
                return contacten;
                
            }
            catch (Exception)
            {

                throw;
            }
            
        }

      
    }
}
