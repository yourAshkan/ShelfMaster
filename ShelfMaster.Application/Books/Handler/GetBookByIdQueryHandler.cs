using AutoMapper;
using MediatR;
using ShelfMaster.Application.Books.Query;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Domain.IRepository;

namespace ShelfMaster.Application.Books.Handler;

public class GetBookByIdQueryHandler(IBookRepository _repo,IMapper _mapper) : IRequestHandler<GetBookByIdQuery, BookDto?>
{
    public async Task<BookDto?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
		try
		{
			var book = await _repo.GetByIdAsync(request.Id);
			if (book == null)
				throw new Exception("Book Not Found!");

			return _mapper.Map<BookDto>(book);
		}
		catch (Exception ex)
		{
			throw new Exception("Error: ", ex);
		}
    }
}
