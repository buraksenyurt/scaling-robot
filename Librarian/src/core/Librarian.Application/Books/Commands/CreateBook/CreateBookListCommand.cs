using Librarian.Application.Common.Interfaces;
using Librarian.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Librarian.Application.Books.Commands.CreateBook
{
    /*
     * Kitap listesine yeni bir kitap ekleme işini üstlenen MediatR tipleri.
     * 
     * Komut kitap için gerekli parametreleri alırken geriye insert sonrası oluşan bir int değer(kuvvetle muhtemel primary key id değeri) dönüyor.
     * 
     * Handler sınıfı IApplicationDbContext üstünden gelen Entity Context nesnesini kullanarak kitabı repository'ye ekliyor.
     * 
     */
    public class CreateBookCommand
        : IRequest<int>
    {
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Publisher { get; set; }
        public byte Row { get; set; }
        public byte Column { get; set; }
    }

    public class CreateBookCommandHandler
        : IRequestHandler<CreateBookCommand, int>
    {
        private readonly IApplicationDbContext _context;
        public CreateBookCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var b = new Book
            {
                Title = request.Title,
                Authors = request.Authors,
                Publisher = request.Publisher,
                Row = request.Row,
                Column = request.Column
            };
            _context.Books.Add(b);
            await _context.SaveChangesAsync(cancellationToken);

            return b.Id;
        }
    }
}
