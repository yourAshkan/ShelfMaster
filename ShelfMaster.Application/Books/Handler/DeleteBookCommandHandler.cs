using MediatR;
using ShelfMaster.Application.Books.Command;
using ShelfMaster.Domain.IRepository;

namespace ShelfMaster.Application.Books.Handler;

public class DeleteBookCommandHandler(IBookRepository _repo) : IRequestHandler<DeleteBookCommand, bool>
{
    public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var book = await _repo.GetByIdAsync(request.Id);
            if (book == null)
                throw new Exception("Book not Found!");

            await _repo.DeleteAsync(book.Id);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Erorr : ", ex);
        }
    }
}
