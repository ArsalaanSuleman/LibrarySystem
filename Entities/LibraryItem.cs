namespace LibrarySystem.Entities

// "abstract" betyr at du aldri kan skrive "new LibraryItem()" direkte.
// Den eksisterer bare som en base andre klasser arver fra.
{
    public abstract class LibraryItem
    {
        public int Id { get; set; }
        public string? Title {get; set; }
        public bool IsAvailable {get; set; } = true;

        // "abstract" metode = ingen implementasjon her.
        // Hver subklasse MÅ overstyre den.
        public abstract string GetDisplayInfo();
    }
}