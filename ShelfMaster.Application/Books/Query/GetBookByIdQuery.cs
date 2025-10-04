using MediatR;
using ShelfMaster.Application.DTOs;

namespace ShelfMaster.Application.Books.Query;

public class GetBookByIdQuery(int id) : IRequest<BookDto?>
{
    public int Id { get; set; } = id;
}
