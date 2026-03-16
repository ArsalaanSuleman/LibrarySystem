namespace LibrarySystem.Services;

using MediatR;
using LibrarySystem.Events;
using LibrarySystem.Interfaces;
using LibrarySystem.Entities;

public class LoanService
{
    private readonly IBookRepository _bookRepo;
    private readonly IMemberRepository _memberRepo;
    private readonly IMediator _mediator;

    // DI leverer alle tre avhengighetene – LoanService oppretter ingenting selv
    public LoanService(
        IBookRepository bookRepo,
        IMemberRepository memberRepo,
        IMediator mediator)
    {
        _bookRepo = bookRepo;
        _memberRepo = memberRepo;
        _mediator = mediator;
    }

    public async Task BorrowBookAsync(int bookId, int memberId)
    {
        var book = await _bookRepo.GetByIdAsync(bookId)
            ?? throw new InvalidOperationException($"Bok med id {bookId} finnes ikke.");

        if (!book.IsAvailable)
            throw new InvalidOperationException($"\"{book.Title}\" er allerede utlånt.");

        var member = await _memberRepo.GetByIdAsync(memberId)
            ?? throw new InvalidOperationException($"Medlem med id {memberId} finnes ikke.");

        // Oppdater boken
        book.IsAvailable = false;
        await _bookRepo.UpdateAsync(book);

        // Lagre lånet
        var loan = new Loan
        {
            BookId = bookId,
            MemberId = memberId,
            BorrowedAt = DateTime.Now,
            DueDate = DateTime.Now.AddDays(14)
        };

        // Publiser hendelsen – LoanService bryr seg ikke om hvem som lytter
        await _mediator.Publish(new BookBorrowedEvent(
            bookId, book.Title ?? "Ukjent tittel", memberId, member.Name, loan.DueDate));
    }

    public async Task ReturnBookAsync(int bookId)
    {
        var book = await _bookRepo.GetByIdAsync(bookId)
            ?? throw new InvalidOperationException($"Bok med id {bookId} finnes ikke.");

        if (book.IsAvailable)
            throw new InvalidOperationException($"\"{book.Title}\" er ikke utlånt.");

        book.IsAvailable = true;
        await _bookRepo.UpdateAsync(book);

        Console.WriteLine($"✅ \"{book.Title}\" er returnert.");
    }
}