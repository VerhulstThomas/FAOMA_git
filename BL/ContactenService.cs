using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Faoma4;


namespace BL
{
    public class ContactenService
    {
        private ContactensDao cDao = new ContactensDao();
        public List<contacten> contactsFromThisUser()
        {
            List<contacten> doorTeGevenContacten = new List<contacten>();
            //ContactensDao cDao = new ContactensDao();

            return doorTeGevenContacten = cDao.getContactsFromThisUser().ToList();
        }

        public void createContact(contacten contact)
        {
            cDao.createContact(contact);
        }

        public void deleteContact(long id)
        {
            cDao.deleteContact(id);
        }

        public contacten FindContactWithId(long? id)
        {
            return cDao.FindContactWithId(id);
        }

        public void EditContact(contacten contact)
        {
            cDao.EditContact(contact);
        }



    }
}
