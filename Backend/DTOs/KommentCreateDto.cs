// backend/DTOs/KommentCreateDto.cs

using System.ComponentModel.DataAnnotations;

namespace TemaKommentApp.DTOs
{
    public class KommentCreateDto
    {
        [Required]
        public int TemaId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string FelhasznaloNev { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Szoveg { get; set; }
    }
}