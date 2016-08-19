namespace Faoma4
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("contacten")]
    public partial class contacten
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public contacten()
        {
            contact_document = new HashSet<contact_document>();
            serveraccount_contacten = new HashSet<serveraccount_contacten>();
        }

        public long id { get; set; }

        [Required]
        [StringLength(30)]
        public string Bedrijfsnaam { get; set; }

        [Column("E-mail")]
        [Required]
        [StringLength(30)]
        public string E_mail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<contact_document> contact_document { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<serveraccount_contacten> serveraccount_contacten { get; set; }
    }
}
