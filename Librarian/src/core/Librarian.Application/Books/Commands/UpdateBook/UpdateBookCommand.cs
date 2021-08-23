using Librarian.Application.Common.Exceptions;
using Librarian.Application.Common.Interfaces;
using Librarian.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Librarian.Application.Books.Commands.UpdateBook
{
    /*
     * Bir kitabın bilgilerini güncellemek isteyebilirim. Adı, sanı, yazarları, yayıncası, hangi rafta olduğu vesaire.
     * Topluca bunların güncellemesini ele alan Command'ın beklediği özellikler aşağıdaki gibidir.
     * 
     * Handler sınıfı da bu Command'i kullanarak repository üzerinde gerekli güncellemeleri yapar. 
     * Id ile gelen kitap bilgisi bulunamazsa ortama BookNotFoundException isimli bizim yazdığımız bir istisna tipi fırlatılır.
     * 
     */
    public partial class UpdateBookCommand : IRequest
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public string Authors { get; set; }
        public byte Row { get; set; }
        public byte Column { get; set; }
        public Language Language { get; set; }
        public Status Status{ get; set; }
    }

    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateBookCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var b = await _context.Books.FindAsync(request.BookId);
            if (b == null)
            {
                throw new BookNotFoundException(request.BookId);
            }
            b.Title = request.Title;
            b.Authors = request.Authors;
            b.Publisher = request.Publisher;
            b.Language = request.Language;
            b.Status = request.Status;
            b.Row = request.Row;
            b.Column = request.Column;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
