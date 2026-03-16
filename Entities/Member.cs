namespace LibrarySystem.Entities;

public class Member
{
    public int Id {get; set;}
    public string Name {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;


    // En member kan ha mange aktive lån – representert som en liste
    public List<Loan> ActiveLoans {get; set;} = new ();
}
