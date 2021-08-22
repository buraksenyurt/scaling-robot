using AutoMapper;
using Librarian.Application.Common.Mappings;
using Librarian.Domain.Entities;

namespace Librarian.Application.Dtos.Books
{
    public class BookDto
        : IMapFrom<Book>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Publisher { get; set; }
        public int Language { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Book, BookDto>()
                .ForMember(dest => dest.Language
                    , opt => opt.MapFrom(source => (int)source.Language)
                    );
        }
    }
}
