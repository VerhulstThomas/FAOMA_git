namespace Faoma4
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Document")]
    public partial class Document
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Document()
        {
            contact_document = new HashSet<contact_document>();
        }

        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long id { get; set; }

        //[Required]
        [StringLength(80)]
        public string naam { get; set; }

        //[Required]
        [StringLength(150)]
        public string verzendersEmail { get; set; }
        
        [StringLength(150)]
        public string link { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? datum { get; set; }

        [Range(0, 1, ErrorMessage = "0 = niet  1 = wel")]
        public byte? isBetaald { get; set; }

        public byte? isOpgehaald { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<contact_document> contact_document { get; set; }
    }
}
