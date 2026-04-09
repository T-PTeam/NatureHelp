namespace Domain.Models.Profile;

public class AppDictEntry
{
    public Guid Id { get; set; }
    public string EntryKey { get; set; } = null!;
    public string ValueJson { get; set; } = null!;
}
