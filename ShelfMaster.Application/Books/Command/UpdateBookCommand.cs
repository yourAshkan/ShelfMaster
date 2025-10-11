using MediatR;
using ShelfMaster.Application.DTOs;

namespace ShelfMaster.Application.Books.Command;

public class UpdateBookCommand(int id, string title, string author, string description, string imageUrl, int availablecount) : IRequest<BookDto?>
{
    public int Id { get; set; } = id;
    public string? Title { get; set; } = title;
    public string? Author { get; set; } = author;
    public string? Description { get; set; } = description;
    public string? ImageURL { get; set; } = imageUrl;
    public int AvailableCount { get; set; } = availablecount;
}
