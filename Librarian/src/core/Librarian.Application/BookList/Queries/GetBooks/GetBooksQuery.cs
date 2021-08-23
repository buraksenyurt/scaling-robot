using AutoMapper;
using AutoMapper.QueryableExtensions;
using Librarian.Application.Common.Interfaces;
using Librarian.Application.Dtos.Books;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librarian.Application.BookList.Queries.GetBooks
{
    /*
     * Query ile tüm kitap listesinin çekilmesi sürecini ele alıyoruz.
     * 
     * Talebe karşılık BooksViewModel nesnesi dönülüyor ki o da için BookDto tipinden bir liste barındırmakta.
     * 
     */
    public class GetBooksQuery
        :IRequest<BooksViewModel>
    {        
    }

    public class GetBooksQueryHandler
        : IRequestHandler<GetBooksQuery, BooksViewModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetBooksQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<BooksViewModel> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var books = new BooksViewModel
            {
                BookList=await _context
                    .Books
                    .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                    .OrderBy(b=>b.Title)
                    .ToListAsync(cancellationToken)
            };

            return books;            
        }
    }
}
