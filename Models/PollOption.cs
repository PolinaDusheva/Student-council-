namespace StudentCouncil.Models;

public class PollOption
{
    public int Id { get; set; }
    public int PollId { get; set; }
    public string Text { get; set; } = "";
    public int Votes { get; set; } = 0;
}
