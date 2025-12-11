using Microsoft.EntityFrameworkCore;
using TemaKommentApp.Models; // <-- Entitásaink (Tema, Komment) eléréséhez

namespace TemaKommentApp.Data
{
    public class AppDbContext : DbContext
    {
        // Az osztály konstruktora, ami az adatbázis beállításait kapja a Program.cs-ből
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Táblák leképezése:
        public DbSet<Tema> Temak { get; set; }
        public DbSet<Komment> Kommentek { get; set; }

        // Ez a metódus biztosítja az 1:N kapcsolatot a Tema és Komment között.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Egy Tema (t) sok Kommentet (k) tartalmazhat
            modelBuilder.Entity<Tema>()
                .HasMany(t => t.Kommentek) 
                // Egy Komment (k) egy Témához tartozik
                .WithOne(k => k.Tema)     
                // Az idegen kulcs (FK) a Komment táblában található TemaId
                .HasForeignKey(k => k.TemaId); 

            base.OnModelCreating(modelBuilder);
        }
    }
}