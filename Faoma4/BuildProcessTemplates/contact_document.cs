namespace Faoma4
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class contact_document
    {
        public long id { get; set; }

        public long contactId { get; set; }

        public long documentId { get; set; }

        public virtual contacten contacten { get; set; }

        public virtual Document Document { get; set; }
    }
}
