using AutoMapper;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Domain.Entities;

namespace ShelfMaster.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book,BookDto>().ReverseMap();

        CreateMap<User,UserDto>().ReverseMap();

        CreateMap<Loan,LoanDto>()
              .ForMember(x => x.BookTitle, y => y.MapFrom(z => z.Book != null ? z.Book.Title : null))
              .ReverseMap();
    }
}
