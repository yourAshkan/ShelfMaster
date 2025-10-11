using AutoMapper;
using MediatR;
using ShelfMaster.Application.Books.Command;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Domain.Entities;
using ShelfMaster.Domain.IRepository;

namespace ShelfMaster.Application.Books.Handler
{
    public class UpdateBookCommandHandler(IBookRepository _repo,IMapper _mapper) : IRequestHandler<UpdateBookCommand, BookDto?>
    {
        public async Task<BookDto?> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var book = await _repo.GetByIdAsync(request.Id);
                if (book == null)
                    throw new Exception("Book Not Found!");


                book.Edit(request.Title, request.Author, request.ImageURL, request.Description, request.AvailableCount);
                await _repo.UpdateAsync(book);
                return _mapper.Map<BookDto>(book);
            }
            catch (Exception ex)
            {
                throw new Exception("Erorr!", ex);
            }
        }
    }
}
