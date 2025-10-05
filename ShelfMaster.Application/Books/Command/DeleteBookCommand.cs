using MediatR;

namespace ShelfMaster.Application.Books.Command;

public class DeleteBookCommand(int id) : IRequest<bool>
{
    public int Id { get; set; } = id;
}
