using Librarian.Application.Common.Exceptions;
using Librarian.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librarian.Application.Books.Commands.DeleteBook
{
    /*
     * Kitap silme işini ele aldığımı Command ve Handler tipleri.
     * 
     * Bir kitabı silmek için talebin içinde Id bilgisinin olması yeterli. Command buna göre düzenlendi.
     * Silme operasyonunu ele alan Handler tipimiz yine ilgili kitabı envanterde arıyor.
     * Eğer bulamazsa BookNotFoundException fırlatılıyor.
     * 
     */
    public class DeleteBookCommand
        : IRequest
    {
        public int BookId { get; set; }
    }

    public class DeleteBookCommandHandler
        : IRequestHandler<DeleteBookCommand>
    {
        private readonly IApplicationDbContext _context;
        public DeleteBookCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var b = await _context.Books
              .Where(l => l.Id == request.BookId)
              .SingleOrDefaultAsync(cancellationToken);

            if (b == null)
            {
                throw new BookNotFoundException(request.BookId);
            }

            _context.Books.Remove(b);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
