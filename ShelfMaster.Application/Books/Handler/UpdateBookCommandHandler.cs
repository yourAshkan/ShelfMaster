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
                var book = _mapper.Map<Book>(request.Id);

                book.Edit(request.Title, request.Author, request.Description, request.ImageURL);
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
