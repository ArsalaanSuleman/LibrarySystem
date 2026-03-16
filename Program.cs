using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LibrarySystem.Data;
using LibrarySystem.Interfaces;
using LibrarySystem.Repositories;
using LibrarySystem.Services;
using LibrarySystem.UI;

// ServiceCollection er DI-containeren – registeret over alle avhengigheter.
// Her bestemmer vi hvilke konkrete klasser som leveres når noen ber om et interface.
var services = new ServiceCollection();

// Logging må registreres først – MediatR krever det
services.AddLogging();

// Registrer databasen – vi velger SQLite HER, ikke inne i repository-klassene.
// Bytter vi til PostgreSQL en dag, endrer vi bare denne linjen.
services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite("Data Source=library.db"));

// Registrer repositories:
// "Når noen ber om IBookRepository, lever EfBookRepository"
// AddScoped = én instans per scope – passer perfekt med EF Core sin DbContext
services.AddScoped<IBookRepository, EfBookRepository>();
services.AddScoped<IMemberRepository, EfMemberRepository>();

// Registrer services – forretningslogikken
// LibraryService håndterer bøker og medlemmer
// LoanService håndterer utlån, retur og frister
services.AddScoped<LibraryService>();
services.AddScoped<LoanService>();

// Registrer ConsoleUI – også via DI, slik at den får sine
// avhengigheter (LibraryService, LoanService) injisert automatisk
services.AddScoped<ConsoleUI>();

// Registrer MediatR – scanner automatisk etter alle INotificationHandler-klasser
// i prosjektet, f.eks. BookBorrowedHandler
services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Bygg containeren – etter dette kan vi ikke registrere flere ting.
// provider er nå "restauranten som er åpen" – services var bare oppskriftsboken.
var provider = services.BuildServiceProvider();

// Vi oppretter et scope fordi EF Core sin DbContext er scoped.
// Alt som skjer innenfor dette blokken deler samme databaseforbindelse.
using (var scope = provider.CreateScope())
{
    // Opprett databasen automatisk hvis den ikke finnes
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    await db.Database.EnsureCreatedAsync();

    // Hent ConsoleUI fra DI-containeren – legg merke til at vi aldri skriver "new".
    // DI sørger for at LibraryService og LoanService allerede er klare inni ConsoleUI.
    var ui = scope.ServiceProvider.GetRequiredService<ConsoleUI>();
    await ui.RunAsync();
}