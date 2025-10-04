using MediatR;

namespace ShelfMaster.Application.Books.Command;

public class DeleteBookCommand : IRequest<bool>
{
    public int Id { get; set; }
}
