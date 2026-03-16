namespace LibrarySystem.Entities;

// Book arver alt fra LibraryItem (Id, Title, IsAvailable)
// og legger til sitt eget (Author, ISBN, Genre)
public class Book : LibraryItem
{
    public string? Author {get; set;} = string.Empty;
    public string ISBN {get; set;} = string.Empty;
    public string Genre {get; set;} = string.Empty;

    public override string GetDisplayInfo()
    {
        string status = IsAvailable ? "Available" : "Checked Out";
        return $"[{Id}] \"{Title}\" av {Author} – {Genre} ({status})";

    }
}