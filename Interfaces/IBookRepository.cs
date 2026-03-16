namespace LibrarySystem.Interfaces;

using LibrarySystem.Entities;

// Dette er kontrakten. LibraryService vet bare om DETTE –
// ikke om EF Core, ikke om SQLite, ikke om noe annet.
public interface IBookRepository
{
    Task<List<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<List<Book>> SearchAsync(string query);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);
}