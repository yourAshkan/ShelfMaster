using AutoMapper;
using ShelfMaster.Application.Books.Command;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Domain.Entities;

namespace ShelfMaster.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book,BookDto>().ReverseMap();

        CreateMap<CreateBookCommand, Book>()
              .ForMember(dest => dest.Id, opt => opt.Ignore())
              .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
              .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.AvailableCount > 0))
              .ForMember(dest => dest.Loans, opt => opt.Ignore());


        CreateMap<User, UserDto>().ReverseMap();

        CreateMap<Loan,LoanDto>()
              .ForMember(x => x.BookTitle, y => y.MapFrom(z => z.Book != null ? z.Book.Title : null))
              .ReverseMap();
    }
}
