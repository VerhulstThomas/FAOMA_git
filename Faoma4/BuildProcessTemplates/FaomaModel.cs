namespace Faoma4
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FaomaModel : DbContext
    {
        public FaomaModel()
            : base("name=FaomaModel")
        {
        }

        public virtual DbSet<contact_document> contact_document { get; set; }
        public virtual DbSet<contacten> contacten { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<serverAccount> serverAccount { get; set; }
        public virtual DbSet<serveraccount_contacten> serveraccount_contacten { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<contacten>()
                .Property(e => e.Bedrijfsnaam)
                .IsFixedLength();

            modelBuilder.Entity<contacten>()
                .Property(e => e.E_mail)
                .IsFixedLength();

            modelBuilder.Entity<contacten>()
                .HasMany(e => e.contact_document)
                .WithRequired(e => e.contacten)
                .HasForeignKey(e => e.contactId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<contacten>()
                .HasMany(e => e.serveraccount_contacten)
                .WithRequired(e => e.contacten)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.naam)
                .IsFixedLength();

            modelBuilder.Entity<Document>()
                .Property(e => e.verzendersEmail)
                .IsFixedLength();

            modelBuilder.Entity<Document>()
                .Property(e => e.link)
                .IsFixedLength();

            modelBuilder.Entity<Document>()
                .HasMany(e => e.contact_document)
                .WithRequired(e => e.Document)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<serverAccount>()
                .Property(e => e.username)
                .IsFixedLength();

            modelBuilder.Entity<serverAccount>()
                .Property(e => e.password)
                .IsFixedLength();

            modelBuilder.Entity<serverAccount>()
                .Property(e => e.teBeherenEmail)
                .IsFixedLength();

            modelBuilder.Entity<serverAccount>()
                .Property(e => e.teBeherenEmailPW)
                .IsFixedLength();

            modelBuilder.Entity<serverAccount>()
                .Property(e => e.beheerdersEmail)
                .IsFixedLength();

            modelBuilder.Entity<serverAccount>()
                .HasMany(e => e.serveraccount_contacten)
                .WithRequired(e => e.serverAccount)
                .WillCascadeOnDelete(false);
        }
    }
}
