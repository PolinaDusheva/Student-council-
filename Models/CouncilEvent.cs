namespace StudentCouncil.Models;

public class CouncilEvent
{

    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Location { get; set; } = "";

    public string Description { get; set; } = "";

    public DateTime Date { get; set; } = DateTime.Today.AddDays(7);

    public int MaxParticipants { get; set; } = 30;

    public string Organizer { get; set; } = "";
}
