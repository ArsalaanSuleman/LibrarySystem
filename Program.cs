using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LibrarySystem.Data;
using LibrarySystem.Interfaces;
using LibrarySystem.Repositories;
using LibrarySystem.Services;

// ServiceCollection er DI-containeren – den er registeret over alle
// avhengigheter i applikasjonen. Her bestemmer vi hvilke konkrete
// klasser som leveres når noen ber om et interface.
var services = new ServiceCollection();

services.AddLogging();

// Registrer databasen – vi velger SQLite HER, ikke inne i repository-klassene
services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite("Data Source=library.db"));

// Registrer repositories:
// "Når noen ber om IBookRepository, lever EfBookRepository"
// AddScoped = én instans per "request" (passer perfekt med EF Core)
services.AddScoped<IBookRepository, EfBookRepository>();
services.AddScoped<IMemberRepository, EfMemberRepository>();

// Registrer services
services.AddScoped<LoanService>();

// Registrer MediatR – den scanner automatisk etter alle INotificationHandler-klasser
services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Bygg containeren – etter dette kan vi ikke registrere flere ting
var provider = services.BuildServiceProvider();

// Opprett databasen automatisk hvis den ikke finnes
using (var scope = provider.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    await db.Database.EnsureCreatedAsync();
}

// Hent LoanService fra DI-containeren – legg merke til at vi aldri skriver "new"
var loanService = provider.GetRequiredService<LoanService>();

Console.WriteLine("📚 LibrarySystem startet!");