using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TemaKommentApp.Models; // <-- Győződj meg róla, hogy ez szerepel
using System.Text.Json.Serialization;

namespace TemaKommentApp.Models
{
    public class Komment
    {
        // Primary Key
        [Key]
        public int Id { get; set; }

        // Foreign Key: Kapcsolat a Tema táblához
        public int TemaId { get; set; } 

        [Required]
        [MaxLength(50)]
        public required string FelhasznaloNev { get; set; }

        [Required]
        [MaxLength(250)] 
        public required string Szoveg { get; set; }

        [Required]
        public DateTime Datum { get; set; }

        // Navigációs tulajdonság: Egy Komment egy Témához tartozik.
        [ForeignKey("TemaId")]
        [JsonIgnore]
        public Tema? Tema { get; set; } // '?' jelzi, hogy lehet null, ha nincs betöltve
    }
}