using MediatR;
using ShelfMaster.Application.DTOs;

namespace ShelfMaster.Application.Books.Query;

public class GetAllBookQuery : IRequest<List<BookDto?>>
{
}
