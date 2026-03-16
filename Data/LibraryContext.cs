namespace LibrarySystem.Data;

using Microsoft.EntityFrameworkCore;
using LibrarySystem.Entities;

// DbContext er EF Cores "gateway" til databasen.
// Hver DbSet<T> tilsvarer én tabell.
public class LibraryContext : DbContext
{
    // Konstruktøren tar DbContextOptions – dette er DI i praksis!
    // Vi bestemmer IKKE her om det er SQLite eller noe annet.
    // Det bestemmes når vi registrerer konteksten i Program.cs.
    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Her forteller vi EF Core om relasjoner.
        // En Loan har én Book og én Member.
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Book)
            .WithMany()
            .HasForeignKey(l => l.BookId);

        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Member)
            .WithMany(m => m.ActiveLoans)
            .HasForeignKey(l => l.MemberId);
    }
}