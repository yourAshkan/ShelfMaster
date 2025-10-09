using MediatR;
using ShelfMaster.Application.DTOs;
using System.Text.Json.Serialization;

namespace ShelfMaster.Application.Books.Command;

[method: JsonConstructor]
public class CreateBookCommand(string title, string author, string description, string imageurl, int availablecount) : IRequest<BookDto?>
{
    public string? Title { get; set; } = title;
    public string? Author { get; set; } = author;
    public string? Description { get; set; } = description;
    public string? ImageURL { get; set; } = imageurl;
    public int AvailableCount { get; set; } = availablecount;
}
