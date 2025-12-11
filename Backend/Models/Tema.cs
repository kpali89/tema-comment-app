using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TemaKommentApp.Models; // <-- Győződj meg róla, hogy ez szerepel

namespace TemaKommentApp.Models
{
    public class Tema
    {
        // Primary Key (Automatikus növekmény)
        [Key]
        public int Id { get; set; }

        [Required] // Kötelező mező
        [MaxLength(100)] // Maximális hossza 100 karakter
        public required string Cim { get; set; } // A C# 11 'required' módosítója

        // Navigációs tulajdonság: Egy Téma sok Kommentet tartalmazhat.
        public ICollection<Komment> Kommentek { get; set; } = new List<Komment>();
    }
}