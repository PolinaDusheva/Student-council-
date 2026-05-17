namespace StudentCouncil.Models;

public class Training
{
    public int Id { get; set; }
    public string Sport { get; set; } = "";
    public string Day { get; set; } = "";
    public string Time { get; set; } = "";
    public string Location { get; set; } = "";
    public string Coach { get; set; } = "";
}
