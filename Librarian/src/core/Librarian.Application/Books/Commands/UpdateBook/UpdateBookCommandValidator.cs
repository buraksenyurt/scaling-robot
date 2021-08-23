using FluentValidation;
using Librarian.Application.Common.Interfaces;

namespace Librarian.Application.Books.Commands.UpdateBook
{
    /*
     * 
     * Kitap bilgilerinin güncellendiği Command nesnesi için hazırlanmış doğrulama tipi. 
     * Esasında CreateBookCommandValidator ile neredeyse aynı.
     * 
     */
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateBookCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Title)
              .NotEmpty().WithMessage("Kitabın başlığı olmalı!")
              .MaximumLength(100).WithMessage("Bu kitabın adı çok uzun!");

            RuleFor(v => v.Authors)
              .NotEmpty().WithMessage("Kitabın yazarları olmalı!")
              .MinimumLength(5).WithMessage("Bence yazarın adı beş karakterden fazladı. Bir daha bak!")
              .MaximumLength(200).WithMessage("Ne çok yazarı varmış. İsimleri kısaltalım.");

            RuleFor(v => v.Publisher)
              .NotEmpty().WithMessage("Elbette bir yayıncısı olmalı!").
              MaximumLength(30).WithMessage("Yayıncı adı 30 karakteri geçmeyiversin!");
        }
    }
}
