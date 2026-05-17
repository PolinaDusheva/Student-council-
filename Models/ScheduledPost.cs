namespace StudentCouncil.Models;

public class ScheduledPost
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public string Platform { get; set; } = "Instagram";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "Чернова";
}
