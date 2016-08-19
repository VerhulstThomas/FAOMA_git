namespace Faoma4
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("serverAccount")]
    public partial class serverAccount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public serverAccount()
        {
            serveraccount_contacten = new HashSet<serveraccount_contacten>();
        }

        public long id { get; set; }

        //[Required dit gaf een error ModelState isvalid = false
        [StringLength(100)]
        public string username { get; set; }

        //Required]
        [StringLength(100)]
        public string password { get; set; }

        [Required]
        [StringLength(100)]
        public string teBeherenEmail { get; set; }

        [Required]
        [StringLength(100)]
        public string teBeherenEmailPW { get; set; }

        [Range(1, 500, ErrorMessage = "Kies een waarde tussen 1 en 500 minuten")]
        public int looptijd { get; set; }

        [StringLength(100)]
        public string beheerdersEmail { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? lastCheked { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<serveraccount_contacten> serveraccount_contacten { get; set; }
    }
}
