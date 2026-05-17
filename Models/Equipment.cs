namespace StudentCouncil.Models;

public class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string BorrowedBy { get; set; } = "";
    public DateTime? ReturnDate { get; set; }
}
