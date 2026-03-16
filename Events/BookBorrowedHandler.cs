namespace LibrarySystem.Events;

using MediatR;

// INotificationHandler<T> sier: "denne klassen håndterer BookBorrowedEvent"
// LoanService som publiserer hendelsen vet IKKE at denne handleren eksisterer.
// Det er løst kobling – vi kan legge til flere handlers uten å røre LoanService.
public class BookBorrowedHandler : INotificationHandler<BookBorrowedEvent>
{
    public Task Handle(BookBorrowedEvent notification, CancellationToken ct)
    {
        // I et ekte system ville dette sendt en e-post eller SMS.
        // Nå skriver vi bare til konsollen som en demo.
        Console.WriteLine();
        Console.WriteLine($"📬 Varsel: \"{notification.BookTitle}\" er lånt ut til " +
                          $"{notification.MemberName}. Frist: {notification.DueDate:dd.MM.yyyy}");
        return Task.CompletedTask;
    }
}