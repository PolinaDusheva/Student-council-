namespace StudentCouncil.Models;

public class NewsPost
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Body { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Today;
}
