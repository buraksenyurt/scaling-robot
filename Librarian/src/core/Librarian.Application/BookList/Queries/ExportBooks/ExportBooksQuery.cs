using AutoMapper;
using AutoMapper.QueryableExtensions;
using Librarian.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Librarian.Application.BookList.Queries.ExportBooks
{
    public class ExportBooksQuery
        :IRequest<ExportBooksViewModel>
    {
    }

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
