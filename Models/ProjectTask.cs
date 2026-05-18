namespace StudentCouncil.Models;

public class ProjectTask
{

    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string CreatedBy { get; set; } = "";

    public string Assignee { get; set; } = "";

    public string Status { get; set; } = "За изпълнение";
}
