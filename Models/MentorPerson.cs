namespace StudentCouncil.Models;

public class MentorPerson
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Role { get; set; } = "Ментор";
    public string PairedWith { get; set; } = "";
}
