namespace StudentCouncil.Models;

public class Volunteer
{
    public int Id { get; set; }
    public int InitiativeId { get; set; }
    public string Name { get; set; } = "";
    public int Hours { get; set; } = 1;
}
