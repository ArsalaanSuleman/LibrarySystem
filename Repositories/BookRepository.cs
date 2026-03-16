namespace LibrarySystem.Repositories;

using Microsoft.EntityFrameworkCore;
using LibrarySystem.Data;
using LibrarySystem.Interfaces;
using LibrarySystem.Entities;

// EfBookRepository oppfyller kontrakten fra IBookRepository.
// Dette er den ENESTE klassen som vet om EF Core og SQLite.
// LibraryService over den vet ingenting om dette.
public class EfBookRepository : IBookRepository
{
    private readonly LibraryContext _context;

    // DI leverer LibraryContext hit automatisk – vi ber ikke om "new LibraryContext()"
    public EfBookRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        // FirstOrDefaultAsync returnerer null hvis ingenting finnes – derav Book?
        return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Book>> SearchAsync(string query)
    {
        // EF Core oversetter dette LINQ-uttrykket til SQL automatisk:
        // SELECT * FROM Books WHERE Title LIKE '%query%' OR Author LIKE '%query%'
        string q = query.ToLower();
        return await _context.Books
            .Where(b => (b.Title != null && b.Title.ToLower().Contains(q)) ||
            (b.Author != null && b.Author.ToLower().Contains(q)) ||
            (b.Genre != null && b.Genre.ToLower().Contains(q)))
            .ToListAsync();
    }

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync(); // sender SQL INSERT til databasen
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync(); // sender SQL UPDATE
    }

    public async Task DeleteAsync(int id)
    {
        var book = await GetByIdAsync(id);
        if (book is not null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync(); // sender SQL DELETE
        }
    }
}