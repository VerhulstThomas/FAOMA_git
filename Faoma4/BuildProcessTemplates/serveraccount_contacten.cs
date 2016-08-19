namespace Faoma4
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class serveraccount_contacten
    {
        public long id { get; set; }

        public long serverAccountId { get; set; }

        public long contactenId { get; set; }

        public virtual contacten contacten { get; set; }

        public virtual serverAccount serverAccount { get; set; }
    }
}
