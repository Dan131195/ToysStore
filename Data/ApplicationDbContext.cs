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

        // Tabelle di Identity 
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

        // Tabelle del Core Business
        public DbSet<Prodotto> Prodotti { get; set; }
        public DbSet<ImmagineProdotto> ImmaginiProdotto { get; set; }
        public DbSet<RecensioneProdotto> RecensioniProdotto { get; set; }

        // Tabelle di Gestione Utente e Carrello
        public DbSet<Utente> Utenti { get; set; } 
        public DbSet<IndirizzoUtente> IndirizziUtenti { get; set; }
        public DbSet<ProdottoCarrello> ProdottiCarrello { get; set; }

        // Tabelle del Processo di Vendita (Ordini)
        public DbSet<Ordine> Ordini { get; set; }
        public DbSet<StatoOrdine> StatiOrdine { get; set; }
        public DbSet<ProdottoOrdine> ProdottiOrdine { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fondamentale: Chiama sempre il base.OnModelCreating per configurare Identity!
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // 1. CONFIGURAZIONE IDENTITY (Many-to-Many User-Role)
            // ==========================================
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

            // ==========================================
            // 2. PROFILO UTENTE ESTESO (One-to-One)
            // ==========================================
            modelBuilder.Entity<Utente>()
                .HasOne(u => u.User)
                .WithOne(au => au.Utente)
                .HasForeignKey<Utente>(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Rubrica Indirizzi dell'Utente (One-to-Many)
            modelBuilder.Entity<IndirizzoUtente>()
                .HasOne(i => i.User)
                .WithMany(u => u.Indirizzi)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ==========================================
            // 3. CATALOGO PRODOTTI & ELEMENTI CORRELATI
            // ==========================================

            // Precisione decimale per il prezzo del giocattolo nel catalogo
            modelBuilder.Entity<Prodotto>()
                .Property(p => p.PrezzoGiocattolo)
                .HasPrecision(10, 2);

            // Prodotto - ImmagineProdotto (One-to-Many)
            modelBuilder.Entity<ImmagineProdotto>()
                .HasOne(i => i.Prodotto)
                .WithMany(p => p.ImmaginiProdotto)
                .HasForeignKey(i => i.ProdottoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relazione: Un Venditore (ApplicationUser) ha molti Prodotti in vendita
            modelBuilder.Entity<Prodotto>()
                .HasOne(p => p.User)
                .WithMany(u => u.ListaProdotti)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prodotto - RecensioneProdotto (One-to-Many)
            modelBuilder.Entity<RecensioneProdotto>()
                .HasOne(r => r.Prodotto)
                .WithMany(p => p.RecensioniProdotto)
                .HasForeignKey(r => r.ProdottoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Autore della Recensione (User - Recensione)
            modelBuilder.Entity<RecensioneProdotto>()
                .HasOne(r => r.User)
                .WithMany(u => u.RecensioniProdotto)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ==========================================
            // 4. GESTIONE CARRELLO (Tabella di di Giunzione User-Prodotto)
            // ==========================================
            modelBuilder.Entity<ProdottoCarrello>()
                .HasOne(pc => pc.User)
                .WithMany(u => u.ProdottiCarrello)
                .HasForeignKey(pc => pc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProdottoCarrello>()
                .HasOne(pc => pc.Prodotto)
                .WithMany(p => p.ProdottiCarrello)
                .HasForeignKey(pc => pc.ProdottoId)
                .OnDelete(DeleteBehavior.Cascade);

            // ==========================================
            // 5. GESTIONE ORDINI & CASSA (Flusso Vendite)
            // ==========================================

            // Relazione Diretta Utente -> Ordine
            modelBuilder.Entity<Ordine>()
                .HasOne(o => o.User)
                .WithMany(u => u.Ordini)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Impedisce la cancellazione degli ordini se elimini l'utente (integrità fiscale)

            // StatoOrdine - Ordine (One-to-Many)
            modelBuilder.Entity<Ordine>()
                .HasOne(o => o.StatoOrdine)
                .WithMany(s => s.Ordini)
                .HasForeignKey(o => o.StatoOrdineId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ordine - Riga d'Ordine / ProdottoOrdine (One-to-Many)
            modelBuilder.Entity<ProdottoOrdine>()
                .HasOne(po => po.Ordine)
                .WithMany(o => o.ProdottiOrdine)
                .HasForeignKey(po => po.OrdineId)
                .OnDelete(DeleteBehavior.Cascade); // Se l'ordine viene cancellato, le sue righe spariscono

            // Prodotto - Riga d'Ordine (One-to-Many)
            modelBuilder.Entity<ProdottoOrdine>()
                .HasOne(po => po.Prodotto)
                .WithMany(p => p.ProdottiOrdine)
                .HasForeignKey(po => po.ProdottoId)
                .OnDelete(DeleteBehavior.Restrict); // Non puoi cancellare un Prodotto dal catalogo se è presente in un vecchio ordine

            // Configurazioni Decimali per l'Ordine (Prezzi storici bloccati)
            modelBuilder.Entity<Ordine>()
               .Property(o => o.Totale)
               .HasColumnType("decimal(18,2)")
               .HasPrecision(18, 2);

            modelBuilder.Entity<ProdottoOrdine>()
                .Property(p => p.PrezzoUnitario)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            // ==========================================
            // 6. SEEDING DATA (Dati Iniziali di Sistema)
            // ==========================================
            modelBuilder.Entity<StatoOrdine>().HasData(
                new StatoOrdine { StatoOrdineId = 1, Nome = "In Attesa" },
                new StatoOrdine { StatoOrdineId = 2, Nome = "Confermato" },
                new StatoOrdine { StatoOrdineId = 3, Nome = "In Preparazione" },
                new StatoOrdine { StatoOrdineId = 4, Nome = "Spedito" },
                new StatoOrdine { StatoOrdineId = 5, Nome = "Ricevuto" },
                new StatoOrdine { StatoOrdineId = 6, Nome = "Annullato" }
            );
        }
    }
}