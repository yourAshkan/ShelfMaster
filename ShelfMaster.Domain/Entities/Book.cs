namespace ShelfMaster.Domain.Entities;

public class Book
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string? ImageURL { get; private set; }
    public string? Description { get; private set; }
    public int AvailabeCount { get; set; }
    public bool IsAvailable { get; set; } = true;
    public bool IsDeleted { get; set; } = false;

    public List<Loan>? Loan { get; set; }


    public Book()
    {

    }

    public void SoftDeleted()
    {
        IsDeleted = true;
    }
    public void Edit(string newtilte, string author, string? imageURL, string? description)
    {
        Title = newtilte;
        Author = author;
        ImageURL = imageURL;
        Description = description;
    }
}
