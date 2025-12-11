// backend/Controllers/TemakController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Fontos az Include-hoz!
using TemaKommentApp.Data;
using TemaKommentApp.Models;

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
}