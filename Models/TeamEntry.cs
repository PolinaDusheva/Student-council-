namespace StudentCouncil.Models;

public class TeamEntry
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public string Name { get; set; } = "";
    public int Wins { get; set; } = 0;
    public int Losses { get; set; } = 0;
}
