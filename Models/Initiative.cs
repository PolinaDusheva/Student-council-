namespace StudentCouncil.Models;

public class Initiative
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<Volunteer> Volunteers { get; set; } = new();
}
