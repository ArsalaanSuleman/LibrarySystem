namespace LibrarySystem.UI;

using LibrarySystem.Services;

public class ConsoleUI
{
    private readonly LibraryService _libraryService;
    private readonly LoanService _loanService;

    // DI leverer begge services hit
    public ConsoleUI(LibraryService libraryService, LoanService loanService)
    {
        _libraryService = libraryService;
        _loanService = loanService;
    }

    public async Task RunAsync()
    {
        Console.WriteLine("╔════════════════════════════╗");
        Console.WriteLine("║     📚 LibrarySystem        ║");
        Console.WriteLine("╚════════════════════════════╝");

        bool running = true;

        // do/while garanterer at menyen vises minst én gang
        do
        {
            ShowMenu();
            string? input = Console.ReadLine();

            // switch expression – moderne C# syntax
            switch (input?.Trim())
            {
                case "1": await ShowAllBooksAsync(); break;
                case "2": await SearchBooksAsync(); break;
                case "3": await AddBookAsync(); break;
                case "4": await AddMemberAsync(); break;
                case "5": await ShowAllMembersAsync(); break;
                case "6": await BorrowBookAsync(); break;
                case "7": await ReturnBookAsync(); break;
                case "0":
                    Console.WriteLine("\n👋 Avslutter...");
                    running = false;
                    break;
                default:
                    Console.WriteLine("\n⚠️  Ugyldig valg, prøv igjen.");
                    break;
            }

        } while (running);
    }

    private void ShowMenu()
    {
        Console.WriteLine("\n─────────────────────────────");
        Console.WriteLine(" 1. Vis alle bøker");
        Console.WriteLine(" 2. Søk etter bok");
        Console.WriteLine(" 3. Legg til bok");
        Console.WriteLine(" 4. Registrer medlem");
        Console.WriteLine(" 5. Vis alle medlemmer");
        Console.WriteLine(" 6. Lån ut bok");
        Console.WriteLine(" 7. Returner bok");
        Console.WriteLine(" 0. Avslutt");
        Console.WriteLine("─────────────────────────────");
        Console.Write("Velg: ");
    }

    private async Task ShowAllBooksAsync()
    {
        var books = await _libraryService.GetAllBooksAsync();

        Console.WriteLine($"\n📚 Biblioteket har {books.Count} bok(er):\n");

        if (books.Count == 0)
        {
            Console.WriteLine("  Ingen bøker registrert ennå.");
            return;
        }

        foreach (var book in books)
            Console.WriteLine($"  {book.GetDisplayInfo()}");
    }

    private async Task SearchBooksAsync()
    {
        Console.Write("\nSøk (tittel, forfatter eller sjanger): ");
        string? query = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(query)) return;

        var results = await _libraryService.SearchBooksAsync(query);

        Console.WriteLine($"\n🔍 {results.Count} treff:\n");

        if (results.Count == 0)
        {
            Console.WriteLine("  Ingen bøker matchet søket.");
            return;
        }

        foreach (var book in results)
            Console.WriteLine($"  {book.GetDisplayInfo()}");
    }

    private async Task AddBookAsync()
    {
        Console.WriteLine("\n── Legg til bok ──");
        Console.Write("Tittel:    "); string? title  = Console.ReadLine();
        Console.Write("Forfatter: "); string? author = Console.ReadLine();
        Console.Write("Sjanger:   "); string? genre  = Console.ReadLine();
        Console.Write("ISBN:      "); string? isbn   = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
        {
            Console.WriteLine("⚠️  Tittel og forfatter er påkrevd.");
            return;
        }

        await _libraryService.AddBookAsync(
            title, author,
            genre ?? "Ukjent",
            isbn  ?? "Ukjent"
        );
    }

    private async Task AddMemberAsync()
    {
        Console.WriteLine("\n── Registrer medlem ──");
        Console.Write("Navn:  "); string? name  = Console.ReadLine();
        Console.Write("Epost: "); string? email = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("⚠️  Navn er påkrevd.");
            return;
        }

        await _libraryService.AddMemberAsync(name, email ?? "");
    }

    private async Task ShowAllMembersAsync()
    {
        var members = await _libraryService.GetAllMembersAsync();

        Console.WriteLine($"\n👥 {members.Count} medlem(mer):\n");

        if (members.Count == 0)
        {
            Console.WriteLine("  Ingen medlemmer registrert ennå.");
            return;
        }

        foreach (var m in members)
            Console.WriteLine($"  [{m.Id}] {m.Name} – {m.Email} " +
                              $"({m.ActiveLoans.Count} aktive lån)");
    }

    private async Task BorrowBookAsync()
    {
        Console.Write("\nBok-ID:    "); 
        if (!int.TryParse(Console.ReadLine(), out int bookId)) 
        {
            Console.WriteLine("⚠️  Ugyldig ID."); 
            return;
        }

        Console.Write("Medlem-ID: "); 
        if (!int.TryParse(Console.ReadLine(), out int memberId))
        {
            Console.WriteLine("⚠️  Ugyldig ID."); 
            return;
        }

        try
        {
            await _loanService.BorrowBookAsync(bookId, memberId);
        }
        catch (InvalidOperationException ex)
        {
            // Feil fra servicelaget fanges HER i UI-laget – ikke inne i servicen
            Console.WriteLine($"\n❌ {ex.Message}");
        }
    }

    private async Task ReturnBookAsync()
    {
        Console.Write("\nBok-ID: "); 
        if (!int.TryParse(Console.ReadLine(), out int bookId))
        {
            Console.WriteLine("⚠️  Ugyldig ID."); 
            return;
        }

        try
        {
            await _loanService.ReturnBookAsync(bookId);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n❌ {ex.Message}");
        }
    }
}