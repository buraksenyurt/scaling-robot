using FluentValidation;
using Librarian.Application.Common.Interfaces;

namespace Librarian.Application.Books.Commands.CreateBook
{
    /*
     * Kitap ekleme komutu için gerekli doğrulama işlerini üstlenen Validator tipi.
     * Türediği Abstract sınıfın T tipine dikkat edelim. Validasyonu hangi Command için yaptığımızı ifade ediyoruz.
     * Örnek olarak genelde boş olma hali ile uzunluk için doğrulamalar var. Ama farklı kontroller de konabilir.
     */
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateBookCommandValidator(IApplicationDbContext context)
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
