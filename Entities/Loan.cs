namespace LibrarySystem.Entities;

public class Loan
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsReturned { get; set; } = false;

    // Navigation properties – EF Core bruker disse til å forstå
    // relasjoner mellom tabeller. Book og Member er ikke kolonner,
    // men EF Core kan hente dem automatisk via JOIN.
    public Book? Book { get; set; }
    public Member? Member { get; set; }
}