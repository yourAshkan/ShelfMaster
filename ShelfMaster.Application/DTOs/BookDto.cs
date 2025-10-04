namespace ShelfMaster.Application.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string? ImageURL { get; set; }
    public string? Description { get; set; }
    public int AvailableCount { get; set; }
    public bool IsAvailable { get; set; }
}
