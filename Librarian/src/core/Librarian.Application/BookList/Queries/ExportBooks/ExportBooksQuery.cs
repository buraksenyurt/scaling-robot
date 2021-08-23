using AutoMapper;
using AutoMapper.QueryableExtensions;
using Librarian.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Librarian.Application.BookList.Queries.ExportBooks
{
    /*
     * 
     * Query nesnemiz.
     * Talebe karşılık bir ViewModel döndürüleceğini tanımlıyor.
     * 
     */
    public class ExportBooksQuery
        :IRequest<ExportBooksViewModel>
    {
    }

    /*
     * Handler tipimiz.
     * Gelen sorguya ele alıp uygun ViewModel nesnesinin döndürülmesi sağlanıyor.
     * 
     * Kullanması için gereken bağımlılıkları Constructor Injection ile içeriye alıyoruz.
     * Buna göre CSV üretici, AutoMapper nesne dönüştürücü ve EF Core DbContext servislerini içeriye alıyoruz.
     * 
     * Kitap listesini çektiğimiz LINQ sorgusunda ProjectTo metodunu nasıl kullandığımız dikkat edelim.
     * Listenin BookRecord tipinden nesnelere dönüştürülmesi noktasında AutoMapper'ın çalışma zamanı sorumlusu kimse o kullanılıyor olacak.
     * Nitekim BookRecord tipinin IMapFrom<Book> tipini uyguladığını düşünecek olursak çalışma zamanı Book üstünden gelen özelliklerden eş düşenleri, BookRecord karşılıklarına alacak.
     */
    public class ExportBooksQueryHandler : IRequestHandler<ExportBooksQuery, ExportBooksViewModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICsvBuilder _csvBuilder;
        public ExportBooksQueryHandler(IApplicationDbContext context,IMapper mapper,ICsvBuilder csvBuilder)
        {
            _context = context;
            _mapper = mapper;
            _csvBuilder = csvBuilder;
        }
        public async Task<ExportBooksViewModel> Handle(ExportBooksQuery request, CancellationToken cancellationToken)
        {
            var viewModel= new ExportBooksViewModel();

            var list = await _context.Books
                .ProjectTo<BookRecord>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            viewModel.FileName = "Books.csv";
            viewModel.ContentType = "text/csv";
            viewModel.Content = _csvBuilder.BuildFile(list);

            return await Task.FromResult(viewModel);
        }
    }
}
