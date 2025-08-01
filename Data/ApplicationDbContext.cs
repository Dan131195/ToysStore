using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToysStore.Models;
using ToysStore.Models.Auth;

namespace ToysStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

        public DbSet<Prodotto> Prodotti { get; set; }
        public DbSet<ProdottoCarrello> ProdottiCarrello { get; set; }
        public DbSet<ImmagineProdotto> ImmaginiProdotto { get; set; }
        public DbSet<RecensioneProdotto> RecensioniProdotto { get; set; }
        public DbSet<Utente> Utenti { get; set; }
        public DbSet<IndirizzoUtente> IndirizziUtenti { get; set; }
        public DbSet<Ordine> Ordini {  get; set; }
        public DbSet<StatoOrdine> StatiOrdine { get; set;}
        public DbSet<ProdottoOrdine> ProdottiOrdine { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // UTENTE   
            modelBuilder.Entity<ApplicationUserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.ApplicationUser)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.ApplicationRole)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== RELAZIONI PRINCIPALI =====

            // ApplicationUser - Utente (One-to-One)
            modelBuilder.Entity<Utente>()
                .HasOne(u => u.User)
                .WithOne(au => au.Utente)
                .HasForeignKey<Utente>(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ApplicationUser - Prodotto (One-to-Many)
            modelBuilder.Entity<Prodotto>()
                .HasOne(p => p.User)
                .WithMany(u => u.ListaProdotti)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prodotto>()
                .Property(p => p.PrezzoGiocattolo)
                .HasPrecision(10, 2);

            // Prodotto - ImmagineProdotto (One-to-Many)
            modelBuilder.Entity<ImmagineProdotto>()
                .HasOne(i => i.Prodotto)
                .WithMany(p => p.ImmaginiProdotto)
                .HasForeignKey(i => i.ProdottoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prodotto - RecensioneProdotto (One-to-Many)
            modelBuilder.Entity<RecensioneProdotto>()
                .HasOne(r => r.Prodotto)
                .WithMany(p => p.RecensioniProdotto)
                .HasForeignKey(r => r.ProdottoId)
                .OnDelete(DeleteBehavior.Cascade);

            // ApplicationUser - ProdottoCarrello (One-to-Many)
            modelBuilder.Entity<ProdottoCarrello>()
                .HasOne(pc => pc.User)
                .WithMany(u => u.ProdottiCarrello)
                .HasForeignKey(pc => pc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prodotto - ProdottoCarrello (One-to-Many)
            modelBuilder.Entity<ProdottoCarrello>()
                .HasOne(pc => pc.Prodotto)
                .WithMany(p => p.ProdottiCarrello)
                .HasForeignKey(pc => pc.ProdottoId)
                .OnDelete(DeleteBehavior.Cascade);

            // ApplicationUser - IndirizzoUtente (One-to-Many)
            modelBuilder.Entity<IndirizzoUtente>()
                .HasOne(i => i.User)
                .WithMany(u => u.Indirizzi)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // IndirizzoUtente - Ordine (One-to-Many)
            modelBuilder.Entity<Ordine>()
                .HasOne(o => o.IndirizzoUtente)
                .WithMany(i => i.Ordini)
                .HasForeignKey(o => o.IndirizzoUtenteId)
                .OnDelete(DeleteBehavior.NoAction); 

            // StatoOrdine - Ordine (One-to-Many)
            modelBuilder.Entity<Ordine>()
                .HasOne(o => o.StatoOrdine)
                .WithMany(s => s.Ordini)
                .HasForeignKey(o => o.StatoOrdineId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ordine - ProdottoOrdine (One-to-Many)
            modelBuilder.Entity<ProdottoOrdine>()
                .HasOne(po => po.Ordine)
                .WithMany(o => o.ProdottiOrdine)
                .HasForeignKey(po => po.OrdineId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prodotto - ProdottoOrdine (One-to-Many)
            modelBuilder.Entity<ProdottoOrdine>()
                .HasOne(po => po.Prodotto)
                .WithMany(p => p.ProdottiOrdine)
                .HasForeignKey(po => po.ProdottoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StatoOrdine>().HasData(
                new StatoOrdine { StatoOrdineId = 1, Nome = "In Attesa" },
                new StatoOrdine { StatoOrdineId = 2, Nome = "Confermato" },
                new StatoOrdine { StatoOrdineId = 3, Nome = "In Preparazione" },
                new StatoOrdine { StatoOrdineId = 4, Nome = "Spedito" },
                new StatoOrdine { StatoOrdineId = 5, Nome = "Ricevuto" },
                new StatoOrdine { StatoOrdineId = 6, Nome = "Annullato" }
            );

            modelBuilder.Entity<Ordine>()
               .Property(o => o.Totale)
               .HasColumnType("decimal(18,2)")
               .HasPrecision(18, 2);

            // Configurazione per l'entità ProdottoOrdine
            modelBuilder.Entity<ProdottoOrdine>()
                .Property(p => p.PrezzoUnitario)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

        }
    }
}
