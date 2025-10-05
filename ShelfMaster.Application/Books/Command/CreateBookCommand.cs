using MediatR;
using ShelfMaster.Application.DTOs;

namespace ShelfMaster.Application.Books.Command;

public class CreateBookCommand(string title, string author, string description, string imageurl, int availabecount) : IRequest<BookDto?>
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Description { get; set; }
    public string? ImageURL { get; set; }
    public int AvailabeCount { get; set; }
    public int UserId { get; set; }

    public void SetUserId(int userId) => UserId = userId;
}
