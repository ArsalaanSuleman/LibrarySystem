namespace LibrarySystem.Events;

using MediatR;

// INotification er MediatR sitt interface for hendelser.
// Denne klassen er bare en datacontainer – ingen logikk.
// Den sier bare: "dette skjedde, og her er detaljene."
public record BookBorrowedEvent(
    int BookId,
    string BookTitle,
    int MemberId,
    string MemberName,
    DateTime DueDate
) : INotification;