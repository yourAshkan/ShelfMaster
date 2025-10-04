using MediatR;
using ShelfMaster.Application.DTOs;

namespace ShelfMaster.Application.Books.Command;

public class UpdateBookCommand : IRequest<BookDto?>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Description { get; set; }
    public string? ImageURL { get; set; }
    public DateTime? UpdateDate { get; set; } = DateTime.Now;
}
