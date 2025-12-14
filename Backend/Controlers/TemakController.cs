// backend/Controllers/TemakController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Fontos az Include-hoz!
using TemaKommentApp.Data;
using TemaKommentApp.Models;
using TemaKommentApp.DTOs;

[Route("api/[controller]")]
[ApiController]
public class TemakController : ControllerBase
{
    private readonly AppDbContext _context;

    // A Controller a Program.cs-ből megkapja az AppDbContext-et (Dependency Injection)
    public TemakController(AppDbContext context)
    {
        _context = context;
    }

    // ----------------------------------------------------
    // Endpoint 1: GET /api/temak
    // Visszaadja az összes témát az összes kommenttel együtt
    // ----------------------------------------------------
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tema>>> GetTemak()
    {
        // Az Include metódus biztosítja, hogy a Temak entitás navigációs tulajdonsága (Kommentek) is betöltődjön.
        // Ezt hívjuk "Eager Loading"-nak.
        var temak = await _context.Temak
            .Include(t => t.Kommentek)
            .OrderByDescending(t => t.Id) // Legújabb téma felül
            .ToListAsync();
        
        return Ok(temak);
    }

    // ----------------------------------------------------
    // Endpoint 2: POST /api/temak
    // Új téma létrehozása
    // ----------------------------------------------------
    [HttpPost]
    public async Task<ActionResult<Tema>> PostTema(Tema tema)
    {
        // Biztonsági okokból győződjünk meg róla, hogy az ID-t az adatbázis adja.
        tema.Id = 0; 
        
        // Elmenti az új témát
        _context.Temak.Add(tema);
        await _context.SaveChangesAsync();

        // 201 Created státuszkód és a létrejött erőforrás visszaküldése.
        return CreatedAtAction(nameof(GetTemak), new { id = tema.Id }, tema);
    }

    [HttpPost("Komment")] // Ezt a végpontot POST /api/Temak/Komment címen lehet hívni
        public async Task<ActionResult<Komment>> PostKomment(KommentCreateDto kommentDto)
        {
            // 1. Ellenőrizzük, létezik-e a megadott TemaId
            var tema = await _context.Temak.FindAsync(kommentDto.TemaId);

            if (tema == null)
            {
                return NotFound($"A megadott TemaId ({kommentDto.TemaId}) nem található.");
            }

            // 2. Létrehozzuk a Komment modellt a DTO-ból
            var komment = new Komment
            {
                TemaId = kommentDto.TemaId,
                FelhasznaloNev = kommentDto.FelhasznaloNev,
                Szoveg = kommentDto.Szoveg,
                Datum = DateTime.Now // Dátum automatikus beállítása
            };

            // 3. Adatbázis mentés
            _context.Kommentek.Add(komment);
            await _context.SaveChangesAsync();

            // 4. Sikeres válasz (201 Created), visszaküldve a teljes Komment objektumot
            return CreatedAtAction(nameof(GetTemak), new { id = komment.Id }, komment);
        }
}