using AutoMapper;
using MediatR;
using ShelfMaster.Application.Books.Query;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Domain.IRepository;

namespace ShelfMaster.Application.Books.Handler;

public class GetAllBookQueryHandler(IBookRepository _repo,IMapper _mapper) : IRequestHandler<GetAllBookQuery, List<BookDto?>>
{
    public async Task<List<BookDto?>> Handle(GetAllBookQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var books = await _repo.GetAllAsync();

            return _mapper.Map<List<BookDto?>>(books);
        }
        catch (Exception ex)
        {
            throw new Exception("Erorr: ", ex);
        }
    }
}
