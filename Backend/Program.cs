using TemaKommentApp.Data; // Az AppDbContext eléréséhez
using Microsoft.EntityFrameworkCore; // Az UseSqlite metódushoz

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