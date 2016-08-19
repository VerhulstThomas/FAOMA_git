using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Faoma4;

namespace BL
{
    public class DocumentenService
    {
        private DocumentsDao dDao = new DocumentsDao();
        public List<Document> ListOfDocuments()
        {
            return  dDao.ListOfDocuments().ToList();
        }

        public Document FindDocumentWithId(long? id)
        {
            return dDao.FindDocumentWithId(id);
        }

        public void CreateDocument(Document document)
        {
            dDao.CreateDocument(document);
        }

        public void DeleteDocument(long id)
        {
            dDao.DeleteDocument(id);
        }

        public void EditDocument(Document document, long? id)
        {
            dDao.EditDocument(document, id);
        }

    }
}
