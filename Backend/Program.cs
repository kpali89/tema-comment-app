using TemaKommentApp.Data; // Az AppDbContext eléréséhez
using Microsoft.EntityFrameworkCore; // Az UseSqlite metódushoz
using TemaKommentApp.Models;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------------------
// 1. SERVICES (Szolgáltatások) Konfigurációja
// ----------------------------------------------------------------------

// Adatbázis szolgáltatás hozzáadása (EF Core + SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    // Megmondjuk az EF Core-nak, hogy az SQLite-ot használja, és a 
    // kapcsolati stringet az appsettings.json "SqliteConnection" kulcsa alatt találja.
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"));
});

// Alapvető API és Swagger szolgáltatások
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// >>>>>>> ÚJ KÓDBLOKK AZ ADATBÁZIS INICIALIZÁLÁSRA START <<<<<<<
// Ez a blokk csak tesztadatokat szúr be az adatbázisba, ha üres.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Csak akkor tölt be, ha nincsenek témák.
    if (!context.Temak.Any())
    {
        var tema1 = new Tema { Cim = "Miért jó a Vue.js és a C#?" };
        var tema2 = new Tema { Cim = "Hol érdemes fejleszteni?" };

        context.Temak.AddRange(tema1, tema2);
        context.SaveChanges();

        // Kezdő kommentek feltöltése
        var komment1 = new Komment { 
            TemaId = tema1.Id, 
            FelhasznaloNev = "Fejlesztő_Jani", 
            Szoveg = "Szerintem a C# gyors, a Vue pedig egyszerűen tanulható.", 
            Datum = DateTime.Now 
        };
        var komment2 = new Komment { 
            TemaId = tema2.Id, 
            FelhasznaloNev = "Tanuló_Lili", 
            Szoveg = "Codespaces-ben a legjobb, nem kell semmit telepíteni!", 
            Datum = DateTime.Now.AddMinutes(-5) 
        };

        context.Kommentek.AddRange(komment1, komment2);
        context.SaveChanges();
    }
}
// >>>>>>> ÚJ KÓDBLOKK AZ ADATBÁZIS INICIALIZÁLÁSRA END <<<<<<<

// A többi kód (if (app.Environment.IsDevelopment()...) itt folytatódik
// ...

// ----------------------------------------------------------------------
// 2. MIDDLEWARE (Kérésfeldolgozó Lánc) Konfigurációja
// ----------------------------------------------------------------------

// Csak fejlesztői környezetben engedélyezzük a Swagger-t (API dokumentáció)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ----------------------------------------------------------------------
// 3. CORS (Cross-Origin Resource Sharing) Engedélyezése
// ----------------------------------------------------------------------
// EZ KULCSFONTOSSÁGÚ: Engedélyezi, hogy a Vue frontend (ami másik porton fut)
// HTTP kéréseket küldjön a C# backendnek.
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();