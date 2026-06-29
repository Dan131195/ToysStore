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
        public DbSet<ImmagineProdotto> ImmaginiProdotto { get; set; }
        public DbSet<RecensioneProdotto> RecensioniProdotto { get; set; }
        public DbSet<Categoria> Categorie { get; set; }
        public DbSet<Condizione> Condizioni { get; set; }

        public DbSet<Utente> Utenti { get; set; }
        public DbSet<IndirizzoUtente> IndirizziUtenti { get; set; }
        public DbSet<ProdottoCarrello> ProdottiCarrello { get; set; }

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

            modelBuilder.Entity<Categoria>().HasData(
                new Categoria
                {
                    CategoriaId = 1,
                    NomeCategoria = "Prima Infanzia (0-36 mesi)",
                    DescrizioneCategoria = "Sonagli, tappeti gioco, giostrine, primi passi, cavalcabili, massaggiagengive."
                },
                new Categoria
                {
                    CategoriaId = 2,
                    NomeCategoria = "Costruzioni e Mattoncini",
                    DescrizioneCategoria = "LEGO, Mega Bloks, Playmobil, costruzioni in legno, set magnetici."
                },
                new Categoria
                {
                    CategoriaId = 3,
                    NomeCategoria = "Action Figure e Personaggi",
                    DescrizioneCategoria = "Supereroi, personaggi di film/serie TV, anime, wrestling, dinosauri, Gormiti."
                },
                new Categoria
                {
                    CategoriaId = 4,
                    NomeCategoria = "Bambole e Accessori",
                    DescrizioneCategoria = "Barbie, bambolotti (es. Cicciobello), case delle bambole, vestiti, passeggini giocattolo."
                },
                new Categoria
                {
                    CategoriaId = 5,
                    NomeCategoria = "Peluche e Pupazzi",
                    DescrizioneCategoria = "Orsacchiotti, personaggi animati in pezza, animali di peluche, doudou."
                },
                new Categoria
                {
                    CategoriaId = 6,
                    NomeCategoria = "Giochi da Tavolo e di Società",
                    DescrizioneCategoria = "Giochi in scatola classici, giochi di carte, scacchi, dama."
                },
                new Categoria
                {
                    CategoriaId = 7,
                    NomeCategoria = "Puzzle e Rompicapo",
                    DescrizioneCategoria = "Puzzle tradizionali, puzzle 3D, cubi di Rubik, puzzle in legno."
                },
                new Categoria
                {
                    CategoriaId = 8,
                    NomeCategoria = "Veicoli, Radiocomandati e Piste",
                    DescrizioneCategoria = "Macchinine (es. Hot Wheels), trenini, piste elettriche, droni, barche, aerei."
                },
                new Categoria
                {
                    CategoriaId = 9,
                    NomeCategoria = "Educativi e Scientifici",
                    DescrizioneCategoria = "Microscopi, kit per esperimenti (STEM), mappamondi, tablet educativi, giochi di logica."
                },
                new Categoria
                {
                    CategoriaId = 10,
                    NomeCategoria = "Creatività, Arti e Mestieri",
                    DescrizioneCategoria = "Pasta da modellare (Play-Doh), kit per braccialetti, colori, lavagne, timbri."
                },
                new Categoria
                {
                    CategoriaId = 11,
                    NomeCategoria = "Giochi d'Imitazione e Ruolo",
                    DescrizioneCategoria = "Cucine giocattolo, finti attrezzi da lavoro, registratori di cassa, set da dottore."
                },
                new Categoria
                {
                    CategoriaId = 12,
                    NomeCategoria = "Costumi e Travestimenti",
                    DescrizioneCategoria = "Vestiti di Carnevale/Halloween, maschere, bacchette magiche, accessori."
                },
                new Categoria
                {
                    CategoriaId = 13,
                    NomeCategoria = "Sport e Giochi all'Aperto",
                    DescrizioneCategoria = "Biciclette, monopattini, pattini, palloni, pistole ad acqua, altalene, casette."
                },
                new Categoria
                {
                    CategoriaId = 14,
                    NomeCategoria = "Musica e Strumenti Giocattolo",
                    DescrizioneCategoria = "Tastiere elettroniche, chitarre per bambini, xilofoni, batterie, microfoni."
                },
                new Categoria
                {
                    CategoriaId = 15,
                    NomeCategoria = "Videogiochi e Elettronica",
                    DescrizioneCategoria = "Console (Nintendo Switch, PlayStation), videogiochi fisici, accessori gaming, Tamagotchi."
                },
                new Categoria
                {
                    CategoriaId = 16,
                    NomeCategoria = "Libri, Fumetti e Cantastorie",
                    DescrizioneCategoria = "Fiabe, libri interattivi, fumetti per ragazzi, dispositivi audio (Fabbrica delle Storie, Tonies)."
                },
                new Categoria
                {
                    CategoriaId = 17,
                    NomeCategoria = "Zaini e Articoli Scolastici",
                    DescrizioneCategoria = "Zaini con personaggi, astucci, portapranzi, grembiuli."
                }
            );

            modelBuilder.Entity<Condizione>().HasData(
                new Condizione
                {
                    CondizioneId = 1,
                    NomeCondizione = "Nuovo e sigillato",
                    DescrizioneCondizione = "Il giocattolo è nuovo, mai aperto e si trova nella sua confezione originale con i sigilli intatti. Non presenta alcun danno."
                },
                new Condizione
                {
                    CondizioneId = 2,
                    NomeCondizione = "Nuovo senza confezione",
                    DescrizioneCondizione = "Il giocattolo non è mai stato usato per giocare, ma non ha più la scatola originale o le etichette. Non presenta il minimo segno di usura ed è completo di tutti gli accessori."
                },
                new Condizione
                {
                    CondizioneId = 3,
                    NomeCondizione = "Ottimo",
                    DescrizioneCondizione = "Usato pochissimo e tenuto con cura. Non ci sono difetti visibili, graffi o scoloriture. Tutti i pezzi originali sono presenti e, se elettronico, funziona perfettamente."
                },
                new Condizione
                {
                    CondizioneId = 4,
                    NomeCondizione = "Buono",
                    DescrizioneCondizione = "Il giocattolo è stato usato e amato. Mostra segni di usura normali e leggeri (es. piccoli graffi superficiali o adesivi leggermente consumati). È comunque completo per poterci giocare e strutturalmente integro."
                },
                new Condizione
                {
                    CondizioneId = 5,
                    NomeCondizione = "Accettabile",
                    DescrizioneCondizione = "Mostra segni di usura evidenti dovuti a un uso frequente. Potrebbe avere graffi profondi, vernice scolorita o mancare di accessori non essenziali (che non impediscono il funzionamento principale del gioco)."
                },
                new Condizione
                {
                    CondizioneId = 6,
                    NomeCondizione = "Con difetti",
                    DescrizioneCondizione = "Il giocattolo presenta difetti importanti, componenti elettroniche non funzionanti, rotture o pezzi mancanti fondamentali. Viene venduto principalmente per essere riparato, riutilizzato per parti di ricambio (es. lotti di mattoncini Lego) o restauro."
                }
            );
        }
    }
}