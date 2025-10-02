namespace ShelfMaster.Domain.Book.Entity;

public class Book
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string Image { get; private set; }
    public string Description { get; private set; }
    public bool IsAvailable { get; set; } = true;

    public Book()
    {
        
    }
}
