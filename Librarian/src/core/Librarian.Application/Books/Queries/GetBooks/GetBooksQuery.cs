using AutoMapper;
using AutoMapper.QueryableExtensions;
using Librarian.Application.Common.Interfaces;
using Librarian.Application.Dtos.Books;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Librarian.Application.Books.Queries.GetBooks
{
    /*
     * Query ile tüm kitap listesinin çekilmesi sürecini ele alıyoruz.
     * 
     * Talebe karşılık BooksViewModel nesnesi dönülüyor ki o da için BookDto tipinden bir liste barındırmakta.
     * 
     */
    public class GetBooksQuery
        : IRequest<BooksViewModel>
    {
    }

    public class GetBooksQueryHandler
        : IRequestHandler<GetBooksQuery, BooksViewModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache; //Redis Cache servisini Constructor Injection ile içeriye alıyoruz
        public GetBooksQueryHandler(IApplicationDbContext context, IMapper mapper, IDistributedCache distributedCache)
        {
            _context = context;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        public async Task<BooksViewModel> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            const string cacheKey = "GetBooks";
            BooksViewModel books;
            string serializedBookList;

            // Öncelikle cache'de GetBooks key değeri ile duran bir içerik var mı kontrol ediyoruz
            var redisData = await _distributedCache.GetAsync(cacheKey, cancellationToken);

            if (redisData == null) // İçerik yoksa,
            {
                books = new BooksViewModel
                {
                    BookList = await _context
                    .Books
                    .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                    .OrderBy(b => b.Title)
                    .ToListAsync(cancellationToken)
                }; // normal EF üstünden repository'ye gidip veriyi çekiyor

                serializedBookList = JsonConvert.SerializeObject(books); // onu JSON olarak serileştiriyor
                redisData = Encoding.UTF8.GetBytes(serializedBookList); // UTF8 formatında encode edip
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)) // 10 dakika süreyle
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1)); // ve son 1 dakika içerisinde yeni bir istek gelmemişse
                await _distributedCache.SetAsync(cacheKey, redisData, options, cancellationToken); // cache'e alıyoruz

                return books;
            }

            serializedBookList = Encoding.UTF8.GetString(redisData); // içerik cache üstünde varsa onu UTF8 encode ettirip
            books = JsonConvert.DeserializeObject<BooksViewModel>(serializedBookList); // JSON'dan ters serileştirerek model nesnesine alarak geriye döndürüyoruz

            return books;
        }
    }
}
