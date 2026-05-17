namespace StudentCouncil.Models;

public class Tournament
{
    public int Id { get; set; }
    public string Sport { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Today.AddDays(14);
    public List<TeamEntry> Teams { get; set; } = new();
}
