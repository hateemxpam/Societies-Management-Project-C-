namespace FAST.Societies.Desktop.Models;

public record EventDisplay(int EventId, string SocietyName, string Title, DateTime EventDate, string Status)
{
    public string DisplayName => $"{Title} ({SocietyName}) - {EventDate:MMM dd, yyyy}";
}
