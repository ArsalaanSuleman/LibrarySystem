namespace LibrarySystem.Services;

using LibrarySystem.Interfaces;
using LibrarySystem.Entities;

public class LibraryService
{
    private readonly IBookRepository _bookRepo;
    private readonly IMemberRepository _memberRepo;

    public LibraryService(IBookRepository bookRepo, IMemberRepository memberRepo)
    {
        _bookRepo = bookRepo;
        _memberRepo = memberRepo;
    }

    public async Task<List<Book>> GetAllBooksAsync()
        => await _bookRepo.GetAllAsync();

    public async Task<List<Book>> SearchBooksAsync(string query)
        => await _bookRepo.SearchAsync(query);

    public async Task AddBookAsync(string title, string author, string genre, string isbn)
    {
        var book = new Book
        {
            Title = title,
            Author = author,
            Genre = genre,
            ISBN = isbn,
            IsAvailable = true
        };
        await _bookRepo.AddAsync(book);
        Console.WriteLine($"✅ \"{title}\" ble lagt til i biblioteket.");
    }

    public async Task AddMemberAsync(string name, string email)
    {
        var member = new Member { Name = name, Email = email };
        await _memberRepo.AddAsync(member);
        Console.WriteLine($"✅ Medlem \"{name}\" ble registrert.");
    }

    public async Task<List<Member>> GetAllMembersAsync()
        => await _memberRepo.GetAllAsync();
}