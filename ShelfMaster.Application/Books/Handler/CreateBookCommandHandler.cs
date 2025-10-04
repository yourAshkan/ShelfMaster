using AutoMapper;
using MediatR;
using ShelfMaster.Application.Books.Command;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Domain.Entities;
using ShelfMaster.Domain.IRepository;

namespace ShelfMaster.Application.Books.Handler;

public class CreateBookCommandHandler(IBookRepository _repo,IMapper _mapper) : IRequestHandler<CreateBookCommand, BookDto?>
{
    public async Task<BookDto?> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
		try
		{
			var book = _mapper.Map<Book>(request);

			await _repo.AddAsync(book);
			return _mapper.Map<BookDto?>(book);
		}
		catch (Exception ex)
		{
			throw new Exception("Erorr!", ex);
		}
    }
}
